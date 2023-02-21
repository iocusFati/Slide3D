using System;
using Gameplay.Chunks.Spawning;
using Gameplay.PlayerFolder;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
    public interface IGameFactory : IService
    {
        event Action<Transform> OnPlayerCreated;
        event Action<Transform> OnLocationCreated;
        void CreateLocation();
        Player CreatePlayer(Vector3 at, ChunkSpawner chunkSpawner);
    }
}