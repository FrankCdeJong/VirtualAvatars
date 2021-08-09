using UnityEngine;

/// <summary>
/// CameraController is attached to an object which contains several Camera game objects. This class allows a camera to
/// be enabled.
/// </summary>
public class CameraController : MonoBehaviour {
    private Camera[] _cameras;

    /// <summary>
    /// When the game is run this gets the object's children's Camera objects and adds them to a list
    /// </summary>
    private void Start() {
        _cameras = gameObject.GetComponentsInChildren<Camera>();
    }

    /// <summary>
    /// This function enables a camera from a list of cameras.
    /// </summary>
    /// <param name="cameraNumber">The index value of cameras list</param>
    public void ShowCamera(int cameraNumber) {
        foreach (var t in _cameras) {
            t.enabled = false;
        }

        _cameras[cameraNumber].enabled = true;
    }
}
