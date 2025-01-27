using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyLogic : MonoBehaviour {
    public float speed = 10f;

    private Transform target;

    private int id = 0;

    public GlobalBuffTypes buff;


    public int damage = 1;
    public int maxHealth = 1;
    public float amount = 0.1f;
    float difficultyValue;

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

    //use thsi for finding nearest tower if it enters range use trigger enter/ collisions (speherecast) smart man beer sayd so
    /*
    protected void FindNewTarget() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.Range, targetLayerMask);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (Collider collider in colliders) {
            //change to sqrMag (better perform)
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestEnemy = collider.gameObject;
            }
        }

        currentTarget = nearestEnemy;
    }
    */

    public void TakeDamage(int damage) {
        currentHealth -= damage; 

        if (currentHealth < 0) {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(this, id));
            EventBus<ChangeInCurrencyEvent>.Publish(new ChangeInCurrencyEvent(this, amount * difficultyValue ));
            BuildManager.instance.AddCurrency(amount);
            Destroy(gameObject); 
        }
    }
}
