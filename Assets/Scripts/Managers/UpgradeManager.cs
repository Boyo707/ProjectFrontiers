using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JetBrains.Annotations;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private ObjectPlacer objectPlacer;

    [SerializeField] private TMP_Text panelTitelText;
    [SerializeField] private TMP_Text panelButtonText;

    [Header("Required Objects")]

    [SerializeField] private GameObject standardStatsPanel; 
    [SerializeField] private GameObject statsUpgradePanel;

    [SerializeField] private GameObject upgradeTowerPanel;
    [SerializeField] private GameObject sellTowerButton;

    private Animator panelAnimator;

    private List<TMP_Text> levelText = new List<TMP_Text>();
    private List<Slider> levelSliders = new List<Slider>();

    //FOR THE FUTURE WHEN WE NEED TO USE LEVELS TO UPGRADE THE STATS
    //MAY NEED A NEW SCRIPT
    //NEEDS A GET XP SO THIS WILL BE REPLACED BY THE CURRENTTOWER.XP
    [SerializeField] private int currentXP = 0;
    //private int availableLevels = 0;

    private GameObject currentTowerObject;
    private TowerBase currentTower;

    private int nextTowerId;

    private bool hasOpened;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < statsUpgradePanel.transform.childCount; i++)
        {
            GameObject currentButton = statsUpgradePanel.transform.GetChild(i).gameObject;
            GameObject deeper = currentButton.transform.GetChild(1).gameObject;
            levelText.Add(deeper.transform.GetChild(3).GetComponent<TMP_Text>());
            levelSliders.Add(deeper.transform.GetChild(1).GetComponent<Slider>());
        }

        panelAnimator = GetComponent<Animator>();
    }
    public void SelectionState(Vector3Int gridPosition, GridData towerGridData)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            int gameObjectIndex;
            gameObjectIndex = towerGridData.GetRepresentationIndex(gridPosition);

            if (gameObjectIndex >= 0)
            {
                GameObject towerObject = objectPlacer.PlacedGameObjects[gameObjectIndex];

                SetTower(towerObject);

                MovePanel(true);
            }
        }
    }

    public void SellTower()
    {
        float newCurrency = DatabaseAcces.instance.database.Towers[currentTower.id].Cost / 2;

        BuildManager.instance.AddCurrency(newCurrency);

        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, currentTowerObject.transform.position));

        MovePanel(false);

        ClearPanel();
    }

    public void SetTower(GameObject selectedTower)
    {
        //IF STATEMENT HERE FOR WHEN CHECKING IF PLAYER HAS ENOUGH CURRENCY TO BUY THE TOWER

        standardStatsPanel.SetActive(true);
        statsUpgradePanel.SetActive(true);
        sellTowerButton.SetActive(true);

        currentTowerObject = selectedTower;

        currentTower = selectedTower.GetComponent<TowerBase>();

        gameObject.SetActive(true);

        DrawTitle();
        DrawStatUpgrades();
        DrawTowerUpgrades();
    }

    private void DrawTitle()
    {
        panelTitelText.text = $"{DatabaseAcces.instance.database.Towers[currentTower.id].Name}";
    }

    private void DrawStatUpgrades()
    {
        //Health
        levelText[0].text = $"Level: {currentTower.CurrentUpgrades.Health}";
        levelSliders[0].value = (float)currentTower.CurrentUpgrades.Health/10;

        //Damage
        levelText[1].text = $"Level: {currentTower.CurrentUpgrades.Damage}";
        levelSliders[1].value = (float)currentTower.CurrentUpgrades.Damage / 10;

        //Firerate
        levelText[2].text = $"Level: {currentTower.CurrentUpgrades.FireRate}";
        levelSliders[2].value = (float)currentTower.CurrentUpgrades.FireRate / 10;

        //Range
        levelText[3].text = $"Level: {currentTower.CurrentUpgrades.Range}";
        levelSliders[3].value = (float)currentTower.CurrentUpgrades.Range / 10;
    }

    public void UpgradeStat(int statIndex)
    {
        bool hasReachedMaxLevel = false;
        if (currentTower.CurrentUpgrades.Health >= 10 ||
            currentTower.CurrentUpgrades.Damage >= 10 ||
            currentTower.CurrentUpgrades.FireRate >= 10 ||
            currentTower.CurrentUpgrades.Range >= 10)
        {
            hasReachedMaxLevel = true;
        }

        switch (statIndex)
        {
            //HEALTH
            case 0:
                currentTower.CurrentUpgrades.Health = IncreaseLevel(currentTower.CurrentUpgrades.Health, hasReachedMaxLevel);
                break;
            //DAMAGE
            case 1:
                currentTower.CurrentUpgrades.Damage = IncreaseLevel(currentTower.CurrentUpgrades.Damage, hasReachedMaxLevel);
                break;
            //FIRERATE
            case 2:
                currentTower.CurrentUpgrades.FireRate = IncreaseLevel(currentTower.CurrentUpgrades.FireRate, hasReachedMaxLevel);
                break;
            //RANGE
            case 3:
                currentTower.CurrentUpgrades.Range = IncreaseLevel(currentTower.CurrentUpgrades.Range, hasReachedMaxLevel);
                break;
        }

        DrawStatUpgrades();
        DrawTowerUpgrades();
    }
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

    public void DowngradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                currentTower.CurrentUpgrades.Health = DecreaseLevel(currentTower.CurrentUpgrades.Health);
                break;
            //DAMAGE
            case 1:
                currentTower.CurrentUpgrades.Damage = DecreaseLevel(currentTower.CurrentUpgrades.Damage);

                break;
            //FIRERATE
            case 2:
                currentTower.CurrentUpgrades.FireRate = DecreaseLevel(currentTower.CurrentUpgrades.FireRate);

                break;
            //RANGE
            case 3:

                currentTower.CurrentUpgrades.Range = DecreaseLevel(currentTower.CurrentUpgrades.Range);
                break;
        }
        DrawStatUpgrades();
        DrawTowerUpgrades();
    }
    private int DecreaseLevel(int currentLevel)
    {
        int newLevel = Mathf.Clamp(currentLevel - 1, 0, 10);

        return newLevel;
    }

    private void DrawTowerUpgrades()
    {
        for (int i = 0; i < DatabaseAcces.instance.database.TowerUpgrades.Count; i++)
        {
            int validChecks = 0;

            TowerUpgrades databaseTowerUpgrades = DatabaseAcces.instance.database.TowerUpgrades[i];

            if (currentTower.id != databaseTowerUpgrades.SelectedTower)
            {
                continue;
            }

            if(currentTower.CurrentUpgrades.Health >= databaseTowerUpgrades.StatsNeeded.Health)
            {
                validChecks++;
                nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (currentTower.CurrentUpgrades.Damage >= databaseTowerUpgrades.StatsNeeded.Damage)
            {
                validChecks++;
                nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (currentTower.CurrentUpgrades.FireRate >= databaseTowerUpgrades.StatsNeeded.FireRate)
            {
                validChecks++;
                nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (currentTower.CurrentUpgrades.Range >= databaseTowerUpgrades.StatsNeeded.Range)
            {
                validChecks++;
                nextTowerId = databaseTowerUpgrades.NewTower;
            }

            if (validChecks == 4)
            {
                upgradeTowerPanel.SetActive(true);
                upgradeTowerPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = $"{DatabaseAcces.instance.database.Towers[nextTowerId].Name}";
            }
            else
            {
                upgradeTowerPanel.SetActive(false);
            }
        }

        
    }
    public void UpgradeTower()
    {
        Vector3 currentPos = currentTowerObject.transform.position;

        //remove
        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, currentPos));

        //create
        EventBus<CreateTowerEvent>.Publish(new CreateTowerEvent(this, currentPos, nextTowerId));

        //get current tower from created tower
        SetTower(objectPlacer.PlacedGameObjects.Last());

        upgradeTowerPanel.SetActive(false);
    }

    private void ClearPanel()
    {
        panelTitelText.text = "No tower selected";
        standardStatsPanel.SetActive(false);
        statsUpgradePanel.SetActive(false);
        upgradeTowerPanel.SetActive(false);
        sellTowerButton.SetActive(false);
    }

    public void MovePanel()
    {
        hasOpened = !hasOpened;
        panelButtonText.text = hasOpened ? ">" : "<";
        panelAnimator.SetBool("MovePanel", hasOpened);
    }
    public void MovePanel(bool open)
    {
        hasOpened = open;
        panelButtonText.text = hasOpened ? ">" : "<";
        panelAnimator.SetBool("MovePanel", hasOpened);
    }
}
