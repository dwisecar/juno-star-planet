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
    public class LucyHealth : Health
    {
        protected Color _flickeringColor = new Color32(255, 20, 20, 0);
        protected SpriteRenderer _sprite;
        protected LucyInventory _characterInventory;

        protected _2dxFX_CompressionFX _damageSpriteEffect;
        public GameObject ElectricalEffect;

        public float _superSuitDefenseMultiplier = 1;
        public float _ultraSuitDefenseMultiplier = 1;

        //SuperSuit Aquired Bool
        public bool _superSuitAcquired = false;
        public bool _ultraSuitAcquired = false;
        protected float _reducedDamage;

        // the amount batteries will increase health capacity
        public int BatteryIncreaseAmount = 10;

        public bool _knockbackReducerAcquired = false;

        public float FadeOutTime = .3f;

        protected AreaOneManager areaOneManager;
        protected AreaTwoManager areaTwoManager;
        protected AreaThreeManager areaThreeManager;
        protected AreaFourManager areaFourManager;
        protected AreaFiveManager areaFiveManager;

        /// <summary>
        /// On Start, we initialize our health
        /// </summary>
        protected override void Start()
        {
            Initialization();
            InitializeSpriteColor();
        }

        /// <summary>
        /// Grabs useful components, enables damage and gets the inital color
        /// </summary>
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
                _renderer.material.color = Color.white;
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

            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = GetComponent<LucyInventory>();
            _damageSpriteEffect = GetComponent<_2dxFX_CompressionFX>();


            _initialPosition = transform.position;
            _initialized = true;
            CurrentHealth = InitialHealth;
            DamageEnabled();

            // _characterInventory.CheckPowerNodeInventories();
            UpdateHealthBar(false);
            UpdateHealthCounter(CurrentHealth);

            //absolute bandaid for issue where GUI says health is 10 when actully full. Only when starting a scene in area 3 and 5.
            StartCoroutine(WaitAFrameAndCheckHealth());

            InitializeSpriteColor();

        }

        protected override void InitializeSpriteColor()
        {
            if (_renderer != null)
            {
                if (_renderer.material.HasProperty("_Color"))
                {
                    _initialColor = _renderer.material.color;
                }
            }
        }

        protected IEnumerator WaitAFrameAndCheckHealth()
        {
            yield return new WaitForEndOfFrame();
            UpdateHealthCounter(CurrentHealth);
        }

        public virtual void SuperSuitAquired()
        {
            _superSuitAcquired = true;
        }

        public virtual void UltraSuitAquired()
        {
            _ultraSuitAcquired = true;
        }

        public virtual void KnockbackReducerAcquired()
        {
            _knockbackReducerAcquired = true;
        }

        /// <summary>
        /// Called when the object takes damage
        /// </summary>
        /// <param name="damage">The amount of health points that will get lost.</param>
        /// <param name="instigator">The object that caused the damage.</param>
        /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage.</param>
        /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
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

            if (ElectricalEffect != null)
            {
                StartCoroutine(PlayElectricalEffect());
            }

            // if the Super Suit is aquired, the damage variable is reduced.
            if (_superSuitAcquired == true)
            {
                _reducedDamage = (damage * _superSuitDefenseMultiplier);
                damage = Mathf.FloorToInt(_reducedDamage);
            }

            // if the Super Suit is aquired, the damage variable is reduced.
            if (_ultraSuitAcquired == true)
            {
                _reducedDamage = (damage * _ultraSuitDefenseMultiplier);
                damage = Mathf.FloorToInt(_reducedDamage);
            }

            // we decrease the character's health by the damage
            float previousHealth = CurrentHealth;
            CurrentHealth -= damage;

            // we prevent the character from colliding with Projectiles, Player and Enemies
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // we trigger a damage taken event
            MMEventManager.TriggerEvent(new MMDamageTakenEvent(_character, instigator, CurrentHealth, damage, previousHealth));

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
                    StartCoroutine(MMImage.Flicker(_renderer, _initialColor, _flickeringColor, 0.03f, flickerDuration));
                    
                }

                ResetSpriteColor();
            }

            // we update the health bar
            UpdateHealthBar(true);

            //NEW CODE for health counter
            UpdateHealthCounter(CurrentHealth);

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
                BackgroundMusic.Instance.PlayChillRoomMusic();
            }
        }

        public override void Revive()
        {
            base.Revive();
            BackgroundMusic.Instance.PlayBackgroundMusic();
        }

        public virtual void UpdateHealthCounter(int _newHealth)
        {
            GUIManager.Instance.UpdateHealthCounter(_newHealth);

            //Update the HUD color
            if ((_newHealth <= 50) && (_newHealth >= 21))
            {
                GUIManager.Instance.SwitchHUDColor("Yellow");
            }

            if (_newHealth <= 20)
            {
                GUIManager.Instance.SwitchHUDColor("Red");
            }

            if (_newHealth >= 51)
            {
                GUIManager.Instance.SwitchHUDColor("Blue");
            }
        }


        public virtual void BatteryObtained()
        {
            //increase max health by 10
            MaximumHealth += BatteryIncreaseAmount;
            InitialHealth += BatteryIncreaseAmount;


            //reset health to full
            ResetHealthToMaxHealth();
            UpdateHealthCounter(CurrentHealth);
        }

        public virtual void InitializeHealthWithNodesInInventory(int healthAmount)
        {
            MaximumHealth = healthAmount;
            InitialHealth = healthAmount;
        }

        //for when you pickup health
        public virtual void HealthRecoveryPickedUp(int pickupSize)
        {
            CurrentHealth += pickupSize;

            if (CurrentHealth > MaximumHealth)
            {
                CurrentHealth = MaximumHealth;
            }

            UpdateHealthBar(true);
            UpdateHealthCounter(CurrentHealth);
        }

        public override void ResetHealthToMaxHealth()
        {
            base.ResetHealthToMaxHealth();
            UpdateHealthCounter(CurrentHealth);
        }

        //Effect for player taking damage
        protected IEnumerator PlayElectricalEffect()
        {
            if (ElectricalEffect != null)
            {
                ElectricalEffect.SetActive(true);
                _damageSpriteEffect.enabled = true;
                yield return new WaitForSeconds(1f);
                ElectricalEffect.SetActive(false);
                _damageSpriteEffect.enabled = false;
            }
        }

        
    }
}
