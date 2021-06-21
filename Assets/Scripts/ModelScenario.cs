using System;
using System.Collections;
using FacialExpressions;
using UnityEngine;

public class ModelScenario : MonoBehaviour {
    private readonly ModelData _data = new ModelData();
    public GameObject avatarA;
    public GameObject avatarB;

    private EmotionController GetEmotionController(GameObject avatar) {
        return avatar.GetComponent<PlayerEmotionController>().Controller;
    }

    private void ModelDataToAvatar(float x11, float x12, float x13, float x21, float x22) {
        // Debug.Log(x11 + " " + x12 + " " + x13 + " " + x21 + " " + x22);
        var e = GetEmotionController(avatarA);
        StartCoroutine(e.SetEmotion("angry", x12 * 100));
    }

    private IEnumerator ApplyDataToAvatars(float yieldTime) {
        // Calling this function twice will result in some weird behavior. Maybe there should be a check to prevent
        // this from happening accidentally? TODO

        // We collect each column and assign it to a list of floats
        var max = _data.GetColumnData("time").Count;
        var x11Break = _data.GetColumnData("X11 - Break");
        var x12Anger = _data.GetColumnData("X12 - anger");
        var x13Threaten = _data.GetColumnData("X13 - threaten");
        var x21Gazeaway = _data.GetColumnData("X21 - gazeaway");
        var x22Walkaway = _data.GetColumnData("X22 - walkaway ");

        for (var i = 0; i < max; i++) {
            // Here we get all values in one row and call a function which converts the data to some action of the 
            // avatar.
            ModelDataToAvatar(x11Break[i], x12Anger[i], x13Threaten[i], x21Gazeaway[i], x22Walkaway[i]);

            // We wait for yieldTime. After the execution continues in this loop having kept all the values.
            // This way we arent' busy waiting or have to do some complicated time keeping ourselves.
            yield return new WaitForSeconds(yieldTime);
        }

        yield return true;
    }

    private void Start() {
        // Here we read the model data. The data is stored in the ModelData class called _data;
        _data.ReadCsv();
        foreach (var c in _data.columnHeaders) {
            Debug.Log(c);
        }

        // Double check that the PlayerEmotionController script is added to both avatars
        if (GetEmotionController(avatarA) == null) {
            throw new Exception("Avatar A does not have an PlayerEmotionController script. " +
                                "Without this attached we cannot set the emotions");
        }

        if (GetEmotionController(avatarB) == null) {
            throw new Exception("Avatar B does not have an PlayerEmotionController script. " +
                                "Without this attached we cannot set the emotions");
        }

        var time = _data.GetColumnData("time");
        if (time == null) {
            Debug.LogWarning("We have not read time data!");
            return;
        }

       
        StartCoroutine(ApplyDataToAvatars(0.5f));
    }
}
