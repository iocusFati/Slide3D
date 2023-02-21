using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public class PlayerParticles
    {
        private ParticleSystem _blockStack;

        public void Initialize(ParticleSystem blockStack)
        {
            _blockStack = blockStack;
        }

        public void DustBurst()
        {
            _blockStack.Play();
        }
    }
}