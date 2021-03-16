using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class HealthRecoveryPickup : MonoBehaviour
    {
        protected LucyHealth _lucyHealth;
        protected int pickupHealthAmount = 10;
        protected Color _initialColor;
        protected Color _flickerColor = new Color32(255, 255, 255, 0);
        protected Renderer _renderer;
        public bool BigHealthPickup;
        public bool FullHealthPickup;
        public AudioClip PickupSfx;

        // Start is called before the first frame update
        void Start()
        {
            if(BigHealthPickup) { pickupHealthAmount = 25; }
            
            else { pickupHealthAmount = 10; }

            if (gameObject.GetComponentNoAlloc<SpriteRenderer>() != null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }

            InitializeSpriteColor();
            StartCoroutine(Expire());
            DestroyObjectDelayed();
        }

        protected void OnTriggerEnter2D(Collider2D _collider)
        {
            _lucyHealth = _collider.gameObject.GetComponent<LucyHealth>();

            if(_lucyHealth != null)
            {
                if(FullHealthPickup)
                {
                    _lucyHealth.ResetHealthToMaxHealth();
                }
                else
                {
                    _lucyHealth.HealthRecoveryPickedUp(pickupHealthAmount);
                }

                PlayPickupSfx();
              
                Destroy(gameObject);
            }


        }

        protected virtual void InitializeSpriteColor()
        {

            if (_renderer != null)
            {
                if (_renderer.material.HasProperty("_Color"))
                {
                    _initialColor = _renderer.material.color;
                }
            }
        }

        protected virtual IEnumerator Expire()
        {
            yield return new WaitForSeconds(5f);
            Flicker();
        }

        protected void Flicker()
        {
            StartCoroutine(MMImage.Flicker(_renderer, _initialColor, _flickerColor, 0.05f, 3f));
        }

        protected void DestroyObjectDelayed()
        {
            Destroy(gameObject, 8f);
        }

        protected void PlayPickupSfx()
        {
            if (PickupSfx!=null)
            {
                SoundManager.Instance.PlaySound(PickupSfx, transform.position);

            }
        }
    }
}