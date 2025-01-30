using UnityEngine;
using UnityEngine.UIElements;

public class AddOrRemoveClass : MonoBehaviour
{
    private VisualElement rootElement;
    private ProgressBar healthBar;
    private Button healthIncreaseButton;
    private Button healthDecreaseButton;

    private ProgressBar damageBar;
    private Button damageIncreaseButton;
    private Button damageDecreaseButton;

    private ProgressBar firerateBar;
    private Button firerateIncreaseButton;
    private Button firerateDecreaseButton;

    private ProgressBar rangeBar;
    private Button rangeIncreaseButton;
    private Button rangeDecreaseButton;

    private Button placeTowersButton;

    void OnEnable()
    {
        // Get the root element of the UI Document
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;
        healthBar = rootElement.Q<ProgressBar>("HealthBar");
        damageBar = rootElement.Q<ProgressBar>("DamageBar");
        firerateBar = rootElement.Q<ProgressBar>("FirerateBar");
        rangeBar = rootElement.Q<ProgressBar>("RangeBar");

        // Get specific UI elements by name or type
        healthIncreaseButton = rootElement.Q<Button>("HealthIncreaseButton");
        healthDecreaseButton = rootElement.Q<Button>("HealthDecreaseButton");
        damageIncreaseButton = rootElement.Q<Button>("DamageIncreaseButton");
        damageDecreaseButton = rootElement.Q<Button>("DamageDecreaseButton");
        firerateIncreaseButton = rootElement.Q<Button>("FirerateIncreaseButton");
        firerateDecreaseButton = rootElement.Q<Button>("FirerateDecreaseButton");
        rangeIncreaseButton = rootElement.Q<Button>("RangeIncreaseButton");
        rangeDecreaseButton = rootElement.Q<Button>("RangeDecreaseButton");
        
        if (healthBar != null)
        {
            healthBar.value = 0; // Starting value (0% progress)
            healthBar.lowValue = 0;
            healthBar.highValue = 10; // Set the max progress value
        }
        if (damageBar != null)
        {
            damageBar.value = 0;
            damageBar.lowValue = 0;
            damageBar.highValue = 10;
        }
        if (firerateBar != null)
        {
            firerateBar.value = 0;
            firerateBar.lowValue = 0;
            firerateBar.highValue = 10;
        }
        if (rangeBar != null)
        {
            rangeBar.value = 0;
            rangeBar.lowValue = 0;
            rangeBar.highValue = 10;
        }

        // Register button click events
        if (healthIncreaseButton != null)
        {
            healthIncreaseButton.clicked += () => ChangeStat("health", 1); // Increase health on click
            // Debug.Log("Health Increased");
        } if (healthDecreaseButton != null)
        {
            healthDecreaseButton.clicked += () => ChangeStat("health", -1); // Decrease health on click
            // Debug.Log("Health Decreased");
        }

        if (damageIncreaseButton != null)
        {
            damageIncreaseButton.clicked += () => ChangeStat("damage", 1);
            // Debug.Log("Damage Increased");
        } if (damageDecreaseButton != null)
        {
            damageDecreaseButton.clicked += () => ChangeStat("damage", -1);
            // Debug.Log("Damage Decreased");
        }

        if (firerateIncreaseButton != null)
        {
            firerateIncreaseButton.clicked += () => ChangeStat("firerate", 1); // Increase health on click
            // Debug.Log("Health Increased");
        } if (firerateDecreaseButton != null)
        {
            firerateDecreaseButton.clicked += () => ChangeStat("firerate", -1); // Decrease health on click
            // Debug.Log("Health Decreased");
        }

        if (rangeIncreaseButton != null)
        {
            rangeIncreaseButton.clicked += () => ChangeStat("range", 1);
            
        } if (rangeDecreaseButton != null)
        {
            rangeDecreaseButton.clicked += () => ChangeStat("range", -1);
            // Debug.Log("Range Decreased");
        }
    }

    private void ChangeStat(string statName, int value)
    {
        if (statName == "health" && healthBar != null)
        {
            Debug.Log("Health Changed");
            healthBar.value = Mathf.Clamp(healthBar.value + value, healthBar.lowValue, healthBar.highValue);
        }

        if (statName == "damage" && damageBar != null)
        {
            Debug.Log("Damage Changed");
            damageBar.value = Mathf.Clamp(damageBar.value + value, damageBar.lowValue, damageBar.highValue);
        }

        if (statName == "firerate" && firerateBar != null)
        {
            Debug.Log("Firerate Changed");
            firerateBar.value = Mathf.Clamp(firerateBar.value + value, firerateBar.lowValue, firerateBar.highValue);
        }

        if (statName == "range" && rangeBar != null)
        {
            Debug.Log("Range Changed");
            rangeBar.value = Mathf.Clamp(rangeBar.value + value, rangeBar.lowValue, rangeBar.highValue);
        }
    }
}
