using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private DatabaseAcces _dataBase;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private ObjectPlacer _objectPlacer;

    [Header("Required Objects")]
    [SerializeField] private GameObject _towerPanel;

    [SerializeField] private List<GameObject> _buttons = new List<GameObject>();

    [SerializeField] private List<TMP_Text> _levelText = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> _statTexts = new List<TMP_Text>(); 

    private Turret _currentTower;

    //[SerializeField] private TMP_Text _text;
    //currently selected tower

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _statTexts.Add(_buttons[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>());
            _levelText.Add(_buttons[i].transform.GetChild(1).GetComponent<TMP_Text>());
        }
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
        _statTexts[0].text = $"Health\nCost: {5}";
        _levelText[0].text = $"Level: {_currentTower.CurrentUpgrades.Health}";
        _statTexts[1].text = $"Health\nCost: {5}";
        _levelText[1].text = $"Level: {_currentTower.CurrentUpgrades.Damage}";
        _statTexts[2].text = $"Health\nCost: {5}";
        _levelText[2].text = $"Level: {_currentTower.CurrentUpgrades.FireRate}";
        _statTexts[3].text = $"Health\nCost: {5}";
        _levelText[3].text = $"Level: {_currentTower.CurrentUpgrades.Range}";

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
