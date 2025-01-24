using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public ObjectDatabase database;
    public int currency = 0;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }
}
