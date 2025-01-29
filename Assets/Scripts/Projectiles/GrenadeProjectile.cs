using System;
using UnityEngine;

public class GrenadeProjectile : ProjectileBase
{
    [Header("ExplosionOptions")]
    [SerializeField] private GameObject explosionChild;
    [SerializeField] private float explosionRadius;

    [Header("Curve Options")]
    [SerializeField] private AnimationCurve projectileCurve;

    private float archPercentage;

    private float currentProjectileTime;

    public override void CalculationAction()
    {
        if (archPercentage < 1)
        {
            Debug.Log("weeeeeee");
            currentProjectileTime += Time.deltaTime;
            archPercentage = currentProjectileTime / 10 * projectileSpeed;
        }
        else
        {
            InstantiateExplosion();
            Destroy(gameObject);
        }

    }

    public override void PhysicsAction()
    {
        if (projectileTargetPosition != null)
        {
            Vector3 normalSlerp = Vector3.Lerp(startingPos, projectileTargetPosition, archPercentage);

            rb.position = new Vector3(normalSlerp.x, normalSlerp.y * projectileCurve.Evaluate(archPercentage), normalSlerp.z);
        }
    }

    public override void OnHit()
    {
        Debug.Log("I GOT TRIGGERD");
    }

    private void InstantiateExplosion()
    {
        explosionChild.GetComponent<Explosion>().AssignVariables(origin, projectileDamage, explosionRadius);
        explosionChild.transform.parent = null;
        explosionChild.transform.position = transform.position;
        explosionChild.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
