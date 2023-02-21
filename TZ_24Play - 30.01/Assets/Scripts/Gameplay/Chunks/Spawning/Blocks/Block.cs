using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Chunks.Spawning.Blocks
{
    public class Block : MonoBehaviour
    {
        private BlockPool _blockPool;
        private Transform _head;

        public void Construct(BlockPool blockPool)
        {
            _blockPool = blockPool;
        }

        public void Release() => 
            _blockPool.Release(this);
    }
}