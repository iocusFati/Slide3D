using System.Collections.Generic;
using Infrastructure.AssetProvider;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Chunks.Spawning.Blocks
{
    public class BlockPool
    {
        private readonly IAssets _assetProvider;
        
        private IObjectPool<Block> _blockPool;
        private readonly List<Block> _activeCoins = new();

        private IObjectPool<Block> Pool
        {
            get
            {
                return _blockPool ??= new ObjectPool<Block>(
                    SpawnBlock,
                    block => { block.gameObject.SetActive(true); }, 
                    block => { block.gameObject.SetActive(false); },
                    block => { Object.Destroy(block.gameObject); });
            }
        }

        public BlockPool(IAssets assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public Block GetBlock()
        {
            return Pool.Get();
        }

        public void Release(Block block)
        {
            Pool.Release(block);
        }

        private Block SpawnBlock()
        {
            var spawnBlock = _assetProvider.Instantiate<Block>(AssetPaths.Block);
            spawnBlock.Construct(this);
            
            return spawnBlock;
        }
    }
}