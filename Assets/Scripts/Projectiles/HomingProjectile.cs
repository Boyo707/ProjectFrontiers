using UnityEngine;

public class HomingProjectile : ProjectileBase
{
    public override void CalculationAction()
    {
        Vector3 direction = (projectileTarget.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, direction);
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = lookRotation;
    }
}
