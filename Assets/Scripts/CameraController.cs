using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera _cameraA;
    private Camera _cameraB;
    private Camera _cameraOverview;

    private void Start() {
        var cameras = gameObject.GetComponentsInChildren<Camera>();
        _cameraA = cameras[0];
        _cameraB = cameras[1];
        _cameraOverview = cameras[2];
    }

    public void ShowCameraA() {
        _cameraA.enabled = true;
        _cameraB.enabled = false;
        _cameraOverview.enabled = false;
    }

    public void ShowCameraB() {
        _cameraA.enabled = false;
        _cameraB.enabled = true;
        _cameraOverview.enabled = false;
    }

    public void ShowOverheadCamera() {
        _cameraA.enabled = false;
        _cameraB.enabled = false;
        _cameraOverview.enabled = true;
    }
}
