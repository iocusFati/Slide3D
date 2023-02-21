using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Data
{
    [CreateAssetMenu(fileName = "UIData", menuName = "StaticData/UIData")]
    public class UIStaticData : ScriptableObject
    {
        [Header("Curtain")]
        public float ShowAnimationTime;

        [FormerlySerializedAs("ScaleScoreBy")] [Header("Quest")]
        public float ScaleScoreTo;
        public float ScaleScoreDuration;
    }
}