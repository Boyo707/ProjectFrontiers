using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "MyAssets/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject {
    public List<Towers> Towers;
    public List<Enemies> Enemies;
}

[Serializable]
public class Towers {
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
    [field: SerializeField]
    public int MaxHealth { get; private set; }
    [field: SerializeField]
    public int BonusHealth { get; private set; }
    [field: SerializeField]
    public int FireRate { get; private set; }
    [field: SerializeField]
    public int TurnSpeed { get; private set; }
    [field: SerializeField]
    public int Damage { get; private set; }
    [field: SerializeField]
    public int KillEfficienty { get; private set; }
    [field: SerializeField]
    public int Range { get; private set; }
}

[Serializable]
public class Enemies {
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int Id { get; private set; }
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
