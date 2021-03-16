using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this ability to a Character to have it handle horizontal movement (walk, and potentially run, crawl, etc)
    /// Animator parameters : Speed (float), Walking (bool)
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement")]
    public class LucyHorizontalMovement : CharacterHorizontalMovement
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall."; }

        public bool MuteTouchTheGroundSfx;

        protected override void Initialization()
        {
            base.Initialization();
            StartCoroutine(MuteLanding());
        }

        /// <summary>
        /// Called at the very start of the ability's cycle, and intended to be overridden, looks for input and calls
        /// methods if conditions are met
        /// </summary>
        protected override void HandleInput()
        {
            if (_inputManager.AimLockButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed && _controller.State.IsGrounded)
            {
                _horizontalMovement = 0f;

                if ((_horizontalInput > 0f) && (!_character.IsFacingRight))
                {
                    _character.Flip();
                }

                if ((_horizontalInput < 0f) && (_character.IsFacingRight))
                {
                    _character.Flip();
                }
            }
            else
            {
                //actual original code
                _horizontalMovement = _horizontalInput;
            }
        }

        public virtual void JustTeleported()
        {
            StartCoroutine(MuteLanding());
        }

        //to mute landing when passing through doorways.
        public virtual IEnumerator MuteLanding()
        {
            MuteTouchTheGroundSfx = true;
            yield return new WaitForSeconds(1.5f);
            MuteTouchTheGroundSfx = false;
        }

        protected override void CheckJustGotGrounded()
        {
            // if the character just got grounded
            if (_controller.State.JustGotGrounded)
            {
                if (_controller.State.ColliderResized)
                {
                    _movement.ChangeState(CharacterStates.MovementStates.Crouching);
                }
                else
                {
                    _movement.ChangeState(CharacterStates.MovementStates.Idle);
                }

                _controller.SlowFall(0f);
                if (TouchTheGroundEffect != null)
                {
                    Instantiate(TouchTheGroundEffect, _controller.BoundsBottom, transform.rotation);
                }

                //if you just got through a doorway, sfx won't play when you hit the ground
                if(!MuteTouchTheGroundSfx)
                {
                    PlayTouchTheGroundSfx();
                }

            }
        }

        /// <summary>
        /// Adds required animator parameters to the animator parameters list if they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
            RegisterAnimatorParameter("Walking", AnimatorControllerParameterType.Bool);
            RegisterAnimatorParameter ("AimLocked", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// Sends the current speed and the current value of the Walking state to the animator
        /// </summary>
        public override void UpdateAnimator()
        {
            MMAnimator.UpdateAnimatorFloat(_animator, "Speed", Mathf.Abs(_normalizedHorizontalSpeed), _character._animatorParameters);
            MMAnimator.UpdateAnimatorBool(_animator, "Walking", (_movement.CurrentState == CharacterStates.MovementStates.Walking), _character._animatorParameters);
            MMAnimator.UpdateAnimatorBool(_animator, "AimLocked", (_inputManager.AimLockButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed), _character._animatorParameters);
        }

    }
}