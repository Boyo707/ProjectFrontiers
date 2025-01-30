using UnityEngine;

public enum projectileOrigin { tower, enemy }

public class ProjectileBase : MonoBehaviour
{
    //target of the projectile will be given by the shooter on instantiate

    //Target does not determine its range. The projectile will keep traveling

    //projectiles needs to be booled in the enemy or tower IF it starts lagging.

    [Header("Normal Options")]
    [SerializeField] protected Transform projectileTarget;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] private float lifeTime;

    protected Vector3 projectileTargetPosition;

    private float currentTime = 0;

    protected int projectileDamage;

    protected Vector3 startingPos;

    [SerializeField]protected projectileOrigin origin;
   
    protected Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;

        rb = GetComponent<Rigidbody>();

        if (lifeTime <= 0)
        {
            Debug.Log($"GameObject {gameObject.name} does not have a lifeTime");
        }
        else
        {
            currentTime = lifeTime;
        }

        if (projectileTarget != null)
        {

            Vector3 direction = (projectileTarget.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, direction);
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        CalculationAction();
        
        if (rb.linearVelocity.magnitude > projectileSpeed)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, projectileSpeed);
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            //life time ended
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        PhysicsAction();
    }

    //everything to do with physics or moving the projectile will be written in here.
    public virtual void PhysicsAction()
    {
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    //everything that needs to be calculated outside every frame will be written in here.
    public virtual void CalculationAction()
    {
        
    }

    public virtual void OnHit()
    {
        Destroy(gameObject);
    }


    //Assings the important values to the projectile
    //if the shooter wants to assign a target to rotate towards
    public void AssignValues(projectileOrigin origin, Transform target, int damage)
    {
        this.origin = origin;

        projectileDamage = damage;

        projectileTarget = target;

        if (projectileTarget != null)
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

    public void AssignValues(projectileOrigin origin, Vector3 location, int damage)
    {
        this.origin = origin;

        projectileDamage = damage;

        projectileTargetPosition = location;

        if (projectileTargetPosition != null)
        {

            Vector3 direction = (projectileTargetPosition - transform.position).normalized;
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

    //if the shooter wants to feed the projectile direction directly
    public void AssignValues(projectileOrigin origin, Quaternion rotation, int damage)
    {
        this.origin = origin;

        projectileDamage = damage;

        transform.rotation = rotation;
    }


    private void OnTriggerEnter(Collider other)
    {

        ///implement 2d sprites for enemies
        ///basic enemy mellee no projectile
       
        switch (origin)
        {
            case projectileOrigin.tower:
                EnemyBase enemyComponent = other.GetComponent<EnemyBase>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(projectileDamage);
                    OnHit();
                }
                break;

            case projectileOrigin.enemy:
                TowerBase towerComponent = other.GetComponent<TowerBase>();
                if (towerComponent != null)
                {
                    towerComponent.TakeDamage(projectileDamage);
                    OnHit();
                }
                break;
        }
    }

}
