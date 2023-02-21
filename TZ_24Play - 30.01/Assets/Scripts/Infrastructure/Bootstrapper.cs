using Infrastructure.Services;
using Infrastructure.States;

namespace Infrastructure
{
    public class Bootstrapper : Ticker, ICoroutineRunner
    {
        private void Awake()
        {
            SceneLoader sceneLoader = new SceneLoader(this);
            GameStateMachine gameStateMachine = new GameStateMachine(sceneLoader, AllServices.Container, this, this);
            
            gameStateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
