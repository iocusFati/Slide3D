using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.CameraBehaviour.Data
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "StaticData/CameraData")]
    public class CameraStaticData : ScriptableObject
    {
        public float AlignWithPlayerTime;
        public float OffsetFromPlayer;
        
        [Header("Shaking")]
        public float ShakeTime;
        public Vector3 ShakeShiftMin;
        public float ShakeMinMagnitude;
        public AnimationCurve ShakeStrengthCurve;
    }
}