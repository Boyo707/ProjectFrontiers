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

    private void OnEnable()
    {
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveTower;
    }

    private void Start() {
        StopPlacement();
        towerGridData = new GridData();
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

    public void StartRemoving()
    {
        StopPlacement();
        upgradeManager.EnableUI(false);
        gridVisualisation.SetActive(true);
        buildingState = new SellState(grid, previewSystem, towerGridData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void RemoveTower(TowerDestroyedEvent e)
    {
        Vector3Int gridposition = grid.WorldToCell(e.tower.transform.position);

        if (towerGridData == null)
        {
            //If nothing is in its spot
        }
        else
        {
            int gameObjectIndex = towerGridData.GetRepresentationIndex(gridposition);
            if (gameObjectIndex == -1)
            {
                return;
            }

            //ask wes
            //sells the tower if it comes from SellState
            

            towerGridData.RemoveObjectAt(gridposition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
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

    private void OnDisable()
    {
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveTower;
    }
}
