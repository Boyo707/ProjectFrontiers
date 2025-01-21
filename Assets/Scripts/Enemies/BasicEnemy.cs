using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BasicEnemy : EnemyBase {

    public Transform firePoint;

    protected override void Attack() {
        if (currentTarget != null) {
            //Instantiate(hitscanEffect, currentTarget.transform.position, Quaternion.identity);
            currentTarget.GetComponent<TowerBase>()?.TakeDamage(stats.Damage);
            Debug.Log($"Enemy {id} performed a hitscan shot!");
        }
        //if (towerData.projectilePrefab != null && firePoint != null) {
        //GameObject projectile = Instantiate(towerData.projectilePrefab, firePoint.position, firePoint.rotation);
        // Set projectile behavior (e.g., direction, damage)
        //Debug.Log($"{towerData.towerName} shot a projectile!");
        //}
    }
}
