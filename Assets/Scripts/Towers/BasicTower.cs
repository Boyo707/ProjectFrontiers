using UnityEngine;

public class BasicTower : TowerBase {
    public Transform firePoint;

    protected override void Shoot() {
        if (currentTarget != null) {
            //Instantiate(hitscanEffect, currentTarget.transform.position, Quaternion.identity);
            currentTarget.GetComponent<EnemyLogic>()?.TakeDamage(stats.Damage);
            Debug.Log($"{id} performed a hitscan shot!");
        }
        //if (towerData.projectilePrefab != null && firePoint != null) {
        //GameObject projectile = Instantiate(towerData.projectilePrefab, firePoint.position, firePoint.rotation);
        // Set projectile behavior (e.g., direction, damage)
        //Debug.Log($"{towerData.towerName} shot a projectile!");
        //}
    }
}