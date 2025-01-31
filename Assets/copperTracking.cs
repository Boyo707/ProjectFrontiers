using UnityEngine;
using UnityEngine.UIElements;

public class copperTracking : MonoBehaviour {
    private float currency;
    private VisualElement rootElement;

    private Label currencyTop;
    private Label currencyTowerMenu;

    // Use this for initialization
    void Start() {
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        currencyTop = rootElement.Q<Label>("CurrencyText");
        currencyTowerMenu = rootElement.Q<Label>("SubTitle-Currency");
    }

    void Update() {
        currencyTop.text = "Copper: " + GameManager.instance.GetCurrency();
        currencyTowerMenu.text = "Copper: " + GameManager.instance.GetCurrency();
    }
}
