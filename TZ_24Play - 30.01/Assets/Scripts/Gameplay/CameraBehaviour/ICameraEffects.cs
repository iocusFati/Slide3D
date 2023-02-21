using Infrastructure.Services;

namespace Gameplay.CameraBehaviour
{
    public interface ICameraEffects : IService
    {
        void Initialize();
        void StartShaking();
    }
}