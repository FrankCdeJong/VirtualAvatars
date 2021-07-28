using System;
using System.Collections;
using System.Collections.Generic;
using AvatarActions;
using AvatarFacialExpressions;
using UnityEngine;

public class VirtualiseExample : MonoBehaviour {
    // We read our model here already
    private static readonly ModelData Data = new ModelData(@"./Assets/Scenes/Example/modelData.csv");

    // The scenario has two avatars
    public GameObject avatarA;
    public GameObject avatarB;

    // Each avatar has an emotion controller
    private EmotionController _avatarAEmotionController;
    private EmotionController _avatarBEmotionController;

    public CameraController cameraController;

    // We read the number of rows of data.
    private readonly int _max = Data.GetColumnData("time").Count;

    // We read each expression state that is relevant to the virtualization.
    private readonly List<float> _x11Break = Data.GetColumnData("X11 - Break");
    private readonly List<float> _x12Anger = Data.GetColumnData("X12 - anger");
    private readonly List<float> _x13Threaten = Data.GetColumnData("X13 - threaten");
    private readonly List<float> _x21Gazeaway = Data.GetColumnData("X21 - gazeaway");
    private readonly List<float> _x22Walkaway = Data.GetColumnData("X22 - walkaway ");

    // An avatar has an walk away action
    private AvatarAction _walkAway;
    private LookAway _lookAway;

    private void ModelDataToAvatar(float x11, float x12, float x13, float x21, float x22) {
        // Once our walk away state reaches the threshold of 0.5 we trigger walking away. 
        if (x22 >= 0.5) {
            if (!_walkAway.Triggered) StartCoroutine(_walkAway.Trigger());
        }

        StartCoroutine(_lookAway.SetIntensity(x21));

        // Each row we read a new angry state so we have to set it each time.
        StartCoroutine(_avatarAEmotionController.SetEmotion("angry", x12 * 100));

        // TODO: x11, x13
    }

    private IEnumerator ApplyDataToAvatars(float yieldTime) {
        // Calling this function twice will result in some weird behavior. Maybe there should be a check to prevent
        // this from happening accidentally? TODO

        // We loop through each row of the data and get each value in that row. We call a function which will apply
        // the data to the avatars.
        // i = 45 (we skip the first 45 rows of all zeros)
        for (var i = 45; i < _max; i++) {
            // Camera are triggered based on this scenario.
            if (i > 50 && i < 70)
                cameraController.ShowCamera(1);
            if (i > 70 && i < 100)
                cameraController.ShowCamera(0);
            if (i > 100 && i < 130)
                cameraController.ShowCamera(1);
            if (i > 130 && i < 180)
                cameraController.ShowCamera(0);
            if (i > 180 && i < 205)
                cameraController.ShowCamera(1);
            if (i > 205 && i < 215)
                cameraController.ShowCamera(2);
            if (i > 215)
                cameraController.ShowCamera(0);

            ModelDataToAvatar(_x11Break[i], _x12Anger[i], _x13Threaten[i], _x21Gazeaway[i], _x22Walkaway[i]);

            // We wait for yieldTime. After the execution continues in this loop having kept all the values.
            // This way we arent' busy waiting or have to do some complicated time keeping ourselves.
            yield return new WaitForSeconds(yieldTime);
        }

        yield return true;
    }

    private void Start() {
        // IMPORTANT: avatarA and avatarB need to be set inside of Unity.

        // Read the emotion controllers
        _avatarAEmotionController = avatarA.GetComponent<AvatarEmotionController>().Controller;
        _avatarBEmotionController = avatarB.GetComponent<AvatarEmotionController>().Controller;

        // Double check that the PlayerEmotionController script is added to both avatars. (This could go wrong if the 
        // scripts aren't loaded yet)
        if (_avatarAEmotionController == null)
            throw new Exception(
                "Avatar A does not have an PlayerEmotionController script. Without this attached we cannot set the emotions");

        if (_avatarBEmotionController == null)
            throw new Exception(
                "Avatar B does not have an PlayerEmotionController script. Without this attached we cannot set the emotions");

        // Read time data.
        var time = Data.GetColumnData("time");
        if (time == null) {
            Debug.LogWarning("We have not read time data!");
            return;
        }

        cameraController.ShowCamera(2);

        // We initialize the walk away action with its Body, Animator and Transform objects
        _walkAway = new WalkAway(avatarB.GetComponent<Rigidbody>(), avatarB.GetComponent<Animator>(),
            avatarB.GetComponent<Transform>());

        _lookAway = new LookAway(avatarB.GetComponent<Rigidbody>(), avatarB.GetComponent<Animator>(),
            avatarB.GetComponent<Transform>());

        // Test action: trigger a simple animation
        StartCoroutine(new AngryPoint(avatarA.GetComponent<Rigidbody>(), avatarA.GetComponent<Animator>(),
            avatarA.GetComponent<Transform>()).Trigger());

        StartCoroutine(ApplyDataToAvatars(0.2f));
    }
}
