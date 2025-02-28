using UnityEngine;

namespace SpawnerExample
{
    public class SpawnPoint : MonoBehaviour
    {
        private CircleCollider2D _collider;
        private bool _isEmpty = true;
        private Enemy _spawnObject;

        public bool IsEmpty => _isEmpty;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ImpassableTerritory component))
                TakePosition();
        }

        public void TakePosition(Enemy character)
        {
            if (_spawnObject == null)
                _spawnObject = character;
        }

        public void TakePosition()
        {
            _isEmpty = false;
        }

        public void DropPosition()
        {
            _isEmpty = true;
        }
    }
}