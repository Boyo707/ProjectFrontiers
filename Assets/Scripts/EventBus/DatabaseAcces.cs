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

    public Enemy GetEnemyById(int enemyid) {
        return database.Enemies[enemyid];
    }

    public Enemy GetEnemyByDifficulty(int difficulty) {
        Enemy[] enemies = new Enemy[0];

        foreach (Enemy enemy in database.Enemies) {
            if (enemy.Difficulty == difficulty) {
                enemies.Append(enemy);
            }
        }

        //add randomiser to wich enemy it return on that difficulty

        return enemies[0];
    }
}
