using Gameplay.CameraBehaviour.Data;
using Gameplay.Chunks.Data;
using Gameplay.PlayerFolder.Data;
using UI.Data;

namespace Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        ChunkStaticData ChunkData { get; }
        PlayerStaticData PlayerData { get; }
        CameraStaticData CameraData { get; }
        UIStaticData UIData { get; }
    }
}