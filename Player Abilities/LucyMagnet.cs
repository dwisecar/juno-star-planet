using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a character and it'll be able to crouch and crawl
    /// Animator parameters : Crouching, Crawling
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Magnet")]
    public class LucyMagnet : CharacterAbility
    {
        public PointEffector2D _magnet;
        public CircleCollider2D _areaEffector;
        public ParticleSystem _magnetParticles;

        public float MagnetWalkSpeed = 0f;
        public float _stickThreshold = .2f;
        public float _maxMagnetForce = 500f;
        public float _particleSpeed = 3f;
        public bool AttractingMode = false;

        protected float NormalWalkSpeed;
        protected bool _magnetOn = false;
        protected float _verticalRightStickInput;


        /// <summary>
        /// On Start(), we set our tunnel flag to false
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            if(_magnet != null){
                _magnet.enabled = false;
                _areaEffector.enabled = false;
            }

            if (_characterBasicMovement != null)
            {
                NormalWalkSpeed = _characterBasicMovement.MovementSpeed;
            }

        }

        /// <summary>
        /// Every frame, we check if we're crouched and if we still should be
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();
            MagnetOff();
        

        }

        /// <summary>
        /// At the start of the ability's cycle, we check if we're pressing down. If yes, we call Crouch()
        /// </summary>
        protected override void HandleInput()
        {
             _verticalRightStickInput = _inputManager.SecondaryMovement.y;

            if((Mathf.Abs(_verticalRightStickInput) < -_stickThreshold) || (Mathf.Abs(_verticalRightStickInput) > _stickThreshold))
            {
                MagnetOn();
            }

        }

        /// <summary>
        /// If we're pressing down, we check if we can crouch or crawl, and change states accordingly
        /// </summary>
        protected virtual void MagnetOn()
        {
            if (!AbilityPermitted // if the ability is not permitted
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal) // or if we're not in our normal stance
                || (!_controller.State.IsGrounded) // or if we're not grounded
                || (_movement.CurrentState == CharacterStates.MovementStates.WallJumping)
                || (_movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                || (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking)
                || (_movement.CurrentState == CharacterStates.MovementStates.Gripping)) // or if we're gripping sure why not
            {
                // we do nothing and exit
                return;
            }

            // if this is the first time we're here, we trigger our sounds
            if (_movement.CurrentState != CharacterStates.MovementStates.LookingUp)
            {
                // we play the crouch start sound 
                PlayAbilityStartSfx();
                PlayAbilityUsedSfx();
            }

            _magnetOn = true;

            // we set the character's state to Crouching and if it's also moving we set it to Crawling
            _movement.ChangeState(CharacterStates.MovementStates.LookingUp);

            // we change our character's speed
            if (_characterBasicMovement != null)
            {
                _characterBasicMovement.MovementSpeed = MagnetWalkSpeed;
            }

            if(_magnet != null)
            {
                _areaEffector.enabled = true;
                _magnet.enabled = true;
                _magnet.forceMagnitude = (_verticalRightStickInput * _maxMagnetForce);
                ParticleSystem.EmissionModule emissionModule = _magnetParticles.emission;
                emissionModule.enabled = true;
                ParticleSystem.MainModule mainModule = _magnetParticles.main;
                mainModule.startSpeed = _verticalRightStickInput * _particleSpeed;

            }

            if(_magnet.forceMagnitude < -.01f)
            {
                AttractingMode = true;
            }
            else
            {
                AttractingMode = false;
            }

        }

        public virtual void MagnetOff()
        {

            // we play our stop sound and turn the magent off
            if (_movement.CurrentState == CharacterStates.MovementStates.LookingUp)
            {
                if (((Mathf.Abs(_verticalRightStickInput) > -_stickThreshold) && (Mathf.Abs(_verticalRightStickInput) < _stickThreshold)) || (!_controller.State.IsGrounded))
                {
                    // we change our character's speed
                    if (_characterBasicMovement != null)
                    {
                        _characterBasicMovement.MovementSpeed = NormalWalkSpeed;
                    }

                    if (_magnet != null)
                    {
                        _magnet.enabled = false;
                        _areaEffector.enabled = false;
                        ParticleSystem.EmissionModule emissionModule = _magnetParticles.emission;
                        emissionModule.enabled = false;

                    }

                    AttractingMode = false;
                    _movement.ChangeState(CharacterStates.MovementStates.Idle);

                    StopAbilityUsedSfx();
                    PlayAbilityStopSfx();
                }
            }
        }


        /// <summary>
        /// Adds required animator parameters to the animator parameters list if they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("LookingUp", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of the ability's cycle, we send our current magneting state to the animator
        /// </summary>
        public override void UpdateAnimator()
        {
            MMAnimator.UpdateAnimatorBool(_animator, "LookingUp", (_movement.CurrentState == CharacterStates.MovementStates.LookingUp), _character._animatorParameters);
        }


    }
}