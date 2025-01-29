using UnityEngine;

public class BasicEnemy : EnemyBase {

    public Transform firePoint;

    protected override void Attack() {
        if (currentTarget != null) {
            currentTarget.transform.parent?.GetComponent<TowerBase>()?.TakeDamage(stats.Damage);
        }
    }
}
