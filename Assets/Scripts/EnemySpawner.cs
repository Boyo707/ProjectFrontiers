using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour {
    public float spawnRadius = 5f;
    public AnimationCurve spawnRate;
    private float spawnTimer = 0;

    [SerializeField]
    private int currentEnemiesAlive = 0;

    [SerializeField]
    private float gameTime = 0f;

    private void Start() {
        EventBus<EnemyKilledEvent>.OnEvent += EnemyDied;
    }

    private void Update() {
        gameTime += Time.deltaTime;

        if (spawnTimer <= 0) {
            int enemyId = GetEnemyIdBasedOnTime();

            InstantiateEnemy(enemyId);
            spawnTimer = 1f / spawnRate.Evaluate(gameTime);
        }
        else {
            spawnTimer -= Time.deltaTime;
        }
    }

    private int GetEnemyIdBasedOnTime() {
        return 0;
    }

    private void OnDestroy() {
        EventBus<EnemyKilledEvent>.OnEvent -= EnemyDied;
    }

    void InstantiateEnemy(int enemyId) {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;
        
        Vector3 position = new(x, 0f, y);

        Instantiate(DatabaseAcces.instance.GetEnemyById(enemyId).Prefab, position, Quaternion.identity);
        currentEnemiesAlive++;
        EventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemyId));
    }

    private void EnemyDied(EnemyKilledEvent e) => currentEnemiesAlive--;
}