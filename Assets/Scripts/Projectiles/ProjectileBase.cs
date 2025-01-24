using UnityEngine;

public enum projectileOrigin { tower, enemy }

public abstract class ProjectileBase : MonoBehaviour
{
    [Header("Normal Options")]
    [SerializeField] protected Transform projectileTarget;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] private float lifeTime;

    private bool hasALifeTime = true;

    private float currentTime = 0;

    protected int projectileDamage;

    protected Vector3 startingPos;

    [HideInInspector] public projectileOrigin origin = projectileOrigin.tower;
   
    protected Rigidbody rb;

    //Projectile variants for tower and enemy will be devided with collision matrix - layers
    //target of the projectile will be given by the shooter on instantiate
    //potential booling of the projectiles
    //does not need a base script/inheritance


    //Target does not determine its range. The projectile will keep traveling

    //projectiles needs to be booled in the enemy or tower IF it starts lagging.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;

        if (lifeTime <= 0)
        {
            hasALifeTime = false;
            Debug.Log($"GameObject {gameObject.name} does not have a lifeTime");
        }
        else
        {
            currentTime = lifeTime;
        }
        rb = GetComponent<Rigidbody>();
        Debug.Log(rb);

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

        if (hasALifeTime)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                //life time ended
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        PhysicsAction();
    }

    public virtual void PhysicsAction()
    {
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    public virtual void CalculationAction()
    {
        
    }

    public virtual void OnTriggerHit(Collider other)
    {
        //component.SendMessage("TakeDamage", projectileDamage);
        //gameObject.SetActive(false);
        //on trigger enter. Apply damage and destroy/bool the projectile

    }

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

    public void AssignValues(projectileOrigin origin, Quaternion rotation, int damage)
    {
        this.origin = origin;

        projectileDamage = damage;

        transform.rotation = rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (origin)
        {
            case projectileOrigin.tower:
                EnemyBase enemyComponent = other.GetComponent<EnemyBase>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(projectileDamage);
                    //OnTriggerHit(other, enemyComponent);
                }
                break;

            case projectileOrigin.enemy:
                TowerBase towerComponent = other.GetComponent<TowerBase>();
                if (towerComponent != null)
                {
                    towerComponent.TakeDamage(projectileDamage);
                    //OnTriggerHit(other, towerComponent);
                }
                break;
        }
    }

}
