using System;
using UnityEngine;
public class Event {
    public object Source { get; private set; }

    protected Event(object source) {
        Source = source;
    }
}

#region GridManager events
public class PlaceTowerEvent : Event {
    public int towerId;
    public PlaceTowerEvent(object source, int towerId) : base(source) {
        this.towerId = towerId;
    }
}
public class SelectTowerEvent : Event {
    public GameObject tower;
    public SelectTowerEvent(object source, GameObject tower) : base(source) {
        this.tower = tower;
    }
}
public class StartPlaceMentmodeEvent : Event {
    public StartPlaceMentmodeEvent(object source) : base(source) {
    }
}
public class ExitPlacementModeEvent : Event {
    public ExitPlacementModeEvent(object source) : base(source) {
    }
}


#endregion


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
    public int enemyId;
    public EnemyKilledEvent(object source, int enemyId) : base(source) {
        this.enemyId = enemyId;
    }
}
public class WaveCompletedEvent : Event {
    public int waveIndex;
    public WaveCompletedEvent(object source, int waveIndex) : base(source) {
        this.waveIndex = waveIndex;
    }
}
public class ResetWavesEvent : Event {
    public ResetWavesEvent(object source) : base(source) {
    }
}

#endregion

#region turret events
public class TowerCreatedEvent : Event {
    public GameObject tower;

    public TowerCreatedEvent(object source, GameObject tower) : base(source) {
        this.tower = tower;
    }
}

public class TowerUpgradedEvent : Event {
    public GameObject tower;

    public TowerUpgradedEvent(object source, GameObject tower) : base(source) {
        this.tower = tower;
    }
}

public class TowerDestroyedEvent : Event {
    public GameObject tower;
    public TowerDestroyedEvent(object source, GameObject tower) : base(source) {
        this.tower = tower;
    }
}

#endregion

#region difficulty events
public class ChangeInGlobalBuffEvent : Event {
    public GlobalBuffTypes globalBuff;
    public ChangeInGlobalBuffEvent(object source, GlobalBuffTypes globalBuff) : base(source) {
        this.globalBuff = globalBuff;
    }
}

public class ChangeInDifficultyBuffEvent : Event {
    public int difficulty;
    public ChangeInDifficultyBuffEvent(object source, int difficulty) : base(source) {
        this.difficulty = difficulty;
    }
}

#endregion


#region Currency

public class ChangeInCurrencyEvent : Event {
    public float currency;
    public ChangeInCurrencyEvent(object source, float currency) : base(source) {
        this.currency = currency;
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