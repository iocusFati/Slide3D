using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Chunks
{
    public class Chunk : MonoBehaviour
    {
        public Transform Start;
        public Transform End;

        [SerializeField] private List<GameObject> _blockPickUp;

        public void ActivatePickUps()
        {
            foreach (var block in _blockPickUp) 
                block.SetActive(true);
        }
    }
}