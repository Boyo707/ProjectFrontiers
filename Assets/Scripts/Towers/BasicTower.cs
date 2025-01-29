using UnityEngine;

public class BasicTower : TowerBase {

    [Header("Muzzle")]
    [SerializeField] private GameObject towerProjectile;
    public Transform firePoint;

    protected override void Shoot() 
    {
        if (currentTarget != null) {
            GameObject projectile = Instantiate(towerProjectile, firePoint.position, Quaternion.identity);
            projectile.GetComponent<ProjectileBase>().AssignValues(projectileOrigin.tower, partToRotate.rotation, stats.Damage);
        }
    }
}