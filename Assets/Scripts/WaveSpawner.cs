using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Spawner.
/// uses the database waves to know what, when and how many to spawn in each wave
/// it creates an struct to compare the amount it has spawned an need to spawn.
/// 
/// </summary>

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner instance;

    private List<Enemy> enemies;
    private List<Wave> waves;

    private Wave currentWave;
    private Queue<int> enemyQueue = new();

    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float spawnRadius = 5f;
    private float spawnTimer = 0f;

    [SerializeField] private int waveIndex = 0;
    private GlobalBuffTypes globalBuffs;

    private void OnEnable() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start() {
        EventBus<ChangeInGlobalBuffEvent>.OnEvent += UpdateGlobalBuff;

        enemies = DatabaseAcces.instance.database.Enemies;
        waves = DatabaseAcces.instance.database.Waves;

        GetWaveData(waveIndex);
    }

    private void Update() {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnRate) {
            int enemiesToSpawn = Mathf.FloorToInt(spawnTimer/ spawnRate);

            for (int i = 0; i < enemiesToSpawn; i++) {
                if (enemyQueue.Count <= 0) {
                    waveIndex++;
                    GetWaveData(waveIndex);
                }

                int enemyId = enemyQueue.Dequeue();
                SpawnEnemy(enemyId);
            }

            spawnTimer = 0f;
        }
    }

    private void GetWaveData(int index) {
        if (index == waves.Count - 1) {
            Debug.Log("resetWaves event triggered");

            EventBus<ResetWavesEvent>.Publish(new ResetWavesEvent(this));
            waveIndex = 0;
        }

        currentWave = waves[index];
        spawnRate = currentWave.SpawnRate;

        List<int>  enemyList = new();
        foreach (EnemyEntry entry in currentWave.EnemiesToSpawn) {
            for (int i = 0; i < entry.amount; i++) {
                enemyList.Add(entry.id);
            }
        }
        Shuffle(enemyList);

        enemyQueue.Clear();
        foreach (int enemyId in enemyList) {
            enemyQueue.Enqueue(enemyId);
        }
    }

    private void Shuffle<T>(List<T> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    private void SpawnEnemy(int id) {
        Vector3 position = SetRandomSpawnPosition();

        Enemy enemy = enemies[id];

        GameObject spawnedEnemy = Instantiate(enemy.Prefab, position, Quaternion.identity);

        spawnedEnemy.GetComponentInChildren<EnemyLogic>().buff = globalBuffs;

        EventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemy.Id));
    }

    private Vector3 SetRandomSpawnPosition() {
        float minAngle = -Mathf.PI / 4;
        float maxAngle = Mathf.PI / 4;

        float angle = Random.Range(minAngle, maxAngle);
        float x = Mathf.Tan(angle) * spawnRadius;

        return new Vector3(x, 1f, spawnRadius);
    }

    private void UpdateGlobalBuff(ChangeInGlobalBuffEvent e) {
        Debug.Log("Update Global Buff evnt triggered in spawner");
    }

    private void OnDestroy() {
        EventBus<ChangeInGlobalBuffEvent>.OnEvent -= UpdateGlobalBuff;
    }
}