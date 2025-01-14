using UnityEngine;

public class PlacementSystem : MonoBehaviour {

    public TowerLocations TowerLocations;

    [SerializeField]
    GameObject cellIndicator;
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectDatabase database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualisation;

    private void Start() {
        StopPlacement();
    }

    public void StartPlacement(int id) {
        selectedObjectIndex = database.Towers.FindIndex(data => data.Id == id);

        if(selectedObjectIndex < 0) {
            Debug.LogError($"Cant find index in database id: {id}");
            return;
        }
        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure() {
        if (inputManager.IsPointerOverUI()) {
            return;
        }

        GameObject towerToPlace = database.Towers[selectedObjectIndex].Prefab;

        TowerLocations.AddTowerLocation(towerToPlace);
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridposition = grid.WorldToCell(mousePosition);
        GameObject newGameobject = Instantiate(towerToPlace);
        newGameobject.transform.position = grid.CellToWorld(gridposition);
    }

    private void StopPlacement() {
        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update() {
        if (selectedObjectIndex < 0) {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridposition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridposition);
    }
}
