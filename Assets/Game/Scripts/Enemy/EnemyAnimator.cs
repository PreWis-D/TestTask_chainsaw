using UnityEditor;
using UnityEngine;

namespace Assets.Game.Scripts.Enemy
{
    public class EnemyAnimator
    {
        private readonly Animator _animator;
        private readonly int _hashIsMove = Animator.StringToHash("Move");
        private readonly int _hashIsPanic = Animator.StringToHash("Panic");

        public EnemyAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void Idle()
        {
            _animator.SetBool(_hashIsMove, false);
            _animator.SetBool(_hashIsPanic, false);
        }

        public void Move()
        {
            _animator.SetBool(_hashIsMove, true);
        }

        public void Panic()
        {
            _animator.SetBool(_hashIsPanic, true);
        }
    }
}