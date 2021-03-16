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
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Handle EMP")]
    public class CharacterHandleEMP : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component will allow your character to pickup and use weapons. What the weapon will do is defined in the Weapon classes. This just describes the behaviour of the 'hand' holding the weapon, not the weapon itself. Here you can set an initial weapon for your character to start with, allow weapon pickup, and specify a weapon attachment (a transform inside of your character, could be just an empty child gameobject, or a subpart of your model."; }

        /// the initial weapon owned by the character
        public Weapon InitialWeapon;
        /// the position the weapon will be attached to. If left blank, will be this.transform.
        public Transform WeaponAttachment;
        /// if this is set to true, the character can pick up PickableWeapons
        public bool CanPickupWeapons = true;
        /// returns the current equipped weapon
        public Weapon CurrentWeapon { get; protected set; }
        /// if this is true you won't have to release your fire button to auto reload
        public bool ContinuousPress = false;

        protected float _fireTimer = 0f;
        protected float _secondaryHorizontalMovement;
        protected float _secondaryVerticalMovement;
        protected WeaponAim _aimableWeapon;
        protected WeaponIK _weaponIK;
        protected Transform _leftHandTarget = null;
        protected Transform _rightHandTarget = null;

        protected LucyFlamethrower _lucyFlamethrower;
        public bool EMPReady = false;
        public bool EMPDeployed = false;

        // Initialization
        protected override void Initialization()
        {
            base.Initialization();

            Setup();

         
        }

        /// <summary>
        /// Grabs various components and inits stuff
        /// </summary>
        public virtual void Setup()
        {
            // filler if the WeaponAttachment has not been set
            if (WeaponAttachment == null)
            {
                WeaponAttachment = transform;
            }
            if (_animator != null)
            {
                _weaponIK = _animator.GetComponent<WeaponIK>();
            }
            // we set the initial weapon
            if (InitialWeapon != null)
            {
                ChangeWeapon(InitialWeapon, null);
            }
            _character = gameObject.GetComponentNoAlloc<Character>();

            _lucyFlamethrower = GetComponent<LucyFlamethrower>();
        }

        /// <summary>
        /// Every frame we check if it's needed to update the ammo display
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();
            UpdateAmmoDisplay();

            _lucyFlamethrower.UpdateFlamethrowerBar();
       
            if (_lucyFlamethrower.FlamethrowerFuelDurationLeft >= 5f)
            {
                EMPReady = true;
            }
        }

        /// <summary>
        /// Gets input and triggers methods based on what's been pressed
        /// </summary>
        protected override void HandleInput()
        {

            if ((_inputManager.AimLockButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed) && (_inputManager.DetonateButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)) 
            {
                ShootStart();
            }

            if (_inputManager.DetonateButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
            {
                ShootStop();
            }

        }

        /// <summary>
        /// Causes the character to start shooting
        /// </summary>
        public virtual void ShootStart()
        {
            // if the Shoot action is enabled in the permissions, we continue, if not we do nothing.  If the player is dead we do nothing.
            if (!AbilityPermitted
                || (CurrentWeapon == null)
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
                || (EMPReady == false))
            {
                return;
            }

            JumpBoostEffect();
 
            CurrentWeapon.WeaponInputStart();

            EMPDeployed = true;
            _lucyFlamethrower.FlamethrowerFuelDurationLeft = 1;
            EMPReady = false;
            _lucyFlamethrower.GetRefuelStartedAfterEMP();
        }

        /// <summary>
        /// Causes the character to stop shooting
        /// </summary>
        public virtual void ShootStop()
        {
            // if the Shoot action is enabled in the permissions, we continue, if not we do nothing
            if (!AbilityPermitted
                || (CurrentWeapon == null))
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

            CurrentWeapon.TurnWeaponOff();

            EMPDeployed = false;
            _lucyFlamethrower.JetpackStop();
        }

        /// <summary>
        /// Reloads the weapon
        /// </summary>
        protected virtual void Reload()
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.InitiateReloadWeapon();
            }
        }

        /// <summary>
        /// Changes the character's current weapon to the one passed as a parameter
        /// </summary>
        /// <param name="newWeapon">The new weapon.</param>
        public virtual void ChangeWeapon(Weapon newWeapon, string weaponID)
        {

            // if the character already has a weapon, we make it stop shooting
            if (CurrentWeapon != null)
            {
                ShootStop();
                Destroy(CurrentWeapon.gameObject);
            }

            if (newWeapon != null)
            {
                CurrentWeapon = (Weapon)Instantiate(newWeapon, WeaponAttachment.transform.position + newWeapon.WeaponAttachmentOffset, WeaponAttachment.transform.rotation);
                CurrentWeapon.transform.parent = WeaponAttachment.transform;
                CurrentWeapon.SetOwnerEMP(_character, this);
                CurrentWeapon.WeaponID = weaponID;
                _aimableWeapon = CurrentWeapon.GetComponent<WeaponAim>();
                // we handle (optional) inverse kinematics (IK) 
                if (_weaponIK != null)
                {
                    _weaponIK.SetHandles(CurrentWeapon.LeftHandHandle, CurrentWeapon.RightHandHandle);
                }
                // we turn off the gun's emitters.
                CurrentWeapon.Initialization();
                //InitializeAnimatorParameters();
                if (!_character.IsFacingRight)
                {
                    if (CurrentWeapon != null)
                    {
                        CurrentWeapon.FlipWeapon();
                        CurrentWeapon.FlipWeaponModel();
                    }
                }
            }
            else
            {
                CurrentWeapon = null;
            }
        }

        /// <summary>
        /// Flips the current weapon if needed
        /// </summary>
        public override void Flip()
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.FlipWeapon();
                if (CurrentWeapon.FlipWeaponOnCharacterFlip)
                {
                    CurrentWeapon.FlipWeaponModel();
                }
            }
        }

        /// <summary>
        /// Updates the ammo display bar and text.
        /// </summary>
        public virtual void UpdateAmmoDisplay()
        {
            if ((GUIManager.Instance != null) && (_character.CharacterType == Character.CharacterTypes.Player))
            {
                if (CurrentWeapon == null)
                {
                    GUIManager.Instance.SetAmmoDisplays(false, _character.PlayerID);
                    return;
                }

                if (!CurrentWeapon.MagazineBased && (CurrentWeapon.WeaponAmmo == null))
                {
                    GUIManager.Instance.SetAmmoDisplays(false, _character.PlayerID);
                    return;
                }

                if (CurrentWeapon.WeaponAmmo == null)
                {
                    GUIManager.Instance.SetAmmoDisplays(true, _character.PlayerID);
                    GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, 0, 0, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, false);
                    return;
                }
                else
                {
                    GUIManager.Instance.SetAmmoDisplays(true, _character.PlayerID);
                    GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, CurrentWeapon.WeaponAmmo.CurrentAmmoAvailable, CurrentWeapon.WeaponAmmo.MaxAmmo, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, true);
                    return;
                }
            }
        }

        protected virtual void JumpBoostEffect()
        {
            if (_controller.State.IsGrounded)
            {
                _controller.AddVerticalForce(10 );
                _animator.SetBool("Idle", true);
            }

            
        }

    }
}
