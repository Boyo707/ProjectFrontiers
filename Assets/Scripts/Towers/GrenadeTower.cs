using UnityEngine;

public class GrenadeTower: TowerBase {

    [Header("Muzzle")]
    [SerializeField] private GameObject towerProjectile;
    public Transform firePoint;

    protected override void FindNewTarget()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.Range, targetLayerMask);

        float farthestDistance = 0;
        GameObject farthestEnemy = null;

        foreach (Collider collider in colliders)
        {
            //change to sqrMag (better perform)
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestEnemy = collider.gameObject;
            }
        }

        currentTarget = farthestEnemy;
    }

    protected override void Shoot() 
    {
        if (currentTarget != null) {
            GameObject projectile = Instantiate(towerProjectile, firePoint.position, Quaternion.identity);
            Debug.Log(currentTarget.transform);
            projectile.GetComponent<ProjectileBase>().AssignValues(projectileOrigin.tower, currentTarget.transform.position, stats.Damage);
        }
    }
}