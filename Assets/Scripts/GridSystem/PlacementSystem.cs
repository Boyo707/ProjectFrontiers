using System.Linq;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {


    [Header("Required GameObjects")]
    [SerializeField] private GameObject gridVisualisation;

    [Header("Required Components")]
    [SerializeField] private TowerLocations TowerLocations;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private ObjectPlacer objectPlacer;

    [SerializeField] private Grid grid;
    [SerializeField] private GridData towerGridData;

    [SerializeField] private PreviewSystem previewSystem;

    [SerializeField] private UpgradeManager upgradeManager;

    [SerializeField] private ObjectDatabase database;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private bool isBuilding;

    private void OnEnable()
    {
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveTower;
        EventBus<CreateTowerEvent>.OnEvent += PlaceTower;
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

        if (isBuilding == false)
        {
            upgradeManager.SelectionState(gridPosition, towerGridData);
            return;
        }


        if (lastDetectedPosition != gridPosition)
        {
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int id) {
        StopPlacement();
        gridVisualisation.SetActive(true);

        isBuilding = true;

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

        EventBus<CreateTowerEvent>.Publish(new CreateTowerEvent(this, gridposition, 0));

    }
    private void StopPlacement()
    {
        if (isBuilding == false)
        {
            return;
        }
        gridVisualisation.SetActive(false);

        lastDetectedPosition = Vector3Int.zero;

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        isBuilding = false;
    }

    private void PlaceTower(CreateTowerEvent e)
    {
        Vector3Int gridposition = grid.WorldToCell(e.towerPosition);

        GridData selectedData = towerGridData;
        int gameObjectIndex = towerGridData.GetRepresentationIndex(gridposition);

        int selectedObjectIndex = database.Towers.FindIndex(data => data.Id == e.towerId);

        bool placementValidity = selectedData.CanPlaceObjectAt(gridposition, database.Towers[selectedObjectIndex].Size);


        if (placementValidity == false)
        {
            Debug.Log("Im already occupied");
            return;
        }

        int index = objectPlacer.PlaceObject(database.Towers[selectedObjectIndex].Prefab, grid.CellToWorld(gridposition));

        upgradeManager.SetTower(objectPlacer.PlacedGameObjects.Last()); 

        GridData selectData = towerGridData;

        selectData.AddObjectAt(gridposition,
            database.Towers[selectedObjectIndex].Size,
            database.Towers[selectedObjectIndex].Id,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridposition), false);
    }
    private void RemoveTower(TowerDestroyedEvent e)
    {
        Vector3Int gridposition = grid.WorldToCell(e.towerPosition);

        if (towerGridData == null)
        {
            //If nothing is in the spot you want to remove
        }
        else
        {
            int gameObjectIndex = towerGridData.GetRepresentationIndex(gridposition);
            if (gameObjectIndex == -1)
            {
                return;
            }

            towerGridData.RemoveObjectAt(gridposition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
    }
    private void OnDisable()
    {
        EventBus<TowerDestroyedEvent>.OnEvent -= RemoveTower;
        EventBus<CreateTowerEvent>.OnEvent -= PlaceTower;
    }
}
