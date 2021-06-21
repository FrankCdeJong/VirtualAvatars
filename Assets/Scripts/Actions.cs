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

        public abstract IEnumerator Trigger();
    }

    public class AngryGesture3 : AvatarAction {
        public AngryGesture3(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        public override IEnumerator Trigger() {
            if (Triggered) yield break;
            Triggered = true;
            
            Animator.SetTrigger("AngryGesture3");
            yield return new WaitForSeconds(4f);
            yield return false;
        }
    }
    
    public class AngryPoint : AvatarAction {
        public AngryPoint(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }
    
        public override IEnumerator Trigger() {
            if (Triggered) yield break;
            Triggered = true;
            
            yield return new WaitForSeconds(7f);
            Animator.SetTrigger("AngryPoint");
            yield return new WaitForSeconds(3f);
            yield return false;
        }
    }

    public class WalkAway : AvatarAction {
        public WalkAway(Rigidbody body, Animator animator, Transform transform) : base(body, animator, transform) {
        }

        public override IEnumerator Trigger() {
            if (Triggered) yield break;
            Triggered = true;
            
            const float maxSideMotion = .76f;
            var currentSideMotion = 0f;
            const float stepSideMotion = maxSideMotion / 22;

            const float maxForwardMotion = .65f;
            var currentForwardMotion = 0f;
            const float stepForwardMotion = maxForwardMotion / 22;

            for (var i = 0; i < 45; i++) {
                Body.transform.Rotate(Vector3.up * 2);

                if (i <= 22) {
                    currentSideMotion += stepSideMotion;
                    currentForwardMotion += stepForwardMotion;
                } else {
                    currentSideMotion -= stepSideMotion;
                }

                Animator.SetFloat("PlayerSideMotion", currentSideMotion);
                Animator.SetFloat("PlayerForwardMotion", currentForwardMotion);

                yield return new WaitForSeconds(0.01f);
            }

            Animator.SetFloat("PlayerSideMotion", 0f);
            Body.AddForce(100f * Transform.forward);

            for (var i = 0; i < 500; i++) {
                Animator.SetFloat("PlayerForwardMotion", 1.14f);
                if (Body.velocity.magnitude <= MaximumVelocity) {
                    Body.AddForce(MovementForce * Transform.forward);
                }

                yield return new WaitForSeconds(0.01f);
            }

            Animator.SetFloat("PlayerSideMotion", 0f);
            Animator.SetFloat("PlayerForwardMotion", 0f);
            Body.AddForce(-Transform.forward);

            yield return false;
        }
    }
}
