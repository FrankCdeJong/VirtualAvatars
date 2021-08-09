using AvatarFacialExpressions;
using UnityEngine;

/// <summary>
/// This class is necessary to "attach" the EmotionController class to the Avatar. Once the Awake event is called
/// a new instance of the EmotionController class is created using the game object this class is attached to. To access
/// the EmotionController class use the GetComponent function:
/// gameObject.GetComponent&lt;AvatarEmotionController&lt;().Controller; this returns an instance of EmotionController.
/// </summary>
public class AvatarEmotionController : MonoBehaviour {
    public EmotionController Controller;

    private void Awake() {
        var ourObject = gameObject;
        Controller = new EmotionController(ourObject);
    }
}
