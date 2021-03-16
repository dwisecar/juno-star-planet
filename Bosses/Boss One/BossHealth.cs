using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
    /// and what happens when it dies.
    /// </summary>
    [AddComponentMenu("Exoplanet/Scripts/Baddies/BossHealth")]
    public class BossHealth : Health
    {
        [Header("Health Pickups")]
        public GameObject FullHealthPickup;
        public Transform HealthSpawnLocation;

        [Header("Beam Weakness/Strength")]
        public string Weakness;
        public string Strength;

        [Header("Doors To Open On Death")]
        public GameObject door1;
        public GameObject door2;

        [Header("Damage/Death Effects")]
        public GameObject SmokingEffect;
        public GameObject ElectricalEffect;
        public AudioClip DeathSfx;
        public AudioClip BossRoarSfx;
        public Vector3 DeathExplosionOffset = new Vector3(0, -2, 0);
        public SpriteRenderer _bossColor;

        protected RemoteMine remoteMine;
        protected Rigidbody2D _rigidbody2D;
        protected CharacterFollowPath _followPath;


        public bool ThisIsBossOne;
        public bool ThisIsBossTwo;
        public bool ThisIsBossThree;
        public bool ThisIsBossFour;
        public bool ThisIsBossFive;

        protected override void Initialization()
        {
            base.Initialization();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _followPath = GetComponent<CharacterFollowPath>();
            _bossColor = GetComponent<SpriteRenderer>();
        }

        public void DisableChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void EnableChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
        {
            base.Damage(damage, instigator, flickerDuration, invincibilityDuration);
            //CheckHealthUpdateColor();

            if(ElectricalEffect!=null)
            {
                StartCoroutine(PlayElectricalEffect());
            }
        }

        protected void CheckHealthUpdateColor()
        {
            if(_bossColor!=null)
            {
                if (CurrentHealth < 100) { _bossColor.color = new Color(1, .9f, .9f); }
                if (CurrentHealth < 90) { _bossColor.color = new Color(1, .85f, .85f); }
                if (CurrentHealth < 80) { _bossColor.color = new Color(1, .8f, .8f); }
                if (CurrentHealth < 70) { _bossColor.color = new Color(1, .75f, .75f); }
                if (CurrentHealth < 60) { _bossColor.color = new Color(1, .7f, .7f); }
                if (CurrentHealth < 50) { _bossColor.color = new Color(1, .65f, .65f); }
                if (CurrentHealth < 40) { _bossColor.color = new Color(1, .6f, .6f); }
                if (CurrentHealth < 30) { _bossColor.color = new Color(1, .55f, .55f); }
                if (CurrentHealth < 20) { _bossColor.color = new Color(1, .5f, .5f); }
                if (CurrentHealth < 10) { _bossColor.color = new Color(1, .45f, .45f); }
            }
        }

        /// <summary>
        /// Plays a sound when the character is hit
        /// </summary>
        protected virtual void PlayDeathSfx()
        {
            if (DamageSfx != null)
            {
                SoundManager.Instance.PlaySound(DeathSfx, transform.position);
            }
        }

        /// <summary>
        /// Kills the character, vibrates the device, instantiates death effects, handles points, etc
        /// </summary>
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
            if (SmokingEffect != null)
            {
                GameObject instantiatedSmokeEffect = (GameObject)Instantiate(SmokingEffect, transform.position, transform.rotation, transform);
                instantiatedSmokeEffect.transform.localScale = transform.localScale;
                //instantiatedSmokeEffect.transform.parent = this.gameObject.transform;
            }

           

            // Adds points if needed.
            if (PointsWhenDestroyed != 0)
            {
                // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
                CorgiEnginePointsEvent.Trigger(PointsMethods.Add, PointsWhenDestroyed);
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Death");
            }

            //if moving on a path, set speed to zero;
            if(_followPath!=null)
            {
                _followPath.FollowPathSpeed = 0;
            }

            //Death roar
            PlayBossRoarSfx();

            //flicker before destruction
            FlickerOnDeath();

            if (OnDeath != null)
            {
                OnDeath();
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

        protected override void DestroyObject()
        {
            // instantiates the destroy effect
            if (DeathEffect != null)
            {
                GameObject instantiatedEffect = (GameObject)Instantiate(DeathEffect, transform.position + DeathExplosionOffset, transform.rotation);
                instantiatedEffect.transform.localScale = transform.localScale;
            }

            BackgroundMusic.Instance.PlayChillRoomMusic();

            PlayDeathSfx();
            DisableChildren();
            SpawnFullHealthPickup();
            OpenBossDoors();
            KeepBossDead();

            base.DestroyObject();
        }

        public override void Revive()
        {
            base.Revive();
            EnableChildren();
        }

        protected virtual void SpawnFullHealthPickup()
        {
            Instantiate(FullHealthPickup, HealthSpawnLocation.position, HealthSpawnLocation.rotation);
        }

        protected virtual void OpenBossDoors()
        {
            if(door1!=null) { door1.SetActive(false); }
            if (door2 != null) { door2.SetActive(false); }
        }

        protected void FlickerOnDeath()
        {
            StartCoroutine(MMImage.Flicker(_renderer, _initialColor, _flickerColor, 0.05f, DelayBeforeDestruction));
        }

        protected IEnumerator PlayElectricalEffect()
        {
            if(ElectricalEffect!=null)
            {
                ElectricalEffect.SetActive(true);
                yield return new WaitForSeconds(1f);
                ElectricalEffect.SetActive(false);

            }
        }

        protected virtual void PlayBossRoarSfx()
        {
            if (BossRoarSfx != null)
            {
                SoundManager.Instance.PlaySound(BossRoarSfx, transform.position);
            }
        }

        protected void KeepBossDead()
        {
            if(ThisIsBossOne)
            {
                GameManager.Instance.BossOneDead = true;
            }
            if (ThisIsBossTwo)
            {
                GameManager.Instance.BossTwoDead = true;
            }
            if (ThisIsBossThree)
            {
                GameManager.Instance.BossThreeDead = true;
            }
            if (ThisIsBossFour)
            {
                GameManager.Instance.BossFourDead = true;
            }
            if (ThisIsBossFive)
            {
                GameManager.Instance.BossFiveDead = true;
            }
        }
    }
}