using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.Build;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private ObjectPlacer _objectPlacer;

    [Header("Required Objects")]
    [SerializeField] private GameObject _towerPanel;

    [SerializeField] private GameObject _buttonsPanel;

    [SerializeField]private List<TMP_Text> _levelText = new List<TMP_Text>();

    private bool _hasReachedMaxLevel;

    private GameObject _currentTowerObject;
    private TowerBase _currentTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _buttonsPanel.transform.childCount; i++)
        {
            GameObject currentButton = _buttonsPanel.transform.GetChild(i).gameObject;
            GameObject deeper = currentButton.transform.GetChild(1).gameObject;
            _levelText.Add(deeper.transform.GetChild(3).GetComponent<TMP_Text>());
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
    }


    public void SetTower(GameObject selectedTower)
    {
        _currentTower = selectedTower.GetComponent<TowerBase>();
        _currentTowerObject = selectedTower;

        _towerPanel.SetActive(true);

        DrawText();
    }

    private void DrawText()
    {
        //Health
        _levelText[0].text = $"Level: {_currentTower.CurrentUpgrades.Health}";

        //Damage
        _levelText[1].text = $"Level: {_currentTower.CurrentUpgrades.Damage}";

        //Firerate
        _levelText[2].text = $"Level: {_currentTower.CurrentUpgrades.FireRate}";

        //Range
        _levelText[3].text = $"Level: {_currentTower.CurrentUpgrades.Range}";
    }

    public void UpgradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                
                int healthUpgrade = _currentTower.CurrentUpgrades.Health + 1;
                _currentTower.CurrentUpgrades.Health = Mathf.Clamp(healthUpgrade, 0, 10);
                //if(_currentTower.CurrentUpgrades.Health ==)
                break;
            //DAMAGE
            case 1:
                _currentTower.CurrentUpgrades.Damage++;
                break;
            //FIRERATE
            case 2:
                _currentTower.CurrentUpgrades.FireRate++;
                break;
            //RANGE
            case 3:
                _currentTower.CurrentUpgrades.Range++;
                break;
        }
        DrawText();
    }
    public void DowngradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                _currentTower.CurrentUpgrades.Health--;
                break;
            //DAMAGE
            case 1:
                _currentTower.CurrentUpgrades.Damage--;
                break;
            //FIRERATE
            case 2:
                _currentTower.CurrentUpgrades.FireRate--;
                break;
            //RANGE
            case 3:
                _currentTower.CurrentUpgrades.Range--;
                break;
        }
        DrawText();
    }

    public void DrawTowerUpgrades()
    {
        //DatabaseAcces.instance.database.TowerUpgrades

        //this void activates when level 10 is reached.
        //get current tower possible upgrades
        //instantiate towers amount
        //Get instantiated tower, assign names and image
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
