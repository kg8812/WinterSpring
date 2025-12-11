using System.IO;
using System.Linq;
using chamwhy.Model;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace chamwhy
{
    public class MapDataMaker : MonoBehaviour
    {
        [ReadOnly] public MapBoxPermanentData MapBoxPermanentData;
        public int sectorID;
        public Vector2 size = new Vector2(2, 2); // 사각형 크기
        public Vector2 position = Vector2.zero;
        public SectorObject[] clearObtainObjects;

#if UNITY_EDITOR

        [Button("맵 박스 데이터 생성", 50)]
        void CreateSectorData()
        {
            string path = $"Assets/06_ScriptableObjects/Datas/Map/MapBox/MapBox_{sectorID}.asset";
            if (MapBoxPermanentData == null)
            {
                MapBoxPermanentData = AssetDatabase.LoadAssetAtPath<MapBoxPermanentData>(path);
                if (MapBoxPermanentData == null)
                {
                    MapBoxPermanentData = ScriptableObject.CreateInstance(typeof(MapBoxPermanentData)) as MapBoxPermanentData;
                    AssetDatabase.CreateAsset(MapBoxPermanentData, path);
                }
            }

            MapBoxPermanentData.mapBoxId = sectorID;
            
            AssetDatabase.SaveAssets();
        }

        [Button("MainWorld에서 경계선 데이터 가져오기", 50)]
        private void LoadColliderData()
        {
            string scenePath = $"Assets/01_Scenes/{Define.SceneNames.MainWorldSceneName}.unity";
            string objectName = "SectorLC_" + sectorID.ToString();
            // 씬 로드 (Additive 모드, 활성화하지 않음)
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            if (!scene.IsValid())
            {
                Debug.LogError($"씬 '{scenePath}'을 찾을 수 없습니다.");
                return;
            }

            // 모든 루트 게임 오브젝트 가져오기
            GameObject[] rootObjects = scene.GetRootGameObjects();
            GameObject targetObject = GameObject.Find(objectName);

            // 오브젝트 찾기
            foreach (var obj in rootObjects)
            {
                if (obj.name == objectName)
                {
                    targetObject = obj;
                    break;
                }
            }

            if (targetObject == null)
            {
                Debug.LogError($"오브젝트 '{objectName}'을 찾을 수 없습니다.");
                UnloadScene(scene);
                return;
            }

            targetObject = targetObject.transform.Find("SectorLoader").gameObject;
            if (targetObject == null)
            {
                Debug.LogError($"오브젝트 '{objectName}'을 찾을 수 없습니다.");
                UnloadScene(scene);
                return;
            }

            // BoxCollider2D 가져오기
            BoxCollider2D collider = targetObject.GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                Debug.LogError($"오브젝트 '{objectName}'에 BoxCollider2D가 없습니다.");
                UnloadScene(scene);
                return;
            }

            // 데이터 저장
            size = collider.size;
            position = collider.transform.position;

            Debug.Log($"'{objectName}'의 BoxCollider2D 데이터를 가져왔습니다. (Size: {size}, Position: {position})");

            // 씬 언로드
            UnloadScene(scene);
        }

        [Button("경계선 데이터 등록", 50)]
        void UploadColliderData()
        {
            CreateSectorData();

            MapBoxPermanentData.mapBoxData.size = size;
            MapBoxPermanentData.mapBoxData.position = position;

            EditorUtility.SetDirty(MapBoxPermanentData);
            AssetDatabase.SaveAssets();
        }
        
        
        [Button("고정 획득 요소 등록", 50)]
        void UploadFoundObjsData()
        {
            CreateSectorData();

            MapBoxPermanentData.requiredObtainableGuids = clearObtainObjects.Select(fo => fo.guid.ToString()).ToArray();

            EditorUtility.SetDirty(MapBoxPermanentData);
            AssetDatabase.SaveAssets();
        }

        private void UnloadScene(Scene scene)
        {
            EditorSceneManager.CloseScene(scene, true); // 씬을 언로드
            Debug.Log($"씬'을 언로드했습니다.");
        }
        
        private void OnDrawGizmos()
        {
            // sector size
            Gizmos.color = Color.red;
            Vector3 center = position;
            Gizmos.DrawWireCube(center, size);
        }

#endif
        
        
    }
}