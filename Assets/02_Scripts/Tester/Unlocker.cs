using Save.Schema;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _02_Scripts.Tester
{
    public class Unlocker: MonoBehaviour
    {
        public CodexData.CodexType codexType;
        public int id;

        [Button(ButtonSizes.Medium)]
        public void UnLock()
        {
            DataAccess.Codex.UnLock(codexType, id);
        }
    }
}