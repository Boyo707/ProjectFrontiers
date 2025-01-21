using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private DatabaseAcces _dataBase;
    //[SerializeField] private InputManager _inputManager;
    //[SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private ObjectPlacer _objectPlacer;

    [Header("Required Objects")]
    [SerializeField] private GameObject _towerPanel;

    [SerializeField] private GameObject _buttonsPanel;

    private List<TMP_Text> _levelText = new List<TMP_Text>();
    private List<TMP_Text> _statTexts = new List<TMP_Text>(); 

    private Turret _currentTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _buttonsPanel.transform.childCount; i++)
        {
            GameObject currentButton = _buttonsPanel.transform.GetChild(i).gameObject;
            _statTexts.Add(currentButton.transform.GetChild(0).GetComponentInChildren<TMP_Text>());
            _levelText.Add(currentButton.transform.GetChild(1).GetComponent<TMP_Text>());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    //Open tower UI - Temp Panel with upgrade button [+] 
    //ask if upgrade has a max
    //  Turn into another prefab - Remove Current Turret - Get upgraded turret ID - Place new turret
    
    public void SetTower(Turret selectedTower)
    {
        _currentTower = selectedTower;
        _towerPanel.SetActive(true);

        DrawText();
    }

    private void DrawText()
    {
        //Health
        _statTexts[0].text = $"Health\nCost: {5}";
        _levelText[0].text = $"Level: {_currentTower.CurrentUpgrades.Health}";

        //Damage
        _statTexts[1].text = $"Damage\nCost: {5}";
        _levelText[1].text = $"Level: {_currentTower.CurrentUpgrades.Damage}";

        //Firerate
        _statTexts[2].text = $"Firerate\nCost: {5}";
        _levelText[2].text = $"Level: {_currentTower.CurrentUpgrades.FireRate}";

        //Range
        _statTexts[3].text = $"Range\nCost: {5}";
        _levelText[3].text = $"Level: {_currentTower.CurrentUpgrades.Range}";

    }

    public void UpgradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                _currentTower.CurrentUpgrades.Health++;
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

    public void DrawTowerUpgrades()
    {
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


    }

    public void SelectionState(Vector3Int gridPosition, GridData towerGridData)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            int gameObjectIndex;
            gameObjectIndex = towerGridData.GetRepresentationIndex(gridPosition);

            if (gameObjectIndex >= 0)
            {
                Turret turret = _objectPlacer.PlacedGameObjects[gameObjectIndex].GetComponentInChildren<Turret>();

                SetTower(turret);

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
