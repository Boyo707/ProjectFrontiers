using UnityEngine;

public class GlobalBuff : MonoBehaviour {
    public static GlobalBuff instance;

    public static int globalBuff = 0;

    public void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void SetBuffTo(int percentage) {
        globalBuff = percentage;

        EventBus<ChangeInGlobalBuffEvent>.Publish(new ChangeInGlobalBuffEvent(this, percentage));
    }
    public void ChangeBuffBy(int percentage) {
        globalBuff = globalBuff + percentage;

        EventBus<ChangeInGlobalBuffEvent>.Publish(new ChangeInGlobalBuffEvent(this, percentage));
    }
}
