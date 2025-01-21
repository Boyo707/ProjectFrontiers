using UnityEngine;

public class SniperTower : TowerBase {
    //public Transform firePoint;
    //public GameObject hitscanEffectPrefab;

    protected override void Shoot() {
        if (currentTarget != null) {
            //Instantiate(hitscanEffect, currentTarget.transform.position, Quaternion.identity);
            currentTarget.GetComponent<EnemyBase>()?.TakeDamage(stats.Damage);
            Debug.Log($"{id} performed a hitscan shot!");
        }
    }
}