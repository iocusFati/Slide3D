using System.Collections.Generic;
using Gameplay.Chunks.Spawning.Blocks;
using Infrastructure.Services;
using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public interface IBlockStacker : IService
    {
        void Initialize(Player player);
        void Stack(Transform hitBlock);
        void RemoveBlock(Block block);
        void TryGetStackedBlocksDown();
        void TryGetCollidedBlocksDown();
        void DisableCollided();
        void SetStackToDefault();
        void ImmediatelyDisableCollided();
        List<Block> BlocksExceptHead();
    }
}