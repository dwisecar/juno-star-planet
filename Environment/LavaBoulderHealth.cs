using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class LavaBoulderHealth : Health
    {
        protected SpriteRenderer sprite;
        public float totalDamage;
       

        protected override void Initialization()
        {
            base.Initialization();
            sprite = GetComponent<SpriteRenderer>();
        }

        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
        {
            // if the object is invulnerable, we do nothing and exit
            if (Invulnerable)
            {
                return;
            }

            // if we're already below zero, we do nothing and exit
            if ((CurrentHealth <= 0) && (InitialHealth != 0))
            {
                return;
            }

            // we decrease the character's health by the damage
            float previousHealth = CurrentHealth;
            CurrentHealth -= damage;

            Redden(damage);

            if (OnHit != null)
            {
                OnHit();
            }

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            // we prevent the character from colliding with Projectiles, Player and Enemies
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // we trigger a damage taken event
            MMDamageTakenEvent.Trigger(_character, instigator, CurrentHealth, damage, previousHealth);

            if (_animator != null)
            {
                _animator.SetTrigger("Damage");
            }

            // we play the sound the player makes when it gets hit
            PlayHitSfx();

            // When the character takes damage, we create an auto destroy hurt particle system
            if (DamageEffect != null)
            {
                Instantiate(DamageEffect, transform.position, transform.rotation);
            }

            if (FlickerSpriteOnHit)
            {
                // We make the character's sprite flicker
                if (_renderer != null)
                {
                    StartCoroutine(MMImage.Flicker(_renderer, _initialColor, _flickerColor, 0.05f, flickerDuration));
                }
            }

            // we update the health bar
            UpdateHealthBar(true);

            // if health has reached zero
            if (CurrentHealth <= 0)
            {
                // we set its health to zero (useful for the healthbar)
                CurrentHealth = 0;
                if (_character != null)
                {
                    if (_character.CharacterType == Character.CharacterTypes.Player)
                    {
                        LevelManager.Instance.KillPlayer(_character);
                        return;
                    }
                }

                Kill();
            }
        }

        protected void Redden(int damage)
        {
            totalDamage += damage;

            sprite.color = new Color(1, 1-(totalDamage/50), 1-(totalDamage/50));
        }
    }
}