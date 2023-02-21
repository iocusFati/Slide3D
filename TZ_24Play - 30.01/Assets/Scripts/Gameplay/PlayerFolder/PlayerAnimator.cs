using UnityEngine;

namespace Gameplay.PlayerFolder
{
    public class PlayerAnimator : MonoBehaviour
    {
        private const string JumpAnimationName = "Jump";
        
        [SerializeField] private Animator _animator;

        public void Jump()
        {
            _animator.SetTrigger(JumpAnimationName);
        }
    }
}