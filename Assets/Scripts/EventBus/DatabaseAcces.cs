using UnityEngine;

public class DatabaseAcces : MonoBehaviour {
    public ObjectDatabase database;

    public static DatabaseAcces instance;

    public void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }
    /*
    private void Awake() {
        EventBus<RequestDataEvent<Enemies>>.OnEvent += GetEnemyObject;
    }*/
    public Enemies GetEnemyObject(int enemyid) {
        // probably do some fucking validation
        return database.Enemies[enemyid];
    }
    /*
    public void GetEnemyObject(RequestDataEvent<Enemies> e)
    {
        // probably do some fucking validation
        e.Callback(database.Enemies[e.id]);
    }*/
}
