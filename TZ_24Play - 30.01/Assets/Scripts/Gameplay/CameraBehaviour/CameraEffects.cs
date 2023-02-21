using System.Collections;
using Gameplay.CameraBehaviour.Data;
using Infrastructure;
using UnityEngine;

namespace Gameplay.CameraBehaviour
{
    public class CameraEffects : ICameraEffects
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AnimationCurve _shakeStrengthCurve;
        private readonly Vector3 _shakeMinShift;
        private readonly float _shakeMinMagnitude;
        private readonly float _shakeTime;

        private Transform _camera;

        public CameraEffects(ICoroutineRunner coroutineRunner, CameraStaticData cameraStaticData)
        {
            _coroutineRunner = coroutineRunner;

            _shakeTime = cameraStaticData.ShakeTime;
            _shakeMinMagnitude = cameraStaticData.ShakeMinMagnitude;
            _shakeMinShift = cameraStaticData.ShakeShiftMin;
            _shakeStrengthCurve = cameraStaticData.ShakeStrengthCurve;
        }

        public void Initialize()
        {
            _camera = Camera.main.transform;
        }

        public void StartShaking() => 
            _coroutineRunner.StartCoroutine(Shake());

        private IEnumerator Shake()
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakePos = shakePos.magnitude < _shakeMinMagnitude ? _shakeMinShift : shakePos;
            Vector3 startPos = _camera.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < _shakeTime)
            {
                elapsedTime += Time.deltaTime;
                float shakeStrength = _shakeStrengthCurve.Evaluate(elapsedTime / _shakeTime);
                shakePos *= -1;
                _camera.localPosition = startPos + shakePos * shakeStrength;

                yield return null;
            }

            _camera.localPosition = Vector3.zero;
        }
    }
}