using UnityEngine;

public class Turret : MonoBehaviour {

    private EnemyLogic target = null;
    private Transform targetLocation = null;

    [SerializeField] private ObjectDatabase database;
    [SerializeField] public int id = 0;

    TowerStats stats;

    private bool isLookingAtTarget = false;
    private float fireCountDown = 0f;
    private int currentHealth;

    [Header("UnitySetUpFields")]
    public LayerMask layerMask;
    public Transform partToRotate;

    private void Awake() {
        if (database.Towers[id] == null) {
            Debug.LogError("Bro je tower bestaat niet in de DB");
            return;
        }

        stats = database.Towers[id].Stats;
    }

    private void Start() {
        currentHealth = stats.MaxHealth;
    }

    private void Update() {
        if (target == null) GetNewTarget();
        else if (Vector3.Distance(transform.position, targetLocation.position) > stats.Range) GetNewTarget();

        if (target != null) {
            LookAtTarget();

            if (fireCountDown <= 0f && isLookingAtTarget) Shoot();
        }
        if (fireCountDown > 0f) fireCountDown -= Time.deltaTime;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth < 0f) Destroy(this.gameObject);
    }

    private void Shoot() {
        fireCountDown = 1f / stats.FireRate;
        target.TakeDamage(stats.Damage, stats.KillEfficienty);
    }

    private void LookAtTarget() {
        Vector3 dir = targetLocation.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * stats.TurnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        Ray ray = new Ray(partToRotate.transform.position, partToRotate.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, stats.Range)){
            //Debug.Log("Target hit: " + hitInfo.collider.gameObject);
            if (hitInfo.collider.gameObject == target.gameObject) isLookingAtTarget = true;
            else isLookingAtTarget = false;
        }
        else isLookingAtTarget = false;

        Debug.DrawRay(ray.origin, ray.direction * stats.Range, isLookingAtTarget ? Color.green : Color.red);
    }

    private void GetNewTarget() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.Range, layerMask);

        if (colliders.Length == 0) return;

        float shotestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (Collider collider in colliders) {
            GameObject enemy = collider.gameObject;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shotestDistance) {
                shotestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy == null) SetTarget(null);
        else SetTarget(nearestEnemy);

        void SetTarget(GameObject newTarget) {
            if (newTarget == null) {
                targetLocation = null;
                target = null;
            }
            else {
                targetLocation = newTarget.transform;
                target = newTarget.GetComponent<EnemyLogic>();
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.Range);
    }
}
