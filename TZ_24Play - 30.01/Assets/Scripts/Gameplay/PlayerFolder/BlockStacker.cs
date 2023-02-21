using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Chunks.Spawning.Blocks;
using Gameplay.PlayerFolder.Data;
using Infrastructure;
using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public class BlockStacker : IBlockStacker
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly BlockPool _blockPool;
        private readonly float _heightAboveLast;
        private readonly float _timeBeforeDisableBlock;
        private readonly float _comparisonTolerance = 0.07f;
        private readonly float _getBlockDownSpeed;

        private Player _player;
        private Transform _playerTransform;
        private Transform _playerBody;
        
        private Transform _currentBlock;
        private float _firstBlockPosY;
        private float _bodyAboveTheHighestBlockPosY;

        private readonly List<Block> _stackedBlocks = new();
        private List<Block> _collidedBlocks = new();

        public BlockStacker(
            ICoroutineRunner coroutineRunner,
            BlockPool blockPool,
            PlayerStaticData playerStaticData)
        {
            _coroutineRunner = coroutineRunner;
            _blockPool = blockPool;

            _heightAboveLast = playerStaticData.HeightAboveLast;
            _timeBeforeDisableBlock = playerStaticData.TimeBeforeDisableBlock;
            _getBlockDownSpeed = playerStaticData.GetBlockDownSpeed;
        }

        public void Initialize(Player player)
        {
            _player = player;
            _playerBody = _player.PlayerBody;
            _playerTransform = _player.transform;
            
            _stackedBlocks.Add(_player.FirstBlock);
            _firstBlockPosY = _player.FirstBlock.transform.position.y;
            _bodyAboveTheHighestBlockPosY = _playerBody.position.y - _firstBlockPosY;
        }
        
        public void Stack(Transform hitBlock)
        {
            if (_currentBlock == hitBlock)
                return;

            Block stackBlock = _blockPool.GetBlock();
            PutUnderPlayer(stackBlock);
            RaisePlayer();
            
            _player.Animator.Jump();
            stackBlock.transform.SetParent(_playerTransform);
            _stackedBlocks.Add(stackBlock);
        }
        
        public void RemoveBlock(Block block)
        {
            _stackedBlocks.Remove(block);
            _collidedBlocks.Add(block);
        }

        public void TryGetStackedBlocksDown() => 
            TryGetBlocksDown(_stackedBlocks);

        public void TryGetCollidedBlocksDown()
        {
            _collidedBlocks = _collidedBlocks.OrderBy(block => block.transform.position.y).Reverse().ToList();
            for (int i = 0; i < _collidedBlocks.Count; i++)
            {
                Debug.Log($"Block {i} {_collidedBlocks[i].transform.position.y}");
            }

            TryGetBlocksDown(_collidedBlocks);
        }

        public void DisableCollided()
        {
            foreach (var block in _collidedBlocks)
            {
                _coroutineRunner.StartCoroutine(DisableBlock(block));
            }            
            
            _collidedBlocks.Clear();
        }

        public void SetStackToDefault()
        {
            for (int i = 1; i < _stackedBlocks.Count; i++)
            {
                _stackedBlocks[1].Release();
                _stackedBlocks[1].transform.SetParent(null);
                _stackedBlocks.Remove(_stackedBlocks[1]);
            }
        }

        public void ImmediatelyDisableCollided()
        {
            foreach (var block in _collidedBlocks.Where(block => block.gameObject.activeSelf))
            {
                block.Release();
            }

            _collidedBlocks.Clear();
        }

        public List<Block> BlocksExceptHead()
        {
            var result = new List<Block>(_stackedBlocks);
            result.Remove(result[0]);

            return result;
        }

        private void TryGetBlocksDown(List<Block> blocks)
        {
            if (!IsRoomBelowBlock(blocks, out var fallDistance)) return;
            
            int movePosNum = 0;
            for (int blockNum = blocks.Count - 1; blockNum >= 0; blockNum--)
            {
                blocks[blockNum].transform.DOMoveY(_firstBlockPosY + _heightAboveLast * movePosNum, 
                    _getBlockDownSpeed * (fallDistance * 0.6f) * Time.deltaTime);
                movePosNum++;
            }
        }

        private bool IsRoomBelowBlock(List<Block> blocks, out float fallDistance)
        {
            fallDistance = blocks[0].transform.position.y - _firstBlockPosY;
            
            if (Math.Abs(fallDistance) > _comparisonTolerance)
                return true;
            
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                fallDistance = blocks[i + 1].transform.position.y - blocks[i].transform.position.y;
                if (fallDistance > _heightAboveLast)
                    return true;
            }

            return false;
        }

        private IEnumerator DisableBlock(Block block)
        {
            yield return new WaitForSecondsRealtime(_timeBeforeDisableBlock);

            block.Release();
        }

        private void PutUnderPlayer(Block stackBlock)
        {
            var highestBlockPos = _stackedBlocks[^1].transform.position;
            stackBlock.transform.position = highestBlockPos;
        }

        private void RaisePlayer()
        {
            var position = _playerTransform.position;
            position = new Vector3(position.x, position.y + _heightAboveLast, position.z);
            _playerTransform.position = position;
        }
    }
}