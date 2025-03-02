using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace SpawnerExample
{
    public class Spawner
    {
        private readonly List<SpawnPoint> _spawnPoints = new();
        private readonly EnemiesConfig _enemiesConfig;
        private readonly EnemyType[] _enemyTypes;
        private readonly int _maxCount;
        private readonly float _cooldown;
        private readonly Transform _self;

        private List<Enemy> _enemies = new();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isWork;

        public event UnityAction<Enemy> Spawned;
        public event UnityAction<Enemy> EnemyDied;

        public Spawner(List<SpawnPoint> spawnPoints, EnemiesConfig enemiesConfig, EnemyType[] enemyTypes, int maxCount, float cooldown, Transform self)
        {
            _spawnPoints = spawnPoints;
            _enemiesConfig = enemiesConfig;
            _enemyTypes = enemyTypes;
            _maxCount = maxCount;
            _cooldown = cooldown;
            _self = self;

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartSpawn()
        {
            _isWork = true;
            Spawn(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask Spawn(CancellationToken token)
        {
            while (_isWork)
            {
                await UniTask.Delay((int)(_cooldown * 1000), cancellationToken: token);

                if (_enemies.Count < _maxCount)
                {
                    int randomNumber = Random.Range(0, _spawnPoints.Count - 1);

                    if (_spawnPoints[randomNumber].IsEmpty)
                    {
                        var data = GetPrefabData();
                        Enemy enemy = PoolManager.GetPool(data.Prefab, _spawnPoints[randomNumber].transform.position);
                        enemy.transform.SetParent(_self);
                        enemy.Init(_spawnPoints, randomNumber, data);
                        _enemies.Add(enemy);
                        enemy.Died += OnEnemyDied;
                        Spawned?.Invoke(enemy);
                    }
                }
            }
        }

        private EnemyPrefabData GetPrefabData()
        {
            List<EnemyPrefabData> datas = new List<EnemyPrefabData>();

            for (int i = 0; i < _enemyTypes.Length; i++)
            {
                for (int j = 0; j < _enemiesConfig.EnemyPrefabDatas.Length; j++)
                {
                    if (_enemyTypes[i] == _enemiesConfig.EnemyPrefabDatas[j].Type)
                        datas.Add(_enemiesConfig.EnemyPrefabDatas[j]);
                }
            }

            int random = Random.Range(0, datas.Count);
            return datas[random];
        }

        private void OnEnemyDied(IDamagable damagable)
        {
            var enemy = damagable as Enemy;
            enemy.Died -= OnEnemyDied;
            _enemies.Remove(enemy);
            EnemyDied?.Invoke(enemy);
        }

        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}