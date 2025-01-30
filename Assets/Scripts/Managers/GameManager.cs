using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public ObjectDatabase database;
    private int currency = 100;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        EventBus<EnemyKilledEvent>.OnEvent += GetEnemyValue;
    }

    private void GetEnemyValue(EnemyKilledEvent e) {
        int value = (int)database.Enemies[e.enemyId].DifficultyValue;
        AddCurrency(value);
    }

    public int GetCurrency() {
        return currency;
    }

    public bool CanAfford(int cost) {
        return currency >= cost;
    }

    public void AddCurrency(int amount) {
        currency += amount;
    }

    public void RemoveCurrency(int amount) {
        if (CanAfford(amount)) currency -= amount;
    }

    private void OnDestroy() {
        EventBus<EnemyKilledEvent>.OnEvent -= GetEnemyValue;
    }
}
