using System.Collections;
using UnityEngine;

namespace AvatarActions {
    public abstract class AvatarAction {
        protected readonly Rigidbody Body;
        protected readonly Animator Animator;
        protected readonly Transform Transform;
        protected const float MovementForce = 30f;
        protected const float MaximumVelocity = 28f;
        public bool Triggered = false;

        protected AvatarAction(Rigidbody body, Animator animator, Transform transform) {
            Body = body;
            Animator = animator;
            Transform = transform;
        }

        // This abstract class should implement an action you want an avatar to perform. 
        public abstract IEnumerator Trigger();
    }

    public class AngryGesture3 : AvatarAction {
        // This Action triggers an animation
        public AngryGesture3(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
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

    public class AngryPoint : AvatarAction {
        // This Action triggers an animation
        public AngryPoint(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
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

    public class WalkAway : AvatarAction {
        // This class triggers a walk away action. When applied to an avatar, it turns 90 degrees and then walks away.
        public WalkAway(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        public override IEnumerator Trigger() {
            // Check to make sure we aren't triggered twice
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
