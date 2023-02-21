using System;
using Infrastructure.AssetProvider;
using Infrastructure.States;
using UI.Data;
using UI.HUD;
using UnityEngine;

namespace UI.Services
{
    public class UIFactory : IUIFactory
    {
        private const int _hudRootOrder = 0;
        private const int _curtainRootOrder = 1;

        private readonly IAssets _assetProvider;
        private readonly UIStaticData _uiStaticData;
        private readonly GameStateMachine _gameStateMachine;

        private Transform _uiRoot;

        public event Action<HUD_Display> OnHUDCreated;

        public UIFactory(IAssets assetProvider, UIStaticData uiStaticData, GameStateMachine gameStateMachine)
        {
            _assetProvider = assetProvider;
            _uiStaticData = uiStaticData;
            _gameStateMachine = gameStateMachine;
        }

        public Curtain CreateCurtain()
        {
            var uiRoot = CreateUIRoot(_curtainRootOrder);
            var curtain = _assetProvider.Instantiate<Curtain>(AssetPaths.Curtain, uiRoot);
            curtain.Construct(_uiStaticData, uiRoot);
            
            return curtain;
        }
        

        public HUD_Display CreateHUD()
        {
            _uiRoot = CreateUIRoot(_hudRootOrder);
            
            var hudDisplay = _assetProvider.Instantiate<HUD_Display>(AssetPaths.UIHUD, _uiRoot);
            hudDisplay.Construct(_gameStateMachine);
            OnHUDCreated.Invoke(hudDisplay);
            
            return hudDisplay;
        }
        
        private Transform CreateUIRoot(int order)
        {
            var canvas = _assetProvider.Instantiate<Canvas>(AssetPaths.UIRoot);
            canvas.sortingOrder = order;

            return canvas.transform;
        }
    }
}