using UnityEngine;

public class SniperTower : TowerBase {

    [Header("Sniper VFX")]
    [SerializeField] private GameObject muzzleFlashParticle;
    [SerializeField] private GameObject enemyHitParticle;

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
        if (currentTarget != null)
        {
            Instantiate(muzzleFlashParticle, firePoint.position, Quaternion.identity);
            Instantiate(enemyHitParticle, currentTarget.transform.position, Quaternion.identity);
            currentTarget.GetComponent<EnemyBase>().TakeDamage(stats.Damage);
        }
    }
    
}