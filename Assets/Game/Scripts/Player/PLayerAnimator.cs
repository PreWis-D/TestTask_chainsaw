using System;
using UnityEditor;
using UnityEngine;

public class PLayerAnimator
{
    private readonly PlayerMovement _playerMovement;
    private readonly Animator _animator;
    private readonly int _hashIsMove = Animator.StringToHash("Move");

    private bool _isMoved;

    public PLayerAnimator(PlayerMovement playerMovement, Animator animator)
    {
        _playerMovement = playerMovement ?? throw new ArgumentNullException(nameof(playerMovement));
        _animator = animator ?? throw new ArgumentNullException(nameof(animator));
    }

    public void Update()
    {
        HandleMoveAnimation();
    }

    private void HandleMoveAnimation()
    {
        if (_isMoved == _playerMovement.IsMovePressed)
            return;

        _isMoved = _playerMovement.IsMovePressed;
        _animator.SetBool(_hashIsMove, _isMoved);
    }
}