using System.Collections.Generic;
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

    private GridData floorData;


    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;
    private void Start() {
        StopPlacement();
        floorData = new GridData();
    }

    public void StartPlacement(int id) {
        StopPlacement();

        gridVisualisation.SetActive(true);

        buildingState = new PlacementState(id,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, objectPlacer);
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

    private void Update() {
        if (buildingState == null) {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}
