using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject enemyProjectile;

    protected override void Attack()
    {
        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileBase>().AssignValues(projectileOrigin.enemy, currentTarget.transform, stats.Damage);
    }

}
