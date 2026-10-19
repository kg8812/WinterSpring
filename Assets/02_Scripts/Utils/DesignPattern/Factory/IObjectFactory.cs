using System.Collections.Generic;
using DG.Tweening;
using Default;
using Spine.Unity;
using UnityEngine;

namespace Apis
{
    public abstract class IObjectFactory
    {
        static AddressablePooling pool;
        
        protected static AddressablePooling Pool
        {
            get
            {
                pool ??= new ("Object");
                return pool;
            }
        }

        /// <summary>로딩 단계에서 미리 인스턴스를 만들어 둔다.</summary>
        public static void PrewarmPool(string address, int count, AssetLifetime lifetime = AssetLifetime.Global)
        {
            Pool.Prewarm(address, count, lifetime);
        }

        /// <summary>씬 전환 시 Scene 스코프 프리팹으로 만든 오브젝트를 정리한다.</summary>
        public static void ReleaseScenePool()
        {
            pool?.ReleaseScene();
        }

        /// <summary>게임 도중 새로 생성된 풀 오브젝트 목록을 출력한다. 프리웜 수량 결정용.</summary>
        public static void LogPoolReport()
        {
            if (pool == null)
            {
                Debug.Log("[Pool:Object] 아직 생성된 풀이 없습니다.");
                return;
            }

            pool.LogCreateReport();
        }

        private readonly Dictionary<GameObject, Sequence> returnSequences = new();

        public abstract GameObject Get(string addressName, Vector2? pos = null);

        public Sequence Return(GameObject target, float time = 0) // 반환 함수
        {
            if (returnSequences.TryGetValue(target, out var sq))
            {
                sq?.Kill();
                returnSequences.Remove(target);
            }

            if (Mathf.Approximately(time,0))
            {
                ReturnTarget(target);
                return null;
            }
            
            Sequence seq = DOTween.Sequence();

            returnSequences.Add(target,seq);
            
            seq.AppendInterval(time);
            seq.AppendCallback(() =>
            {
                ReturnTarget(target);
                returnSequences.Remove(target);
            });

            return seq;
        }

        void ReturnTarget(GameObject target)
        {
            // 지연 반환(Return의 time > 0)은 씬 전환 뒤에 도착할 수 있다. 그때 target은 이미 파괴돼 있을 수 있다.
            if (target == null) return;

            if (target.TryGetComponent(out BoneFollower boneFollower))
            {
                Object.Destroy(boneFollower);
            }
            
            Pool.Return(target);
        }
    }
}