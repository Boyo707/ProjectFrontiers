using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class SpaceShipHealthStatus : MonoBehaviour {
        [SerializeField] GameObject SpaceShip;
        private float lastHealth;
        private float currentHealth;
        private float maxHealth;
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
            if(lastHealth != currentHealth) {
                lastHealth = currentHealth;
                healthUpgradeBar.value = Mathf.Clamp(currentHealth, healthUpgradeBar.lowValue, maxHealth);
                // healthUpgradeBar.value = (float)currentHealth / maxHealth;
            }
        }
    }
}