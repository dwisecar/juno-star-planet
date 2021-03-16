using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class PlasmaBladeDamager : MonoBehaviour
    {
        protected CharacterHandleMelee _meleeWeapon;
        protected Vector2 _initialVelo;
        protected Vector3 impactPoint;
        protected CircleCollider2D _circleCollider;
        protected CorgiController _controller;
        public GameObject ReflectEffect;
        public AudioClip ReflectSfx;
        public Vector2 KnockBackForceOnHit = new Vector2(5, 0);


        // Use this for initialization
        void Start()
        {
            _meleeWeapon = GetComponentInParent<CharacterHandleMelee>();
            _controller = GetComponentInParent<CorgiController>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckForMeleeAttack();
        }
        protected void CheckForMeleeAttack()
        {
            if(_meleeWeapon.MeleeColliderOn)
            {
                _circleCollider.enabled = true;
            }
            if(_meleeWeapon.MeleeColliderOn == false)
            {
                _circleCollider.enabled = false;
            }
        }

        protected void OnTriggerEnter2D(Collider2D _collision)
        {
            //Rigidbody2D rb2d = _collision.GetComponent<Rigidbody2D>();
            //Animator anim = _collision.GetComponent<Animator>();
            //Transform projTransform = _collision.GetComponent<Transform>();
            //Health _health = _collision.GetComponent<Health>();

            //if (_collision.gameObject.tag == "Projectiles")
            //{
            //    InstantiateReflectEffect();
            //    PlayReflectSound();
            //}

            //if (_collision.gameObject.tag == "PlayerProjectiles")
            //{
            //    if (anim != null)
            //    {
            //        anim.SetBool("SuperCharged", true);
            //    }

            //    if (rb2d != null)
            //    {
            //        _initialVelo = rb2d.velocity;
            //        rb2d.velocity = Vector2.zero;
            //        impactPoint = projTransform.position;
            //        rb2d.velocity = -_initialVelo;
            //    }
            //    InstantiateReflectEffect();
            //    PlayReflectSound();
            //}

            if ((_collision.gameObject.layer == 13) || (_collision.gameObject.layer == 28))
            {
                _controller.SetForce(KnockBackForceOnHit);
            }
        }

        protected virtual void InstantiateReflectEffect()
        {
            // instantiates the destroy effect
            if (ReflectEffect != null)
            {
                GameObject instantiatedEffect = (GameObject)Instantiate(ReflectEffect, impactPoint, transform.rotation);
                instantiatedEffect.transform.localScale = transform.localScale;
            }
        }

        protected virtual void PlayReflectSound()
        {
            if (ReflectSfx != null)
            {
                SoundManager.Instance.PlaySound(ReflectSfx, transform.position);
            }
        }
    }
}