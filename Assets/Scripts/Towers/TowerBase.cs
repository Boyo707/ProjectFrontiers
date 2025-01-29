using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour {
    private List<Tower> towers;

    public TowerStats stats;
    public TowerUpgradeLevels lastUpgrades;
    public TowerUpgradeLevels currentUpgrades;

    public int currentHealth;
    protected float shootCooldown = 0f;

    protected GameObject currentTarget = null;
    protected bool isLookingAtTarget = false;
    protected bool isBuffTower = false;

    private bool spawnProtection = true;
    private float spawnProtectionTimer = 3f;
    private float lifeTime = 0;

    [Header("Unity Setup")]
    public int id = -1;
    public LayerMask targetLayerMask;
    public Transform partToRotate;
    public float rotationSpeed;

    protected virtual void Start() {
        towers = GameManager.instance.database.Towers;
        
        try {
            stats = new TowerStats {
                Health = towers[id].Stats.Health,
                Damage = towers[id].Stats.Damage,
                FireRate = towers[id].Stats.FireRate,
                Range = towers[id].Stats.Range // times square (if useing sqrMag)
            };

            currentUpgrades = new TowerUpgradeLevels {
                Health = 1,
                Damage = 1,
                FireRate = 1,
                Range = 1
            };

            lastUpgrades = new TowerUpgradeLevels {
                Health = 1,
                Damage = 1,
                FireRate = 1,
                Range = 1
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
        if (lifeTime > spawnProtectionTimer) {
            spawnProtection = false;
        }
        lifeTime += Time.deltaTime;

        if (currentTarget == null || !IsTargetInRange(currentTarget)) {
            FindNewTarget();
        }

        if (currentTarget != null) {
            RotateToTarget();

            if (shootCooldown <= 0f) {
                if (isBuffTower) {
                    ApplyEffectToTowersInRange();
                    shootCooldown = 1f / stats.FireRate;
                }

                if (isLookingAtTarget) {
                    Shoot();
                    shootCooldown = 1f / stats.FireRate;
                }
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

    public void HealTower(int amount) {
        //Debug.Log("Tower Took DAmage");
        currentHealth += amount;

        if (currentHealth > stats.Health) {
            currentHealth = stats.Health;
        }
    }
    // Rotation with direct anglecheck instead of raycast
    protected void RotateToTarget() {
        Vector3 direction = (currentTarget.transform.position - partToRotate.position).normalized;
        float angleToTarget = Vector3.Angle(partToRotate.forward, direction);
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed);

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

    protected virtual void FindNewTarget() {
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

    protected virtual void ApplyEffectToTowersInRange() {
    }

    protected List<TowerBase> GetTowersInRange() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.Range, targetLayerMask);
        List<TowerBase> towersInRange = new();

        foreach (Collider collider in colliders) {
            TowerBase tower = collider.GetComponentInParent<TowerBase>();
            if (tower != null && tower != this) {
                towersInRange.Add(tower);
            }
        }

        return towersInRange;
    }
    public void UpdateStats() {
        if (currentUpgrades.Health != lastUpgrades.Health) {
            stats.Health += (currentUpgrades.Health - lastUpgrades.Health) * 10; // Example multiplier
            lastUpgrades.Health = currentUpgrades.Health;
        }
        if (currentUpgrades.Damage != lastUpgrades.Damage) {
            stats.Damage += (currentUpgrades.Damage - lastUpgrades.Damage) * 5; // Example multiplier
            lastUpgrades.Damage = currentUpgrades.Damage;
        }
        if (currentUpgrades.FireRate != lastUpgrades.FireRate) {
            stats.FireRate += (currentUpgrades.FireRate - lastUpgrades.FireRate) * 0.1f; // Example multiplier
            lastUpgrades.FireRate = currentUpgrades.FireRate;
        }
        if (currentUpgrades.Range != lastUpgrades.Range) {
            stats.Range += (currentUpgrades.Range - lastUpgrades.Range) * 1f; // Example multiplier
            lastUpgrades.Range = currentUpgrades.Range;
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(partToRotate.transform.position, stats.Range);
    }
}