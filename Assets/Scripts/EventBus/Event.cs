using System;
using UnityEngine;

public class Event {
    public object Source { get; private set; }

    protected Event(object source) {
        Source = source;
    }
}

#region enemy events
/// <summary>
/// Event triggered when enemy is spawned, takes in spawned enemy as parameter
/// </summary>
public class EnemySpawnedEvent : Event {
    public int enemyId;
    public EnemySpawnedEvent(object source, int enemyId) : base(source) {
        this.enemyId = enemyId;
    }
}

public class EnemyKilledEvent : Event {
    public EnemyKilledEvent(object source) : base(source) {
    }
}

#endregion

#region turret events


#endregion

#region difficulty events
public class IncreaseDifficultyEvent : Event {
    public float Difficulty;
    public IncreaseDifficultyEvent(object source, float Difficulty) : base(source) {
        this.Difficulty = Difficulty;
    }
}

public class DecreaseDifficultyEvent : Event {
    public float Difficulty;
    public DecreaseDifficultyEvent(object source, float Difficulty) : base(source) {
        this.Difficulty = Difficulty;
    }
}
#endregion















public class RequestDataEvent<T> : Event where T : class {
    public Action<T> Callback;
    public int id;
    public RequestDataEvent(object source, Action<T> callback, int id) : base(source) {
        Callback = callback;
        this.id = id;
    }
}

/*
#region Game flow events

public class GamePausedEvent : Event {
    public GamePausedEvent(object source) : base(source) {
    }
}

public class GameResumedEvent : Event {
    public GameResumedEvent(object source) : base(source) {
    }
}

/// <summary>
/// Event triggered when the game is lost.
/// </summary>
public class GameOverEvent : Event {
    public GameOverEvent(object source) : base(source) {
    }
}

public class GameWonEvent : Event {
    public GameWonEvent(object source) : base(source) {
    }
}
#endregion

#region Wave management events
/// <summary>
/// Event triggered when a new wave starts, holding the wave number
/// </summary>
public class WaveStartedEvent : Event {
    public int WaveNumber;
    public WaveStartedEvent(object source, int waveNumber) : base(source) {
        WaveNumber = waveNumber;
    }
}

public class WaveEndedEvent : Event {
    public WaveEndedEvent(object source) : base(source) {
    }
}

/// <summary>
/// Event triggered every second during the countdown before a wave starts.
/// </summary>
public class WaveCountdownEvent : Event {
    public int RemainingCountdown;
    public WaveCountdownEvent(object source, int remainingCountdown) : base(source) {
        RemainingCountdown = remainingCountdown;
    }
}
#endregion

#region Enemy events
/// <summary>
/// Event triggered when enemy is spawned, takes in spawned enemy as parameter
/// </summary>
public class EnemySpawnedEvent : Event {
    public Enemy spawnedEnemy;
    public EnemySpawnedEvent(object source, Enemy spawnedEnemy) : base(source) {
        this.spawnedEnemy = spawnedEnemy;
    }
}

/// <summary>
/// Event triggered when an enemy reaches the end of the path.
/// </summary>
public class EnemyReachedEndEvent : Event {
    public EnemyData enemyData;
    public EnemyReachedEndEvent(object source, EnemyData data) : base(source) {
        enemyData = data;
    }
}

public class EnemyKilledEvent : Event {
    EnemyData data;
    public EnemyKilledEvent(object source, EnemyData data) : base(source) {
        this.data = data;
    }
}
#endregion

#region Tile events
/// <summary>
/// Event triggered when a tile is selected, when tile == null no tile is selected.
/// </summary>
public class TileSelectedEvent : Event {
    public GameObject Tile;
    public TileSelectedEvent(object source, GameObject tile) : base(source) {
        Tile = tile;
    }
}

#endregion

#region Tower events
public class TowerPlacedEvent : Event {
    public TowerPlacedEvent(object source) : base(source) {
    }
}

public class TowerSoldEvent : Event {
    public TowerSoldEvent(object source) : base(source) {
    }
}
/// <summary>
/// Event triggered when a tower is upgraded.
/// </summary>
public class TowerUpgradedEvent : Event {
    public int cost;
    public TowerUpgradedEvent(object source, int cost) : base(source) {
        this.cost = cost;
    }
}

/// <summary>
/// Event triggered when a tower is selected.
/// </summary>
public class TowerSelectedEvent : Event {
    public TowerData tower;
    public TowerSelectedEvent(object source, TowerData tower) : base(source) {
        this.tower = tower;
    }
}

public class TowerDeselectedEvent : Event {
    public TowerDeselectedEvent(object source) : base(source) {
    }
}

/// <summary>
/// Event triggered when a tower is purchased.
/// </summary>
public class TowerPurchaseEvent : Event {
    public TowerData TowerData;
    public TowerPurchaseEvent(object source, TowerData data) : base(source) {
        this.TowerData = data;
    }
}
#endregion

#region Resource events

/// <summary>
/// Event triggered when the gold value changes.
/// </summary>
public class GoldChangedEvent : Event {
    public int NewValue;
    public GoldChangedEvent(object source, int newValue) : base(source) {
        NewValue = newValue;
    }
}

/// <summary>
/// Event triggered when the health value changes.
/// </summary>
public class HealthChangedEvent : Event {
    public int NewValue;
    public HealthChangedEvent(object source, int newValue) : base(source) {
        NewValue = newValue;
    }
}
#endregion

 */