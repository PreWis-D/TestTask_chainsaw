using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Chainsaw _chainsaw;

    private PlayerMovement _movement;
    private PLayerAnimator _animator;

    public void Init(JoystickVirtual joystickVirtual)
    {
        _movement = new PlayerMovement(this.transform, joystickVirtual);

        var animator = GetComponentInChildren<Animator>();
        _animator = new PLayerAnimator(_movement, animator);
    }

    private void Update()
    {
        _animator.Update();
    }

    private void FixedUpdate()
    {
        _movement.FixedUpdate();
    }
}
