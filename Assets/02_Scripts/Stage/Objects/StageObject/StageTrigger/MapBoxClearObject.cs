using System;
using UnityEngine;

namespace chamwhy
{
    [RequireComponent(typeof(SectorObject))]
    public class MapBoxClearObject: MonoBehaviour
    {
        private SectorObject _so;

        private void Awake()
        {
            _so = GetComponent<SectorObject>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _so.Activate();
            }
        }
    }
}