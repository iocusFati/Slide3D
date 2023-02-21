using System;
using Gameplay.PlayerFolder;
using UI.HUD;
using UI.Services;

namespace Infrastructure.States
{
    public class GameLostState : IState
    {
        private HUD_Display _HUD;
        
        private readonly IBlockStacker _blockStacker;

        public GameLostState(IUIFactory uiFactory, IBlockStacker blockStacker)
        {
            _blockStacker = blockStacker;
            
            uiFactory.OnHUDCreated += display => _HUD = display;
        }

        public void Enter()
        {
            _HUD.ShowLose();
        }

        public void Exit()
        {
            _HUD.HideLose();
            _blockStacker.SetStackToDefault();
        }
    }
}