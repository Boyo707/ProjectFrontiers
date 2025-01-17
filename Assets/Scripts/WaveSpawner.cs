using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner instance;

    [SerializeField]
    private int wave = 0;
    Wave waveData;
    private bool waveSpawning = false;

    public float spawnRadius = 5f;

    [SerializeField]
    private float gameTime = 0f;
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
        gameTime += Time.deltaTime;

        if (gameTime == 50f) return;

        if (currentEnemiesAlive > 20) return;
        if (!waveSpawning) {
            waveSpawning = true;
            waveData = DatabaseAcces.instance.GetWave(wave);
        }
        else {
            SpawnWave(waveData);
        }
    }

    private void SpawnWave(Wave waveData) {
        Vector3 position = SetSpawnPosition();

        Enemy enemy = DatabaseAcces.instance.GetEnemy(0);

        InstantiateEnemyAtPosition(enemy, position);
    }

    private Vector3 SetSpawnPosition() {
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