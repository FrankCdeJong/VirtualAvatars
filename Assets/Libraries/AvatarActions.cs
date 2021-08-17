using System;
using System.Collections;
using AvatarFacialExpressions;
using UnityEngine;

namespace AvatarActions {
    /// <summary>
    /// The abstract AvatarAction class is a class that is used to implement an action to be performed on an Avatar.
    /// Any number of things can be executed or triggered in an action. To implement a new action, create a new class
    /// with the name of the action you want to implement. This new class should extend this abstract class and
    /// implement its abstract method. Creating an instance of the sub class is done by providing the Rigidbody,
    /// Animator, and Transform objects. These objects should not be null however, you could get away with it if you
    /// do not use an object that is null.
    /// </summary>
    public abstract class AvatarAction {
        protected readonly Rigidbody Body;
        protected readonly Animator Animator;
        protected readonly Transform Transform;
        protected const float MovementForce = 30f;
        protected const float MaximumVelocity = 28f;
        public bool Triggered;

        /// <summary>
        /// Constructor sets the protected attributes of the class Rigidbody, Animator, and Transform. These attributes
        /// should be from the same game object.
        /// </summary>
        /// <param name="body">The Rigidbody of the game object.</param>
        /// <param name="animator">The Animator of the game object.</param>
        /// <param name="transform">The Transform of the game object.</param>
        protected AvatarAction(Rigidbody body, Animator animator, Transform transform) {
            Body = body;
            Animator = animator;
            Transform = transform;
        }

        // This abstract class should implement an action you want an avatar to perform. 
        /// <summary>
        /// The Trigger method is called to execute the action and perform any action on the Rigidbody, Animator or,
        /// Transform objects. Triggering this should set the Triggered attribute to true so it can check if the action
        /// has been triggered yet.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator Trigger();
    }

    /// <summary>
    /// The LookAway action causes the Avatar to look away by moving the eyes to the right and rotating it by a few
    /// degrees. A custom function is added to set the intensity of the look away. E.g., setting the intensity to 0.5
    /// will make the avatar look away only 50% from the maximum look away position. The Trigger function is not
    /// implemented (yet) and this action should be triggered by calling SetIntensity(). 
    /// </summary>
    public class LookAway : AvatarAction {
        private readonly EyeMovementController _eyes;
        private const int MaxLookAwayX = 70;
        private const int MaxLookAwayY = -50;

        /// <summary>
        /// See AvatarAction class for more details.
        /// </summary>
        public LookAway(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
            _eyes = new EyeMovementController(body.gameObject);
        }

        public override IEnumerator Trigger() {
            yield return null;
        }

        /// <summary>
        /// The SetIntensity function sets the Avatar's eye position away from the default. Depending on the intensity
        /// the eyes are moved further away from the default. The maximum position is defined in MaxLookAwayX and
        /// MaxLookAwayY. It takes 200ms for the eye position to be set.
        /// </summary>
        /// <param name="intensity">A float value between 0 and 1. Where 1 is the maximum intensity and 0 is the least intense.</param>
        /// <returns></returns>
        public IEnumerator SetIntensity(float intensity) {
            var x = MaxLookAwayX * intensity;
            var y = MaxLookAwayY * intensity;
            yield return _eyes.SetEyeXOrYPosition("x", (int) Math.Round(x), 0.2f);
            yield return _eyes.SetEyeXOrYPosition("y", (int) Math.Round(y), 0.2f);

            if (intensity > 0.5 && intensity < 0.97)
                Transform.Rotate(Vector3.up / 6);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class AvatarEyeMoveAction : AvatarAction {
        private readonly EyeMovementController _eyes;

        /// <summary>
        /// See AvatarAction class for more details.
        /// </summary>
        public AvatarEyeMoveAction(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
            _eyes = new EyeMovementController(body.gameObject);
        }

        public override IEnumerator Trigger() {
            yield return null;
        }

        /// <summary>
        /// Set the X and Y position of an avatar's eyes
        /// </summary>
        /// <param name="x">Value between -100 and 100 for left and right</param>
        /// <param name="y">Value between -100 and 100 for up and down</param>
        /// <returns></returns>
        public IEnumerator SetEyeXOrYIntensity(int x, int y) {
            yield return _eyes.SetEyeXOrYPosition("x", x, 0.2f);
            yield return _eyes.SetEyeXOrYPosition("y", y, 0.2f);
        }
    }

    /// <summary>
    /// The AngryGesture3 action triggers an animation in the Avatar. This animation can be triggered only once.
    /// </summary>
    public class AvatarMakeAngryGesture : AvatarAction {
        public AvatarMakeAngryGesture(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        public override IEnumerator Trigger() {
            // Check to make sure we aren't triggered twice
            if (Triggered) yield break;
            Triggered = true;

            // In this case we only use the Animator and set a, so called trigger, this performs an animation and
            // resets the state after the animation finishes.
            Animator.SetTrigger("AngryGesture3");
            yield return new WaitForSeconds(4f);
            yield return false;
        }
    }

    /// <summary>
    /// The AngryPoint action triggers an animation in the avatar. It can be triggered only once.
    /// </summary>
    public class AvatarMakeAngryPointGesture : AvatarAction {
        // This Action triggers an animation
        public AvatarMakeAngryPointGesture(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        public override IEnumerator Trigger() {
            // Check to make sure we aren't triggered twice
            if (Triggered) yield break;
            Triggered = true;

            // In this case we only use the Animator and set a, so called trigger, this performs an animation and
            // resets the state after the animation finishes.
            yield return new WaitForSeconds(7f);
            Animator.SetTrigger("AngryPoint");
            yield return new WaitForSeconds(3f);
            yield return false;
        }
    }

    /// <summary>
    /// This class triggers a walk away action. When applied to an avatar, it turns 90 degrees and then walks away.
    /// </summary>
    public class WalkAway : AvatarAction {
        public WalkAway(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        /// <summary>
        /// Triggering this action causes the avatar to rotate by 90 degrees and then walk away.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Trigger() {
            if (Triggered) yield break;
            Triggered = true;

            // Based on some testing the max motion values are set to a "magic" number making it look somewhat nice.
            // As the turn is performed we assume the avatar is not moving so we need to increment the animation value
            // to become more intense as the turn is performed. We calculate the step size by taking the max value and
            // dividing by 22. Why 22? Again this is a bit of a magic number. We turn by 90 degrees in a for loop. To
            // make the turn happen at a specific speed we iterate 45 times and turn the avatar by 2 degrees each time.
            // The animation values speed up as the turn happens and slows down as it finishes. Therefore once we reach
            // a 45 degree turn we start decreasing the animation values again. So the step size is maxValue / ~ 22.
            const float maxSideMotion = .76f;
            var currentSideMotion = 0f;
            const float stepSideMotion = maxSideMotion / 22;

            const float maxForwardMotion = .65f;
            var currentForwardMotion = 0f;
            const float stepForwardMotion = maxForwardMotion / 22;

            // We iterate 45 times and rotate by 2 degrees each iteration.
            for (var i = 0; i < 45; i++) {
                Body.transform.Rotate(Vector3.up * 2);

                // In the first part the intensity of the animation increases.
                if (i <= 22) {
                    currentSideMotion += stepSideMotion;
                    currentForwardMotion += stepForwardMotion;
                } else {
                    // and in the second part it decreases again.
                    currentSideMotion -= stepSideMotion;
                }

                Animator.SetFloat("PlayerSideMotion", currentSideMotion);
                Animator.SetFloat("PlayerForwardMotion", currentForwardMotion);

                yield return new WaitForSeconds(0.01f);
            }

            // We finishes rotating so we set our side motion to 0.
            Animator.SetFloat("PlayerSideMotion", 0f);

            // We add some initial force so we start moving a bit faster.
            Body.AddForce(100f * Transform.forward);

            // We loop 500 times and add some force each iteration until we reach the maximum velocity.
            for (var i = 0; i < 500; i++) {
                Animator.SetFloat("PlayerForwardMotion", 1.14f);
                if (Body.velocity.magnitude <= MaximumVelocity) {
                    Body.AddForce(MovementForce * Transform.forward);
                }

                yield return new WaitForSeconds(0.01f);
            }

            // We are done walking so we set our motion to 0.
            // TODO: our force isn't 0 yet but our animation is 0. So we are gliding until our momentum is 0 again.
            // Does this fix it? TODO
            Body.AddForce(-Transform.forward);
            Animator.SetFloat("PlayerSideMotion", 0f);
            Animator.SetFloat("PlayerForwardMotion", 0f);

            yield return false;
        }
    }
}
