using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
    /// and what happens when it dies.
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Core/Health")]
    public class FanHealth : Health
    {
        /// <summary>
        /// Kills the character, vibrates the device, instantiates death effects, handles points, etc
        /// </summary>
        /// 

        public float FadeOutTime = 1f;
        public SpriteRenderer FanSprite;

        protected override void Initialization()
        {
            _animator = GetComponent<Animator>();
            if (_animator != null)
            {
                _animator.logWarnings = false;
            }

            _character = GetComponent<Character>();
            if (gameObject.GetComponentNoAlloc<SpriteRenderer>() != null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
            if (_character != null)
            {
                if (_character.CharacterModel != null)
                {
                    if (_character.CharacterModel.GetComponentInChildren<Renderer>() != null)
                    {
                        _renderer = _character.CharacterModel.GetComponentInChildren<Renderer>();
                    }
                }
            }
            _autoRespawn = GetComponent<AutoRespawn>();
            _controller = GetComponent<CorgiController>();
            _healthBar = GetComponent<MMHealthBar>();
            _collider2D = GetComponent<Collider2D>();


            _initialPosition = transform.position;
            _initialized = true;
            CurrentHealth = InitialHealth;
            DamageEnabled();
            UpdateHealthBar(false);
        }
        public override void Kill()
        {
            // we make our handheld device vibrate
            if (VibrateOnDeath)
            {
#if UNITY_ANDROID || UNITY_IPHONE
                    Handheld.Vibrate(); 
#endif
            }

            // we prevent further damage
            DamageDisabled();

            // instantiates the destroy effect
            if (DeathEffect != null)
            {
                GameObject instantiatedEffect = (GameObject)Instantiate(DeathEffect, transform.position, transform.rotation);
                instantiatedEffect.transform.localScale = transform.localScale;
            }



            // Adds points if needed.
            if (PointsWhenDestroyed != 0)
            {
                // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
                MMEventManager.TriggerEvent(new CorgiEnginePointsEvent(PointsMethods.Add, PointsWhenDestroyed));
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Death");
            }

            // if we have a controller, removes collisions, restores parameters for a potential respawn, and applies a death force
            if (_controller != null)
            {
                // we make it ignore the collisions from now on
                if (CollisionsOffOnDeath)
                {
                    _controller.CollisionsOff();
                    if (_collider2D != null)
                    {
                        _collider2D.enabled = false;
                    }
                }

                // we reset our parameters
                _controller.ResetParameters();

                // we apply our death force
                if (DeathForce != Vector2.zero)
                {
                    _controller.GravityActive(true);
                    _controller.SetForce(DeathForce);
                }
            }

            if (OnDeath != null)
            {
                OnDeath();
            }

            // if we have a character, we want to change its state
            if (_character != null)
            {
                // we set its dead state to true
                _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
                _character.Reset();

                // if this is a player, we quit here
                if (_character.CharacterType == Character.CharacterTypes.Player)
                {
                    return;
                }
            }


            if(FanSprite != null)
            {
                FanSprite.enabled = true;
            }

            if (DelayBeforeDestruction > 0f)
            {
                Invoke("DestroyObject", DelayBeforeDestruction);
            }
            else
            {
                // finally we destroy the object
                DestroyObject();
            }
        }
    }
}