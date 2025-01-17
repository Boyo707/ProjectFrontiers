using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner instance;

    [SerializeField]
    private int waveIndex = 0;
    private Wave waveData;
    private int currentEnemyIndex = 0;
    private int enemiesSpawned = 0;

    public float spawnRate = 1f;
    private float spawnTimer = 0f;
    public float spawnRadius = 5f;

    [SerializeField]
    private float gameTimer = 0f;
    private int currentEnemiesAlive = 0;

    public void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        EventBus<EnemyKilledEvent>.OnEvent += EnemyDied;
    }

    private void Update() {
        if (waveData == null) {
            waveData = DatabaseAcces.instance.GetWave(waveIndex);
        }

        gameTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // check end of wave
        if (currentEnemyIndex == waveData.EnemiesToSpawn.Count) {
            waveIndex++;
            waveData = DatabaseAcces.instance.GetWave(waveIndex);
            spawnRate = waveData.SpawnRate;
        }

        // spawn enemy at x rate
        if (spawnTimer >= spawnRate) {
            EnemyEntry entry = waveData.EnemiesToSpawn[currentEnemyIndex];

            if (enemiesSpawned < entry.amount) {
                SpawnEnemy(entry.id);
                enemiesSpawned++;

                spawnTimer = 0f;
            }
            else {
                currentEnemyIndex++;
                enemiesSpawned = 0;
            }
        }
    }

    private void SpawnEnemy(int id) {
        Vector3 position = SetRandomSpawnPosition();
        Enemy enemy = DatabaseAcces.instance.GetEnemy(id);
        InstantiateEnemyAtPosition(enemy, position);
    }

    private Vector3 SetRandomSpawnPosition() {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;

        return new(x, 0f, y);
    }

    void InstantiateEnemyAtPosition(Enemy enemy, Vector3 position) {
        Instantiate(enemy.Prefab, position, Quaternion.identity);
        EventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemy.Id));
        currentEnemiesAlive++;
    }

    private void EnemyDied(EnemyKilledEvent e) => currentEnemiesAlive--;

    private void OnDestroy() {
        EventBus<EnemyKilledEvent>.OnEvent -= EnemyDied;
    }
}