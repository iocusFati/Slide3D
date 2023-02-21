using Infrastructure.Services;

namespace Gameplay.Chunks.Spawning.Pool
{
    public interface IChunkPool : IService
    {
        void GetChunk(out Chunk chunk);
        void ReleaseAll();
        void Release(Chunk chunk);
        void PreparePool();
    }
}