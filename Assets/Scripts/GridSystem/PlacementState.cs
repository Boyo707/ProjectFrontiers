using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase database;
    GridData floorData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabase database,
                          GridData floorData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.Towers.FindIndex(data => data.Id == ID);

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingplacementPreview(database.Towers[selectedObjectIndex].Prefab,
                database.Towers[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {ID}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            Debug.Log("Im already occupied");
            return;
        }

        int index = objectPlacer.PlaceObject(database.Towers[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));


        GridData selectData = floorData;

        selectData.AddObjectAt(gridPosition,
            database.Towers[selectedObjectIndex].Size,
            database.Towers[selectedObjectIndex].Id,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = floorData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.Towers[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
