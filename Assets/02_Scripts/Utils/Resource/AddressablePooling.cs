using System.Collections.Generic;
using Default;
using UnityEngine;

namespace Apis
{
    /// <summary>
    ///     주소 단위 오브젝트 풀.
    ///     인스턴스는 순수 Unity 오브젝트이므로(ResourceUtil.Instantiate 참조) Addressables 핸들을 갖지 않는다.
    ///     대여된 오브젝트는 외부에서 파괴될 수 있으며, 이 클래스는 그런 오브젝트를 만나도 깨지지 않도록 방어한다.
    /// </summary>
    public class AddressablePooling
    {
        private static GameObject poolParent;

        // 대기 중인 오브젝트. 주소별로 나눠 보관한다.
        private readonly Dictionary<string, Queue<GameObject>> _idle = new();

        // 대기 큐에 들어있는 인스턴스 ID. 중복 반환을 O(1)로 검사하기 위한 것.
        private readonly HashSet<int> _idleIds = new();

        // 이 풀이 만든 모든 오브젝트의 추적 정보. 파괴된 오브젝트도 ID로 다룰 수 있도록 int를 키로 쓴다.
        private readonly Dictionary<int, ObjectInfo> _infos = new();

        // 주소별 프리팹 스코프. Prewarm이 Scene으로 등록한 주소는 Get이 새로 만들 때도 같은 스코프를 쓴다.
        // 등록되지 않은 주소는 Global.
        private readonly Dictionary<string, AssetLifetime> _lifetimes = new();

        private readonly string _name;
        private readonly GameObject pool;

        public AddressablePooling(string name)
        {
            _name = name;
            pool = new GameObject(name + " Pool");
            pool.transform.SetParent(PoolParent.transform);
        }

        public static GameObject PoolParent
        {
            get
            {
                if (poolParent == null)
                {
                    poolParent = new GameObject("Pooling");
                    Object.DontDestroyOnLoad(poolParent.gameObject);
                }

                return poolParent;
            }
        }

        /// <summary>
        ///     로딩 단계에서 미리 인스턴스를 만들어 둔다. 런타임 중 첫 생성 스파이크를 없애기 위한 것.
        ///     프리팹이 이미 다른 스코프(예: Scene)로 로드돼 있다면 같은 lifetime을 넘겨야 그 캐시를 그대로 쓴다.
        /// </summary>
        public void Prewarm(string address, int count, AssetLifetime lifetime = AssetLifetime.Global)
        {
            _lifetimes[address] = lifetime;
            var idle = GetIdleQueue(address);

            for (var i = 0; i < count; i++)
            {
                var obj = CreateNew(address, lifetime);
                if (obj == null) return; // 주소가 잘못된 경우이므로 반복해도 의미가 없다

                Park(obj, idle);
            }
        }

        public GameObject Get(string address, Vector2? pos = null)
        {
            var idle = GetIdleQueue(address);
            var obj = DequeueAlive(idle);

            if (obj == null)
            {
                // 새로 만든 오브젝트도 일단 풀(DontDestroyOnLoad)에 넣었다가 꺼낸다. 기존 풀과 같은 흐름을 유지하기 위함:
                // 1) 대여 오브젝트는 항상 DontDestroyOnLoad 씬에 속한다. 현재 씬에 바로 두면 처음 만든 것만 씬 전환 때 파괴된다.
                // 2) 생성 직후 OnEnable → OnDisable → OnEnable → OnGet 순서가 재사용 때와 같아진다.
                var created = CreateNew(address, LifetimeOf(address));
                if (created == null) return null;

                RecordRuntimeCreate(address);

                Park(created, idle);
                obj = DequeueAlive(idle);
            }

            obj.transform.SetParent(null);

            if (_infos.TryGetValue(obj.GetInstanceID(), out var info))
            {
                obj.transform.rotation = info.rotation;
                obj.transform.localScale = info.scale;
            }

            if (pos != null) obj.transform.position = (Vector2)pos;

            obj.SetActive(true);

            foreach (var poolObject in obj.GetComponents<IPoolObject>()) poolObject.OnGet();

            return obj;
        }

        /// <summary>대기 큐에서 살아있는 오브젝트 하나를 꺼낸다. 외부에서 파괴된 것은 건너뛰고 추적 정보도 정리한다.</summary>
        private GameObject DequeueAlive(Queue<GameObject> idle)
        {
            while (idle.Count > 0)
            {
                var candidate = idle.Dequeue();
                var candidateId = candidate.GetInstanceID();
                _idleIds.Remove(candidateId);

                if (candidate != null) return candidate;

                _infos.Remove(candidateId);
            }

            return null;
        }

        public void Return(GameObject obj)
        {
            if (obj == null) return;

            var id = obj.GetInstanceID();

            // 이 풀이 만들지 않았거나 추적이 끊긴(씬 스코프 정리) 오브젝트는 그냥 파기한다.
            if (!_infos.TryGetValue(id, out var info))
            {
                Object.Destroy(obj);
                return;
            }

            if (!_idleIds.Add(id)) return; // 이미 반환된 오브젝트

            foreach (var poolObject in obj.GetComponents<IPoolObject>()) poolObject.OnReturn();

            obj.SetActive(false);
            obj.transform.SetParent(pool.transform);
            obj.transform.localPosition = info.position;
            obj.transform.rotation = info.rotation;
            obj.transform.localScale = info.scale;

            GetIdleQueue(info.address).Enqueue(obj);

            // 기존 풀과 동일하게 한 프레임 뒤에도 트랜스폼을 되돌린다.
            // 반환 직후 같은 프레임에 트윈 등이 트랜스폼을 한 번 더 건드려도 원래 값으로 돌아오게 하기 위함.
            Utils.ActionAfterFrame(() =>
            {
                if (obj == null || !_idleIds.Contains(id)) return; // 그새 다시 대여됐거나 파괴됨

                obj.transform.localPosition = info.position;
                obj.transform.rotation = info.rotation;
                obj.transform.localScale = info.scale;
            });
        }

        /// <summary>
        ///     Scene 스코프 프리팹으로 만든 오브젝트를 정리한다. 씬 전환 시, 애셋 해제 직전에 호출한다.
        ///     대기 중인 것은 파기하고, 대여 중인 것은 추적만 끊어 반환될 때 파기되게 한다
        ///     (곧 해제될 애셋을 참조하므로 다시 빌려주면 안 된다).
        ///     Global 오브젝트는 건드리지 않는다. 아이템 정령처럼 씬을 넘어 살아있는 대여 오브젝트가 있기 때문.
        /// </summary>
        public void ReleaseScene()
        {
            var removed = new List<int>();

            foreach (var pair in _infos)
            {
                var info = pair.Value;

                if (info.obj == null)
                {
                    removed.Add(pair.Key);
                    continue;
                }

                if (info.lifetime != AssetLifetime.Scene) continue;

                if (_idleIds.Contains(pair.Key)) Object.Destroy(info.obj);
                removed.Add(pair.Key);
            }

            foreach (var id in removed)
            {
                _infos.Remove(id);
                _idleIds.Remove(id);
            }

            // Destroy는 프레임 끝에 처리되므로 큐에서 직접 걷어내야 같은 프레임의 Get이 파기 예정 오브젝트를 꺼내지 않는다.
            foreach (var address in new List<string>(_idle.Keys))
            {
                var kept = new Queue<GameObject>();
                foreach (var obj in _idle[address])
                    if (obj != null && _idleIds.Contains(obj.GetInstanceID()))
                        kept.Enqueue(obj);

                _idle[address] = kept;
            }

            foreach (var address in new List<string>(_lifetimes.Keys))
                if (_lifetimes[address] == AssetLifetime.Scene)
                    _lifetimes.Remove(address);
        }

        /// <summary>
        ///     이 풀이 만든 오브젝트를 모두 파기하고 초기 상태로 되돌린다.
        /// </summary>
        public void Clear()
        {
            foreach (var info in _infos.Values)
                if (info.obj != null)
                    Object.Destroy(info.obj);

            _idle.Clear();
            _idleIds.Clear();
            _infos.Clear();
            _lifetimes.Clear();
        }


        #region 프리웜 누락 진단

        // 프리웜 없이 게임 도중 Get에서 새로 만든 개수와 그때의 씬. 개수는 동시에 필요했던 최대 수에 가깝다
        // (반환된 오브젝트는 재사용되므로 모자랄 때만 새로 만든다). PreloadManifest.prewarm의 count를 정하는 근거.
        private readonly Dictionary<string, int> _runtimeCreated = new();
        private readonly Dictionary<string, SortedSet<string>> _runtimeCreatedScenes = new();

        [System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        private void RecordRuntimeCreate(string address)
        {
            _runtimeCreated[address] = _runtimeCreated.TryGetValue(address, out var count) ? count + 1 : 1;

            if (!_runtimeCreatedScenes.TryGetValue(address, out var scenes))
            {
                scenes = new SortedSet<string>();
                _runtimeCreatedScenes.Add(address, scenes);
            }

            scenes.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        /// <summary>
        ///     게임 도중 새로 생성된 풀 오브젝트를 출력한다. 한 줄에 "주소 x개수 | scenes: 씬1, 씬2" 형식.
        /// </summary>
        public void LogCreateReport()
        {
            if (_runtimeCreated.Count == 0)
            {
                Debug.Log($"[Pool:{_name}] 게임 도중 새로 생성된 오브젝트 없음.");
                return;
            }

            var lines = new List<string>();
            foreach (var pair in _runtimeCreated)
                lines.Add($"{pair.Key} x{pair.Value} | scenes: {string.Join(", ", _runtimeCreatedScenes[pair.Key])}");
            lines.Sort(System.StringComparer.Ordinal);

            Debug.LogWarning(
                $"[Pool:{_name}] 게임 도중 새로 생성 {_runtimeCreated.Count}종 — PreloadManifest.prewarm 후보:\n"
                + string.Join("\n", lines));
        }

        #endregion

        private AssetLifetime LifetimeOf(string address)
        {
            return _lifetimes.TryGetValue(address, out var lifetime) ? lifetime : AssetLifetime.Global;
        }

        private GameObject CreateNew(string address, AssetLifetime lifetime)
        {
            // 부모를 지정하지 않고 생성해 프리팹 원본 트랜스폼 값을 그대로 캡처한다.
            var obj = ResourceUtil.Instantiate(address, null, lifetime);
            if (obj == null)
            {
                Debug.LogError($"[Pool:{_name}] Failed to instantiate '{address}'.");
                return null;
            }

            _infos[obj.GetInstanceID()] = new ObjectInfo(obj, address, lifetime);
            return obj;
        }

        private void Park(GameObject obj, Queue<GameObject> idle)
        {
            obj.SetActive(false);
            obj.transform.SetParent(pool.transform);
            idle.Enqueue(obj);
            _idleIds.Add(obj.GetInstanceID());
        }

        private Queue<GameObject> GetIdleQueue(string address)
        {
            if (_idle.TryGetValue(address, out var idle)) return idle;

            idle = new Queue<GameObject>();
            _idle.Add(address, idle);
            return idle;
        }

        private readonly struct ObjectInfo
        {
            public ObjectInfo(GameObject obj, string address, AssetLifetime lifetime)
            {
                this.obj = obj;
                var transform = obj.transform;
                rotation = transform.rotation;
                position = transform.localPosition;
                scale = transform.localScale;
                this.address = address;
                this.lifetime = lifetime;
            }

            public readonly GameObject obj;
            public readonly Quaternion rotation;
            public readonly Vector3 position;
            public readonly Vector3 scale;
            public readonly string address;
            public readonly AssetLifetime lifetime;
        }
    }
}
