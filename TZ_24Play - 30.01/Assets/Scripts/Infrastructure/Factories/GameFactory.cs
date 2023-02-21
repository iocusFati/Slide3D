using System;
using Gameplay.CameraBehaviour;
using Gameplay.Chunks.Spawning;
using Gameplay.Chunks.Spawning.Blocks;
using Gameplay.PlayerFolder;
using Gameplay.PlayerFolder.Data;
using Infrastructure.AssetProvider;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assetProvider;
        private readonly PlayerMovement _playerMovement;
        private readonly ICameraEffects _cameraEffects;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly PlayerStaticData _playerStaticData;
        private readonly IBlockStacker _blockStacker;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly BlockPool _blockPool;

        private Player _player;
        private Transform _location;

        public event Action<Transform> OnLocationCreated;

        public event Action<Transform> OnPlayerCreated; 

        public GameFactory(
            IAssets assets,
            IGameStateMachine gameStateMachine,
            PlayerStaticData playerStaticData,
            PlayerMovement playerMovement,
            ICameraEffects cameraEffects,
            IBlockStacker blockStacker, 
            ICoroutineRunner coroutineRunner)
        {
            _blockStacker = blockStacker;
            _coroutineRunner = coroutineRunner;
            _assetProvider = assets;
            _gameStateMachine = gameStateMachine;
            _playerStaticData = playerStaticData;
            _playerMovement = playerMovement;
            _cameraEffects = cameraEffects;
        }

        public Player CreatePlayer(Vector3 at, ChunkSpawner chunkSpawner)
        {
            _player = _assetProvider.Instantiate<Player>(AssetPaths.PlayerPath, at);
            _player.MoveCollidedBlocks(_location);
            _blockStacker.Initialize(_player);
            var playerAnimator = _player.GetComponent<PlayerAnimator>();
            var blockStackText = new BlockStackText(_playerStaticData, _assetProvider);
            blockStackText.Initialize(_player.BlockStackText, _location);

            _player.Construct(
                chunkSpawner, playerAnimator, blockStackText,
                _blockStacker, _cameraEffects, _gameStateMachine, _playerStaticData);
            
            var playerTransform = _player.transform;
            _playerMovement.Player = playerTransform;

            OnPlayerCreated?.Invoke(playerTransform);

            return _player;
        }

        public void CreateLocation()
        {
            var location = new GameObject();

            _location = location.transform;
            _playerMovement.Location = _location;

            OnLocationCreated.Invoke(_location);
        }
    }
}