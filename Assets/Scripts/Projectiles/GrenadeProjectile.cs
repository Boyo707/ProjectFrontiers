using System;
using UnityEngine;

public class GrenadeProjectile : ProjectileBase
{
    [Header("Curve Options")]
    [SerializeField] private AnimationCurve projectileCurve;

    [SerializeField] private GameObject explosionChild;
    [SerializeField] private float explosionRadius;

    private float archPercentage;

    public override void CalculationAction()
    {
        if (archPercentage < 1)
        {
            archPercentage = Time.time / 10 * projectileSpeed;
        }
        else
        {
            InstantiateExplosion();
            Destroy(gameObject);
        }
    }

    public override void PhysicsAction()
    {
        Vector3 normalSlerp = Vector3.Lerp(startingPos, projectileTarget.position, archPercentage);

        rb.position = new Vector3(normalSlerp.x, normalSlerp.y * projectileCurve.Evaluate(archPercentage), normalSlerp.z);
    }

    public override void OnHit()
    {
        InstantiateExplosion();
        Destroy(gameObject);
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
