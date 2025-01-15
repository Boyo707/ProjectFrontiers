using UnityEngine;

public class EnemyLogic : MonoBehaviour {
    public float speed = 10f;

    private Transform target;

    public int damage = 1;
    public int maxHealth = 1;
    public float amount = 0.1f;

    private int currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    void Start() {
        GetNextTarget();
    }

    void Update() {
        Vector3 dir = target.position - transform.position;
        transform.Translate(Time.deltaTime * speed * dir.normalized);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f) {
            GetNextTarget();
        }
    }
    
    private void GetNextTarget() {
        if (target != null)
            Destroy(gameObject);

        float closetTarget = Mathf.Infinity;
        Transform nearestTarget = null;

        foreach (Transform tower in TowerLocations.towers) {
            float distanceToTarget = Vector3.Distance(transform.position, tower.position);
            if (distanceToTarget < closetTarget) {
                closetTarget = distanceToTarget;
                nearestTarget = tower;
            }
        }

        if (nearestTarget == null) {
            Destroy(gameObject);
            return;
        }
        else
            target = nearestTarget;

        /*
        if (waypointIndex < TowerLocations.towers.Length - 1) {
            waypointIndex++;
            target = TowerLocations.towers[waypointIndex];
        }
        else {
            //target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        */
    }

    public void TakeDamage(int damage, float currencyEfficienty) {
        currentHealth -= damage;

        if (currentHealth < 0) {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(this));
            BuildManager.instance.AddCurrency(amount * currencyEfficienty);
            Destroy(gameObject); 
        }
    }
}
