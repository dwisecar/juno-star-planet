using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class ShotBarrierNode : MonoBehaviour
    {

        protected Animator animator;
        public MovingPlatform Doorway;
        public AudioClip DoorOpenSound;
        protected SpriteRenderer _doorSprite;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Projectiles") || collision.CompareTag("PlayerProjectiles"))
            {
                OpenDoor();
            }

        }

        public virtual void OnParticleCollision(GameObject collider)
        {
            if (collider.CompareTag("Projectiles") || collider.CompareTag("PlayerProjectiles"))
            {
                OpenDoor();
            }

        }

        

        public void OpenDoor()
        {
            if(animator!=null)
            {
                animator.SetBool("Charged", true);
            }

            PlayDoorOpenSound();
            if (Doorway != null)
            {
                Doorway.AuthorizeMovement();
            }
        }
    
        protected void PlayDoorOpenSound()
        {
            if (DoorOpenSound != null)
            {
                SoundManager.Instance.PlaySound(DoorOpenSound, transform.position);
            }
        }
    }
}