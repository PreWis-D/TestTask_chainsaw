using Reflex.Attributes;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Chainsaw _chainsaw;

    private PlayerMovement _movement;
    private PLayerAnimator _animator;

    public void Init(UIHandler uIHandler, PlayerConfig playerConfig)
    {
        var trail = PoolManager.GetPool(playerConfig.TrailPrefab, transform.position);
        _movement = new PlayerMovement(this.transform, uIHandler.JoystickVirtual, trail, playerConfig);

        var animator = GetComponentInChildren<Animator>();
        _animator = new PLayerAnimator(_movement, animator);

        _chainsaw.Init(uIHandler.JoystickVirtual);
        _chainsaw.Activate();
    }

    public void SetReward(RewardLoot rewardLoot)
    {
        Debug.Log($"get {rewardLoot.Type} count: {rewardLoot.Reward}");
    }

    private void Update()
    {
        _movement.TurnResistance(_chainsaw.Damagables.Count > 0);    
        _animator.Update();
    }

    private void FixedUpdate()
    {
        _movement.FixedUpdate();
    }
}