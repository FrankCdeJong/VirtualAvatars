using System;
using FacialExpressions;
using UnityEngine;

public class AvatarGazeAway : MonoBehaviour {
    private EyeMovementController _eyes;
    private const int MaxLookAwayX = 70;
    private const int MaxLookAwayY = -50;

    private void Awake() {
        var ourObject = gameObject;
        _eyes = new EyeMovementController(ourObject);
    }

    public void GazeAway(float intensity) {
        var x = MaxLookAwayX * intensity;
        var y = MaxLookAwayY * intensity;
        StartCoroutine(_eyes.SetEyeXOrYPosition("x", (int) Math.Round(x), 0.5f));
        StartCoroutine(_eyes.SetEyeXOrYPosition("y", (int) Math.Round(y), 0.5f));
    }
}
