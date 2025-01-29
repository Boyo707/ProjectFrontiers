using UnityEngine;

public class BuffTower : TowerBase {
    private int lastDamageBuff; 

    protected override void Start() {
        base.Start();
        isBuffTower = true;
    }

    protected override void ApplyEffectToTowersInRange() {
        int currentDamageBuff = stats.Damage;

        var towersInRange = GetTowersInRange();
        foreach (var tower in towersInRange) {
            tower.stats.Damage -= lastDamageBuff;
            tower.stats.Damage += currentDamageBuff;
        }

        lastDamageBuff = currentDamageBuff;
    }

    protected override void Shoot() {
    }

    
}
