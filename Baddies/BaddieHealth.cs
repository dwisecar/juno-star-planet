using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
    /// and what happens when it dies.
    /// </summary>
    [AddComponentMenu("Exoplanet/Scripts/Baddies/BaddieHealth")]
    public class BaddieHealth : Health
    {
        [Header("Health Pickups")]
        public GameObject SmallHealthPickup;
        public GameObject LargeHealthPickup;
        public Transform HealthSpawnLocation;

        [Header("Beam Weakness/Strength")]
        public string Weakness;
        public string Strength;

        public GameObject SmokingEffect;
        public AudioClip DeathSfx;

        protected RemoteMine remoteMine;
        protected Rigidbody2D _rigidbody2D;

        protected override void Initialization()
        {
            base.Initialization();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        //Used in the manual respawn from doorways
        public void DisableChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void DetachRemoteMines()
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("PlayerProjectiles"))
                {
                    child.transform.localScale = child.transform.localScale * 2;
                    /*RemoteDetonator remoteDetonator = child.GetComponent<RemoteDetonator>();
                    remoteDetonator.TestBool = true;
                    remoteDetonator.Destroy();*/
                }
                
            }
        }

        public void ClearRemoteMineChildren()
        {
            Debug.Log(transform.childCount);
            int i = 0;

            //Array to hold all child obj
            GameObject[] allChildren = new GameObject[transform.childCount];

            //Find all child obj and store to that array
            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            //Now destroy them
            foreach (GameObject child in allChildren)
            {
                if(child.CompareTag("PlayerProjectiles"))
                {
                    Destroy(child);
                }
            }

            Debug.Log(transform.childCount);
        }

        //Used in the manual respawn from doorways
        public void EnableChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Plays a sound when the character is hit
        /// </summary>
        protected virtual void PlayDeathSfx()
        {
            if (DeathSfx != null)
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

            PlayDeathSfx();

            // we prevent further damage
            DamageDisabled();

            //detatch any remote mines so they don't disappear forever
            //ClearRemoteMineChildren();

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
                GameObject instantiatedEffect = (GameObject)Instantiate(DeathEffect, transform.position, transform.rotation);
                instantiatedEffect.transform.localScale = transform.localScale;
            }

            
            DisableChildren();

            if(LargeHealthPickup!=null && SmallHealthPickup!=null && HealthSpawnLocation!= null)
            {
                SpawnHealthPickup();
            }

            base.DestroyObject();
        }

        public override void Revive()
        {
            base.Revive();
            EnableChildren();
        }

        protected virtual void SpawnHealthPickup()
        {
            float _randomValue = Random.value;

            //0-2 does nothing, 3-7 grants small health, 8-10 grants large health.
            if((_randomValue > .2f) && (_randomValue < .7f)) { Instantiate(SmallHealthPickup, HealthSpawnLocation.position, HealthSpawnLocation.rotation); }
            if (_randomValue >= .7f) { Instantiate(LargeHealthPickup, HealthSpawnLocation.position, HealthSpawnLocation.rotation); }
        }
    }
}