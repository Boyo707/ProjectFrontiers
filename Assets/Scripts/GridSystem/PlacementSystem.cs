using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour {
    public static PlacementSystem instance;
    public List<GameObject> towersInGame;

    [SerializeField] private ObjectDatabase database;

    [Header("Required GameObjects")]
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Renderer[] cellIndicatorRenderer;
    [Header("Required Components")]
    [SerializeField] private InputManager inputManager;
    //[SerializeField] private UpgradeManager upgradeManager;


    [SerializeField] private bool isBuilding;

    [SerializeField] private Grid grid;
    [SerializeField] private GridData gridData = new();

    [SerializeField] private GameObject gridVisualisation;

    [SerializeField] private Material previewMaterialPrefab;
    [SerializeField] private GameObject previewObject;

    [SerializeField] private int selectedTowerIndex = -1;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveTowerAtLocation;
    }

    private void Start() {
        database = GameManager.instance.database;

        StopPlacementMode();

        cellIndicatorRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
    }

    /* old update loop
    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        UpdatePosition(gridPosition, true);

        if (isBuilding == false)
        {
            //upgradeManager.SelectionState(gridPosition, towerGridData);
            return;
        }
        UpdateState(towerGridData, gridPosition);

        if (lastDetectedPosition != gridPosition)
        {
            lastDetectedPosition = gridPosition;
        }
    }
    */

    private void Update() {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (!isBuilding) return;

        // Update the preview while in placement mode
        UpdatePreview(gridPosition);
    }

    public void StartPlacementMode(int towerId) {
        // Stop any existing placement
        StopPlacementMode();

        // Activate grid visualization
        gridVisualisation.SetActive(true);
        isBuilding = true;

        // Assign the selected tower prefab and prepare preview
        selectedTowerIndex = -1;
        selectedTowerIndex = database.Towers.FindIndex(data => data.Id == towerId);

        if (selectedTowerIndex > -1) {
            previewObject = Instantiate(database.Towers[selectedTowerIndex].Prefab);
            PreparePreview(previewObject);
        }

        // Subscribe to input events
        inputManager.OnClicked += PlaceTower;
        inputManager.OnExit += StopPlacementMode;

        void PreparePreview(GameObject previewObject) {
            // Remove all scripts from the prefab
            MonoBehaviour[] scripts = previewObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts) Destroy(script);

            //set material to preview material
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++) {
                    materials[i] = previewMaterialPrefab;
                }
                renderer.materials = materials;
            }
        }
    }
    public void StopPlacementMode() {
        if (!isBuilding) return;

        // Deactivate grid visualization
        gridVisualisation.SetActive(false);

        // Destroy preview object
        if (previewObject != null) Destroy(previewObject);

        // Reset state
        isBuilding = false;

        // Unsubscribe from input events
        inputManager.OnClicked -= PlaceTower;
        inputManager.OnExit -= StopPlacementMode;
    }
    private void PlaceTower() {
        if (IsPointerOverUI()) return;

        Tower tower = database.Towers[selectedTowerIndex];

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);


        if (!gridData.CanPlaceObjectAt(gridPosition, tower.Size)) {
            // maybe blink the material for extra input/feel
            Debug.Log("Invalid placement location!");
            return;
        }

        // instantiate tower
        GameObject newTower = Instantiate(tower.Prefab);

        // move tower to position && add to towers ingame
        newTower.transform.position = grid.CellToWorld(gridPosition);
        towersInGame.Add(newTower);

        // Update grid data to mark the cells as occupied
        gridData.AddObjectAt(
            gridPosition,
            tower.Size,
            tower.Id,
            towersInGame.Count - 1
        );

        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, newTower));
        Debug.Log("Tower placed successfully!");
    }
    private void RemoveTowerAtLocation(TowerDestroyedEvent e) {
        GameObject tower = e.tower;

        Vector3Int gridPosition = grid.WorldToCell(tower.transform.position);
        int towerIndex = gridData.GetRepresentationIndex(gridPosition);

        if (towerIndex == -1) {
            Debug.Log("No tower found at the specified location!");
            return;
        }

        // Remove tower from the grid data and game world
        gridData.RemoveObjectAt(gridPosition);
        if (towersInGame.Count <= towerIndex || towersInGame[towerIndex] == null) {
            return;
        }

        Destroy(towersInGame[towerIndex]);
        towersInGame.Remove(tower);

        Debug.Log("Tower removed successfully!");
    }
    private void UpdatePreview(Vector3Int gridPosition) {
        // Check if placement is valid
        bool isValid = gridData.CanPlaceObjectAt(gridPosition, database.Towers[selectedTowerIndex].Size);

        // Update the preview object's position and color
        UpdatePosition(grid.CellToWorld(gridPosition), isValid);

        void UpdatePosition(Vector3 position, bool validity) {
            if (previewObject != null) {
                // MovePreview
                previewObject.transform.position = new Vector3(
                    position.x,
                    position.y + 0.05f, // ofsett to stop texture meshing
                    position.z
                );
            }

            // move cursor
            cellIndicator.transform.position = position;

            Color c = validity ? Color.white : Color.red;
            foreach (Renderer renderer in cellIndicatorRenderer) {
                renderer.material.color = c;
            }

            c.a = 0.5f;
            previewMaterialPrefab.color = c;
        }
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
}