using Unity.VisualScripting;
using UnityEngine;

public class TowerLocations : MonoBehaviour {
    public static Transform[] towers;

    private void Awake() {
        GetAllTowerLocations();
    }

    private void GetAllTowerLocations() {
        towers = new Transform[transform.childCount];

        for (int i = 0; i < towers.Length; i++) {
            towers[i] = transform.GetChild(i).GetChild(0);
        }
    }

    public void AddTowerLocation(GameObject tower) {
        towers.AddRange(tower.transform.GetChild(0));
        towers[towers.Length-1] = tower.transform.GetChild(0);

        foreach (Transform x in towers) {
            Debug.Log(x);
        }
    }
}
