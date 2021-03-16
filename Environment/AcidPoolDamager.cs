using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this component to an object and it will cause damage to objects that collide with it. 
    /// </summary>
    [AddComponentMenu("ExoPlanet/Scripts/Environment/AcidPoolDamager")]
    public class AcidPoolDamager : DamageOnTouch
    {


        protected LucyFlamethrower _lucyFlamethrower;

        public ParticleSystem EnterAcidSplashEffect;
        public AudioClip EnterAcidSplashSfx;


        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if(collider.gameObject.layer == 9 || collider.gameObject.layer == 16)
            {
                //play splash effect
                if (EnterAcidSplashEffect != null)
                {
                    Instantiate(EnterAcidSplashEffect, collider.transform.position, collider.transform.rotation);
                }
                PlayEnterAcidSplashSfx();
            }
        }

        public override void OnParticleCollision(GameObject collider)
        {
            if (collider.gameObject.GetComponent<Collider2D>() != null)
            {
                Colliding(collider.gameObject.GetComponent<Collider2D>());
            }

        }

        protected void PlaySplashVisualEffect()
        {
        }

        protected virtual void PlayEnterAcidSplashSfx()
        {
            if (EnterAcidSplashSfx != null) { SoundManager.Instance.PlaySound(EnterAcidSplashSfx, transform.position); }
        }

        protected override void Colliding(Collider2D collider)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            // if the object we're colliding with is part of our ignore list, we do nothing and exit
            if (_ignoredGameObjects.Contains(collider.gameObject))
            {
                return;
            }

            // if what we're colliding with isn't part of the target layers, we do nothing and exit
            if (!MMLayers.LayerInLayerMask(collider.gameObject.layer, TargetLayerMask))
            {
                return;
            }

            /*if (Time.time - _knockbackTimer < InvincibilityDuration)
            {
                return;
            }
            else
            {
                _knockbackTimer = Time.time;
            }*/
               

            _lucyHealth = collider.gameObject.GetComponentNoAlloc<LucyHealth>();


            // if what we're colliding with is damageable
            if (_lucyHealth != null)
            {
                if(_lucyHealth._ultraSuitAcquired == false)
                {
                    if (_lucyHealth.CurrentHealth > 0)
                    {
                        OnCollideWithDamageable(_lucyHealth);
                    }

                    
                }
                
            }

            // if what we're colliding with can't be damaged
            else
            {
                OnCollideWithNonDamageable();
            }

            //Make it so the flamethrower doesn't work in water
            _lucyFlamethrower = collider.gameObject.GetComponentNoAlloc<LucyFlamethrower>();

            if(_lucyFlamethrower!=null)
            {
                _lucyFlamethrower.AbilityPermitted = false;
            }
        }

        /// <summary>
        /// Describes what happens when colliding with a damageable object
        /// </summary>
        /// <param name="health">Health.</param>
        protected virtual void OnCollideWithDamageable(LucyHealth health)
        {
            if (health._ultraSuitAcquired == true)
            {
                return;
            }
            else
            {
                // if what we're colliding with is a CorgiController, we apply a knockback force
                _colliderCorgiController = health.gameObject.GetComponentNoAlloc<CorgiController>();

               

                if ((_colliderCorgiController != null) && (DamageCausedKnockbackForce != Vector2.zero) && (!_lucyHealth.Invulnerable))
                {
                    Vector2 totalVelocity = _colliderCorgiController.Speed + _velocity;
                    _knockbackForce.x = -1 * Mathf.Sign(totalVelocity.x) * DamageCausedKnockbackForce.x;
                    _knockbackForce.y = DamageCausedKnockbackForce.y;

                    if (DamageCausedKnockbackType == KnockbackStyles.SetForce)
                    {
                        _colliderCorgiController.SetForce(_knockbackForce);
                    }
                    if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
                    {
                        _colliderCorgiController.AddForce(_knockbackForce);
                    }
                }

                // we apply the damage to the thing we've collided with
                _lucyHealth.Damage(DamageCaused, gameObject, InvincibilityDuration, InvincibilityDuration);
                SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
            }
        }

        protected void OnTriggerExit2D(Collider2D collider)
        {
            //turn flamethrower back and set walk speed to normal on after leaving.
            _lucyFlamethrower = collider.gameObject.GetComponent<LucyFlamethrower>();

            if (_lucyFlamethrower != null)
            {
                _lucyFlamethrower.AbilityPermitted = true;
            }

            if (collider.gameObject.layer == 9)
            {
                //play splash effect
                if (EnterAcidSplashEffect != null)
                {
                    Instantiate(EnterAcidSplashEffect, collider.transform.position, collider.transform.rotation);
                }
                PlayEnterAcidSplashSfx();
            }

        }
    }
}