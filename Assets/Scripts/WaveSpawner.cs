using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    //public GameObject enemyPrefab;

    private Transform spawnPoint;

    private int enemyId = 0;

    public int spawnRate = 10;
    private float spawnTimer = 0;
    public float radius = 5f;

    private void Start() {
        spawnPoint = TowerLocations.towers[0];
    }

    private void Update() {
        if (spawnTimer <= 0) {
            //EventBus<RequestDataEvent<Enemies>>.Publish(new RequestDataEvent<Enemies>(this, InstantiateEnemy, enemyId));
            spawnTimer = 1f / spawnRate;
        }
        else {
            spawnTimer -= Time.deltaTime;
        }
    }

    void InstantiateEnemy(int id /*Enemies enemy*/) {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        Vector3 position = new Vector3(x, 0f, y);
        Instantiate(DatabaseAcces.GetEnemyObject(enemyId).Prefab, position, Quaternion.identity);
        //eventBus<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(this, enemyId));
    }
}

/*
class UIManager {
    awake() {
        EventBus<EnemySpawnedEvent>.OnEvent += OnEnemySpawned;
    }

    OnDestroy() {
        EventBus<EnemySpawnedEvent>.OnEvent -= OnEnemySpawned;
    }

    public void OnEnemySpawned(EnemySpawnedEvent e) {
        EnemiesAlive++;
        //set text
    }
}
*/