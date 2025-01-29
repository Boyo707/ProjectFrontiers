using UnityEngine;

public class RegenTower : TowerBase {
    private float healing = 0f;
    private int lastDamageBuff;

    protected override void Start() {
        base.Start();
        isBuffTower = true;
    }

    protected override void Update() {
        base.Update();
        healing += 1 / stats.Health * Time.deltaTime;
    }

    protected override void ApplyEffectToTowersInRange() {
        int currentDamageBuff = stats.Damage;

        var towersInRange = GetTowersInRange();
        foreach (var tower in towersInRange) {
            tower.stats.Damage -= lastDamageBuff;
            tower.stats.Damage += currentDamageBuff;

            if (healing >= 1) {
                int toHeal = Mathf.FloorToInt(healing);
                HealTower(toHeal);
                healing -= toHeal;
            }
        }

        lastDamageBuff = currentDamageBuff;
    }

    protected override void Shoot() {
    }


}
