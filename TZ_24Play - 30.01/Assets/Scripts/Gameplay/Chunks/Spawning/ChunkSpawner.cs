using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Chunks.Data;
using Gameplay.Chunks.Spawning.Pool;
using Infrastructure;
using Infrastructure.AssetProvider;
using UnityEngine;

namespace Gameplay.Chunks.Spawning
{
    public class ChunkSpawner
    {
        private readonly IChunkPool _chunkPool;
        private readonly IAssets _assetProvider;

        private readonly int _maxChunksNumber;
        private readonly int _firstTimeSpawnNumber;
        private readonly float _raiseToMainLineDuration;
        private readonly float _spawnBelowMainLine;

        private readonly Queue<Chunk> _spawnedChunks = new();

        private Chunk _firstChunk;
        private Transform _location;
        private Chunk _blankChunk;
        private float _mainLinePosY;

        public ChunkSpawner(
            IChunkPool chunkPool, 
            IAssets assets,
            IGameFactory gameFactory,
            ChunkStaticData chunkStaticData)
        {
            _chunkPool = chunkPool;
            _assetProvider = assets;

            _maxChunksNumber = chunkStaticData.MaxChunksNumber;
            _firstTimeSpawnNumber = chunkStaticData.FirstTimeSpawnNumber;
            _raiseToMainLineDuration = chunkStaticData.RaiseToMainLineDuration;
            _spawnBelowMainLine = chunkStaticData.SpawnBelowMainLine;

            gameFactory.OnLocationCreated += location => _location = location;
        }

        public void SpawnFirstChunks()
        {
            GetBlankChunk();

            for (int i = 0; i < _firstTimeSpawnNumber; i++) 
                SpawnChunk(true);
        }

        public void SpawnChunk(bool firstChunk)
        {
            _chunkPool.GetChunk(out var chunk);
            
            chunk.Start.position = _firstChunk.End.position;

            if (!firstChunk) 
                AlignWithMainLine(chunk);

            chunk.transform.SetParent(_location);
            chunk.ActivatePickUps();
            
            _spawnedChunks.Enqueue(chunk);
            _firstChunk = chunk;

            if (_spawnedChunks.Count > _maxChunksNumber)
            {
                var last = _spawnedChunks.Dequeue();
                last.transform.SetParent(null);
                _chunkPool.Release(last);
            }
        }

        private void AlignWithMainLine(Chunk chunk)
        {
            var startTransform = chunk.Start.transform;
            var startPosition = startTransform.position;
            startTransform.position = new Vector3(
                startPosition.x, startPosition.y - _spawnBelowMainLine, startPosition.z);
            startTransform.DOMoveY(_mainLinePosY, _raiseToMainLineDuration);
        }

        private void GetBlankChunk()
        {
            if (_blankChunk is null)
                SpawnBlankChunk();
            else
            {
                _blankChunk.gameObject.SetActive(true);
                _firstChunk = _blankChunk;
                _spawnedChunks.Enqueue(_firstChunk);
            }
        }

        private void SpawnBlankChunk()
        {
            _firstChunk = _assetProvider.Instantiate<Chunk>(AssetPaths.BlankChunk, Vector3.zero);
            _blankChunk = _firstChunk;
            _mainLinePosY = _blankChunk.End.position.y;
            _spawnedChunks.Enqueue(_firstChunk);
            _firstChunk.transform.SetParent(_location);
        }

        public void ReleaseAll()
        {
            _chunkPool.ReleaseAll();
            _blankChunk.gameObject.SetActive(false);
            _spawnedChunks.Clear();
        }
    }
}