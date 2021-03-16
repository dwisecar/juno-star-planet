using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to an object and it will cause damage to objects that collide with it. 
    /// </summary>
    [AddComponentMenu("Exoplanet/Scripts/Weapons/ReflectDamageOnTouch")]
    public class ReflectDamageOnTouch : DamageOnTouch
    {

        protected ReflectBeam reflectBeam;

        protected override void Awake()
        {
            base.Awake();
            reflectBeam = GetComponent<ReflectBeam>();
        }

        /// <summary>
        /// Describes what happens when colliding with a non damageable object
        /// </summary>
        protected override void OnCollideWithNonDamageable()
        {
            if (_corgiController.State.IsCollidingLeft || _corgiController.State.IsCollidingRight)
            {
                //tells the beam to bounce (reverse on the y axis)
                reflectBeam.WallBounce(true);
            }
            if (_corgiController.State.IsCollidingAbove || _corgiController.State.IsCollidingBelow)
            {
                //tells the beam to bounce on the x axis.
                reflectBeam.WallBounce(false);
            }
            SelfDamage(DamageTakenEveryTime + DamageTakenNonDamageable);
        }

        /// <summary>
        /// Applies damage to itself
        /// </summary>
        /// <param name="damage">Damage.</param>
        protected override void SelfDamage(int damage)
        {
            if (_health != null)
            {
                _health.Damage(damage, gameObject, 0f, DamageTakenInvincibilityDuration);
            }

            // we apply knockback to ourself, turning off for reflect beam
            /*if (_corgiController != null)
            {
                Vector2 totalVelocity = _colliderCorgiController.Speed + _velocity;
                Vector2 knockbackForce = new Vector2(
                    -1 * Mathf.Sign(totalVelocity.x) * DamageTakenKnockbackForce.x,
                    -1 * Mathf.Sign(totalVelocity.y) * DamageTakenKnockbackForce.y);

                if (DamageTakenKnockbackType == KnockbackStyles.SetForce)
                {
                    _corgiController.SetForce(knockbackForce);
                }
                if (DamageTakenKnockbackType == KnockbackStyles.AddForce)
                {
                    _corgiController.AddForce(knockbackForce);
                }
            }*/
        }
    }
}