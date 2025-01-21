using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    /*
    [SerializeField] private int Gold = 5;
    [SerializeField] private int Lives = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        EventBus<HealthChangedEvent>.Publish(new HealthChangedEvent(this, Lives));
        EventBus<GoldChangedEvent>.Publish(new GoldChangedEvent(this, Gold));
    }

    private void Awake() {
        EventBus<EnemyReachedEndEvent>.OnEvent += OnEnemyReachedEnd;
        EventBus<TowerPurchaseEvent>.OnEvent += OnTowerPurcase;

    }

    private void OnTowerPurcase(TowerPurchaseEvent e) {
        if (e.TowerData.cost > Gold) {
            Debug.LogError("Something went wrong, tower cost is higher than gold available.");
        }
        Gold -= e.TowerData.cost;
        EventBus<GoldChangedEvent>.Publish(new GoldChangedEvent(this, Gold));
    }

    private void OnDestroy() {
        EventBus<EnemyReachedEndEvent>.OnEvent -= OnEnemyReachedEnd;
    }

    private void OnEnemyReachedEnd(EnemyReachedEndEvent e) {
        Lives -= e.enemyData.Damage;
        EventBus<HealthChangedEvent>.Publish(new HealthChangedEvent(this, Lives));
        if (Lives <= 0) {
            EventBus<GameOverEvent>.Publish(new GameOverEvent(this));
        }
    }

    // Update is called once per frame
    void Update() {

    }*/
}
