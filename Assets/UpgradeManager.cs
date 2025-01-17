using UnityEngine;
using TMPro;
using System;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private DatabaseAcces _dataBase;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private ObjectPlacer _objectPlacer;

    [Header("Required Objects")]
    [SerializeField] private GameObject _towerPanel;

    

    private Turret _currentTower;

    [SerializeField] private TMP_Text _text;
    //currently selected tower

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Needs to communicate with grid and placement system
    //click on tower - able to do that when not placing a tower OR when placing a tower.
    //Open tower UI - Temp Panel with upgrade button [+] 
    //Button to upgrade health - speed - attack etc, When reaching max upgrade of one stat - turn into another prefab
    //  Turn into another prefab - Remove Current Turret - Get upgraded turret ID - Place new turret
    
    public void SetTower(Turret selectedTower)
    {
        _currentTower = selectedTower;
        _towerPanel.SetActive(true);

        DrawText();
    }

    private void DrawText()
    {
        _text.text = $"{_currentTower.CurrentUpgrades.Health}\n" +
              $"{_currentTower.CurrentUpgrades.Damage}\n" +
              $"{_currentTower.CurrentUpgrades.FireRate}\n" +
              $"{_currentTower.CurrentUpgrades.Range}";
    }

    public void UpgradeStat(int statIndex)
    {
        switch (statIndex)
        {
            //HEALTH
            case 0:
                _currentTower.CurrentUpgrades.Health++;
                DrawText();

                break;
            //DAMAGE
            case 1:
                _currentTower.CurrentUpgrades.Damage++;
                DrawText();
                break;
            //FIRERATE
            case 2:
                _currentTower.CurrentUpgrades.FireRate++;
                DrawText();
                break;
            //RANGE
            case 3:
                _currentTower.CurrentUpgrades.Range++;
                DrawText();
                break;
        }
    }

    public void SelectionState(Vector3Int gridPosition, GridData towerGridData)
    {
        //Set this on the click action
        if (Input.GetMouseButtonDown(0))
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
                if (!_inputManager.IsPointerOverUI())
                {
                    EnableUI(false);
                }
            }
        }
    }

    public void EnableUI(bool enable)
    {
        _towerPanel.SetActive(enable);
    }
}
