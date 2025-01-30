using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public ObjectDatabase database;
    private int currency = 100;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public int GetCurrency() {
        return currency;
    }

    public bool CanAfford(int cost) {
        return currency >= cost;
    }

    public void AddCurrency(int amount) {
        currency += amount;
    }

    public void RemoveCurrency(int amount) {
        if (CanAfford(amount)) currency -= amount;
    }
}
