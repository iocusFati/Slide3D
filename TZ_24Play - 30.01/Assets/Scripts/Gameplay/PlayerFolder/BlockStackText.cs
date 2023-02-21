using DG.Tweening;
using Gameplay.PlayerFolder.Data;
using Infrastructure.AssetProvider;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.PlayerFolder
{
    public class BlockStackText
    {
        private Transform _plusOneTextTransform;

        private readonly float _defaultLocalPosY;
        
        private Vector3 _initialPos;
        private Transform _location;

        private readonly float _raiseDuration;
        private readonly float _fadeDuration;
        private readonly float _raiseBy;
        private readonly Color _defaultColor;
        private readonly IAssets _assetProvider;

        private Coroutine _fadeCoroutine;

        private IObjectPool<TextMeshPro> _blockPool;

        private IObjectPool<TextMeshPro> Pool
        {
            get
            {
                return _blockPool ??= new ObjectPool<TextMeshPro>(
                    () => _assetProvider.
                        Instantiate<TextMeshPro>(AssetPaths.PlusOneText, _location),
                    block => { block.gameObject.SetActive(true); }, 
                    block => { block.gameObject.SetActive(false); },
                    block => { Object.Destroy(block.gameObject); });
            }
        }

        public BlockStackText(PlayerStaticData playerData, IAssets assetProvider)
        {
            _assetProvider = assetProvider;
            
            _raiseBy = playerData.RaiseBy;
            _raiseDuration = playerData.RaiseDuration;
            _fadeDuration = playerData.FadeDuration;
            _defaultColor = playerData.DefaultColor;
        }

        public void Initialize(Transform plusOneText, Transform location)
        {
            _plusOneTextTransform = plusOneText;
            _location = location;
        }
        
        public void PlusOne()
        {
            var plusOneText = Pool.Get();
            plusOneText.transform.position = _plusOneTextTransform.position;
            plusOneText.color = _defaultColor;
            plusOneText.gameObject.SetActive(true);
            LookAtCamera();

            var sequence = DOTween.Sequence();
            
            sequence
                .Append(plusOneText.transform
                .DOMoveY(plusOneText.transform.position.y + _raiseBy, _raiseDuration));
            sequence
                .Append(plusOneText.DOColor(Color.clear, _fadeDuration));
            sequence
                .OnComplete(() => Pool.Release(plusOneText));
        }

        private void LookAtCamera()
        {
            _plusOneTextTransform.transform.LookAt(Camera.main.transform);
            _plusOneTextTransform.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}