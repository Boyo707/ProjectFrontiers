using System;
using UnityEngine;

public struct GlobalBuffTypes {
    public int Health;
    public int Damage;
    public int Speed;
    public int Gold;
    public float SpawnRate;
}

public class GlobalBuff : MonoBehaviour {
    public static GlobalBuff instance;

    public GlobalBuffTypes globalBuffs;

    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start() {
        EventBus<ResetWavesEvent>.OnEvent += ChangeDifficulty;
    }

    private void ChangeDifficulty(ResetWavesEvent e) {
        Debug.Log("Increase difficulty bro");
    }

    public void SetBuffTo(int percentage) {
        //globalBuff = percentage;

        EventBus<ChangeInGlobalBuffEvent>.Publish(new ChangeInGlobalBuffEvent(this, globalBuffs));
    }
    public void ChangeBuffBy(int percentage) {
        //globalBuff = globalBuff + percentage;

        EventBus<ChangeInGlobalBuffEvent>.Publish(new ChangeInGlobalBuffEvent(this, globalBuffs));
    }
    private void OnDestroy() {
        EventBus<ResetWavesEvent>.OnEvent -= ChangeDifficulty;
    }

}