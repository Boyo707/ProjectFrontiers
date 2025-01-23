using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    [SerializeField] private ObjectDatabase database;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private Grid grid;

    private int selectedObjectIndex = -1;
    private bool CheckPlacementValidity(GridData towerData, Vector3Int gridPosition, int selectedObjectIndex)
    {
        return towerData.CanPlaceObjectAt(gridPosition, database.Towers[selectedObjectIndex].Size);
    }

    public void AssignPrefab(int Id)
    {
        selectedObjectIndex = database.Towers.FindIndex(data => data.Id == Id);

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingplacementPreview(database.Towers[selectedObjectIndex].Prefab,
                database.Towers[selectedObjectIndex].Size);
        }
    }
    public void EndPreview()
    {
        previewSystem.StopShowingPreview();
    }
    public void UpdateState(GridData towerData, Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(towerData, gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
    
}
