using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
    
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;
    private Vector3 lastPosition;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            OnExit?.Invoke();
        }
    }

    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayerMask)) {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
