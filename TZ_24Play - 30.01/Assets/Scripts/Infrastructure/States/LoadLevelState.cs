using System;
using Gameplay;
using Gameplay.CameraBehaviour;
using Gameplay.Chunks.Spawning;
using Gameplay.Chunks.Spawning.Pool;
using Gameplay.PlayerFolder;
using Infrastructure.AssetProvider;
using Infrastructure.Services.StaticDataService;
using UI;
using UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string WarpInitialPoint = "WarpInitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticData;
        private readonly ICameraEffects _cameraEffects;
        private readonly IAssets _assetProvider;
        private readonly IChunkPool _chunkPool;
        private readonly ITicker _ticker;
        private ICoroutineRunner _coroutineRunner;

        private CameraMovement _cameraMovement;
        private ChunkMovement _chunkMovement;
        private ChunkSpawner _chunkSpawner;
        private Curtain _curtain;
        private Player _player;
        private Vector3 _initialPoint;

        public LoadLevelState(
            GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            ICoroutineRunner coroutineRunner,
            ITicker ticker,
            IGameFactory gameFactory,
            IUIFactory uiFactory,
            IChunkPool chunkPool, 
            IAssets assets, 
            IStaticDataService staticDataService,
            ICameraEffects cameraEffects)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _coroutineRunner = coroutineRunner;
            _ticker = ticker;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _chunkPool = chunkPool;
            _assetProvider = assets;
            _staticData = staticDataService;
            _cameraEffects = cameraEffects;
        }
        public void Enter(string sceneName)
        {
            Action onAppeared;
            _curtain ??= _uiFactory.CreateCurtain();
            
            if (sceneName == SceneManager.GetActiveScene().name) 
                onAppeared = Reload;
            else
                onAppeared = () =>
                    _sceneLoader.Load(sceneName, OnLoaded);
            
            _curtain.Show(onAppeared);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private void OnLoaded()
        {
            _chunkPool.PreparePool();
            _chunkMovement = new ChunkMovement(_gameStateMachine, _gameFactory, _ticker, _staticData.ChunkData);
            _chunkSpawner = new ChunkSpawner(_chunkPool, _assetProvider, _gameFactory, _staticData.ChunkData);
            _gameFactory.CreateLocation();
            _uiFactory.CreateHUD();
            _chunkSpawner.SpawnFirstChunks();

            CreatePlayer();
            CreateWarpEffect();

            _cameraMovement = new CameraMovement(_ticker, _staticData.CameraData);
            _cameraMovement.Initialize(_player.FirstBlock.transform);
            _cameraEffects.Initialize();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void Reload()
        {
            _chunkSpawner.ReleaseAll();
            _chunkSpawner.SpawnFirstChunks();
            
            _player.SetToInitialPos();
            _player.DiscardExtraBlocks();
            _player.transform.position = GameObject.FindGameObjectWithTag(InitialPointTag).transform.position;
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void CreateWarpEffect()
        {
            var warpInitialPoint = GameObject.FindGameObjectWithTag(WarpInitialPoint).transform.position;
            _assetProvider.Instantiate<ParticleSystem>(AssetPaths.WarpEffect, warpInitialPoint);
        }

        private void CreatePlayer()
        {
            _initialPoint = GameObject.FindGameObjectWithTag(InitialPointTag).transform.position;
            _player = _gameFactory.CreatePlayer(_initialPoint, _chunkSpawner);
        }
    }
}