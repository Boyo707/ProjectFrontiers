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

    public List<TowerUpgrades> GetTowerUpgrades(int towerId) {
        return database.TowerUpgrades.FindAll(upgrade => upgrade.SelectedTower == towerId);
    }
}
