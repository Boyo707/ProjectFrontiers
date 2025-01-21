using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {
    [SerializeField] protected new string name;
    [SerializeField] protected EnemyStats stats;

    private int currentHealth;
    protected float attackCooldown = 0f;
    [SerializeField] protected List<GameObject> targets;
    protected GameObject currentTarget = null;

    public GlobalBuffTypes buff = new();

    [Header("Unity Setup")]
    public int id = -1;
    [SerializeField] protected LayerMask targetLayerMask;

    protected virtual void Start() {
        try {
            targets = DatabaseAcces.instance.towersInGame;
            Enemy enemy = DatabaseAcces.instance.database.Enemies[id];

            name = enemy.Name;
            stats = new EnemyStats {
                Health = enemy.Stats.Health,
                Damage = enemy.Stats.Damage,
                Speed = enemy.Stats.Speed,
                FireRate = enemy.Stats.FireRate,
                Range = enemy.Stats.Range,
                Experience = enemy.Stats.Experience
            };
        }
        catch {
            Debug.LogError("error 404: Enemie not found (Check Database or enemie for id)");
            Destroy(gameObject);
            return;
        }

        currentHealth = stats.Health;
    }

    protected virtual void Update() {
        if (currentTarget == null) {
            FindNewTarget();
        }
        if (currentTarget == null) return;

        MoveToTarget();

        if (IsTargetInRange() && attackCooldown <= 0f) {
            Attack();
            attackCooldown = 1f / stats.FireRate;
        }


        if (attackCooldown > 0f) {
            attackCooldown -= Time.deltaTime;
        }
    }

    private bool IsTargetInRange() {
        return Vector3.Distance(transform.position, currentTarget.transform.position) <= stats.Range;
    }

    protected void FindNewTarget() {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets) {
            //change to sqrMag (better perform)
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }

        currentTarget = nearestTarget;
    }

    private void MoveToTarget() {
        Vector3 direction = (currentTarget.transform.position - transform.position);
        transform.Translate(Time.deltaTime * stats.Speed * direction.normalized);
    }

    protected abstract void Attack();

    public void TakeDamage(int damage) {
        currentHealth -= damage; 

        if (currentHealth < 0) {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(this, id));
            Destroy(gameObject); 
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.Range);
    }
}
