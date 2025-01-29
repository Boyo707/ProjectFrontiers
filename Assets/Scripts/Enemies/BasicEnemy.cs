using UnityEngine;

public class BasicEnemy : EnemyBase {

    protected override void Attack() {
        if (currentTarget != null) {
            currentTarget.transform.parent?.GetComponent<TowerBase>()?.TakeDamage(stats.Damage);
        }
    }
}
