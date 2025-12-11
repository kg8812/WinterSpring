using Apis;
using chamwhy.Managers;
using Default;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace _02_Scripts.Tester
{
    public class memoryTester: MonoBehaviour
    {
        public AssetReference test;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                GetResource();
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetResource2();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetResource3();
            }
        }

        [Button(ButtonSizes.Gigantic)]
        public void GetResource()
        {
            Debug.LogError("get");
            ResourceUtil.Load<SpriteAtlas>(test);
        }
        
        public void GetResource2()
        {
            Debug.LogError("get2");
        }
        
        public void GetResource3()
        {
            Debug.LogError("get3");
            ResourceUtil.LoadAll<Object>("memoryTest");
            ResourceUtil.LoadAll<Accessory>("Prefabs/Items/Accessory");
        }
    }
}