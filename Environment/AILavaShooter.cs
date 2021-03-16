using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to a CorgiController2D and it will try to kill your player on sight.
    /// </summary>
    [RequireComponent(typeof(CharacterHandleWeapon))]
    public class AILavaShooter : AIShootOnSight 
    {

        /// <summary>
        /// Every frame, check for the player and try and kill it
        /// </summary>
        protected override void Update()
        {
            if ((_character == null) || (_characterShoot == null)) { return; }

            if ((_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                || (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen))
            {
                _characterShoot.ShootStop();
                return;
            }

            // determine the direction of the raycast 
            _direction = new Vector2(0, -1);

            // we cast a ray in front of the agent to check for a Player
            _raycastOrigin.x = _character.IsFacingRight ? transform.position.x + RaycastOriginOffset.x : transform.position.x - RaycastOriginOffset.x;
            _raycastOrigin.y = transform.position.y + RaycastOriginOffset.y;
            _raycast = MMDebug.RayCast(_raycastOrigin, _direction, ShootDistance, TargetLayerMask, Color.yellow, true);

            // if the raycast has hit something, we shoot
            if (_raycast)
            {
                _characterShoot.ShootStart();
            }
            // otherwise we stop shooting
            else
            {
                _characterShoot.ShootStop();
            }

            if (_characterShoot.CurrentWeapon != null)
            {
                if (_characterShoot.CurrentWeapon.GetComponent<WeaponAim>() != null)
                {
                    //********new code to stop aiming at player feet**************
                    Vector3 playerLocation = new Vector3(LevelManager.Instance.Players[0].transform.position.x, LevelManager.Instance.Players[0].transform.position.y + 1.3f, 0);
                    Vector3 direction = playerLocation - this.transform.position;

                    //Vector3 direction = LevelManager.Instance.Players [0].transform.position - this.transform.position; // ORIGINAL CODE
                    _characterShoot.CurrentWeapon.GetComponent<WeaponAim>().SetCurrentAim(direction);
                }
            }
        }
    }
}