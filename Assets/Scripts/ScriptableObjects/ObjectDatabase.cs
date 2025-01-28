using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "MyAssets/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject {
    public List<Tower> Towers;
    public List<TowerUpgrades> TowerUpgrades;
    public List<Enemy> Enemies;
    public List<Wave> Waves;
}

/// <summary>
/// Tower storage
/// </summary>
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
public class TowerStats {
    public int Health;
    public int Damage;
    public float FireRate;
    public float Range;
}

/// <summary>
/// Tower upgrade data
/// </summary>
[Serializable]
public class TowerUpgrades {
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public int SelectedTower { get; private set; }
    [field: SerializeField]
    public int NewTower { get; private set; }
    [field: SerializeField]
    public TowerUpgradeLevels StatsNeeded { get; private set; }
}

[Serializable]
public class TowerUpgradeLevels {
    [Range(0, 10)]
    public int Health = 1;
    [Range(0, 10)]
    public int Damage = 1;
    [Range(0, 10)]
    public int FireRate = 1;
    [Range(0, 10)]
    public int Range = 1;
}

/// <summary>
/// Enemy data
/// </summary>
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
    public int Health;
    public int Damage;
    public int Speed;
    public float FireRate;
    public float Range;
    public float Experience;
}

/// <summary>
/// Wave information
/// </summary>
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
public class EnemyEntry {
    public int id;
    public int amount;
}