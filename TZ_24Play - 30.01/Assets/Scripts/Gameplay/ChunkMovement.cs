using Gameplay.Chunks.Data;
using Infrastructure;
using Infrastructure.States;
using UnityEngine;

namespace Gameplay
{
    public class ChunkMovement : ITickable
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly float _speed;
        
        private Transform _location;

        public ChunkMovement(IGameStateMachine stateMachine, IGameFactory gameFactory,
            ITicker ticker, ChunkStaticData chunkStaticData)
        {
            _stateMachine = stateMachine;

            _speed = chunkStaticData.MoveSpeed;
            
            ticker.AddTickable(this);
            gameFactory.OnLocationCreated += location => _location = location;
        }

        public void Tick()
        {
            if (!_stateMachine.IsInGameLoop)
                return;
            
            _location.Translate(Vector3.back * _speed);
        }
    }
}