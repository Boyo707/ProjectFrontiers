using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "MyAssets/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject {
    public List<Tower> Towers;
    public List<TowerUpgrades> TowerUpgrades;
    public List<Enemy> Enemies;
    public List<Wave> Waves;
}

[Serializable]
public class Tower {
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public Sprite UISprite { get; private set; }
    [field: SerializeField]
    public float Cost { get; private set; }
    [field: SerializeField]
    public TowerStats Stats { get; private set; }
}

[Serializable]
public struct TowerStats {
    public int Health;
    public float FireRate;
    public int Damage;
    public float Range;
    public float KillEfficienty;
}

[Serializable]
public struct TowerUpgrade {
    public int Health;
    public int FireRate;
    public int Damage;
    public int Range;
    public int KillEfficienty;
}

[Serializable]
public class TowerUpgrades {
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public int SelectedTower { get; private set; }
    [field: SerializeField]
    public int TowerUpgrade { get; private set; }
    [field: SerializeField]
    public TowerUpgrade StatsNeeded { get; private set; }
}


[Serializable]
public class Enemy {
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public float DifficultyValue { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public EnemyStats Stats { get; private set; }
}

[Serializable]
public class EnemyStats {
    [field: SerializeField]
    public int Health { get; private set; }
    [field: SerializeField]
    public int Speed { get; private set; }
    [field: SerializeField]
    public int Damage { get; private set; }
    [field: SerializeField]
    public int Gold { get; private set; }
}

[Serializable]
public class Wave {
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public float SpawnRate { get; private set; }
    [field: SerializeField]
    public List<EnemyEntry> EnemiesToSpawn { get; private set; }
}

[Serializable]
public struct EnemyEntry {
    public int id;
    public int amount;
}