using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseAcces : MonoBehaviour {
    public ObjectDatabase database;

    public static DatabaseAcces instance;

    private void OnEnable() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }


    public void Start() {
    }

    public Enemy GetEnemy(int id) {
        try {
            return database.Enemies[id];
        }
        catch {
            return null;
        }
    }

    public Tower GetTower(int id) {
        try {
            return database.Towers[id];
        }
        catch {
            return null;
        }
    }

    public Wave GetWave(int id) {
        try {
            return database.Waves[id];
        }
        catch {
            return null;
        }
    }

    public List<TowerUpgrades> GetTowerUpgrades(int towerId) {
        return database.TowerUpgrades.FindAll(upgrade => upgrade.SelectedTower == towerId);
    }
}
