using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour {
    private Animator _animator;
    private Rigidbody _rigidbody;
    private const float MovementForce = 30f;
    private const float MaximumVelocity = 28f;
    private float _normalizeRatio;
    private const float RotateSpeed = 50f;


    private void Awake() {
        _normalizeRatio = 2f / (MaximumVelocity * 100);

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void MoveForward() {
        _rigidbody.AddForce(MovementForce * transform.forward);
        _animator.SetFloat("PlayerForwardMotion", _rigidbody.velocity.magnitude * _normalizeRatio);
        _animator.SetFloat("PlayerSideMotion", 0);
    }

    private void MoveRight() {
        _rigidbody.AddForce(MovementForce * transform.right);
        _animator.SetFloat("PlayerSideMotion", 1f);
    }

    private void MoveLeft() {
        _rigidbody.AddForce(MovementForce * -transform.right);
        _animator.SetFloat("PlayerSideMotion", -1f);
    }

    private void MoveBackwards() {
        _rigidbody.AddForce(MovementForce * -transform.forward);
    }

    private void RotateRight() {
        _rigidbody.transform.Rotate(Vector3.up * (RotateSpeed * Time.deltaTime));
    }

    private void RotateLeft() {
        _rigidbody.transform.Rotate(Vector3.up * (-RotateSpeed * Time.deltaTime));
    }

    private void FixedUpdate() {
        // if (Input.GetKey(KeyCode.W)) {
        //     MoveForward();
        // }
        //
        // if (Input.GetKey(KeyCode.S)) {
        //     MoveBackwards();
        // }
        //
        // if (Input.GetKey(KeyCode.D)) {
        //     MoveRight();
        // }
        //
        // if (Input.GetKey(KeyCode.A)) {
        //     MoveLeft();
        // }
        //
        // if (Input.GetKey(KeyCode.E)) {
        //     RotateRight();
        // }
        //
        // if (Input.GetKey(KeyCode.Q)) {
        //     RotateLeft();
        // }
    }
}
