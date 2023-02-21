using Gameplay.CameraBehaviour;
using Gameplay.Chunks.Spawning.Blocks;
using Gameplay.Chunks.Spawning.Pool;
using Gameplay.PlayerFolder;
using Infrastructure.AssetProvider;
using Infrastructure.Factories;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using UI.Services;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "Initial";
        private const string MainSceneName = "GameScene";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ITicker _ticker;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine gameStateMachine, 
            SceneLoader sceneLoader,
            AllServices services,
            ITicker ticker,
            ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _ticker = ticker;
            _coroutineRunner = coroutineRunner;

            RegisterServices(services);
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        private void RegisterServices(AllServices services)
        {
            var staticData = RegisterStaticDataService(services);
            var assets = services.RegisterService<IAssets>
                (new AssetProvider.AssetProvider());
            var inputService = services.RegisterService<IInputService>(
                new InputService());
            var cameraEffects = services.RegisterService<ICameraEffects>(
                new CameraEffects(_coroutineRunner, staticData.CameraData));
            var uiFactory = services.RegisterService<IUIFactory>(
                new UIFactory(assets, staticData.UIData, _gameStateMachine));

            var playerMovement = new PlayerMovement(
                inputService, _ticker, _gameStateMachine, staticData.PlayerData);
            var blockStacker = RegisterBlockStacker(
                services, assets, staticData);
            services.RegisterService<IGameFactory>(
                new GameFactory(assets, _gameStateMachine, staticData.PlayerData, 
                    playerMovement, cameraEffects, blockStacker, _coroutineRunner));
            
            RegisterPool(services, assets, staticData);
        }

        private IBlockStacker RegisterBlockStacker(AllServices services, IAssets assets, IStaticDataService staticData)
        {
            var blockPool = new BlockPool(assets);
            var blockStacker = services.RegisterService<IBlockStacker>(
                new BlockStacker(_coroutineRunner, blockPool, staticData.PlayerData));
            return blockStacker;
        }

        private static void RegisterPool(AllServices services, IAssets assets, IStaticDataService staticData)
        {
            var chunkPool = new ChunkPool(assets, staticData.ChunkData);
            services.RegisterService<IChunkPool>(
                chunkPool);
        }

        private static IStaticDataService RegisterStaticDataService(AllServices services)
        {
            var staticDataService = new StaticDataService();
            staticDataService.Initialize();
            
            return services.RegisterService<IStaticDataService>(staticDataService);
        }
    }
}