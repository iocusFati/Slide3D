using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.PlayerFolder.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StaticData/PlayerData")]
    public class PlayerStaticData : ScriptableObject
    {
        public float Speed;
        public float RightEdgeX;
        public float LeftEdgeX;
        [FormerlySerializedAs("PlayerInitialLocalPos")] public Vector3 FirstBlockInitialLocalPos;
        
        [Header("Blocks")]
        public float HeightAboveLast;
        public float TimeBeforeDisableBlock;
        public float GetBlockDownSpeed;
        
        [Header("StuckText")]
        public float RaiseBy;
        public float RaiseDuration;
        public float FadeDuration;
        public Color DefaultColor;
    }
}