using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public List<GameObject> PlacedGameObjects
    {
        get { return placedGameObjects; }
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex ||
            placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newGameobject = Instantiate(prefab);
        newGameobject.transform.position = position;
        placedGameObjects.Add(newGameobject);
        return placedGameObjects.Count - 1;
    }
}
