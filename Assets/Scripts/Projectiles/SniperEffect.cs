using UnityEngine;

public class SniperEffect : MonoBehaviour
{
    public void DestroyOnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
