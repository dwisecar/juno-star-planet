using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionReleasePhage : AIAction
    {
        [Header("Objects")]
        public GameObject Phage;
        public Transform WeaponAttachment;

        [Header("Sfx")]
        public AudioClip ReleasePhageSfx;

        protected Animator anim;
        protected bool _phageReleased;

        protected override void Initialization()
        {
            base.Initialization();
            anim = GetComponentInChildren<Animator>();
        }

        public override void PerformAction()
        {
            if(!_phageReleased)
            {
                ReleasePhage();
            }
        }

        protected void ReleasePhage()
        {
            Instantiate(Phage, WeaponAttachment.position, WeaponAttachment.rotation);
            PlayPhageReleasedSfx();
            anim.SetBool("ReleasePhage", true);
            _phageReleased = true;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

        }

        public override void OnExitState()
        {
            base.OnExitState();
            _phageReleased = false;
            anim.SetBool("ReleasePhage", false);

        }

        protected void PlayPhageReleasedSfx()
        {
            SoundManager.Instance.PlaySound(ReleasePhageSfx, transform.position);
        }

    }
}