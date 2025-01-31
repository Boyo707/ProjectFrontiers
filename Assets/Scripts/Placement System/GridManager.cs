using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement of towers on the grid.
/// </summary>
public class GridManager : MonoBehaviour {
    #region Definitions
    [SerializeField] private GameObject homeTower;
    public static GridManager instance;
    public List<GameObject> towersInGame;
    //public bool towerInRange = false;

    // original tower data
    private List<Tower> towers;

    [Header("Required GameObjects")]
    [SerializeField] private GameObject cellIndicator;
    private Renderer[] cellIndicatorRenderer;

    private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 lastPosition;

    private bool isPlacementMode = false;

    [SerializeField] public Grid grid;
    public GridData gridData = new();
    [SerializeField] private Transform towerParent;

    [SerializeField] private GameObject gridVisualisation;

    [SerializeField] private Material previewMaterial;
    private GameObject previewObject = null;

    [SerializeField] private int selectedTowerIndex = -1;

    #endregion

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveTowerAtLocation;
        EventBus<PauseGameEvent>.OnEvent += GamePaused;
    }

    private void GamePaused(PauseGameEvent e) {
        StopPlacementMode();
    }

    private void Start() {
        towers = GameManager.instance.database.Towers;

        StopPlacementMode();

        cellIndicatorRenderer = cellIndicator.GetComponentsInChildren<Renderer>();

        // Get the active camera if sceneCamera is not assigned
        if (sceneCamera == null) {
            sceneCamera = Camera.main;
        }


        Tower tower = towers[5];

        Vector3Int homePosition = new(0,0,0);

        // Move tower to position and add to towers in game
        homeTower.transform.position = grid.CellToWorld(new Vector3Int(1,0,1));
        towersInGame.Add(homeTower);

        // Update grid data to mark the cells as occupied
        gridData.AddObjectAt(
            homePosition,
            tower.Size,
            tower.Id,
            homeTower
        );

        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, homeTower));
    }

    private void Update() {
        Vector3 mousePosition = GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        // Update the preview
        UpdatePosition(gridPosition);

        if (Input.GetMouseButtonDown(0)) {
            if (isPlacementMode) PlaceTower();
            else SelectTower();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            StopPlacementMode();
        }
    }

    /// <summary>
    /// Gets the selected map position based on the mouse position.
    /// </summary>
    /// <returns>The world position of the selected map location.</returns>
    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayerMask)) {
            lastPosition = hit.point;
        }

        return lastPosition;
    }

    /// <summary>
    /// Starts the placement mode for a specified tower.
    /// </summary>
    /// <param name="towerId">The ID of the tower to be placed.</param>
    public void StartPlacementMode() {
        // Stop any existing placement
        StopPlacementMode();

        // Activate grid visualization
        gridVisualisation.SetActive(true);

        // Assign the selected tower prefab and prepare preview
        selectedTowerIndex = -1;
        selectedTowerIndex = towers.FindIndex(data => data.Id == 0);

        if (selectedTowerIndex > -1) {
            previewObject = Instantiate(towers[selectedTowerIndex].Prefab);
            PreparePreview(previewObject);
        }

        isPlacementMode = true;
        EventBus<StartPlaceMentmodeEvent>.Publish(new StartPlaceMentmodeEvent(this));

        void PreparePreview(GameObject previewObject) {
            // Remove all scripts from the prefab
            MonoBehaviour[] scripts = previewObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts) Destroy(script);

            // Set material to preview material
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++) {
                    materials[i] = previewMaterial;
                }
                renderer.materials = materials;
            }
        }
    }

    /// <summary>
    /// Stops the placement mode.
    /// </summary>
    /// <param name="e">The event that triggered the stop.</param>
    public void StopPlacementMode() {
        // Deactivate grid visualization
        gridVisualisation.SetActive(false);

        // Destroy preview object
        if (previewObject != null) Destroy(previewObject);

        EventBus<ExitPlacementModeEvent>.Publish(new ExitPlacementModeEvent(this));
        isPlacementMode = false;
    }

    private void SelectTower() {
        // Get tower object from the grid mouse position
        Vector3 mousePosition = GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject tower = gridData.GetRepresentationIndex(gridPosition);

        if (tower != null) {
            if (tower == homeTower) {
                return;
            }

            EventBus<SelectTowerEvent>.Publish(new SelectTowerEvent(this, tower));
        }
    }

    /// <summary>
    /// Places a tower at the selected location.
    /// </summary>
    /// <param name="e">The event that triggered the placement.</param>
    private void PlaceTower() {
        //if (IsPointerOverUI()) return;

        Tower tower = towers[selectedTowerIndex];

        Vector3 mousePosition = GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool canBuy = GameManager.instance.CanAfford((int)tower.Cost);
        if (!gridData.CanPlaceObjectAt(gridPosition, tower.Size) || !canBuy) {
            Debug.Log("Invalid placement location!");
            return;
        }

        // Instantiate tower
        GameObject newTower = Instantiate(tower.Prefab, towerParent);
        GameManager.instance.RemoveCurrency((int)tower.Cost);

        // Move tower to position and add to towers in game
        newTower.transform.position = grid.CellToWorld(gridPosition);
        towersInGame.Add(newTower);

        // Update grid data to mark the cells as occupied
        gridData.AddObjectAt(
            gridPosition,
            tower.Size,
            tower.Id,
            newTower
        );

        EventBus<TowerCreatedEvent>.Publish(new TowerCreatedEvent(this, newTower));
        //Debug.Log("Tower placed successfully!");
    }

    /// <summary>
    /// Removes a tower at the specified location.
    /// </summary>
    /// <param name="e">The event that triggered the removal.</param>
    private void RemoveTowerAtLocation(TowerDestroyedEvent e) {
        GameObject tower = e.tower;
        if (tower == homeTower) {
            GameManager.instance.GameOver();
        }

        Vector3Int gridPosition = grid.WorldToCell(tower.transform.position);

        // Remove tower from the grid data and game world
        gridData.RemoveObjectAt(gridPosition);
        towersInGame.RemoveAt(towersInGame.FindIndex(t => t == tower));
        Destroy(tower);
        StopPlacementMode();
        Debug.Log("Tower removed successfully!");
    }

    /// <summary>
    /// Updates the preview of the tower placement.
    /// </summary>
    /// <param name="gridPosition">The grid position to update the preview at.</param>
    private void UpdatePosition(Vector3Int gridPosition) {
        Vector3 position = grid.CellToWorld(gridPosition);
        Color c;

        if (previewObject != null) {
            bool isValid = true;
            bool canBuy = GameManager.instance.CanAfford((int)towers[selectedTowerIndex].Cost);
            if (!gridData.CanPlaceObjectAt(gridPosition, towers[selectedTowerIndex].Size) || !canBuy) isValid = false;
            c = isValid ? Color.white : Color.red;

            // Move preview
            previewObject.transform.position = new Vector3(
                position.x,
                position.y + 0.05f, // Offset to stop texture meshing
                position.z
            );
        }
        else c = Color.white;

        cellIndicator.transform.position = position;

        foreach (Renderer renderer in cellIndicatorRenderer) {
            renderer.material.color = c;
        }

        c.a = 0.5f;
        previewMaterial.color = c;
    }

    /// <summary>
    /// Checks if the pointer is over a UI element.
    /// </summary>
    /// <returns>True if the pointer is over a UI element, false otherwise.</returns>
    //public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    private void OnDestroy() {
        EventBus<TowerDestroyedEvent>.OnEvent -= RemoveTowerAtLocation;
        EventBus<PauseGameEvent>.OnEvent -= GamePaused;
    }
}
