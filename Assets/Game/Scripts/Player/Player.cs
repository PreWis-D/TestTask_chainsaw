using Reflex.Attributes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Chainsaw _chainsaw;

    private PlayerMovement _movement;
    private PLayerAnimator _animator;
    private JoystickVirtual _joystickVirtual;

    [Inject]
    private void Construct(UIHandler uIHandler)
    {
        _joystickVirtual = uIHandler.JoystickVirtual;
    }

    public void Start()
    {
        _movement = new PlayerMovement(this.transform, _joystickVirtual);

        var animator = GetComponentInChildren<Animator>();
        _animator = new PLayerAnimator(_movement, animator);

        _chainsaw.Init(_joystickVirtual);
        _chainsaw.Activate();
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
