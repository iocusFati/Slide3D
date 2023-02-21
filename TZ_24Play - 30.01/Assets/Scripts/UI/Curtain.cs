using System;
using DG.Tweening;
using UI.Data;
using UnityEngine;

namespace UI
{
    public class Curtain : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private float _showAnimationTime;

        public void Construct(UIStaticData uiStaticData, Transform uiRoot)
        {
            _showAnimationTime = uiStaticData.ShowAnimationTime;

            DontDestroyOnLoad(uiRoot);
        }
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Show(Action onAppeared = null)
        {
            _rectTransform.DOScaleY(1, _showAnimationTime)
                .OnComplete(() => onAppeared?.Invoke());
        }

        public void Hide() => 
            _rectTransform.DOScaleY(0, _showAnimationTime);
    }
}