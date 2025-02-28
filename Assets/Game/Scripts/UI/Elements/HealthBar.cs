using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _backFill;
    [SerializeField] private float _delay = 0.2f;

    private float _health;
    private CancellationTokenSource _cancellationTokenSource;

    private IDamagable _damagable;

    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();    
    }

    public void Init(IDamagable damagable)
    {
        _damagable = damagable;

        SetHealth(_damagable.CurrentHealth);

        Subscribe();

        FillAll();
    }

    private void Subscribe()
    {
        _damagable.DamageTaked += OnTakeDamage;
        _damagable.Died += OnDied;
    }

    private void Unsubscribe()
    {
        if (_damagable != null)
        {
            _damagable.DamageTaked -= OnTakeDamage;
            _damagable.Died -= OnDied;
        }
    }

    public void OnDied(IDamagable damagable)
    {
        Unsubscribe();
    }

    private void OnTakeDamage(float damage)
    {
        if (_fill.fillAmount >= 1)
            UnFade();

        ChangeHealth(-damage, false);
    }

    private void SetHealth(float health)
    {
        _health = health;

        Fade();
    }

    public void ChangeVisible(bool isVisible)
    {
        if (isVisible) UnFade();
        else Fade();
    }

    private void FillAll()
    {
        _fill.fillAmount = 1;
        _backFill.fillAmount = 1;
        Fade();
    }

    private void Fade()
    {
        _healthBar.gameObject.SetActive(false);
    }

    private void UnFade()
    {
        _healthBar.gameObject.SetActive(true);
    }

    private async void ChangeHealth(float value, bool debug)
    {
        var result = value / _health;

        _fill.fillAmount += result;

        await ChangeBackFill(value, _cancellationTokenSource.Token);
    }

    private async UniTask ChangeBackFill(float value, CancellationToken token)
    {
        await UniTask.Delay((int)(_delay * 1000), cancellationToken: token);

        if (_backFill != null && gameObject.activeSelf)
            _backFill.fillAmount += value / _health;

        if (_fill.fillAmount >= 1)
            Fade();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }
}