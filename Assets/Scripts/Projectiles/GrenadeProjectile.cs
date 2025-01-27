using System;
using UnityEngine;

public class GrenadeProjectile : ProjectileBase
{
    [Header("Curve Options")]
    [SerializeField] private AnimationCurve projectileCurve;

    [SerializeField] private GameObject explosionPrefab;

    private float archPercentage;

    public override void CalculationAction()
    {
        if (archPercentage < 1)
        {
            archPercentage = Time.time / 10 * projectileSpeed;
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().AssignVariables(projectileDamage, origin);
            Destroy(gameObject);
        }
    }

    public override void PhysicsAction()
    {
        Vector3 normalSlerp = Vector3.Lerp(startingPos, projectileTarget.position, archPercentage);

        rb.position = new Vector3(normalSlerp.x, normalSlerp.y * projectileCurve.Evaluate(archPercentage), normalSlerp.z);
    }



}
