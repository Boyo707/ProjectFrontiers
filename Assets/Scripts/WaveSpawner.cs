using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner instance;

    [SerializeField]
    private int wave = 0;
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

        if (!waveSpawning) SpawnWave();
    }

    private void SpawnWave() {
        waveSpawning = true;
        Wave waveData = DatabaseAcces.instance.GetWave(wave);

        StartCoroutine(SpawnWaveCoroutine(waveData));
    }

    private IEnumerator SpawnWaveCoroutine(Wave waveData) {
        float spawnRate = waveData.SpawnRate;
        int enemyTypes = waveData.EnemiesToSpawn.Count - 1;

        EnemyEntry enemiesToSpawn;
        enemiesToSpawn.amount = 0;

        int enemyToSpawn;

        bool enemiestospawn = true;

        float timeElapsed = 0f;
        int enemyid = 0;

        while (true) {
            float angle = Random.Range(0f, Mathf.PI * 2);

            float x = Mathf.Cos(angle) * spawnRadius;
            float y = Mathf.Sin(angle) * spawnRadius;

            Vector3 position = new(x, 0f, y);


            Enemy enemy = DatabaseAcces.instance.GetEnemy(enemyid);

            InstantiateEnemyAtPosition(enemy, position);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
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