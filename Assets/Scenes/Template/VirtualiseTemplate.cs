using System;
using System.Collections;
using System.Collections.Generic;
using AvatarActions;
using AvatarFacialExpressions;
using UnityEngine;

public class VirtualiseTemplate : MonoBehaviour {
    // First time to do is read the simulation data
    private static readonly ModelData Data = new ModelData(@"./Assets/Scenes/Template/modelData.csv");

    // The virtualisation can have any number of avatars. Public variables can be assigned inside the Unity Editor.
    // This way we can easily specify in the scene what avatars will be used. Private variables cannot be accessed in 
    // the Unity Editor and instead need to be assigned inside the script.
    public GameObject avatarA;

    public GameObject avatarB;

    public GameObject avatarC;
    // public GameObject avatarD;

    // Each avatar has an emotion controller and we need to create a reference to access them.
    // If an avatar does not need to express an emotion than this reference is not necessary. 
    private EmotionController _avatarAEmotionController;
    private EmotionController _avatarBEmotionController;
    // private EmotionController _avatarCEmotionController;

    // If we want to set the cameras specific to this scenario than adding a reference is also necessary. In the scene 
    // there is a empty game object called "cameras". This object can have any number of child objects of type Camera.
    // Now the camera controller script has a function to set the active camera. It takes a number as argument referring
    // to the index of child cameras. Setting the active camera to 0 will set the first camera in the list active. 
    // Setting the active camera to 2 will set it to the third camera in the list.
    public CameraController cameraController;

    // We read the number of rows of data. The `GetColumnData` reads a specific row in the CSV and needs to be set
    // according for each different virtualisation scenario. In our template this row is called "time". The `.Count`
    // returns us the number of rows in this column.
    private readonly int _max = Data.GetColumnData("time").Count;

    // We read each expression state that is relevant to the virtualisation.
    private readonly List<float> _row1 = Data.GetColumnData("Row 1");
    private readonly List<float> _row2 = Data.GetColumnData("Row 2");
    private readonly List<float> _row3 = Data.GetColumnData("Row 3");
    private readonly List<float> _row4 = Data.GetColumnData("Row 4");
    private readonly List<float> _row5 = Data.GetColumnData("Row 5");
    private readonly List<float> _row6 = Data.GetColumnData("Row 6");
    private readonly List<float> _row7 = Data.GetColumnData("Row 7");

    // Here we specific what actions can going to be used in the virtualisation. Custom actions can be added to the 
    // AvatarActions library in the "Libraries" folder. Any number of actions can be added.
    private AvatarAction _action1;
    private AvatarAction _action2;
    private LookAway _action3;
    private AvatarEyeMoveAction _action4;

    private void Start() {
        // The start function is a Unity event automatically triggered when the scene is run. Here we set up all our
        // references and trigger the virtualisation to start.

        // Each avatar will have an AvatarEmotionController applied to it. Therefore we can use the GetComponent 
        // function to get a reference to this. However if this is somehow missing, an null pointer exception will be
        // raised.
        _avatarAEmotionController = avatarA.GetComponent<AvatarEmotionController>().Controller;
        _avatarBEmotionController = avatarB.GetComponent<AvatarEmotionController>().Controller;
        // _avatarCEmotionController = avatarC.GetComponent<AvatarEmotionController>().Controller;

        // Double check that the PlayerEmotionController script is added to both avatars. (This could go wrong if the 
        // scripts aren't loaded yet)
        if (_avatarAEmotionController == null)
            throw new Exception(
                "Avatar A does not have an PlayerEmotionController script. Without this attached we cannot set the emotions");

        if (_avatarBEmotionController == null)
            throw new Exception(
                "Avatar B does not have an PlayerEmotionController script. Without this attached we cannot set the emotions");

        // if (_avatarCEmotionController == null)
        //    throw new Exception(
        //        "Avatar C does not have an PlayerEmotionController script. Without this attached we cannot set the emotions");


        // Here we can add additional check to make sure that we have correctly loaded the simulation data. 
        // We read the column "time" and check to make sure it exists.
        var time = Data.GetColumnData("time");
        if (time == null) {
            Debug.LogWarning("We have not read time data!");
            return;
        }

        // We set the first camera to be active.
        cameraController.ShowCamera(0);

        // The first action we add is the Angry point action. It triggers an animation on the avatar we want. In this 
        // case we want avatar A to perform this action later so we initialize it with avatar A's properties.
        _action1 = new AvatarMakeAngryPointGesture(avatarA.GetComponent<Rigidbody>(), avatarA.GetComponent<Animator>(),
            avatarA.GetComponent<Transform>());

        // To prevent us from repeating the same GetComponent function every time we can create some local variables.
        var rigidBodyA = avatarA.GetComponent<Rigidbody>();
        var animatorA = avatarA.GetComponent<Animator>();
        var transformA = avatarA.GetComponent<Transform>();

        // Now we can simply pass in the same information easily improving the readability.
        _action2 = new AvatarMakeAngryGesture(rigidBodyA, animatorA, transformA);

        // We repeat this for as many actions as we need
        var rigidBodyB = avatarB.GetComponent<Rigidbody>();
        var animatorB = avatarB.GetComponent<Animator>();
        var transformB = avatarB.GetComponent<Transform>();
        _action3 = new LookAway(rigidBodyB, animatorB, transformB);

        _action4 = new AvatarEyeMoveAction(rigidBodyA, animatorA, transformA);

        // Now that we have all our references set up we can trigger the virtualisation. We need to create a step
        // function that takes all the simulation data and applies it to the avatars.  
        // We trigger this function at a specific interval. In this template we use a 200ms interval but it can be
        // changed to any time interval.
        StartCoroutine(ApplyDataToAvatars(0.2f));

        // StartCoroutine(_avatarCEmotionController.SetEmotion("angry", 100));
    }

    private IEnumerator ApplyDataToAvatars(float yieldTime) {
        // We loop through each row of the data and get each value in that row. We call a function which will apply
        // the data to the avatars.
        for (var i = 0; i < _max; i++) {
            // If we want to set certain cameras to activate a specific time we do it as following
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

            // Now we call a function that applies our actions to the avatars using the data in the current step
            ModelDataToAvatar(_row1[i], _row2[i], _row3[i], _row4[i], _row5[i]);

            // We wait for yieldTime. After the execution continues in this loop having kept all the values.
            // This way we arent' busy waiting or have to do some complicated time keeping ourselves.
            yield return new WaitForSeconds(yieldTime);
        }

        // Exit the application after we have applied all simulation data
        // if (Application.isEditor)
        //     UnityEditor.EditorApplication.isPlaying = false;
        // else
        //     Application.Quit();
        Debug.Log("Read all simulation data");

        yield return true;
    }

    private void ModelDataToAvatar(float row1, float row2, float row3, float row4, float row5) {
        // To trigger an action we can set some specific condition
        if (row1 > 0.5) {
            // Then to trigger the action
            if (!_action1.Triggered) StartCoroutine(_action1.Trigger());
        }

        if (!_action1.Triggered) StartCoroutine(_action2.Trigger());

        // We can set the avatars emotion to a specific intensity as follows:
        StartCoroutine(_avatarAEmotionController.SetEmotion("angry", row2 * 100));

        // Certain actions can have custom functions
        StartCoroutine(_action3.SetIntensity(row1));

        StartCoroutine(_action4.SetEyeXOrYIntensity(75, 0));
    }
}
