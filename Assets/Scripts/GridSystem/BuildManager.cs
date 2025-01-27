using UnityEngine;

public class BuildManager : MonoBehaviour {
    public static BuildManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.Log("Multiple BuildManagers in scene");
            return;
        }

        instance = this;
    }

    public GameObject standardTurretPrefab;

    private void Start() {
        turretToBuild = standardTurretPrefab;
    }

    private GameObject turretToBuild;
    public GameObject GetTurretToBuild() => turretToBuild;


    public FloatVariable currency;
    public void AddCurrency(float amount) => currency.ChangeValueBy(amount);
    public float GetCurrency() => currency.GetValue();
}
