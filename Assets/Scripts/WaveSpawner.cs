using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    private Transform spawnPoint;

    public int spawnRate = 10;
    private float spawnTimer = 0;
    public float radius = 5f;

    private void Start() {
        spawnPoint = TowerLocations.towers[0];
    }

    private void Update() {
        if (spawnTimer <= 0) {
            SpawnEnemy();
            spawnTimer = 1f / spawnRate;
        }
        else {
            spawnTimer -= Time.deltaTime;
        }
    }

    void SpawnEnemy() {
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate the x and y positions using the random angle on the outer edge
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Set y = 0 to keep the objects on the XY plane
        Vector3 position = new Vector3(x, 0f, y); // Using z for 3D (you can adjust if you need 2D instead)


        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
