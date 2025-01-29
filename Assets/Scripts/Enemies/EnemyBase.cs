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
    private readonly float rangeOffset = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip hitAudio;

    protected virtual void Start() {
        try {
            targets = GridManager.instance.towersInGame;
            Enemy enemy = GameManager.instance.database.Enemies[id];

            name = enemy.Name;
            stats = new EnemyStats {
                Health = enemy.Stats.Health,
                Damage = enemy.Stats.Damage,
                Speed = enemy.Stats.Speed,
                FireRate = enemy.Stats.FireRate,
                Range = enemy.Stats.Range + rangeOffset,
                Experience = enemy.Stats.Experience
            };
        }
        catch {
            Debug.LogError("error 404: Enemie not found (Check Database or enemie for id)");
            Destroy(gameObject);
            return;
        }

        EventBus<TowerCreatedEvent>.OnEvent += CheckNewTarget;

        currentHealth = stats.Health;
    }

    protected virtual void Update() {
        if (currentTarget == null) {
            FindNewTarget();
            if (currentTarget == null) return;
        }

        if (!IsTargetInRange()) {
            MoveToTarget();
        } 
        else if (attackCooldown <= 0f) {
            Attack();
            AudioManager.instance.PlayOneShot(attackAudio, true);
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
        if (targets.Count == 0) return;

        float shortestDistanceSqr = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets) {
            float distanceSqr = (transform.position - target.transform.position).sqrMagnitude;
            if (distanceSqr < shortestDistanceSqr) {
                shortestDistanceSqr = distanceSqr;
                nearestTarget = target;
            }
        }
        
        currentTarget = nearestTarget.transform.Find("target_root")?.gameObject;
    }

    private void CheckNewTarget(TowerCreatedEvent e) {
        Invoke(nameof(FindNewTarget), 0.5f);
    }


    private void MoveToTarget() {
        Vector3 direction = (currentTarget.transform.position - transform.position);
        transform.Translate(Time.deltaTime * stats.Speed * direction.normalized);
    }

    protected abstract void Attack();

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        AudioManager.instance.PlayOneShot(hitAudio, true);
        if (currentHealth < 0) {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(this, id));
            Destroy(gameObject); 
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.Range);
    }

    private void OnDestroy() {
        EventBus<TowerCreatedEvent>.OnEvent -= CheckNewTarget;
    }
}
