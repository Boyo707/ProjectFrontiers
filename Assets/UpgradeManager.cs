using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private ObjectPlacer _objectPlacer;

    [Header("Required Objects")]
    [SerializeField] private GameObject _towerPanel;

    [SerializeField] private GameObject _buttonsPanel;

    [SerializeField]private List<TMP_Text> _levelText = new List<TMP_Text>();
    [SerializeField]private List<Slider> _levelSliders = new List<Slider>();

    private bool _hasReachedMaxLevel;

    private GameObject _currentTowerObject;
    private TowerBase _currentTower;

    private int _nextTowerId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _buttonsPanel.transform.childCount; i++)
        {
            GameObject currentButton = _buttonsPanel.transform.GetChild(i).gameObject;
            GameObject deeper = currentButton.transform.GetChild(1).gameObject;
            _levelText.Add(deeper.transform.GetChild(3).GetComponent<TMP_Text>());
            _levelSliders.Add(deeper.transform.GetChild(1).GetComponent<Slider>());
        }
    }


    // tower has a min n max value. Get value from that tower and incease/decrease it
    //  Turn into another prefab - Remove Current Turret - Get upgraded turret ID - Place new turret
    //

    public void SellTower()
    {
        float newCurrency = DatabaseAcces.instance.database.Towers[_currentTower.id].Cost * _currentTower.GetHealthPercentage();

        BuildManager.instance.AddCurrency(newCurrency);

        EventBus<TowerDestroyedEvent>.Publish(new TowerDestroyedEvent(this, _currentTowerObject.transform.position));

        _towerPanel.SetActive(false);
    }


    public void SetTower(GameObject selectedTower)
    {
        _currentTower = selectedTower.GetComponent<TowerBase>();
        _currentTowerObject = selectedTower;

        _towerPanel.SetActive(true);

        DrawStatUpgrades();
    }

    private void DrawStatUpgrades()
    {
        //Health
        _levelText[0].text = $"Level: {_currentTower.CurrentUpgrades.Health}";
        _levelSliders[0].value = (float)_currentTower.CurrentUpgrades.Health/10;

        //Damage
        _levelText[1].text = $"Level: {_currentTower.CurrentUpgrades.Damage}";
        _levelSliders[1].value = (float)_currentTower.CurrentUpgrades.Damage / 10;

        //Firerate
        _levelText[2].text = $"Level: {_currentTower.CurrentUpgrades.FireRate}";
        _levelSliders[2].value = (float)_currentTower.CurrentUpgrades.FireRate / 10;

        //Range
        _levelText[3].text = $"Level: {_currentTower.CurrentUpgrades.Range}";
        _levelSliders[3].value = (float)_currentTower.CurrentUpgrades.Range / 10;
    }

    public void UpgradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                _currentTower.CurrentUpgrades.Health = IncreaseLevel(_currentTower.CurrentUpgrades.Health);
                break;
            //DAMAGE
            case 1:
                _currentTower.CurrentUpgrades.Damage = IncreaseLevel(_currentTower.CurrentUpgrades.Damage);
                break;
            //FIRERATE
            case 2:
                _currentTower.CurrentUpgrades.FireRate = IncreaseLevel(_currentTower.CurrentUpgrades.FireRate);
                break;
            //RANGE
            case 3:
                _currentTower.CurrentUpgrades.Range = IncreaseLevel(_currentTower.CurrentUpgrades.Range);
                break;
        }
        DrawStatUpgrades();
    }
    private int IncreaseLevel(int currentLevel)
    {
        int newLevel = currentLevel + 1;

        newLevel = Mathf.Clamp(newLevel, 0,
            (_hasReachedMaxLevel && currentLevel != 10) ? 9 : 10);

        if (newLevel == 10 && !_hasReachedMaxLevel) 
        { 
            _hasReachedMaxLevel = true;
            DrawTowerUpgrades();
        }

        return newLevel;
    }

    public void DowngradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                _currentTower.CurrentUpgrades.Health = DecreaseLevel(_currentTower.CurrentUpgrades.Health);
                break;
            //DAMAGE
            case 1:
                _currentTower.CurrentUpgrades.Damage = DecreaseLevel(_currentTower.CurrentUpgrades.Damage);

                break;
            //FIRERATE
            case 2:
                _currentTower.CurrentUpgrades.FireRate = DecreaseLevel(_currentTower.CurrentUpgrades.FireRate);

                break;
            //RANGE
            case 3:

                _currentTower.CurrentUpgrades.Range = DecreaseLevel(_currentTower.CurrentUpgrades.Range);
                break;
        }
        DrawStatUpgrades();
    }
    private int DecreaseLevel(int currentLevel)
    {
        if (currentLevel == 10)
        {
            _hasReachedMaxLevel = false;
        }
        int newLevel = currentLevel - 1;
        return Mathf.Clamp(newLevel, 0, 10);
    }

    public void DrawTowerUpgrades()
    {
        //if we are gonna have an upgrade where more then one level is level 10
        //Make from bool to int. then ++ the int each time an if statement is true. And then go from there.
        bool canUpgrade = false;

        for (int i = 0; i < DatabaseAcces.instance.database.TowerUpgrades.Count; i++)
        {
            TowerUpgrades databaseTowerUpgrades = DatabaseAcces.instance.database.TowerUpgrades[i];

            if (_currentTower.id != databaseTowerUpgrades.SelectedTower)
            {
                continue;
            }

            if(_currentTower.CurrentUpgrades.Health >= databaseTowerUpgrades.StatsNeeded.Health)
            {
                canUpgrade = true;
                _nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (_currentTower.CurrentUpgrades.Damage >= databaseTowerUpgrades.StatsNeeded.Damage)
            {
                canUpgrade = true;
                _nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (_currentTower.CurrentUpgrades.FireRate >= databaseTowerUpgrades.StatsNeeded.FireRate)
            {
                canUpgrade = true;
                _nextTowerId = databaseTowerUpgrades.NewTower;
            }
            if (_currentTower.CurrentUpgrades.Range >= databaseTowerUpgrades.StatsNeeded.Range)
            {
                canUpgrade = true;
                _nextTowerId = databaseTowerUpgrades.NewTower;
            }
        }

        if (canUpgrade)
        {
            Debug.Log("CAN UPGRADE THIS");
            //refrence to thw button
            //turn it from inactive to active
            //change the name
            //save the upgrade ID
        }
    }

    public void UpgradeTower()
    {
        //remove current Tower,
        //get tower grid location
        //place new tower in its place.
        //Draw Current tower upgrades

        //only 1 can be level 10
        //others get limited to level 9
        //1 upgrade tower button / scrap upgrade grid
        //Sell button on the tower upgrade panel (NOT YET DECIDED BUT MAKE THE FUNCTION)
        //sell level
        
    }

    

    public void SelectionState(Vector3Int gridPosition, GridData towerGridData)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            int gameObjectIndex;
            gameObjectIndex = towerGridData.GetRepresentationIndex(gridPosition);

            if (gameObjectIndex >= 0)
            {
                GameObject towerObject = _objectPlacer.PlacedGameObjects[gameObjectIndex];

                SetTower(towerObject);

                EnableUI(true);
            }
            else
            {
                EnableUI(false);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            EnableUI(false);
        }
    }

    public void EnableUI(bool enable)
    {
        _towerPanel.SetActive(enable);
    }
}
