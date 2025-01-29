using UnityEngine;

public class Billboarding : MonoBehaviour
{
    //12 hours of work
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
