using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class CurrencyUpdater : MonoBehaviour {
        private float currency;
        private VisualElement rootElement;

        private Label currencyTop;
        private Label currencyTowerMenu;

        // Use this for initialization
        void Start() {
            var uiDocument = GetComponent<UIDocument>();
            rootElement = uiDocument.rootVisualElement;
            
            // currency = GameManager.instance.GetCurrency();
            // maxHealth = GameManager.instance.database.Towers[5].Stats.Health;

            currencyTop = rootElement.Q<Label>("CurrencyText");
            currencyTowerMenu = rootElement.Q<Label>("SubTitle-Currency");
        }

        void Update()
        {
            currencyTop.text = "Copper: " + GameManager.instance.GetCurrency();
            currencyTowerMenu.text = "Copper: " + GameManager.instance.GetCurrency();
            // if(lastHealth != currentHealth) {
            //     lastHealth = currentHealth;
            //     healthUpgradeBar.value = Mathf.Clamp(currentHealth, healthUpgradeBar.lowValue, maxHealth);
            //     // healthUpgradeBar.value = (float)currentHealth / maxHealth;
            // }
        }
    }
}