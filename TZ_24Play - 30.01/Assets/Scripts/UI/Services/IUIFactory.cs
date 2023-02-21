using System;
using Infrastructure.Services;
using UI.HUD;

namespace UI.Services
{
    public interface IUIFactory : IService
    {
        event Action<HUD_Display> OnHUDCreated;
        HUD_Display CreateHUD();
        Curtain CreateCurtain();
    }
}