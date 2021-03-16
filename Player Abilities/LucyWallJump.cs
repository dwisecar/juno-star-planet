using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a Character and it'll be able to walljump
    /// Animator parameters : WallJumping (bool)
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Walljump")]
    public class LucyWallJump : CharacterWalljump
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component allows your character to perform an extra jump while wall clinging only. Here you can determine the force to apply to that jump."; }

        //*****Only change is switching add force to set force on wall jump**************** 

        /// <summary>
        /// Performs a walljump if the conditions are met
        /// </summary>
        protected override void Walljump()
        {
            if (!AbilityPermitted
    || _condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            {
                return;
            }

            // wall jump
            float wallJumpDirection;

            // if we're here the jump button has been pressed. If we were wallclinging, we walljump
            if (_movement.CurrentState == CharacterStates.MovementStates.WallClinging)
            {
                _movement.ChangeState(CharacterStates.MovementStates.WallJumping);

                // we decrease the number of jumps left
                if (_characterJump != null)
                {
                    _characterJump.SetNumberOfJumpsLeft(_characterJump.NumberOfJumpsLeft - 1);
                    _characterJump.SetJumpFlags();
                    // we start our sounds
                    PlayAbilityStartSfx();
                }

                _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                _controller.GravityActive(true);
                _controller.SlowFall(0f);

                // If the character is colliding to the right with something (probably the wall)
                if (_controller.State.IsCollidingRight)
                {
                    wallJumpDirection = -1f;
                }
                else
                {
                    wallJumpDirection = 1f;
                }

                Vector2 walljumpVector = new Vector2(
                                        wallJumpDirection * WallJumpForce.x,
                                        Mathf.Sqrt(2f * WallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity))
                );


                //Only change made to Wall Jumping
                _controller.SetForce(walljumpVector);

                WallJumpHappenedThisFrame = true;

                return;


            }
        }

    }
}
