using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public ObjectDatabase database;
    public List<GameObject> towersInGame;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void OnEnable() {
        EventBus<TowerCreatedEvent>.OnEvent += AddToTowerList;
        EventBus<TowerDestroyedEvent>.OnEvent += RemoveFromTowerList;
    }

    private void AddToTowerList(TowerCreatedEvent e) {
        towersInGame.Add(e.tower);
    }

    private void RemoveFromTowerList(TowerDestroyedEvent e) {
        towersInGame.Remove(e.tower);
    }

    private void OnDestroy() {
        EventBus<TowerCreatedEvent>.OnEvent -= AddToTowerList;
        EventBus<TowerDestroyedEvent>.OnEvent -= RemoveFromTowerList;
    }
}
