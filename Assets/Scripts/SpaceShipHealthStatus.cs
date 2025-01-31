using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class SpaceShipHealthStatus : MonoBehaviour {
        [SerializeField] GameObject SpaceShip;
        private int lastHealth;
        private int currentHealth;
        private int maxHealth;

        private ProgressBar healthUpgradeBar;

        // Use this for initialization
        void Start() {
            currentHealth = SpaceShip.GetComponent<TowerBase>().currentHealth;
            maxHealth = GameManager.instance.database.Towers[5].Stats.Health;

            //healthUpgradeBar = rootElement.Q<ProgressBar>("HealthBar");
        }

        void Update() {
            if(lastHealth != currentHealth) {
                lastHealth = currentHealth;
                //healthUpgradeBar.value = (float)currentHealth / maxHealth;
            }
        }
    }
}