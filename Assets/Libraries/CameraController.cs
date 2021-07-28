using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera[] _cameras;

    private void Start() {
        _cameras = gameObject.GetComponentsInChildren<Camera>();
    }

    public void ShowCamera(int cameraNumber) {
        foreach (var t in _cameras) {
            t.enabled = false;
        }

        _cameras[cameraNumber].enabled = true;
    }
}
