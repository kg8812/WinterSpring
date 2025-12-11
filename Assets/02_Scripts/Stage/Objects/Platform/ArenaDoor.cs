using chamwhy.StageObj;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace chamwhy
{
    public class ArenaDoor : Door
    {
        protected override void Start()
        {
            GameManager.instance.whenArenaStateChanged.AddListener(MoveDoor);
        }
    }
}