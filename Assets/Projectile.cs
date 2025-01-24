using UnityEngine;
using System.Collections.Generic;


public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform projectileTarget;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifeTime;

    private float currentTime = 0;

    private int projectileDamage;

    public enum projectileOrigin { tower, enemy }
    [HideInInspector] public projectileOrigin origin = projectileOrigin.tower;
   
    //private List<GameObject> booledProjectiles;

    private Rigidbody rb;

    //Projectile variants for tower and enemy will be devided with collision matrix - layers
    //target of the projectile will be given by the shooter on instantiate
    //potential booling of the projectiles
    //does not need a base script/inheritance


    //Target does not determine its range. The projectile will keep traveling
    //give projectile a life time incase it misses.
    //if requested to make it always hit its target, make it a lerp or slerp. Might make it harder.

    //projectiles needs to be booled in the enemy or tower IF it starts lagging.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = lifeTime;
        rb = GetComponent<Rigidbody>();

        if (projectileTarget == null)
        {

            Vector3 direction = (projectileTarget.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, direction);
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.linearVelocity.magnitude > projectileSpeed)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, projectileSpeed);
        }

        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            //life time ended
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //projectile moves towards the target overtime with rigidbody
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    public void AssignValues(projectileOrigin origin, Transform target, int damage)
    {
        projectileDamage = damage;

        projectileTarget = target;

        if (projectileTarget == null)
        {

            Vector3 direction = (projectileTarget.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, direction);
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;
            return;
        }
        else
        {
            Debug.LogError($"Target of projectile {gameObject.name} is NULL");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (origin)
        {
            case projectileOrigin.tower:
                other.GetComponent<EnemyBase>().TakeDamage(projectileDamage);
                break;

            case projectileOrigin.enemy:
                other.GetComponent<TowerBase>().TakeDamage(projectileDamage);
                break;
        }
        gameObject.SetActive(false);
        //on trigger enter. Apply damage and destroy/bool the projectile
    }

}
