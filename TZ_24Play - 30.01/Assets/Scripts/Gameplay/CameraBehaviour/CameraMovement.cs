using Gameplay.CameraBehaviour.Data;
using Infrastructure;
using UnityEngine;

namespace Gameplay.CameraBehaviour
{
    public class CameraMovement : ITickable
    {
        private const string MainCameraTag = "MainCamera";
        
        private readonly ITicker _ticker;
        private readonly float _offsetFromPlayer;
        
        private Transform _camera;
        private Transform _playerFirstBlock;

        public CameraMovement(ITicker ticker, CameraStaticData cameraStaticData)
        {
            _ticker = ticker;
            _offsetFromPlayer = cameraStaticData.OffsetFromPlayer;
        }

        public void Initialize(Transform playerFirstBlock)
        {
            _playerFirstBlock = playerFirstBlock;
            _camera = GameObject.FindWithTag(MainCameraTag).transform;
            _ticker.AddTickable(this);
        }

        public void Tick()
        {
            var camPos = _camera.position;
            camPos = new Vector3(camPos.x, camPos.y, _playerFirstBlock.position.z - _offsetFromPlayer);
            _camera.position = camPos;
        }
    }
}