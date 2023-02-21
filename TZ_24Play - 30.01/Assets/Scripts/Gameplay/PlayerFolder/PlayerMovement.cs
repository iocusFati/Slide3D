using Gameplay.Chunks.Spawning.Blocks;
using Gameplay.PlayerFolder.Data;
using Infrastructure;
using Infrastructure.Services.Input;
using Infrastructure.States;
using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public class PlayerMovement : ITickable
    {
        private readonly IInputService _inputService;
        private readonly IGameStateMachine _gameStateMachine;
        
        private readonly float _speed;
        private readonly float _rightEdgeX;
        private readonly float _leftEdgeX;
        private BlockDirections _blockDirection = BlockDirections.None;

        public Transform Player {private get; set;}
        public Transform Location {private get; set;}

        public PlayerMovement(IInputService inputService,
            ITicker ticker,
            IGameStateMachine gameStateMachine,
            PlayerStaticData playerStaticData)
        {
            _inputService = inputService;
            _gameStateMachine = gameStateMachine;

            _speed = playerStaticData.Speed;
            _rightEdgeX = playerStaticData.RightEdgeX;
            _leftEdgeX = playerStaticData.LeftEdgeX;

            ticker.AddTickable(this);
        }

        public void Tick()
        {
            if (!_gameStateMachine.IsInGameLoop)
                return;

            MoveSide();
        }

        public void DetachBlock(Block block) => 
            block.transform.SetParent(Location);

        private void MoveSide()
        {
            
            float movement = _inputService.GetMovement();
            Vector3 playerPos = Player.position;
            Vector3 hypotheticalDisplacement = 
                playerPos + new Vector3(movement * _speed * Time.deltaTime, 0, 0);
            
            switch (_blockDirection)
            {
                case BlockDirections.None:
                    if (hypotheticalDisplacement.x > _rightEdgeX)
                    {
                        Player.position += new Vector3(_rightEdgeX - playerPos.x, 0, 0);
                        _blockDirection = BlockDirections.Right;
                        
                        return;
                    }

                    if (hypotheticalDisplacement.x < _leftEdgeX)
                    {
                        Player.position += new Vector3(_leftEdgeX - playerPos.x, 0, 0);
                        _blockDirection = BlockDirections.Left;
                        
                        return;
                    }

                    break;
                case BlockDirections.Right:
                    if (movement > 0)
                        hypotheticalDisplacement = new Vector3(_rightEdgeX, hypotheticalDisplacement.y, hypotheticalDisplacement.z);
                    else if (movement < 0)
                        _blockDirection = BlockDirections.None;
                    
                    break;
                case BlockDirections.Left:
                    if (movement < 0)
                        hypotheticalDisplacement = new Vector3(_leftEdgeX, hypotheticalDisplacement.y, hypotheticalDisplacement.z);
                    else if (movement > 0)
                        _blockDirection = BlockDirections.None;
                    
                    break;
            }
            
            Player.position = hypotheticalDisplacement;
        }
    }
}