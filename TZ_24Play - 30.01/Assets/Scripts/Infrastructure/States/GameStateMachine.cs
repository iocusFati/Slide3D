using System;
using System.Collections.Generic;
using Gameplay.CameraBehaviour;
using Gameplay.Chunks.Spawning.Pool;
using Gameplay.PlayerFolder;
using Infrastructure.AssetProvider;
using Infrastructure.Services;
using Infrastructure.Services.StaticDataService;
using UI.Services;

namespace Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitState> _states;
        private IExitState _currentState;
        
        public bool IsInGameLoop => _currentState is GameLoopState; 

        public GameStateMachine(SceneLoader sceneLoader, 
            AllServices services,
            ICoroutineRunner coroutineRunner,
            ITicker ticker)
        { 

            _states = new Dictionary<Type, IExitState>
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this, sceneLoader, services, ticker, coroutineRunner),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this , sceneLoader, coroutineRunner, ticker, services.Single<IGameFactory>(), 
                    services.Single<IUIFactory>(), services.Single<IChunkPool>(), services.Single<IAssets>(),
                    services.Single<IStaticDataService>(), services.Single<ICameraEffects>()),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(GameLostState)] = new GameLostState(
                    services.Single<IUIFactory>(), services.Single<IBlockStacker>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState => 
            _states[typeof(TState)] as TState;
    }
}