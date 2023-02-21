using System.Collections.Generic;
using System.Linq;
using Gameplay.Chunks.Data;
using Infrastructure.AssetProvider;
using UnityEngine;

namespace Gameplay.Chunks.Spawning.Pool
{
    public class ChunkPool : IChunkPool
    {
        private readonly IAssets _assetProvider;
        
        private readonly int _numberOfChunksPerKind;
        
        private readonly List<Chunk> _chunkPool = new();


        public ChunkPool(IAssets assetProvider, ChunkStaticData chunkStaticData)
        {
            _assetProvider = assetProvider;

            _numberOfChunksPerKind = chunkStaticData.NumberOfChunksPerKind;
        }

        public void PreparePool()
        {
            foreach (var path in AssetPaths.ChunkPaths)
            {
                for (int i = 0; i < _numberOfChunksPerKind; i++)
                {
                    var chunk = _assetProvider.Instantiate<Chunk>(path);
                    _chunkPool.Add(chunk);
                }
            }

            foreach (var chunk in _chunkPool) 
                Release(chunk);
        }

        public void GetChunk(out Chunk chunk)
        {
            var inactiveChunks = GetInactive();
            var activeFoodLength = inactiveChunks.Length;

            if (inactiveChunks.Any())
            {
                chunk = inactiveChunks.ElementAt(Random.Range(0, activeFoodLength));
                chunk.gameObject.SetActive(true);
            }
            else 
                chunk = SpawnRandomChunk();
        }

        public void ReleaseAll()
        {
            var onlyActive = _chunkPool.Where(chunk => chunk.gameObject.activeSelf);
            foreach (var chunk in onlyActive) 
                Release(chunk);
        }

        public void Release(Chunk chunk) => 
            chunk.gameObject.SetActive(false);

        private Chunk SpawnRandomChunk()
        {
            var chunk = _assetProvider.Instantiate<Chunk>(GetRandomFoodPath());
            return chunk;
        }

        private static string GetRandomFoodPath() => 
            AssetPaths.ChunkPaths.ElementAt(Random.Range(0, AssetPaths.ChunkPaths.Length));

        private Chunk[] GetInactive() => 
            _chunkPool.Where(chunk => !chunk.gameObject.activeSelf).ToArray();
    }
}