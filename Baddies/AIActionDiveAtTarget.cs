using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Requires a CharacterFly ability. Makes the character fly up to the specified MinimumDistance in the direction of the target. That's how the RetroGhosts move.
    /// </summary>
    [RequireComponent(typeof(CharacterFly))]
    public class AIActionDiveAtTarget : AIAction
    {
        /// the minimum distance from the target this Character can reach.
        public float MinimumDistance = 1f;
        public float DrillTime = 3;
        public ParticleSystem ParticleEmitter;
        public AudioClip DrillSfx;

        protected CharacterFly _characterFly;
        protected Animator anim;
        protected int _numberOfJumps = 0;
        protected BoxCollider2D box;
        protected bool drilling;
        protected float returnHeight;

        /// <summary>
        /// On init we grab our CharacterFly ability
        /// </summary>
        protected override void Initialization()
        {
            _characterFly = this.gameObject.GetComponent<CharacterFly>();
            anim = this.gameObject.GetComponent<Animator>();
            box = this.gameObject.GetComponent<BoxCollider2D>();

        }

        /// <summary>
        /// On PerformAction we fly
        /// </summary>
        public override void PerformAction()
        {
            if(!drilling) { Dive(); }
        }

        /// <summary>
        /// Moves the character towards the target if needed
        /// </summary>
        protected virtual void Dive()
        {
            if (_brain.Target == null)
            {
                return;
            }

            //set the distance that the driller will return to after attack
            returnHeight = this.transform.position.y - (_brain.Target.position.y + 1);
            _characterFly.FlySpeed = 20f;
            anim.SetBool("PointDown", true);

            if (this.transform.position.y < (_brain.Target.position.y + 1.5))
            {
                _characterFly.SetVerticalMove(1f);
            }
            else
            {
                _characterFly.SetVerticalMove(-1f);
            }

            if (Mathf.Abs(this.transform.position.y - (_brain.Target.position.y + 1.5f)) < MinimumDistance)
            {
                StartCoroutine(DrillDown());
            }
        }

        protected IEnumerator DrillDown()
        {
            drilling = true;
            box.size = new Vector2(1, 4);
            SparksOn(true);
            PlayDrillSfx();
            yield return new WaitForSeconds(DrillTime);
            anim.SetBool("PointDown", false);
            box.size = new Vector2(6, 1);
            SparksOn(false);
            FloatBackUp();

        }

        protected void FloatBackUp()
        {
            if (_brain.Target == null)
            {
                return;
            }

            _characterFly.FlySpeed = 6f;

            if (this.transform.position.y < (_brain.Target.position.y + 3))
            {
                _characterFly.SetVerticalMove(1f);
            }
            else
            {
                _characterFly.SetVerticalMove(-1f);
            }

            if (Mathf.Abs(this.transform.position.y - (_brain.Target.position.y + 3)) < MinimumDistance)
            {
                _characterFly.SetVerticalMove(0);
            }

        }

        protected void SparksOn(bool on)
        {

            if(on)
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

        /// <summary>
        /// On exit state we stop our movement
        /// </summary>
        public override void OnExitState()
        {
            drilling = false;

            base.OnExitState();

            _characterFly.SetHorizontalMove(0f);
            _characterFly.SetVerticalMove(0f);

        }

        protected void PlayDrillSfx()
        {
            SoundManager.Instance.PlaySound(DrillSfx, transform.position);
        }
    }
}
