using FacialExpressions;
using UnityEngine;

public class AvatarEmotionController : MonoBehaviour {
    public EmotionController Controller;

    private void Awake() {
        var ourObject = gameObject;
        Controller = new EmotionController(ourObject);
    }
}
