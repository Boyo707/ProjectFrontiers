using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeManager : MonoBehaviour {
    [SerializeField] private GameObject selectedTower;


    int upgradeCost = 1;
    //Root element is like the canvas, it's necessary to define to find anything placed on the canvas easily
    private VisualElement rootElement;
    private VisualElement towerMenu;

    private Label currencyText;
    private Label towerMenuTitle;
    private Label towerMenuSubTitle;

    // Stat upgrade labels
    private Label healthUpgradeCost;
    private Label damageUpgradeCost;
    private Label firerateUpgradeCost;
    private Label rangeUpgradeCost;

    // Overlay
    private VisualElement overlay;

    // Buttons
    private Button upgradeButton;
    private Button sellButton;
    private Button closeButton;

    // Stat buttons
    private Button healthIncreaseButton;
    private Button healthDecreaseButton;
    private Button damageIncreaseButton;
    private Button damageDecreaseButton;
    private Button firerateIncreaseButton;
    private Button firerateDecreaseButton;
    private Button rangeIncreaseButton;
    private Button rangeDecreaseButton;

    // Stat bars
    private ProgressBar healthUpgradeBar;
    private ProgressBar damageUpgradeBar;
    private ProgressBar firerateUpgradeBar;
    private ProgressBar rangeUpgradeBar;

    private void OnEnable() {
        var uiDocument = GetComponent<UIDocument>();

        rootElement = uiDocument.rootVisualElement;
        towerMenu = rootElement.Q<VisualElement>("TowerMenu");
        currencyText = rootElement.Q<Label>("CurrencyText");
        towerMenuTitle = rootElement.Q<Label>("TowerMenuTitle");
        towerMenuSubTitle = rootElement.Q<Label>("SubTitle-Currency");

        // Stat upgrade labels
        healthUpgradeCost = rootElement.Q<Label>("HealthBarCostTitle");
        damageUpgradeCost = rootElement.Q<Label>("DamageBarCostTitle");
        firerateUpgradeCost = rootElement.Q<Label>("FirerateBarCostTitle");
        rangeUpgradeCost = rootElement.Q<Label>("RangeBarCostTitle");

        // Grayed-out overlay
        overlay = rootElement.Q<VisualElement>("Grayed-Out-Overlay");

        // Buttons
        upgradeButton = rootElement.Q<Button>("UpgradeButtonButt");
        sellButton = rootElement.Q<Button>("SellButtonButt");
        closeButton = rootElement.Q<Button>("CloseButton");

        healthIncreaseButton = rootElement.Q<Button>("HealthIncreaseButton");
        healthDecreaseButton = rootElement.Q<Button>("HealthDecreaseButton");
        damageIncreaseButton = rootElement.Q<Button>("DamageIncreaseButton");
        damageDecreaseButton = rootElement.Q<Button>("DamageDecreaseButton");
        firerateIncreaseButton = rootElement.Q<Button>("FirerateIncreaseButton");
        firerateDecreaseButton = rootElement.Q<Button>("FirerateDecreaseButton");
        rangeIncreaseButton = rootElement.Q<Button>("RangeIncreaseButton");
        rangeDecreaseButton = rootElement.Q<Button>("RangeDecreaseButton");

        // Stat bars
        healthUpgradeBar = rootElement.Q<ProgressBar>("HealthBar");
        damageUpgradeBar = rootElement.Q<ProgressBar>("DamageBar");
        firerateUpgradeBar = rootElement.Q<ProgressBar>("FirerateBar");
        rangeUpgradeBar = rootElement.Q<ProgressBar>("RangeBar");

        if (closeButton != null) {
            closeButton.clicked += () =>
                towerMenu.style.translate =
                    new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // hide tower menu
        }

        EventBus<SelectTowerEvent>.OnEvent += ShowUpgradePanel;
    }

    private void ShowUpgradePanel(SelectTowerEvent e) {
        // show or hide on selection event
        towerMenu.style.translate =
                new Translate((Length)(-0.3 * rootElement.resolvedStyle.width), 0); // Show menu
            selectedTower = e.tower;
        
        TowerBase tower = selectedTower.GetComponent<TowerBase>();
        string towerTitle = GameManager.instance.database.Towers[tower.id].Name;

        int _currency;
        string _currencyText;

        int healthUpgrade = 1;
        int damageUpgrade = 1;
        int firerateUpgrade = 1;
        int rangeUpgrade = 1;

        UpdateTowerVariables();

        void LevelUpStat(string type) {
            // can level up when 
            // 1. currency is enough
            // 2. upgrade level is not maxed out
            // 3. other stat is not level 10 only one stat can be level 10 others then max at 9

            bool canLevelUp = false;
            int maxLevel = (int)healthUpgradeBar.highValue;

            //if upgrade any is lv 10 max level is 9
            if (healthUpgrade == maxLevel || damageUpgrade == maxLevel || firerateUpgrade == maxLevel || rangeUpgrade == maxLevel) maxLevel--;

            if (type == "health") {
                if (healthUpgrade < maxLevel) {
                    GameManager.instance.RemoveCurrency(healthUpgrade * upgradeCost);
                    tower.CurrentUpgrades.Health++;
                }
            }
            if (type == "damage") {
                if (damageUpgrade < maxLevel) {
                    GameManager.instance.RemoveCurrency(damageUpgrade * upgradeCost);
                    tower.CurrentUpgrades.Damage++;
                } 
            }
            if (type == "fireRate") {
                if (firerateUpgrade < maxLevel) {
                    GameManager.instance.RemoveCurrency(firerateUpgrade * upgradeCost);
                    tower.CurrentUpgrades.FireRate++;
                }
            }
            if (type == "range") {
                if (rangeUpgrade < maxLevel) {
                    GameManager.instance.RemoveCurrency(rangeUpgrade * upgradeCost);
                    tower.CurrentUpgrades.Range++;
                }
            }

            UpdateTowerVariables();
            MoveOverlay();
        }

        if (healthIncreaseButton != null) {
            healthIncreaseButton.clicked += () => {
                LevelUpStat("health");
            };
        }
        if (healthDecreaseButton != null) {
            healthDecreaseButton.clicked += () => {
                if (healthUpgradeBar.value > healthUpgradeBar.lowValue) {
                    GameManager.instance.AddCurrency(healthUpgrade * upgradeCost / 2);
                    tower.CurrentUpgrades.Health--;
                    Debug.Log("Health should have been decreased");
                    UpdateTowerVariables();
                    MoveOverlay();
                }
            };
        }


        if (damageIncreaseButton != null) {
            damageIncreaseButton.clicked += () => {
                LevelUpStat("damage");
            };
        }
        if (damageDecreaseButton != null) {
            damageDecreaseButton.clicked += () => {
                if (firerateUpgradeBar.value > firerateUpgradeBar.lowValue) {
                    GameManager.instance.AddCurrency(damageUpgrade * upgradeCost / 2);
                    tower.CurrentUpgrades.Damage--;
                    UpdateTowerVariables();
                    MoveOverlay();
                    Debug.Log("Firerate should have been decreased");
                }
            };
        }


        if (firerateIncreaseButton != null) {
            firerateIncreaseButton.clicked += () => {
                LevelUpStat("fireRate");
            };
        }
        if (firerateDecreaseButton != null) {
            firerateDecreaseButton.clicked += () => {
                if (firerateUpgradeBar.value > firerateUpgradeBar.lowValue) {
                    GameManager.instance.AddCurrency(firerateUpgrade * upgradeCost / 2);
                    tower.CurrentUpgrades.FireRate--;
                    UpdateTowerVariables();
                    MoveOverlay();
                    Debug.Log("Firerate should have been decreased");
                }
            };
        }


        if (rangeIncreaseButton != null) {
            rangeIncreaseButton.clicked += () => {
                LevelUpStat("range");
            };
        }
        if (rangeDecreaseButton != null) {
            rangeDecreaseButton.clicked += () => {
                if (rangeUpgradeBar.value > rangeUpgradeBar.lowValue) {
                    GameManager.instance.AddCurrency(rangeUpgrade * upgradeCost/2);
                    tower.CurrentUpgrades.Range--;
                    UpdateTowerVariables();
                    MoveOverlay();
                    Debug.Log("Range should have been decreased");
                }
            };
        }


        if (upgradeButton != null) {
            upgradeButton.clicked += () => {
                if (CanUpgrade()) {
                    towerMenu.style.translate =
                        new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu

                    string towerStatLvl10 = "";
                    if (healthUpgradeBar.value == healthUpgradeBar.highValue) towerStatLvl10 = "health";
                    if (damageUpgradeBar.value == damageUpgradeBar.highValue) towerStatLvl10 = "damage";
                    if (firerateUpgradeBar.value == firerateUpgradeBar.highValue) towerStatLvl10 = "fireRate";
                    if (rangeUpgradeBar.value == rangeUpgradeBar.highValue) towerStatLvl10 = "range";

                    UpgradeTower(selectedTower, towerStatLvl10);

                    healthUpgradeBar.value = healthUpgradeBar.lowValue;
                    damageUpgradeBar.value = damageUpgradeBar.lowValue;
                    firerateUpgradeBar.value = firerateUpgradeBar.lowValue;
                    rangeUpgradeBar.value = rangeUpgradeBar.lowValue;
                    UpdateTowerVariables();
                    MoveOverlay();
                }
            };
        }

        if (sellButton != null) {
            sellButton.clicked += () => {
                // Debug.Log("Sell test");
                SellTower(selectedTower);
                towerMenu.style.translate =
                    new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu
            };

        }


        bool CanUpgrade() {
            if (healthUpgradeBar.value == healthUpgradeBar.highValue
                || damageUpgradeBar.value == damageUpgradeBar.highValue
                || firerateUpgradeBar.value == firerateUpgradeBar.highValue
                || rangeUpgradeBar.value == rangeUpgradeBar.highValue) {
                return true;
            }

            return false;
        }

        void MoveOverlay() {
            if (CanUpgrade()) overlay.style.translate = new Translate(500, 0);
            else overlay.style.translate = new Translate(0, 0);
        }

        void UpdateTowerVariables() {
            _currency = GameManager.instance.GetCurrency();
            _currencyText = "Copper: " + _currency;

            healthUpgrade = tower.CurrentUpgrades.Health;
            damageUpgrade = tower.CurrentUpgrades.Damage;
            firerateUpgrade = tower.CurrentUpgrades.FireRate;
            rangeUpgrade = tower.CurrentUpgrades.Range;

            AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
        }
    }

    private void AssignLabelValues(string _currencyText, string towerTitle, int _currency, int healthUpgrade, int damageUpgrade, int firerateUpgrade, int rangeUpgrade) {
        currencyText.text = _currencyText;
        towerMenu.style.translate = new Translate((Length)(-0.3 * rootElement.resolvedStyle.width), 0);

        towerMenuTitle.text = towerTitle;
        towerMenuSubTitle.text = _currencyText;

        // Debug.Log("Currency is: "+ _currency);
        healthUpgradeBar.value = Mathf.Clamp(healthUpgrade, healthUpgradeBar.lowValue, healthUpgradeBar.highValue);
        healthUpgradeCost.text = "Cost: " + (healthUpgrade * upgradeCost);
        damageUpgradeBar.value = Mathf.Clamp(damageUpgrade, damageUpgradeBar.lowValue, damageUpgradeBar.highValue);
        damageUpgradeCost.text = "Cost: " + (damageUpgrade * upgradeCost);
        firerateUpgradeBar.value = Mathf.Clamp(firerateUpgrade, firerateUpgradeBar.lowValue, firerateUpgradeBar.highValue);
        firerateUpgradeCost.text = "Cost: " + (firerateUpgrade * upgradeCost);
        rangeUpgradeBar.value = Mathf.Clamp(rangeUpgrade, rangeUpgradeBar.lowValue, rangeUpgradeBar.highValue);
        rangeUpgradeCost.text = "Cost: " + (rangeUpgrade * upgradeCost);
    }

    private void SellTower(GameObject towerToSell) {
        int newCurrency = (int)GameManager.instance.database.Towers[towerToSell.GetComponent<TowerBase>().id].Cost / 2;
        GameManager.instance.AddCurrency(newCurrency);

        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, towerToSell));
    }

    private void UpgradeTower(GameObject towerToUpgrade, string statLevel10) {
        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, towerToUpgrade));

        TowerBase selectedTower = towerToUpgrade.GetComponent<TowerBase>();
        int selectedTowerId = selectedTower.id;
        int upgradeTowerInto = -1;

        List<TowerUpgrades> upgradeTable = GameManager.instance.database.TowerUpgrades;
        List<Tower> towers = GameManager.instance.database.Towers;

        Grid grid = GridManager.instance.grid;
        GridData gridData = GridManager.instance.gridData;

        List<GameObject> towersInGame = GridManager.instance.towersInGame;

        foreach (TowerUpgrades upgrade in upgradeTable) {
            if (upgrade.SelectedTower == selectedTowerId && upgrade.stat == statLevel10) {
                upgradeTowerInto = upgrade.NewTower;
                break;
            }
        }

        if (upgradeTowerInto < 0) return;
        Tower tower = towers[upgradeTowerInto];

        Vector3 mousePosition = towerToUpgrade.transform.position;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        GameObject newTower = Instantiate(tower.Prefab);

        newTower.transform.position = grid.CellToWorld(gridPosition);
        towersInGame.Add(newTower);

        // Update grid data to mark the cells as occupied
        gridData.AddObjectAt(
            gridPosition,
            tower.Size,
            tower.Id,
            newTower
        );

        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, newTower));
    }

    private void OnDestroy() {
        EventBus<SelectTowerEvent>.OnEvent -= ShowUpgradePanel;
    }
}
