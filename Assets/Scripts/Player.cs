using UnityEngine;

/*
 * work on components
    facial expression
    spawning characters
    actions (animations)
    reading model data
    allowing ppl to import their own character and linking it
 */

public class Player : MonoBehaviour {
    private bool jump;
    private float horizontalVelocity;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     jump = true;
        // }
        //
        // horizontalVelocity = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        // if (jump) {
        //     rigidBody.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
        //     jump = false;
        // }
        //
        // Vector3 velocity = rigidBody.velocity;
        // rigidBody.velocity = new Vector3(horizontalVelocity, velocity.y, 0);
    }
}
