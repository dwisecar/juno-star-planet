using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionDrillAttack : AIAction
    {

        public AudioClip DrillSfx;
        public ParticleSystem ParticleEmitter;

        protected Animator anim;
        protected CharacterHorizontalMovement movement;
        protected BoxCollider2D box;

        protected float boxHeight = 2.8f;
        protected float boxWidth = 2.5f;

        protected bool attacking;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            movement = GetComponent<CharacterHorizontalMovement>();
            box = GetComponent<BoxCollider2D>();
        }

        protected void SparksOn(bool on)
        {

            if (on)
            {
                if (ParticleEmitter != null)
                {
                    ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                    emissionModule.enabled = true;
                }
            }

            if (!on)
            {
                if (ParticleEmitter != null)
                {
                    ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                    emissionModule.enabled = false;
                }
            }

        }

        protected IEnumerator AttackMode()
        {
            attacking = true;
            anim.SetBool("Attacking", true);
            movement.MovementSpeed = 0f;
            yield return new WaitForSeconds(.2f);
            box.size = new Vector2(6.35f, boxHeight);
            SparksOn(true);
            yield return new WaitForSeconds(1f);
            anim.SetBool("Attacking", false);
            movement.MovementSpeed = 3f;
            box.size = new Vector2(2, boxHeight);
            attacking = false;
            SparksOn(false);

        }

        public override void PerformAction()
        {
            if(!attacking)
            {
                StartCoroutine(AttackMode());
            }
        }

        public override void OnExitState()
        {
            anim.SetBool("Attacking", false);
            movement.MovementSpeed = 3f;
            box.size = new Vector2(2, boxHeight);

            base.OnExitState();

        }
    }
}