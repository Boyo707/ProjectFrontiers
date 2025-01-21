using System.Collections.Generic;
using UnityEngine;

public class TowerLogic : MonoBehaviour {
    private List<Tower> towers;

    protected int id = 0;
    
    private Tower tower;
    protected TowerUpgradeLevels CurrentUpgrades;

    private int currentHealth;
    private float shootCountDown = 0f;

    private GameObject target = null;
    private bool isLookingAtTarget = false;

    [Header("UnitySetUpFields")]
    public LayerMask layerMask;
    public Transform partToRotate;

    private void Start() {
        towers = DatabaseAcces.instance.database.Towers;

        try {
            tower = towers[id];
        }
        catch {
            Debug.LogError("error 404: Tower not found (Check Database or tower for id)");
            Destroy(this.gameObject);
            return;
        }

        currentHealth = tower.Stats.Health;
    }

    private void Update() {
        if (target == null) GetNewTarget();
        else if (Vector3.Distance(transform.position, target.transform.position) > tower.Stats.Range) GetNewTarget();

        if (target != null) {
            LookAtClosestTargetInRange();

            if (shootCountDown <= 0f && isLookingAtTarget) ShootHitScan();
        }
        if (shootCountDown > 0f) shootCountDown -= Time.deltaTime;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0f) Destroy(this.gameObject);
    }

    protected void ShootHitScan() {
        Debug.Log("Shot You instandly bitch");
        shootCountDown = 1f / tower.Stats.FireRate;

        target.GetComponent<EnemyLogic>().TakeDamage(tower.Stats.Damage);
    }

    protected void ShootProjectile() {
        Debug.Log("Shot Projectile MF");
        shootCountDown = 1f / tower.Stats.FireRate;
        //instansiate projectile
    }

    protected void LookAtClosestTargetInRange() {
        Vector3 dir = target.transform.position - partToRotate.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 15).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        Ray ray = new(partToRotate.transform.position, partToRotate.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, tower.Stats.Range)){
            if (hitInfo.collider.gameObject == target) isLookingAtTarget = true;
            else isLookingAtTarget = false;
        }
        else isLookingAtTarget = false;

        Debug.DrawRay(ray.origin, ray.direction * tower.Stats.Range, isLookingAtTarget ? Color.green : Color.red);
    }

    protected void GetNewTarget() {
        Collider[] colliders = Physics.OverlapSphere(
            partToRotate.transform.position,
            tower.Stats.Range,
            layerMask);

        //if no one in range set target to null
        if (colliders.Length == 0) {
            target = null;
            return;
        }

        float shotestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (Collider collider in colliders) {
            GameObject enemy = collider.gameObject;

            float distanceToEnemy = Vector3.Distance(partToRotate.transform.position, enemy.transform.position);
            if (distanceToEnemy < shotestDistance) {
                shotestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        target = nearestEnemy;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(partToRotate.transform.position, tower.Stats.Range);
    }
}
