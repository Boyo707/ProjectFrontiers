using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {

    public TowerLocations TowerLocations;

    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectDatabase database;

    [SerializeField]
    private GameObject gridVisualisation;

    public GridData towerGridData { get; private set; }


    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField] 
    private UpgradeManager upgradeManager;

    IBuildingState buildingState;
    private void Start() {
        StopPlacement();
        towerGridData = new GridData();
    }

    public void StartPlacement(int id) {
        StopPlacement();
        upgradeManager.EnableUI(false);
        gridVisualisation.SetActive(true);

        buildingState = new PlacementState(id,
                                           grid,
                                           previewSystem,
                                           database,
                                           towerGridData,
                                           objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        upgradeManager.EnableUI(false);
        gridVisualisation.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, towerGridData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    private void PlaceStructure() 
    {
        if (inputManager.IsPointerOverUI()) 
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridposition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridposition);
    }

    private void StopPlacement() {
        if(buildingState == null)
        {
            return;
        }
        gridVisualisation.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    

    private void Update() 
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        previewSystem.UpdatePosition(gridPosition, true);

        if (buildingState == null) 
        {
            upgradeManager.SelectionState(gridPosition, towerGridData);
            return;
        }


        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}
