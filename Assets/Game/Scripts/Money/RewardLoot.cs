using DG.Tweening;
using UnityEngine;

public class RewardLoot : MonoBehaviour
{
    private float _radius = 2f;
    private bool _isActive;
    private Tween _tween;
    private Sequence _sequence;

    public int Reward { get; private set; }
    public MoneyType Type { get; private set; }

    public void Spawn(int reward, MoneyType moneyType)
    {
        Reward = reward;
        Type = moneyType;

        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(0f, 361f));

        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence
            .Append(transform.DOJump(transform.position + randomOffset, 0.5f, 1, 0.5f))
            .Join(transform.DOLocalRotate(randomRotation, 0.5f))
            .OnComplete(() => _isActive = true);
    }

    private void Update()
    {
        if (_isActive)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius);

            for (int i = 0; i < collider2Ds.Length; i++)
            {
                var player = collider2Ds[i].transform.GetComponent<Player>();
                if (player)
                {
                    MoveToPlayer(player);
                }
            }
        }
    }

    private void MoveToPlayer(Player player)
    {
        _isActive = false;
        _tween.Kill();
        _tween = transform.DOLocalMove(player.transform.position, 0.25f).SetEase(Ease.InBack);
        _tween.OnComplete(() =>
        {
            player.SetReward(this);
            PoolManager.SetPool(this);
        });
    }

    private void OnDestroy()
    {
        _tween.Kill();
        _sequence.Kill();
    }
}
