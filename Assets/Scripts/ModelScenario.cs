using System;
using System.Collections;
using System.Collections.Generic;
using AvatarActions;
using FacialExpressions;
using UnityEngine;

public class ModelScenario : MonoBehaviour {
    private static readonly ModelData Data = new ModelData();
    public GameObject avatarA;
    public GameObject avatarB;

    private EmotionController _avatarAEmotionController;
    private EmotionController _avatarBEmotionController;

    private readonly int _max = Data.GetColumnData("time").Count;
    private readonly List<float> _x11Break = Data.GetColumnData("X11 - Break");
    private readonly List<float> _x12Anger = Data.GetColumnData("X12 - anger");
    private readonly List<float> _x13Threaten = Data.GetColumnData("X13 - threaten");
    private readonly List<float> _x21Gazeaway = Data.GetColumnData("X21 - gazeaway");
    private readonly List<float> _x22Walkaway = Data.GetColumnData("X22 - walkaway ");

    private AvatarAction _walkAway;

    private void ModelDataToAvatar(float x11, float x12, float x13, float x21, float x22) {
        // Debug.Log(x11 + " " + x12 + " " + x13 + " " + x21 + " " + x22);
        if (x22 >= 0.5) {
            if (!_walkAway.Triggered) StartCoroutine(_walkAway.Trigger());
        }

        StartCoroutine(_avatarAEmotionController.SetEmotion("angry", x12 * 100));
    }

    private IEnumerator ApplyDataToAvatars(float yieldTime) {
        // Calling this function twice will result in some weird behavior. Maybe there should be a check to prevent
        // this from happening accidentally? TODO

        for (var i = 45; i < _max; i++) {
            // Here we get all values in one row and call a function which converts the data to some action of the 
            // avatar.
            ModelDataToAvatar(_x11Break[i], _x12Anger[i], _x13Threaten[i], _x21Gazeaway[i], _x22Walkaway[i]);

            // We wait for yieldTime. After the execution continues in this loop having kept all the values.
            // This way we arent' busy waiting or have to do some complicated time keeping ourselves.
            yield return new WaitForSeconds(yieldTime);
        }

        yield return true;
    }

    private void Start() {
        _avatarAEmotionController = avatarA.GetComponent<PlayerEmotionController>().Controller;
        _avatarBEmotionController = avatarB.GetComponent<PlayerEmotionController>().Controller;

        // Double check that the PlayerEmotionController script is added to both avatars. (This could go wrong if the 
        // scripts aren't loaded yet)
        if (_avatarAEmotionController == null) {
            throw new Exception("Avatar A does not have an PlayerEmotionController script. " +
                                "Without this attached we cannot set the emotions");
        }

        if (_avatarBEmotionController == null) {
            throw new Exception("Avatar B does not have an PlayerEmotionController script. " +
                                "Without this attached we cannot set the emotions");
        }

        var time = Data.GetColumnData("time");
        if (time == null) {
            Debug.LogWarning("We have not read time data!");
            return;
        }

        _walkAway = new WalkAway(avatarB.GetComponent<Rigidbody>(), avatarB.GetComponent<Animator>(),
            avatarB.GetComponent<Transform>());

        StartCoroutine(new AngryPoint(avatarA.GetComponent<Rigidbody>(), avatarA.GetComponent<Animator>(),
            avatarA.GetComponent<Transform>()).Trigger());

        StartCoroutine(ApplyDataToAvatars(0.5f));
    }
}
