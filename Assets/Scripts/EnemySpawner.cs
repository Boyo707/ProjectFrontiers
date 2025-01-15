using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour {
    public float spawnRadius = 5f;
    public int spawnRate = 10;
    private float spawnTimer = 0;

    [SerializeField]
    private int currentEnemiesAlive = 0;
    [SerializeField]
    private int minEnemiesAlive = 10;

    [SerializeField]
    private float gameTime = 0f;

    private void Start() {
        EventBus<EnemyKilledEvent>.OnEvent += EnemyDied;
    }

    private void Update() {
        gameTime += Time.deltaTime;

        if (spawnTimer <= 0 || currentEnemiesAlive < minEnemiesAlive) {
            int enemyId = GetEnemyIdBasedOnTimeOrWave(gameTime);

            InstantiateEnemy(enemyId);
            //EventBus<RequestDataEvent<Enemy>>.Publish(new RequestDataEvent<Enemy>(this, InstantiateEnemy, enemyId));
            spawnTimer = 1f / spawnRate;
        }
        else {
            spawnTimer -= Time.deltaTime;
        }
    }

    private int GetEnemyIdBasedOnTimeOrWave(float gameTime) {
        if (gameTime < 30f) {
            return 0;
        }
        else if (gameTime < 60f) {
            minEnemiesAlive = 20;
            return 1;
        }
        else if (gameTime < 120f) {
            minEnemiesAlive = 30;
            return 2;
        }
        else {
            minEnemiesAlive = 50;
            return 3;
        }
    }

    private void OnDestroy() {
        EventBus<EnemyKilledEvent>.OnEvent -= EnemyDied;
    }

    void InstantiateEnemy(int enemyId) {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;

        Vector3 position = new Vector3(x, 0f, y);

        Instantiate(DatabaseAcces.instance.GetEnemyById(enemyId).Prefab, position, Quaternion.identity);
        currentEnemiesAlive++;
        EventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemyId));
    }

    private void EnemyDied(EnemyKilledEvent e) => currentEnemiesAlive--;
}