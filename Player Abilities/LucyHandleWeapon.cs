using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this class to a character so it can use weapons
    /// Note that this component will trigger animations (if their parameter is present in the Animator), based on 
    /// the current weapon's Animations
    /// Animator parameters : defined from the Weapon's inspector
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Handle Weapon")]
    public class LucyHandleWeapon : CharacterHandleWeapon
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component will allow your character to pickup and use weapons. What the weapon will do is defined in the Weapon classes. This just describes the behaviour of the 'hand' holding the weapon, not the weapon itself. Here you can set an initial weapon for your character to start with, allow weapon pickup, and specify a weapon attachment (a transform inside of your character, could be just an empty child gameobject, or a subpart of your model."; }

        [Header("ParticleSystem")]
        public ParticleSystem ParticleAcceleratorEmitter;
        public ParticleSystem ParticleAcceleratorPlusEmitter;

        public ParticleSystem HyperBeamEmitter;

        public bool ShootingParticleAccelerator = false;
        public bool ShootingParticleAcceleratorPlus = false;

        public bool ShootingHyperBeam = false;

        protected Vector3 _initialPosition;

        protected Vector2 _lookingDown = new Vector2(0, -1);
        protected Vector2 _lookingDownward = new Vector2(1, -1);
        protected Vector2 _lookingUpward = new Vector2(1, 1);
        protected Vector2 _lookingUp = new Vector2(0, 1);
        protected Vector2 _lookingForward = new Vector2(0, 0);
        protected Vector2 _walkingForward = new Vector2(1, 0);

        protected _2dxFX_PlasmaRainbow _plasmaRainbow;

        protected override void Initialization()
        {
            base.Initialization();
            _plasmaRainbow = GetComponent<_2dxFX_PlasmaRainbow>();
        }

        /// <summary>
        /// Causes the character to start shooting
        /// </summary>
        public override void ShootStart()
        {
            base.ShootStart();

            if (ShootingParticleAccelerator == true)
            {
                _initialPosition = ParticleAcceleratorEmitter.transform.localPosition;
                ParticleSystem.EmissionModule emissionModule = ParticleAcceleratorEmitter.emission;
                emissionModule.enabled = true;

                if (_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUpward)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDownward)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == _lookingForward) || (_character.LinkedInputManager.PrimaryMovement == _walkingForward))
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(1, 0);
                    }
                }
                if (!_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 1))
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(-1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, -1))
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(-1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == new Vector2(0, 0)) || (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 0)))
                    {
                        ParticleAcceleratorEmitter.transform.forward = new Vector3(-1, 0);
                    }
                }
                if (_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    ParticleAcceleratorEmitter.transform.forward = new Vector3(-1, 0);
                }
                if (!_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    ParticleAcceleratorEmitter.transform.forward = new Vector3(1, 0);
                }
            }

            if (ShootingParticleAcceleratorPlus == true)
            {
                _initialPosition = ParticleAcceleratorPlusEmitter.transform.localPosition;
                ParticleSystem.EmissionModule plusEmissionModule = ParticleAcceleratorPlusEmitter.emission;
                plusEmissionModule.enabled = true;

                if (_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUpward)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDownward)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == _lookingForward) || (_character.LinkedInputManager.PrimaryMovement == _walkingForward))
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(1, 0);
                    }
                }
                if (!_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 1))
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(-1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, -1))
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(-1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == new Vector2(0, 0)) || (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 0)))
                    {
                        ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(-1, 0);
                    }
                }
                if (_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(-1, 0);
                }
                if (!_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    ParticleAcceleratorPlusEmitter.transform.forward = new Vector3(1, 0);
                }
            }

            if (ShootingHyperBeam == true)
            {
                _plasmaRainbow.enabled = true;

                _initialPosition = HyperBeamEmitter.transform.localPosition;
                ParticleSystem.EmissionModule emissionModule = HyperBeamEmitter.emission;
                emissionModule.enabled = true;

                if (_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUpward)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDownward)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == _lookingForward) || (_character.LinkedInputManager.PrimaryMovement == _walkingForward))
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(1, 0);
                    }
                }
                if (!_character.IsFacingRight && _movement.CurrentState != CharacterStates.MovementStates.WallClinging)
                {
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingUp)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(0, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == _lookingDown)
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(0, -1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 1))
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(-1, 1);
                    }
                    if (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, -1))
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(-1, -1);
                    }
                    if ((_character.LinkedInputManager.PrimaryMovement == new Vector2(0, 0)) || (_character.LinkedInputManager.PrimaryMovement == new Vector2(-1, 0)))
                    {
                        HyperBeamEmitter.transform.forward = new Vector3(-1, 0);
                    }
                }

                if (_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    HyperBeamEmitter.transform.forward = new Vector3(-1, 0);
                }
                if (!_character.IsFacingRight && _movement.CurrentState == CharacterStates.MovementStates.WallClinging)
                {
                    HyperBeamEmitter.transform.forward = new Vector3(1, 0);
                }

            }

        }

        /// <summary>
        /// Causes the character to stop shooting
        /// </summary>
        public override void ShootStop()
        {

            // if the Shoot action is enabled in the permissions, we continue, if not we do nothing
            if (!AbilityPermitted
                || (CurrentWeapon == null)
                || (_movement == null))
            {
                return;
            }

            if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing && CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle)
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReload)
                || (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStart)
                || (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStop))
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse) && (!CurrentWeapon.DelayBeforeUseReleaseInterruption))
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses) && (!CurrentWeapon.TimeBetweenUsesReleaseInterruption))
            {
                return;
            }

            _initialPosition = ParticleAcceleratorEmitter.transform.localPosition;
            ParticleSystem.EmissionModule emissionModule = ParticleAcceleratorEmitter.emission;
            emissionModule.enabled = false;

            ShootingParticleAccelerator = false;

            _initialPosition = ParticleAcceleratorPlusEmitter.transform.localPosition;
            ParticleSystem.EmissionModule plusEmissionModule = ParticleAcceleratorPlusEmitter.emission;
            plusEmissionModule.enabled = false;

            ShootingParticleAcceleratorPlus = false;

            _initialPosition = HyperBeamEmitter.transform.localPosition;
            ParticleSystem.EmissionModule hyperBeamEmissionModule = HyperBeamEmitter.emission;
            hyperBeamEmissionModule.enabled = false;

            _plasmaRainbow.enabled = false;
            ShootingHyperBeam = false;

            CurrentWeapon.TurnWeaponOff();
        }
    }
}