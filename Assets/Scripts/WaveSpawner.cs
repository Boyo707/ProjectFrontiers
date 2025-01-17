using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy Spawner.
/// uses the database waves to know what, when and how many to spawn in each wave
/// it creates an struct to compare the amount it has spawned an need to spawn.
/// 
/// NOOT:
/// - remove struct an convert it into an list 
///     as you only need an list of amount of enemies you need to spawn
///     and the list of enemies you need to spawn the same order as the wavedata 
///     that contains the wave spawn enemies and id's
/// 
/// - update global buff so when the global buff changes you can update the local variable
/// - add different spawn logic for more differsity
/// </summary>

public struct EnemiesToSpawn {
    public int id;        
    public int amount;

    public EnemiesToSpawn(int id, int amount) {
        this.id = id;
        this.amount = amount;
    }
}

public class WaveSpawner : MonoBehaviour {
    public static WaveSpawner instance;

    private Wave waveData;
    private List<EnemiesToSpawn> enemySpawnList = new();

    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnTimer = 0f;

    [SerializeField] private int waveIndex = 0;

    private GlobalBuffTypes globalBuffs;

    public void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        EventBus<ChangeInGlobalBuffEvent>.OnEvent += UpdateGlobalBuff;
    }

    private void Update() {
        spawnTimer += Time.deltaTime;

        if (waveData == null) {
            GetWaveData(waveIndex);
            return;
        }

        if (spawnTimer >= spawnRate) {
            //get random enemy from enemies you need to spawn
            int randomEnemyIndex = Random.Range(0, enemySpawnList.Count);
            //get reference to enemyenty object
            EnemiesToSpawn randomEnemy = enemySpawnList[randomEnemyIndex];
            SpawnEnemy(randomEnemy.id);
            randomEnemy.amount--;

            // check if you spawned all enemies of that type
            if (randomEnemy.amount <= 0) {
                enemySpawnList.RemoveAt(randomEnemyIndex);

                // if no more enemies need to be spawned go to next wave
                if (enemySpawnList.Count <= 0) {
                    waveIndex++;
                    GetWaveData(waveIndex);
                    return;
                }
            } 
            else enemySpawnList[randomEnemyIndex] = randomEnemy;

            spawnTimer = 0;
        }
    }

    private void GetWaveData(int index) {
        waveData = DatabaseAcces.instance.GetWave(index);

        if (waveData == null) {
            Debug.Log("resetWaves event triggered");
            EventBus<ResetWavesEvent>.Publish(new ResetWavesEvent(this));

            waveIndex = 0;
            GetWaveData(waveIndex);
            return;
        }

        spawnRate = waveData.SpawnRate;
        foreach (EnemyEntry wave in waveData.EnemiesToSpawn) {
            Debug.Log(wave.amount);
            enemySpawnList.Add(new EnemiesToSpawn(wave.id, wave.amount));
        }

        EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(this, waveIndex));
    }

    private void SpawnEnemy(int id) {
        Vector3 position = SetRandomSpawnPosition();

        //get enemy from DB
        Enemy enemy = DatabaseAcces.instance.GetEnemy(id);

        //Instantiate enemy
        GameObject spawnedEnemy = Instantiate(enemy.Prefab, position, Quaternion.identity);
        spawnedEnemy.GetComponentInChildren<EnemyLogic>().buff = globalBuffs;
        EventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemy.Id));
    }

    private Vector3 SetRandomSpawnPosition() {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;

        return new(x, 0f, y);
    }

    private void UpdateGlobalBuff(ChangeInGlobalBuffEvent e) {
        Debug.Log("Update Global Buff evnt triggered in spawner");
    }

    private void OnDestroy() {
        EventBus<ChangeInGlobalBuffEvent>.OnEvent -= UpdateGlobalBuff;
    }
}