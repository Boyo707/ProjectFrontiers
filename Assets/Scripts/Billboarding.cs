using UnityEngine;

public class Billboarding : MonoBehaviour
{
    [SerializeField] private Vector3 rotationOffset;
    //12 hours of work
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform, transform.forward);

        if (transform.parent != null)
        {
            //transform.LookAt(Camera.main.transform, transform.parent.transform.forward);
        }

        transform.rotation = Quaternion.Euler(
            transform.localEulerAngles.x + rotationOffset.x,
            180 + rotationOffset.y,
            0 + rotationOffset.z);
    }
}
