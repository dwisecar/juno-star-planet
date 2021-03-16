using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a character and it'll be able to jetpack
    /// Animator parameters : Jetpacking (bool)
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Jetpack")]
    public class LucyFlamethrower : CharacterJetpack
    {
        public ParticleSystem EffectParticleEmitter;
        protected Vector3 _effectInitialPosition;

        public bool FlameThrowerObtained = false;
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "Add this component to a character and it'll be able to activate a jetpack and fly through the level. Here you can define the force to apply when jetpacking, the particle system to use, various fuel info, and optionnally what sound to play when the jetpack gets fully refueled"; }

        /// the remaining jetpack fuel duration (in seconds)
        public float FlamethrowerFuelDurationLeft { get; set; }

        public float JetpackForceVertical = 2.5f;
        /// the force applied by the jetpack
        public float JetpackForceHorizontal = 2.5f;

        [Header("Damage Area")]
        public Collider2D SFTDamageAreaCollider;
        public Collider2D DamageAreaCollider;

        public bool SuperFlamethrowerObtained = false;
        public bool ShootingUpward = false;
        public bool ShootingUp = false; 

        protected Vector2 _lookingDown = new Vector2(0, -1);
        protected Vector2 _lookingDownward = new Vector2(1, -1);
        protected Vector2 _lookingUpward = new Vector2(1, 1);
        protected Vector2 _lookingUpwardLeft = new Vector2(-1, 1);
        protected Vector2 _lookingUp = new Vector2(0, 1);
        protected Vector2 _lookingForward = new Vector2(0, 0);
        protected Vector2 _walkingForward = new Vector2(1, 0);

        protected bool rechargeBoosterObtained = false;
        protected float rechargeRate = 0.9f; // How fast it recharges, default 0.5f

        //Get player health so damage can be disabled duting super flamethrower use
        protected Health _lucyHealth;

        protected CharacterHandleEMP _characterHandleEMP;

        /// <summary>
        /// On Start(), we grab our particle emitter if there's one, and setup our fuel reserves
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();

            _lucyHealth = GetComponent<Health>();
            _characterHandleEMP = GetComponent<CharacterHandleEMP>();

            if (ParticleEmitter != null)
            {
                _initialPosition = ParticleEmitter.transform.localPosition;
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = false;

            }

            if (EffectParticleEmitter != null)
            {
                _effectInitialPosition = EffectParticleEmitter.transform.localPosition;
                ParticleSystem.EmissionModule effectEmissionModule = EffectParticleEmitter.emission;
                effectEmissionModule.enabled = false;

            }

            FlamethrowerFuelDurationLeft = JetpackFuelDuration;
            _jetpackRefuelCooldownWFS = new WaitForSeconds(JetpackRefuelCooldown);

            if (GUIManager.Instance != null && _character.CharacterType == Character.CharacterTypes.Player)
            {
                GUIManager.Instance.SetJetpackBar(!JetpackUnlimited, _character.PlayerID);
                UpdateFlamethrowerBar();
            }

        }

        /// <summary>
        /// Every frame, we check input to see if we're pressing or releasing the jetpack button
        /// </summary>
        protected override void HandleInput()
        {
            if (_inputManager.JetpackButton.State.CurrentState == MMInput.ButtonStates.ButtonDown || _inputManager.JetpackButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed)
            {
                JetpackStart();
            }

            if (_inputManager.JetpackButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
            {
                JetpackStop();
            }

            ////////////start of rotate partical emmitter code///////////////
            if (_character.IsFacingRight)
            {
                if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                {
                    ParticleEmitter.transform.forward = new Vector3(0, 1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                {
                    ParticleEmitter.transform.forward = new Vector3(0, -1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, -90);

                }
                if (_character.LinkedInputManager.PrimaryMovement == _lookingUpward)
                {
                    ParticleEmitter.transform.forward = new Vector3(1, 1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 45);

                }
                if (_character.LinkedInputManager.PrimaryMovement == _lookingDownward)
                {
                    ParticleEmitter.transform.forward = new Vector3(1, -1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, -45);

                }
                if ((_character.LinkedInputManager.PrimaryMovement == _lookingForward) || (_character.LinkedInputManager.PrimaryMovement == _walkingForward))
                {
                    ParticleEmitter.transform.forward = new Vector3(1, 0);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 0);

                }
            }
            else
            {
                if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                {
                    ParticleEmitter.transform.forward = new Vector3(0, 1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                {
                    ParticleEmitter.transform.forward = new Vector3(0, -1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                if (_character.LinkedInputManager.PrimaryMovement == _lookingUpwardLeft)
                {
                    ParticleEmitter.transform.forward = new Vector3(-1, 1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 135);
                }
                if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, -1))
                {
                    ParticleEmitter.transform.forward = new Vector3(-1, -1);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, -135);
                }
                if ((_character.LinkedInputManager.PrimaryMovement == new Vector2(0, 0)) || (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 0)))
                {
                    ParticleEmitter.transform.forward = new Vector3(-1, 0);
                    DamageAreaCollider.transform.eulerAngles = new Vector3(0, 0, 180);
                }
            }

        }

        /// <summary>
        /// Causes the character to start its jetpack.
        /// </summary>
        public override void JetpackStart()
        {

            if ((!AbilityPermitted) // if the ability is not permitted
                || (!_stillFuelLeft) // or if there's no fuel left
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)// or if we're not in normal conditions
                || (FlameThrowerObtained == false)) // or if we haven't obtained the flamethrower yet
            {
                return;
            }

            // if the jetpack is not unlimited and if we don't have fuel left
            if ((!JetpackUnlimited) && (FlamethrowerFuelDurationLeft <= 0f))
            {
                // we stop the jetpack and exit
                JetpackStop();
                _stillFuelLeft = false;
                return;
            }

            // we set the force
            if ((!_controller.State.IsGrounded) || (JetpackForceVertical + _controller.ForcesApplied.y >= 0))
            {

                //***********************Disabled so that player doesn't bounce while shooting upward*****************************8

                // if the character is standing on a moving platform and not pressing the down button,
                /*if ((_controller.State.IsGrounded) && (_controller.State.OnAMovingPlatform))
                {
                    // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid air
                   StartCoroutine(_controller.DisableCollisionsWithMovingPlatforms(MovingPlatformsJumpCollisionOffDuration));
                   _controller.DetachFromMovingPlatform();
                }*/

                ////////Start of modified code////////////////////////////////////////////

                //Get player health so damage can be disabled duting super flamethrower use
               if (SuperFlamethrowerObtained == true)
                {
                    _lucyHealth.DamageDisabled();
                    EnableSFTDamageArea();
                }

                //For telling an elevator we are shooting straight up
                ShootingUp = _character.LinkedInputManager.PrimaryMovement == _lookingUp ? true : false;
                ShootingUpward = _character.LinkedInputManager.PrimaryMovement == _lookingUpward || _character.LinkedInputManager.PrimaryMovement == _lookingUpwardLeft
                    ? true
                    : false;

                //if player input is downward, set force upward. Makes it so aiming up doesn't slow the fall from gravity.
                if (_character.LinkedInputManager.PrimaryMovement.y < 0f)
                {

                    _controller.SetVerticalForce(JetpackForceVertical * -_character.LinkedInputManager.PrimaryMovement.y);
                }

                //horizontal force applied when not grounded

                if (_controller.State.IsGrounded && (_character.LinkedInputManager.PrimaryMovement != _lookingUp))
                {

                    //_controller.SetHorizontalForce ((JetpackForceHorizontal / 2) * -_character.LinkedInputManager.PrimaryMovement.x);


                    if (_character.LinkedInputManager.PrimaryMovement.x != 0f)
                    {
                        _characterBasicMovement.MovementSpeedMultiplier = 0f;
                    }

                    if ((_character.LinkedInputManager.PrimaryMovement.x == 0) && (_character.LinkedInputManager.PrimaryMovement.y == 0))
                    {
                        if (_character.IsFacingRight)
                        {
                            _controller.SetHorizontalForce(-(JetpackForceHorizontal / 2));
                        }
                        else
                        {
                            _controller.SetHorizontalForce(JetpackForceHorizontal / 2);
                        }

                    }

                }
                else
                {
                    _controller.SetHorizontalForce(JetpackForceHorizontal * -_character.LinkedInputManager.PrimaryMovement.x);

                    if ((_character.LinkedInputManager.PrimaryMovement.x == 0f) && (_character.LinkedInputManager.PrimaryMovement.y == 0f))
                    {
                        if (_character.IsFacingRight)
                        {
                            _controller.SetHorizontalForce(-JetpackForceHorizontal);
                        }
                        else
                        {
                            _controller.SetHorizontalForce(JetpackForceHorizontal);
                        }

                    }
                }
                ///End of Modified Code////////////////////////////////////////////////////

                //original code
                //_controller.SetVerticalForce (JetpackForce);
            }

            // if this is the first time we're here, we trigger our sounds
            if (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking)
            {
                // we play the jetpack start sound 
                PlayAbilityStartSfx();
                PlayAbilityUsedSfx();
                Jetpacking = true;
            }

            // we set the various states
            _movement.ChangeState(CharacterStates.MovementStates.Jetpacking);

            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = true;
            }

            if (EffectParticleEmitter != null)
            {
                ParticleSystem.EmissionModule effectEmissionModule = EffectParticleEmitter.emission;
                effectEmissionModule.enabled = true;
            }

            //turn on the damager
            DamageAreaCollider.enabled = true;

            // if the jetpack is not unlimited, we start burning fuel
            if (!JetpackUnlimited)
            {
                StartCoroutine(JetpackFuelBurn());

            }
        }


        /// <summary>
        /// Causes the character to stop its jetpack.
        /// </summary>
        public override void JetpackStop()
        {
            if ((!AbilityPermitted) // if the ability is not permitted
                || (_movement.CurrentState == CharacterStates.MovementStates.Gripping) // or if we're in the gripping state
                || (_movement.CurrentState == CharacterStates.MovementStates.LedgeHanging) // or if we're in the ledge hanging state
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)) // or if we're not in normal conditions
                return;

            TurnJetpackElementsOff();

            // we set our current state to the previous recorded one
            _movement.RestorePreviousState();
        }

        protected override void TurnJetpackElementsOff()
        {
            // we play our stop sound
            if (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking)
            {
                StopAbilityUsedSfx();
                PlayAbilityStopSfx();
            }

            // if we have a jetpack particle emitter, we turn it off
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = false;
            }

            if (EffectParticleEmitter != null)
            {
                ParticleSystem.EmissionModule effectEmissionModule = EffectParticleEmitter.emission;
                effectEmissionModule.enabled = false;
            }

            // if the jetpack is not unlimited, we start refueling
            if (!JetpackUnlimited)
            {
                StartCoroutine(JetpackRefuel());
            }
            Jetpacking = false;

            //turn off sft invincibility and elevator movement
            _lucyHealth.DamageEnabled();
            DisableSFTDamageArea();
            DamageAreaCollider.enabled = false;

            ShootingUp = false;
            ShootingUpward = false;
        }


        /// <summary>
        /// Burns the jetpack fuel
        /// </summary>
        /// <returns>The fuel burn.</returns>
        protected override IEnumerator JetpackFuelBurn()
        {
            // while the character is jetpacking and while we have fuel left, we decrease the remaining fuel
            float timer = FlamethrowerFuelDurationLeft;
            while ((timer > 0) && (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking) && (FlamethrowerFuelDurationLeft <= timer))
            {
                timer -= Time.deltaTime;
                FlamethrowerFuelDurationLeft = timer;
                UpdateFlamethrowerBar();
                yield return 0;
            }
        }

        public void GetRefuelStartedAfterEMP()
        {
            StartCoroutine(JetpackRefuel());
        }

        /// Refills the jetpack fuel
        /// </summary>
        /// <returns>The fuel refill.</returns>
        protected override IEnumerator JetpackRefuel()
        {
            // we wait for a while before starting to refill
            yield return _jetpackRefuelCooldownWFS;
            _refueling = true;
            // then we progressively refill the jetpack fuel
            float refuelDuration = FlamethrowerFuelDurationLeft;
            while ((refuelDuration < JetpackFuelDuration) && (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking))
            {
                if (!rechargeBoosterObtained)
                {   
                    rechargeRate = 0.9f;  
                }
                else
                {
                    rechargeRate = 1.5f;
                }


                refuelDuration += Time.deltaTime * rechargeRate;
                FlamethrowerFuelDurationLeft = refuelDuration;
                UpdateFlamethrowerBar();
                // we prevent the character to jetpack again while at low fuel and refueling
                if ((!_stillFuelLeft) && (refuelDuration > MinimumFuelRequirement))
                {
                    _stillFuelLeft = true;
                }
                yield return 0;
            }
            _refueling = false;
            // if we're full, we play our refueled sound 
            if (System.Math.Abs(FlamethrowerFuelDurationLeft - JetpackFuelDuration) < JetpackFuelDuration / 100)
            {
                PlayJetpackRefueledSfx();
            }
        }
        /// <summary>
        /// Every frame, we check if our character is colliding with the ceiling. If that's the case we cap its vertical force
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();

            // if we're not walking anymore, we stop our walking sound
            if (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking && _abilityInProgressSfx != null)
            {
                StopAbilityUsedSfx();
            }

            if (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking && Jetpacking)
            {
                TurnJetpackElementsOff();
            }

            if (_controller.State.IsCollidingAbove && (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking))
            {
                _controller.SetVerticalForce(0);
            }
        }

        /// <summary>
        /// Updates the GUI jetpack bar.
        /// </summary>
        public virtual void UpdateFlamethrowerBar()
        {
            if ((GUIManager.Instance != null) && (_character.CharacterType == Character.CharacterTypes.Player))
            {
                GUIManager.Instance.UpdateJetpackBar(FlamethrowerFuelDurationLeft, 0f, JetpackFuelDuration, _character.PlayerID);
            }
        }

        /// <summary>
        /// Flips the jetpack's emitter horizontally
        /// </summary>
        public override void Flip()
        {
            if (_character == null)
            {
                Initialization();
            }

            if (ParticleEmitter != null)
            {
                // we invert the rotation of the particle emitter
                ParticleEmitter.transform.eulerAngles = new Vector3(ParticleEmitter.transform.eulerAngles.x, ParticleEmitter.transform.eulerAngles.y + 180, ParticleEmitter.transform.eulerAngles.z);

                // we mirror its position around the transform's center
                if (ParticleEmitter.transform.localPosition == _initialPosition)
                {
                    ParticleEmitter.transform.localPosition = Vector3.Scale(_initialPosition, _character.ModelFlipValue);
                }
                else
                {
                    ParticleEmitter.transform.localPosition = _initialPosition;
                }
            }
        }

        /// <summary>
        /// Plays a sound when the jetpack is fully refueled
        /// </summary>
        protected override void PlayJetpackRefueledSfx()
        {
            if (JetpackRefueledSfx != null) { SoundManager.Instance.PlaySound(JetpackRefueledSfx, transform.position); }
        }

        /// <summary>
        /// When the character dies we stop its jetpack
        /// </summary>
        public override void Reset()
        {
            // if we have a jetpack particle emitter, we turn it off
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = false;
            }
            StopAbilityUsedSfx();
            FlamethrowerFuelDurationLeft = JetpackFuelDuration;
            UpdateFlamethrowerBar();
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            _stillFuelLeft = true;
        }

        public virtual void RechargeBoosterObtained()
        {
            rechargeBoosterObtained = true;
        }



        /// <summary>
        /// Adds required animator parameters to the animator parameters list if they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter("Jetpacking", AnimatorControllerParameterType.Bool);
        }

        /// <summary>
        /// At the end of each cycle, we send our character's animator the current jetpacking status
        /// </summary>
        public override void UpdateAnimator()
        {
            MMAnimator.UpdateAnimatorBool(_animator, "Jetpacking", (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking), _character._animatorParameters);
        }

        protected virtual void EnableSFTDamageArea()
        {
            SFTDamageAreaCollider.enabled = true;
        }

        /// <summary>
        /// Disables the damage area.
        /// </summary>
        protected virtual void DisableSFTDamageArea()
        {
            SFTDamageAreaCollider.enabled = false;
        }

    }
}