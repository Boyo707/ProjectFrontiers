using UnityEngine;
using UnityEngine.UIElements;

public class SpaceShipHealthStatus : MonoBehaviour {
    [SerializeField] GameObject SpaceShip;

    [SerializeField] private int lastHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    private VisualElement rootElement;
    private ProgressBar healthUpgradeBar;

    // Use this for initialization
    void Start() {
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        currentHealth = SpaceShip.GetComponent<TowerBase>().currentHealth;
        maxHealth = GameManager.instance.database.Towers[5].Stats.Health;

        healthUpgradeBar = rootElement.Q<ProgressBar>("Mothership-HP");
    }

    void Update() {
        currentHealth = SpaceShip.GetComponent<TowerBase>().currentHealth;
        if (lastHealth != currentHealth) {
            lastHealth = currentHealth;
            healthUpgradeBar.value = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
}