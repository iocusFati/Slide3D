using System.Collections.Generic;
using Gameplay.CameraBehaviour;
using Gameplay.Chunks.Spawning;
using Gameplay.Chunks.Spawning.Blocks;
using Gameplay.PlayerFolder.Data;
using Infrastructure;
using Infrastructure.States;
using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public class Player : MonoBehaviour, ICoroutineRunner
    {
        private const string ChunkPassedTag = "ChunkPassed";
        private const string BlockTag = "Block";
        private const string WallTag = "Wall";
        private const string MainBlockTag = "MainBlock";

        public Block FirstBlock;
        public Transform PlayerBody;
        public Transform BlockStackText;

        [SerializeField] private Transform _collidedHolder;
        [SerializeField] private ParticleSystem _blockStackParticle;

        private ChunkSpawner _chunkSpawner;
        private IBlockStacker _blockStacker;
        private BlockStackText _blockStackText;
        private PlayerMovement _movement;
        private ICameraEffects _cameraEffects;
        private IGameStateMachine _gameStateMachine;
        private Collider _previousEnterColl;
        private Collider _previousExitColl;
        private Vector3 _firstBlockInitialLocalPos;
        private PlayerParticles _playerParticle;
        private bool _ignoreWallColl;

        public PlayerAnimator Animator { get; private set; }

        public void Construct(
            ChunkSpawner chunkSpawner,
            PlayerAnimator playerAnimator,
            BlockStackText blockStackText,
            IBlockStacker blockStacker,
            ICameraEffects cameraEffects,
            IGameStateMachine gameStateMachine,
            PlayerStaticData playerStaticData)
        {
            Animator = playerAnimator;
            _blockStackText = blockStackText;
            _chunkSpawner = chunkSpawner;
            _blockStacker = blockStacker;
            _cameraEffects = cameraEffects;
            _gameStateMachine = gameStateMachine;

            _firstBlockInitialLocalPos = playerStaticData.FirstBlockInitialLocalPos;

            _playerParticle = new PlayerParticles();
            _playerParticle.Initialize(_blockStackParticle);
        }

        public void DiscardExtraBlocks() => 
            _blockStacker.ImmediatelyDisableCollided();

        public void SetToInitialPos()
        {
            Transform firstBlock = FirstBlock.transform;
            firstBlock.SetParent(transform);
            firstBlock.localPosition = _firstBlockInitialLocalPos;
        }

        public void MoveCollidedBlocks(Transform location)
        {
            _collidedHolder.SetParent(location);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == _previousEnterColl)
                return;

            _previousEnterColl = other;
            
            if (other.CompareTag(ChunkPassedTag)) 
                _chunkSpawner.SpawnChunk(false);
            
            if (other.CompareTag(BlockTag))
            {
                _blockStacker.Stack(other.transform);
                _blockStackText.PlusOne();
                _playerParticle.DustBurst();
                other.gameObject.SetActive(false);
            }

            if (other.CompareTag(WallTag))
            {
                _blockStacker.DisableCollided();
            }  
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == _previousExitColl)
                return;
            
            if (other.CompareTag(WallTag))
            {
                _blockStacker.TryGetStackedBlocksDown();
                _previousExitColl = other;
                Debug.Log("OnTriggerExit");
            }        
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collisionGO = collision.gameObject;
            if (collisionGO.CompareTag(WallTag))
            {
                Collider blockCollider = collision.GetContact(0).thisCollider;
                blockCollider.transform.SetParent(_collidedHolder);
                _cameraEffects.StartShaking();

                if (blockCollider.CompareTag(MainBlockTag))
                {
                    // _blockStacker.DisableCollided();
                    List<Block> stackedBlocks = _blockStacker.BlocksExceptHead();
                    foreach (var stackedBlock in stackedBlocks)
                    {
                        stackedBlock.transform.SetParent(_collidedHolder);
                        _blockStacker.RemoveBlock(stackedBlock);
                    }
                    _gameStateMachine.Enter<GameLostState>();
                    // enabled = false;

                    return;
                }

                Block block = blockCollider.GetComponent<Block>();
                //_movement.DetachBlock(block);
                _blockStacker.RemoveBlock(block);
            }
        }    
    }
}