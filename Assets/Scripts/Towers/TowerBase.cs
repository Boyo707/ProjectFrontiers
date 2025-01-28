using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour {
    private List<Tower> towers;

    [SerializeField] protected TowerStats stats;
    public TowerUpgradeLevels CurrentUpgrades = new();

    public int currentHealth;
    protected float shootCooldown = 0f;

    protected GameObject currentTarget = null;
    protected bool isLookingAtTarget = false;

    private bool spawnProtection = true;
    private float spawnProtectionTimer = 3f;
    private float lifeTime = 0;

    [Header("Unity Setup")]
    public int id = -1;
    public LayerMask targetLayerMask;
    public Transform partToRotate;

    protected virtual void Start() {
        towers = GameManager.instance.database.Towers;
        
        try {
            stats = new TowerStats {
                Health = towers[id].Stats.Health,
                Damage = towers[id].Stats.Damage,
                FireRate = towers[id].Stats.FireRate,
                Range = towers[id].Stats.Range // times square (if useing sqrMag)
            };
        }
        catch {
            Debug.LogError("error 404: Tower not found (Check Database or tower for id)");
            Destroy(this.gameObject);
            return;
        }

        currentHealth = stats.Health;
    }

    protected virtual void Update() {
        if (currentTarget == null || !IsTargetInRange(currentTarget)) {
            FindNewTarget();
        }

        if (currentTarget != null) {
            RotateToTarget();

            if (shootCooldown <= 0f && isLookingAtTarget) {
                Shoot();
                shootCooldown = 1f / stats.FireRate;
            }
        }

        if (shootCooldown > 0f) {
            shootCooldown -= Time.deltaTime;
        }

        if (lifeTime > spawnProtectionTimer) spawnProtection = false;

        lifeTime += Time.deltaTime;
    }

    public void TakeDamage(int amount) {
        if (spawnProtection) return;

        //Debug.Log("Tower Took DAmage");
        currentHealth -= amount;

        if (currentHealth <= 0) {
            //Debug.Log("Tower Got desoyed!!!!!");
            //event tower destoryed for grid manager
            EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, gameObject));
            Destroy(this.gameObject);
        }
    }

    // Rotation with direct anglecheck instead of raycast
    protected void RotateToTarget() {
        Vector3 direction = (currentTarget.transform.position - partToRotate.position).normalized;
        float angleToTarget = Vector3.Angle(partToRotate.forward, direction);
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 10f);

        // Use Angle calculation 
        isLookingAtTarget = angleToTarget < 5f;

        /* // Use Raycast 
        Ray ray = new(partToRotate.position, partToRotate.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, stats.Range, targetLayerMask)) {
            isLookingAtTarget = hitInfo.collider.gameObject == currentTarget;
        }
        else {
            isLookingAtTarget = false;
        }

        Debug.DrawRay(ray.origin, ray.direction * stats.Range, isLookingAtTarget ? Color.green : Color.red);
        */
        Debug.DrawLine(partToRotate.position, currentTarget.transform.position, isLookingAtTarget ? Color.green : Color.red);
    }

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

    protected bool IsTargetInRange(GameObject target) {
        return Vector3.Distance(transform.position, target.transform.position) <= stats.Range;
    }

    public float GetHealthPercentage() {
        return currentHealth / stats.Health;
    }

    protected abstract void Shoot();

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(partToRotate.transform.position, stats.Range);
    }
}