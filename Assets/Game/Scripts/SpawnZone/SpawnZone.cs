using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerExample
{
    public class SpawnZone : MonoBehaviour
    {
        [Header("Initialize")]
        [SerializeField] private Vector2Int _spawnZoneSize;
        [SerializeField] private Color _viewColor;
        [SerializeField] private int _step;
        [SerializeField] private SpawnPoint _spawnPointPrefab;
        [SerializeField] private Transform _pointsContainer;

        [Space(10)]
        [Header("Spawner")]
        [SerializeField] private Transform _spawnerTransform;
        [SerializeField] private EnemyType[] _enemyTypes;
        [SerializeField] private int _maxCount;
        [SerializeField] private float _cooldown;

        private const int _minValue = 1;

        private EnemiesConfig _enemyConfig;
        private float _offset = 0.5f;

        public Spawner Spawner { get; private set; }
        public List<SpawnPoint> SpawnPoints { get; private set; } = new List<SpawnPoint>();

        private void OnValidate()
        {
            if (_step <= 0)
                _step = _minValue;
        }

        public void Init(EnemiesConfig enemiesConfig)
        {
            _enemyConfig = enemiesConfig;

            Vector2 offset = new Vector2((_spawnZoneSize.x - _step) * _offset, (_spawnZoneSize.y - _step) * _offset);

            for (int y = 0; y < _spawnZoneSize.y; y += _step)
            {
                for (int x = 0; x < _spawnZoneSize.x; x += _step)
                {
                    SpawnPoint spawnPoint = Instantiate(_spawnPointPrefab);
                    spawnPoint.transform.SetParent(_pointsContainer, false);
                    spawnPoint.transform.localPosition = new Vector3(x - offset.x, y - offset.y, 0);
                    SpawnPoints.Add(spawnPoint);
                }
            }

            Spawner = new Spawner(SpawnPoints, _enemyConfig, _enemyTypes, _maxCount, _cooldown, _spawnerTransform);
            Spawner.StartSpawn();
        }

        private void OnDestroy()
        {
            Spawner.OnDestroy();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _viewColor;
            Gizmos.DrawCube(transform.position, new Vector3(_spawnZoneSize.x, _spawnZoneSize.y, 0));

            Vector2 position = new Vector2((_spawnZoneSize.x - _step) * _offset, (_spawnZoneSize.y - _step) * _offset);

            for (int y = 0; y < _spawnZoneSize.y; y += _step)
            {
                for (int x = 0; x < _spawnZoneSize.x; x += _step)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(transform.position + new Vector3(x - position.x, y - position.y, 0), 0.1f);
                }
            }
        }
#endif
    }
}