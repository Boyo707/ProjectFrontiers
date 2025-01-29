using UnityEngine;

public class Billboarding : MonoBehaviour
{
    //12 hours of work
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform, transform.forward);

        if (transform.parent != null)
        {
            //transform.LookAt(Camera.main.transform, transform.parent.transform.forward);
        }
        else
        {
        }
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 180, 0);
    }
}
