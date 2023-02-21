using Gameplay.CameraBehaviour.Data;
using Gameplay.Chunks.Data;
using Gameplay.PlayerFolder.Data;
using Infrastructure.AssetProvider;
using UI.Data;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public UIStaticData UIData { get; private set; }
        public ChunkStaticData ChunkData { get; private set; }
        public PlayerStaticData PlayerData { get; private set; }
        public CameraStaticData CameraData { get; private set; }

        public void Initialize()
        {
            LoadChunkData();
            LoadPlayerData();
            LoadCameraData();
            LoadUIStaticData();
        }

        private void LoadChunkData() => 
            ChunkData = Resources.Load<ChunkStaticData>(AssetPaths.FoodData);

        private void LoadPlayerData() => 
            PlayerData = Resources.Load<PlayerStaticData>(AssetPaths.PlayerData);

        private void LoadCameraData() =>
            CameraData = Resources.Load<CameraStaticData>(AssetPaths.CameraData);
        
        private void LoadUIStaticData() =>
            UIData = Resources.Load<UIStaticData>(AssetPaths.UIData);
    }
}