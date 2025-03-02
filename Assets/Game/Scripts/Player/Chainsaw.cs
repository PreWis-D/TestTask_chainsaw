using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    [SerializeField] public Transform _model;
    [SerializeField] public ParticleSystem[] _sparks;

    private JoystickVirtual _joystick;
    private Animator _animator;
    private readonly int _hashIsWork = Animator.StringToHash("Work");
    private float _damage = 1;
    private float _colldown = 0.25f;
    private bool _isWork;
    private CancellationTokenSource _cancellationTokenSource;

    public List<IDamagable> Damagables { get; private set; } = new List<IDamagable>();

    public void Init(JoystickVirtual joystickVirtual)
    {
        _joystick = joystickVirtual;
        _animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        _isWork = true;
        _animator.SetBool(_hashIsWork, _isWork);
        _cancellationTokenSource = new CancellationTokenSource();
        Work(_cancellationTokenSource.Token).Forget();
    }

    public void Deactivate()
    {
        _isWork = false;
        _animator.SetBool(_hashIsWork, _isWork);
        _cancellationTokenSource.Cancel();
    }

    public void FixedUpdate()
    {
        HandleDirection();
    }

    private void HandleDirection()
    {
        var direction = new Vector3(_joystick.Direction.x, _joystick.Direction.y, 0) * Time.deltaTime;

        if (direction == Vector3.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = new Vector3(direction.x < 0 ? 180 : 0, 0, 0);

        transform.rotation = Quaternion.Euler(0, 0, angle);
        _model.localEulerAngles = rotation;
    }

    private async UniTask Work(CancellationToken token)
    {
        while (_isWork)
        {
            TryCauseDamage();

            await UniTask.Delay((int)(_colldown * 1000), cancellationToken: token);
        }
    }

    private void TryCauseDamage()
    {
        List<IDamagable> damagables = new List<IDamagable>();

        foreach (var item in Damagables)
            damagables.Add(item);

        for (int i = 0; i < _sparks.Length; i++)
            _sparks[i].gameObject.SetActive(damagables.Count > 0);

        if (damagables.Count > 0)
        {
            foreach (var damagable in damagables)
                damagable.TakeDamage(_damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
            Add(damagable);
    }

    private void Add(IDamagable damagable)
    {
        if (CheckList(damagable) == false)
        {
            Damagables.Add(damagable);
            damagable.Died += Remove;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
            Remove(damagable);
    }

    private void Remove(IDamagable damagable)
    {
        if (CheckList(damagable))
        {
            damagable.Died -= Remove;
            Damagables.Remove(damagable);
        }
    }

    private bool CheckList(IDamagable damagable)
    {
        foreach (var item in Damagables)
            if (item == damagable)
                return true;

        return false;
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}