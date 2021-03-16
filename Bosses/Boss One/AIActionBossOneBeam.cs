using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// An Action that shoots using the currently equipped weapon. If your weapon is in auto mode, will shoot until you exit the state, and will only shoot once in SemiAuto mode. You can optionnally have the character face (left/right) the target, and aim at it (if the weapon has a WeaponAim component).
    /// </summary>
    public class AIActionBossOneBeam : AIAction
    {
        public GameObject Beam;
        public Animator anim;
        public float TimeWithBeamOn = 3f;

        protected Animator beamAnim;
        protected CharacterFly characterFly;
        protected AIDecisionFireBeam beamDecision;

        /// <summary>
        /// On init we grab our CharacterHandleWeapon ability
        /// </summary>
        protected override void Initialization()
        {
            beamAnim = Beam.GetComponent<Animator>();
            characterFly = this.gameObject.GetComponent<CharacterFly>();
            beamDecision = this.gameObject.GetComponent<AIDecisionFireBeam>();
        }

        /// <summary>
        /// On PerformAction we face and aim if needed, and we shoot
        /// </summary>
        public override void PerformAction()
        {
            StartCoroutine(FireBeam());
        }

        protected IEnumerator FireBeam()
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Firing", true);
            yield return new WaitForSeconds(1f);
            Beam.SetActive(true);
            beamAnim.SetBool("BeamOn", true);
            yield return new WaitForSeconds(TimeWithBeamOn);
            Beam.SetActive(false);
            anim.SetBool("Idle", true);
            anim.SetBool("Firing", false);
            beamAnim.SetBool("BeamOn", false);
        }

        /// <summary>
        /// When entering the state we reset our shoot counter and grab our weapon
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();

        }

        /// <summary>
        /// When exiting the state we make sure we're not shooting anymore
        /// </summary>
        public override void OnExitState()
        {
            base.OnExitState();
            Beam.SetActive(false);
            beamAnim.SetBool("BeamOn", false);
        }
    }
}
