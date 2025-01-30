using UnityEngine;
using UnityEngine.UIElements;

public class ChangeText : MonoBehaviour
{
    private VisualElement rootElement;
    private Label topCurrency;
    private Label towerMenuCurrency;
    private ProgressBar healthBar, damageBar, firerateBar, rangeBar;
    private Label towerHealthCost, towerDamageCost, towerFirerateCost, towerRangeCost;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;
        topCurrency = rootElement.Q<Label>("CurrencyText");
        towerMenuCurrency = rootElement.Q<Label>("SubTitle-Currency");
        
        healthBar = rootElement.Q<ProgressBar>("HealthBar");
        damageBar = rootElement.Q<ProgressBar>("DamageBar");
        firerateBar = rootElement.Q<ProgressBar>("FirerateBar");
        rangeBar = rootElement.Q<ProgressBar>("RangeBar");
        
        towerHealthCost = rootElement.Q<Label>("HealthBarCostTitle");
        towerDamageCost = rootElement.Q<Label>("DamageBarCostTitle");
        towerFirerateCost = rootElement.Q<Label>("FirerateBarCostTitle");
        towerRangeCost = rootElement.Q<Label>("RangeBarCostTitle");
        
        // Label visualText = root.Q<Label>("LabelOne");
        UpdateCurrencyText(10);
    }

    void UpdateCurrencyText(int currentCurrency)
    {
        topCurrency.text = "Copper:" + currentCurrency;
        towerMenuCurrency.text = "Copper:" + currentCurrency;
    }
}
