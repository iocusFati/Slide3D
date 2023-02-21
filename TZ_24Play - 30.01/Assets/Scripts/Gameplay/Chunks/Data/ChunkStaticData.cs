using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Chunks.Data
{
    [CreateAssetMenu(fileName = "ChunkData", menuName = "StaticData/ChunkData")]
    public class ChunkStaticData : ScriptableObject
    {
        [Header("Spawning")] 
        public int NumberOfChunksPerKind;
        public int MaxChunksNumber;
        public int FirstTimeSpawnNumber;
        public float SpawnBelowMainLine;
        public float RaiseToMainLineDuration;


        [Header("Properties")]
        public float MoveSpeed;
    }
}