using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseAcces : MonoBehaviour {
    public ObjectDatabase database;

    public static DatabaseAcces instance;

    public void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public Enemy GetEnemy(int enemyid) {
        return database.Enemies[enemyid];
    }

    public Tower GetTowerById(int towerId) {
        return database.Towers[towerId];
    }

    public Wave GetWave(int wave) {
        return database.Waves[wave];
    }

    public List<TowerUpgrades> GetTowerUpgrades(int towerId) {
        return database.TowerUpgrades.FindAll(upgrade => upgrade.SelectedTower == towerId);
    }

    public Enemy GetEnemyByDifficulty(int difficulty) {
        Enemy[] enemies = new Enemy[0];

        foreach (Enemy enemy in database.Enemies) {
            if (enemy.DifficultyValue == difficulty) {
                enemies.Append(enemy);
            }
        }

        //add randomiser to wich enemy it return on that difficulty

        return enemies[0];
    }

    public float GetEnemyValue(int id) => database.Enemies[id].DifficultyValue;
}
