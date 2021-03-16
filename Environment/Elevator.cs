using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a platform and it'll be able to follow a path and carry a character
    /// </summary>
    public class Elevator : MMPathMovement, Respawnable
    {
        [Header("Activation")]
        [Information("Check the <b>Only Moves When Player Is Colliding</b> checkbox to have the object wait for a collision with your player to start moving.", MoreMountains.Tools.InformationAttribute.InformationType.Info, false)]
        /// If true, the object will only move when colliding with the player
        public bool OnlyMovesWhenPlayerIsColliding = false;
        /// If true, this moving platform will reset position and behaviour when the player respawns
        public bool ResetPositionWhenPlayerRespawns = false;
        /// If true, this platform will only moved when commanded to by another script
        public bool ScriptActivated = false;
        public bool StayAnimated = false;

        public AudioClip ElevatorStuckSfx;
        public AudioClip ElevatorMovingSfx;

        protected bool alreadyPlayedStuckSfx;
        protected bool alreadyPlayedMoveSfx;

        protected Collider2D _collider2D;
        protected float _platformTopY;
        protected const float _toleranceY = 0.05f;
        protected bool _scriptActivatedAuthorization = false;
        protected float _initialSpeed;

        protected Animator anim;
        protected LucyFlamethrower _lucyFlamethrower;

        /// <summary>
        /// Flag inits, initial movement determination, and object positioning
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            _collider2D = GetComponent<Collider2D>();
            SetMovementAuthorization(false);
            _initialSpeed = MovementSpeed;
            anim = GetComponent<Animator>();

            if(StayAnimated) { anim.SetBool("ElevatorMoving", true); }

            if (ScriptActivated) { anim.SetBool("ElevatorMoving", false); }
            else { anim.SetBool("ElevatorMoving", true); }

            anim.SetBool("ElevatorStuck", false);

            alreadyPlayedStuckSfx = false;
            alreadyPlayedMoveSfx = false;

        }


        /// <summary>
        /// Gets a value indicating whether this instance can move.
        /// </summary>
        /// <value><c>true</c> if this instance can move; otherwise, <c>false</c>.</value>
        public override bool CanMove
        {
            get
            {
                if (OnlyMovesWhenPlayerIsColliding)
                {
                    if (!_collidingWithPlayer)
                    {
                        return false;
                    }

                    if (_collidingController == null)
                    {
                        return false;
                    }

                    // if we're colliding with a character, we check that's it's actually above the platform's top
                    _platformTopY = (_collider2D != null) ? _collider2D.bounds.max.y : this.transform.position.y;
                    if (_collidingController.ColliderBottomPosition.y < _platformTopY - _toleranceY)
                    {
                        return false;
                    }
                }

                if (ScriptActivated)
                {
                    return _scriptActivatedAuthorization;
                }

                return true;
            }
        }

        protected CorgiController _collidingController = null;
        protected bool _collidingWithPlayer;

        public virtual void SetMovementAuthorization(bool status)
        {
            _scriptActivatedAuthorization = status;
        }

        public virtual void AuthorizeMovement()
        {
            _scriptActivatedAuthorization = true;
            anim.SetBool("ElevatorMoving", true);
            PlayElevatorMovingSound();
        }

        public virtual void ForbidMovement()
        {
            _scriptActivatedAuthorization = false;
            anim.SetBool("ElevatorMoving", false);
            alreadyPlayedMoveSfx = false;
        }

        public virtual void ToggleMovementAuthorization()
        {
            _scriptActivatedAuthorization = !_scriptActivatedAuthorization;
        }

        /// <summary>
        /// When entering collision with something, we check if it's a player, and in that case we set our flag accordingly
        /// </summary>
        /// <param name="collider">Collider.</param>
        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            CorgiController controller = collider.GetComponent<CorgiController>();
            _lucyFlamethrower = collider.GetComponent<LucyFlamethrower>();

            if (controller == null)
                return;

            if (StayAnimated) { anim.SetBool("ElevatorMoving", true); }

            if (ScriptActivated)
            {
                PlayElevatorStuckSound();
                StartCoroutine(PlayStuckElevatorAnimation());

                _collidingWithPlayer = true;
                _collidingController = controller;

                if (_lucyFlamethrower.ShootingUp == true || _lucyFlamethrower.ShootingUpward == true)
                {
                    AuthorizeMovement();
                }
                else
                {
                    ForbidMovement();
                }

                if (_lucyFlamethrower.ShootingUp)
                {
                    MovementSpeed = 4f;
                }
                else
                {
                    MovementSpeed = _initialSpeed;
                }
            }
            else
            {
                if (StayAnimated) { anim.SetBool("ElevatorMoving", true); }
            }



        }

        /// <summary>
        /// When exiting collision with something, we check if it's a player, and in that case we set our flag accordingly
        /// </summary>
        /// <param name="collider">Collider.</param>
        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            CorgiController controller = collider.GetComponent<CorgiController>();
            if (controller == null)
                return;

            _collidingWithPlayer = false;
            _collidingController = null;

            if (StayAnimated) { anim.SetBool("ElevatorMoving", true); }

            if (ScriptActivated) { anim.SetBool("ElevatorMoving", false); }
            else { anim.SetBool("ElevatorMoving", true); }
            anim.SetBool("ElevatorStuck", false);

            alreadyPlayedStuckSfx = false;
            alreadyPlayedMoveSfx = false;
        }

        /// <summary>
        /// When the player respawns, we reset the position and behaviour of this moving platform
        /// </summary>
        /// <param name="checkpoint">Checkpoint.</param>
        /// <param name="player">Player.</param>
        public virtual void OnPlayerRespawn(CheckPoint checkpoint, Character player)
        {
            if (ResetPositionWhenPlayerRespawns)
            {
                Initialization();
            }
        }

        protected IEnumerator PlayStuckElevatorAnimation()
        {

            anim.SetBool("ElevatorStuck", true);
            yield return new WaitForSeconds(1f);
            anim.SetBool("ElevatorStuck", false);

        }

        protected virtual void PlayElevatorStuckSound()
        {
            if (ElevatorStuckSfx != null && !alreadyPlayedStuckSfx)
            {
                SoundManager.Instance.PlaySound(ElevatorStuckSfx, transform.position);
                alreadyPlayedStuckSfx = true;
            }
        }

        protected virtual void PlayElevatorMovingSound()
        {
            if (ElevatorMovingSfx != null && !alreadyPlayedMoveSfx)
            {
                SoundManager.Instance.PlaySound(ElevatorMovingSfx, transform.position);
                alreadyPlayedMoveSfx = true;
            }
        }
    }
}
