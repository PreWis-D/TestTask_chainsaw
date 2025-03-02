using Cinemachine;
using Cysharp.Threading.Tasks;
using SpawnerExample;
using System.Threading;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private float _timeToNormal = 0.25f;
    [SerializeField] private float _shakeValue = 1f;
    [SerializeField] private float _cooldown = 1f;

    private CinemachineBasicMultiChannelPerlin _channelPerlin;
    private CancellationTokenSource _cancellationTokenSource;
    private SpawnZone[] _spawnZones;
    private bool _isCanShaked = true;

    public void Init(Player player, LevelEnvironment level)
    {
        _channelPerlin = _playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cancellationTokenSource = new CancellationTokenSource();
        _playerCamera.m_Follow = player.transform;
        _spawnZones = level.SpawnZones;

        for (int i = 0; i < _spawnZones.Length; i++)
            _spawnZones[i].Spawner.EnemyDied += Shake;
    }

    public void Shake(Enemy enemy)
    {
        if (_isCanShaked)
            WorkShake(_cancellationTokenSource.Token).Forget();
    }

    private async UniTask WorkShake(CancellationToken token)
    {
        _isCanShaked = false;
        ChangeShakeValue(_shakeValue);

        await UniTask.Delay((int)(_timeToNormal * 1000), cancellationToken: token);

        ChangeShakeValue(0);

        await UniTask.Delay((int)(_cooldown * 1000), cancellationToken: token);

        _isCanShaked = true;
    }

    private void ChangeShakeValue(float value)
    {
        _channelPerlin.m_AmplitudeGain = value;
        _channelPerlin.m_FrequencyGain = value;
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();

        for (int i = 0; i < _spawnZones.Length; i++)
            _spawnZones[i].Spawner.EnemyDied -= Shake;
    }
}