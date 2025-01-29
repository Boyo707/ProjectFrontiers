using UnityEngine;

public class Billboarding : MonoBehaviour
{
    //12 hours of work
    private void LateUpdate()
    {
        if (transform.parent != null)
        {
            transform.LookAt(Camera.main.transform, transform.parent.transform.forward);
        }
        else
        {
            transform.LookAt(Camera.main.transform, transform.forward);
        }
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 180, 0);
    }
}
