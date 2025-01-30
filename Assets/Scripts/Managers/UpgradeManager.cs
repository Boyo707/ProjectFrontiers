using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
// using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using Custom_UI_Scripts;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UpgradeManager : MonoBehaviour
{
    // [Header("Required Component")]
    //
    // [SerializeField] private TMP_Text panelTitelText;
    // [SerializeField] private TMP_Text panelButtonText;
    //
    // [Header("Required Objects")]
    //
    // [SerializeField] private GameObject standardStatsPanel; 
    // [SerializeField] private GameObject statsUpgradePanel;
    //
    // [SerializeField] private GameObject upgradeTowerPanel;
    // [SerializeField] private GameObject sellTowerButton;
    //
    // private Animator panelAnimator;

    private List<TMP_Text> levelText = new ();
    // private List<Slider> levelSliders = new ();

    //FOR THE FUTURE WHEN WE NEED TO USE LEVELS TO UPGRADE THE STATS
    //MAY NEED A NEW SCRIPT
    //NEEDS A GET XP SO THIS WILL BE REPLACED BY THE CURRENTTOWER.XP
    [SerializeField] private int currentXP = 0;
    //private int availableLevels = 0;

    private GameObject currentTowerObject;
    private TowerBase currentTower;
    private int nextTowerId;
    private bool hasOpened;

    private int upgradeInc = 1;
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
    

    // private bool showUpgradePanel = false;
    // private TowerBase towerID;
    
    /// <summary>
    /// Sam's colonizing this part::::
    /// </summary>
    private void OnEnable()
    {
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
        // EventBus<SelectTowerEvent>.OnEvent += (@event => { showUpgradePanel = true;
        //     towerID = @event.tower.GetComponent<TowerBase>();
        // });
        // currency.text = "Copper: "+ GameManager.instance.GetCurrency();
        EventBus<SelectTowerEvent>.OnEvent += ShowUpgradePanel;
        if (closeButton != null)
        {
            closeButton.clicked += () =>
                towerMenu.style.translate =
                    new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu
        }
    }

    // show panel
    // set data for panel
    private GameObject selectedTower;
    private void ShowUpgradePanel(SelectTowerEvent e)
    {
        towerMenu.style.translate =
            new Translate((Length)(-0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu
        selectedTower = e.tower;
        TowerBase tower = selectedTower.GetComponent<TowerBase>();
        string towerTitle = GameManager.instance.database.Towers[tower.id].Name;
        int _currency;
        string _currencyText;   
        int healthUpgrade;
        int damageUpgrade;
        int firerateUpgrade;
        int rangeUpgrade;
        
        
        
        
        void UpdateTowerVariables()
        {
            _currency = GameManager.instance.GetCurrency();
            _currencyText = "Copper: " + _currency;
            healthUpgrade = tower.CurrentUpgrades.Health;
            damageUpgrade = tower.CurrentUpgrades.Damage;
            firerateUpgrade = tower.CurrentUpgrades.FireRate;
            rangeUpgrade = tower.CurrentUpgrades.Range;
            // if (CanUpgrade())
            // {
            //     healthUpgradeBar.highValue = healthUpgradeBar.value;
            //     damageUpgradeBar.highValue = damageUpgradeBar.value;
            //     firerateUpgradeBar.highValue = firerateUpgradeBar.value;
            //     rangeUpgradeBar.highValue = rangeUpgradeBar.value;
            // }
        }

        bool CanUpgrade()
        {
            if (healthUpgradeBar.value == healthUpgradeBar.highValue
                || damageUpgradeBar.value == damageUpgradeBar.highValue
                || firerateUpgradeBar.value == firerateUpgradeBar.highValue
                || rangeUpgradeBar.value == rangeUpgradeBar.highValue) return true;
            return false;
        }

        void MoveOverlay()
        {
            if (CanUpgrade()) overlay.style.translate = new Translate(500, 0);
            else overlay.style.translate = new Translate(0, 0); 
        }

        UpdateTowerVariables();
        

        // Health stat increased on click
        if (healthIncreaseButton != null)
        {
            healthIncreaseButton.clicked += () =>
            {
                if (GameManager.instance.CanAfford(healthUpgrade*upgradeInc) && healthUpgradeBar.value < healthUpgradeBar.highValue)
                {
                    GameManager.instance.RemoveCurrency(healthUpgrade*upgradeInc);
                    tower.CurrentUpgrades.Health++;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Health should have been increased");
                    // if (CanUpgrade()) overlay.RemoveFromHierarchy();
                    MoveOverlay();
                }
            };
        }
        if (healthDecreaseButton != null)
        {
            healthDecreaseButton.clicked += () =>
            {
                if (healthUpgradeBar.value > healthUpgradeBar.lowValue)
                {
                    GameManager.instance.AddCurrency(healthUpgrade);
                    tower.CurrentUpgrades.Health--;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Health should have been decreased");
                    MoveOverlay();
                }
            };
        }
        if (damageIncreaseButton != null)
        {
            damageIncreaseButton.clicked += () =>
            {
                if (GameManager.instance.CanAfford(damageUpgrade*upgradeInc) && damageUpgradeBar.value < damageUpgradeBar.highValue)
                {
                    GameManager.instance.RemoveCurrency(damageUpgrade*upgradeInc);
                    tower.CurrentUpgrades.Damage++;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Damage should have been increased");
                    MoveOverlay();
                }
            };
        } 
        if (damageDecreaseButton != null)
        {
            damageDecreaseButton.clicked += () =>
            {
                if (damageUpgradeBar.value > damageUpgradeBar.lowValue)
                {
                    GameManager.instance.AddCurrency(damageUpgrade);
                    tower.CurrentUpgrades.Damage--;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, damageUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Damage should have been decreased");
                }
            };
        }
        if (firerateIncreaseButton != null)
        {
            firerateIncreaseButton.clicked += () =>
            {
                if (GameManager.instance.CanAfford(firerateUpgrade*upgradeInc) && firerateUpgradeBar.value < firerateUpgradeBar.highValue)
                {
                    GameManager.instance.RemoveCurrency(firerateUpgrade*upgradeInc);
                    tower.CurrentUpgrades.FireRate++;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Firerate should have been increased");
                    // if (CanUpgrade()) overlay.SetEnabled(false);
                    if (CanUpgrade()) overlay.style.translate = new Translate(500, 0);
                }
            };
        }
        if (firerateDecreaseButton != null)
        {
            firerateDecreaseButton.clicked += () =>
            {
                if (firerateUpgradeBar.value > firerateUpgradeBar.lowValue)
                {
                    GameManager.instance.AddCurrency(firerateUpgrade);
                    tower.CurrentUpgrades.FireRate--;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, damageUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Firerate should have been decreased");
                }
            };
        }
        if (rangeIncreaseButton != null)
        {
            rangeIncreaseButton.clicked += () =>
            {
                if (GameManager.instance.CanAfford(rangeUpgrade*upgradeInc) && rangeUpgradeBar.value < rangeUpgradeBar.highValue)
                {
                    GameManager.instance.RemoveCurrency(rangeUpgrade*upgradeInc);
                    tower.CurrentUpgrades.Range++;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Range should have been increased");
                    // if (CanUpgrade()) overlay.SetEnabled(false);
                    if (CanUpgrade()) overlay.style.translate = new Translate(500, 0);
                }
            };
        }
        if (rangeDecreaseButton != null)
        {
            rangeDecreaseButton.clicked += () =>
            {
                if (rangeUpgradeBar.value > rangeUpgradeBar.lowValue)
                {
                    GameManager.instance.AddCurrency(rangeUpgrade);
                    tower.CurrentUpgrades.Range--;
                    UpdateTowerVariables();
                    AssignLabelValues(_currencyText, towerTitle, _currency, damageUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                    Debug.Log("Range should have been decreased");
                }
            };
        }

        if (upgradeButton != null)
        {
            upgradeButton.clicked += () =>
            {
                // Debug.Log("Upgrade test");
                // Debug.Log(CanUpgrade());
                if (CanUpgrade())
                {
                    // Debug.Log("Upgrade button pressed.");
                    towerMenu.style.translate =
                        new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu
                    UpgradeTower(selectedTower);
                    healthUpgradeBar.value = healthUpgradeBar.lowValue;
                    damageUpgradeBar.value = damageUpgradeBar.lowValue;
                    firerateUpgradeBar.value = firerateUpgradeBar.lowValue;
                    rangeUpgradeBar.value = rangeUpgradeBar.lowValue;
                    AssignLabelValues(_currencyText, towerTitle, _currency, healthUpgrade, damageUpgrade, firerateUpgrade, rangeUpgrade);
                }
            };
        }

        if (sellButton != null)
        {
            sellButton.clicked += () =>
            {
                // Debug.Log("Sell test");
                SellTower(selectedTower);
                towerMenu.style.translate =
                    new Translate((Length)(0.3 * rootElement.resolvedStyle.width), 0); // Remove tower menu
            };

        }
    }
    private void AssignLabelValues(string _currencyText, string towerTitle, int _currency, int healthUpgrade, int damageUpgrade,
        int firerateUpgrade, int rangeUpgrade)
    {
        currencyText.text = _currencyText;
        towerMenu.style.translate = new Translate((Length)(-0.3*rootElement.resolvedStyle.width), 0);
        towerMenuTitle.text = towerTitle;
        towerMenuSubTitle.text = _currencyText;
        // Debug.Log("Currency is: "+ _currency);
        healthUpgradeBar.value = Mathf.Clamp(healthUpgrade,healthUpgradeBar.lowValue,healthUpgradeBar.highValue);
        healthUpgradeCost.text = "Cost: " + (healthUpgrade * upgradeInc);
        damageUpgradeBar.value = Mathf.Clamp(damageUpgrade, damageUpgradeBar.lowValue, damageUpgradeBar.highValue);
        damageUpgradeCost.text = "Cost: "+(damageUpgrade*upgradeInc);
        firerateUpgradeBar.value = Mathf.Clamp(firerateUpgrade, firerateUpgradeBar.lowValue, firerateUpgradeBar.highValue);
        firerateUpgradeCost.text = "Cost: "+(firerateUpgrade*upgradeInc);
        rangeUpgradeBar.value = Mathf.Clamp(rangeUpgrade, rangeUpgradeBar.lowValue, rangeUpgradeBar.highValue);
        rangeUpgradeCost.text = "Cost: "+(rangeUpgrade*upgradeInc);
    }
    
    private void SellTower(GameObject towerToSell)
    {
        int newCurrency = (int)GameManager.instance.database.Towers[towerToSell.GetComponent<TowerBase>().id].Cost / 2;
        GameManager.instance.AddCurrency(newCurrency);

        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, towerToSell));
    }

    private void UpgradeTower(GameObject towerToUpgrade)
    {
        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, towerToUpgrade));
        
        int selectedTowerId = towerToUpgrade.GetComponent<TowerBase>().id;
        int upgradeTowerInto = -1;
        
        List<TowerUpgrades> upgradeTable = GameManager.instance.database.TowerUpgrades;
        List<Tower> towers = GameManager.instance.database.Towers;

        Grid grid = GridManager.instance.grid;
        GridData gridData = GridManager.instance.gridData;

        List<GameObject> towersInGame = GridManager.instance.towersInGame;
        
        foreach (TowerUpgrades upgrade in upgradeTable)
        {
            if (upgrade.SelectedTower == selectedTowerId)
            {
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
            towersInGame.Count - 1
        );

        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, newTower));
    }
    
    /// <summary>
    /// G E K O L O N I S E E R D
    /// </summary>

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // for (int i = 0; i < statsUpgradePanel.transform.childCount; i++)
        // {
        //     GameObject currentButton = statsUpgradePanel.transform.GetChild(i).gameObject;
        //     GameObject deeper = currentButton.transform.GetChild(1).gameObject;
        //     levelText.Add(deeper.transform.GetChild(3).GetComponent<TMP_Text>());
        //     // levelSliders.Add(deeper.transform.GetChild(1).GetComponent<Slider>());
        // }
        //
        // panelAnimator = GetComponent<Animator>();
    }

    public void SetTower(GameObject selectedTower)
    {
        currentTowerObject = selectedTower;

        currentTower = selectedTower.GetComponent<TowerBase>();

        gameObject.SetActive(true);

        // DrawTitle();
        // DrawStatUpgrades();
        // DrawTowerUpgrades();
    }

    private void DrawTitle()
    {
        // panelTitelText.text = $"{GameManager.instance.database.Towers[currentTower.id].Name}";
    }

    // private void DrawStatUpgrades()
    // {
    //     //Health
    //     levelText[0].text = $"Level: {currentTower.CurrentUpgrades.Health}";
    //     // levelSliders[0].value = (float)currentTower.CurrentUpgrades.Health/10;
    //
    //     //Damage
    //     levelText[1].text = $"Level: {currentTower.CurrentUpgrades.Damage}";
    //     // levelSliders[1].value = (float)currentTower.CurrentUpgrades.Damage / 10;
    //
    //     //Firerate
    //     levelText[2].text = $"Level: {currentTower.CurrentUpgrades.FireRate}";
    //     // levelSliders[2].value = (float)currentTower.CurrentUpgrades.FireRate / 10;
    //
    //     //Range
    //     levelText[3].text = $"Level: {currentTower.CurrentUpgrades.Range}";
    //     // levelSliders[3].value = (float)currentTower.CurrentUpgrades.Range / 10;
    // }

    // public void UpgradeStat(int statIndex)
    // {
    //     bool hasReachedMaxLevel = false;
    //     if (currentTower.currentUpgrades.Health >= 10 ||
    //         currentTower.currentUpgrades.Damage >= 10 ||
    //         currentTower.currentUpgrades.FireRate >= 10 ||
    //         currentTower.currentUpgrades.Range >= 10)
    //     {
    //         hasReachedMaxLevel = true;
    //     }
    //
    //     switch (statIndex)
    //     {
    //         //HEALTH
    //         case 0:
    //             currentTower.currentUpgrades.Health = IncreaseLevel(currentTower.currentUpgrades.Health, hasReachedMaxLevel);
    //             break;
    //         //DAMAGE
    //         case 1:
    //             currentTower.currentUpgrades.Damage = IncreaseLevel(currentTower.currentUpgrades.Damage, hasReachedMaxLevel);
    //             break;
    //         //FIRERATE
    //         case 2:
    //             currentTower.currentUpgrades.FireRate = IncreaseLevel(currentTower.currentUpgrades.FireRate, hasReachedMaxLevel);
    //             break;
    //         //RANGE
    //         case 3:
    //             currentTower.currentUpgrades.Range = IncreaseLevel(currentTower.currentUpgrades.Range, hasReachedMaxLevel);
    //             break;
    //     }
    //
    //     DrawStatUpgrades();
    //     DrawTowerUpgrades();
    // }
    private int IncreaseLevel(int currentLevel, bool max)
    {
        Debug.Log(currentLevel);
        int levelCost = 10 * currentLevel;
        Debug.Log(levelCost);
        Debug.Log(currentXP >= levelCost);
        if (currentXP >= levelCost)
        {
            int newLevel = currentLevel + 1;

            newLevel = Mathf.Clamp(newLevel, 0,
                (max && currentLevel != 10) ? 9 : 10);

            if (newLevel == 10 && !max)
            {
                max = true;
            }
            currentXP -= levelCost;
            return newLevel;
        }
        return currentLevel;
    }

    // public void DowngradeStat(int statIndex)
    // {
    //     switch (statIndex)
    //     {
    //         //HEALTH
    //         case 0:
    //             currentTower.currentUpgrades.Health = DecreaseLevel(currentTower.currentUpgrades.Health);
    //             break;
    //         //DAMAGE
    //         case 1:
    //             currentTower.currentUpgrades.Damage = DecreaseLevel(currentTower.currentUpgrades.Damage);
    //
    //             break;
    //         //FIRERATE
    //         case 2:
    //             currentTower.currentUpgrades.FireRate = DecreaseLevel(currentTower.currentUpgrades.FireRate);
    //
    //             break;
    //         //RANGE
    //         case 3:
    //
    //             currentTower.currentUpgrades.Range = DecreaseLevel(currentTower.currentUpgrades.Range);
    //             break;
    //     }
    //     DrawStatUpgrades();
    //     DrawTowerUpgrades();
    // }
    private int DecreaseLevel(int currentLevel)
    {
        int newLevel = Mathf.Clamp(currentLevel - 1, 0, 10);

        return newLevel;
    }

    // private void DrawTowerUpgrades()
    // {
    //     for (int i = 0; i < GameManager.instance.database.TowerUpgrades.Count; i++)
    //     {
    //         int validChecks = 0;
    //
    //         TowerUpgrades databaseTowerUpgrades = GameManager.instance.database.TowerUpgrades[i];
    //
    //         if (currentTower.id != databaseTowerUpgrades.SelectedTower)
    //         {
    //             continue;
    //         }
    //
    //         if(currentTower.currentUpgrades.Health >= databaseTowerUpgrades.StatsNeeded.Health)
    //         {
    //             validChecks++;
    //             nextTowerId = databaseTowerUpgrades.NewTower;
    //         }
    //         if (currentTower.currentUpgrades.Damage >= databaseTowerUpgrades.StatsNeeded.Damage)
    //         {
    //             validChecks++;
    //             nextTowerId = databaseTowerUpgrades.NewTower;
    //         }
    //         if (currentTower.currentUpgrades.FireRate >= databaseTowerUpgrades.StatsNeeded.FireRate)
    //         {
    //             validChecks++;
    //             nextTowerId = databaseTowerUpgrades.NewTower;
    //         }
    //         if (currentTower.currentUpgrades.Range >= databaseTowerUpgrades.StatsNeeded.Range)
    //         {
    //             validChecks++;
    //             nextTowerId = databaseTowerUpgrades.NewTower;
    //         }
    //
    //         if (validChecks == 4)
    //         {
    //             // upgradeTowerPanel.SetActive(true);
    //             // upgradeTowerPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = $"{GameManager.instance.database.Towers[nextTowerId].Name}";
    //         }
    //         else
    //         {
    //             // upgradeTowerPanel.SetActive(false);
    //         }
    //     }
    //
    //     
    // }
    
    /*
    public void UpgradeTower()
    {
        
        
        
        //Vector3 currentPos = currentTowerObject.transform.position;

        //remove
        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, currentTowerObject));

        //create
        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, currentTowerObject));

        //get current tower from created tower
        SetTower(GridManager.instance.towersInGame.Last());
    }
    */

    // private void ClearPanel()
    // {
    //     panelTitelText.text = "No tower selected";
    //     standardStatsPanel.SetActive(false);
    //     // statsUpgradePanel.SetActive(false);
    //     upgradeTowerPanel.SetActive(false);
    //     sellTowerButton.SetActive(false);
    // }

    public void MovePanel()
    {
        hasOpened = !hasOpened;
        // panelButtonText.text = hasOpened ? ">" : "<";
        // panelAnimator.SetBool("MovePanel", hasOpened);
    }
    public void MovePanel(bool open)
    {
        hasOpened = open;
        // panelButtonText.text = hasOpened ? ">" : "<";
        // panelAnimator.SetBool("MovePanel", hasOpened);
    }
}
