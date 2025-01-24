using System.Linq;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int explosionSize;
    
    private int damage;

    private float curveStartTime;

    SphereCollider sphereCollider;

    projectileOrigin currentOrigin;


    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        transform.GetChild(0).parent = null;    
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("HEEELP");

        Collider[] hitColliders = new Collider[30];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionSize, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            //distance for damage

            //hitColliders[i]
            EnemyBase enemyComponent = hitColliders[i].GetComponent<EnemyBase>();
            TowerBase towerComponent = hitColliders[i].GetComponent<TowerBase>();
            if (enemyComponent != null && currentOrigin == projectileOrigin.tower)
            {
                enemyComponent.TakeDamage(damage);
                //OnTriggerHit(other, enemyComponent);
            }
            else if (towerComponent != null && currentOrigin == projectileOrigin.enemy)
            {
                towerComponent.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"Object {hitColliders[i].gameObject.name} does not contain a enemy or tower base");
            }
        }

        Destroy(gameObject);
    }

    public void AssignVariables(int damage, projectileOrigin origin)
    {
        this.damage = damage;
        currentOrigin = origin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
    }
}
