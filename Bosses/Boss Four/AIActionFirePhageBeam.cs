using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionFirePhageBeam : AIAction
    {
        public GameObject Beam;
        public AudioClip BeamSfx;
        public AudioClip ChargingSfx;

        public int PathPointBeamFiresOn;
        public ParticleSystem ParticleEmitter;

        protected MMPathMovement _path;
        protected Animator anim;
        protected bool beamFired;

        protected override void Start()
        {
            base.Start();
            _path = GetComponent<MMPathMovement>();
            anim = GetComponent<Animator>();
        }

        public override void PerformAction()
        {
            if(_path.GetCurrentIndexPoint() == PathPointBeamFiresOn)
            {
                if(!beamFired)
                {
                    StartCoroutine(FireBeam());
                }
            }
        }

        protected IEnumerator FireBeam()
        {
            beamFired = true;

            PlayChargingSfx();
            anim.SetBool("Shoot", true);
            TurnChargingEmitterOn();

            yield return new WaitForSeconds(1.5f);

            Beam.SetActive(true);
            PlayBeamSfx();
        }

        protected void PlayBeamSfx()
        {
            SoundManager.Instance.PlaySound(BeamSfx, transform.position);
        }

        protected void PlayChargingSfx()
        {
            SoundManager.Instance.PlaySound(ChargingSfx, transform.position);
        }

        protected void TurnChargingEmitterOn()
        {
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = true;

            }
        }

        protected void TurnChargingEmitterOff()
        {
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = false;

            }
        }

        public override void OnExitState()
        {
            anim.SetBool("Shoot", false);
            TurnChargingEmitterOff();
            beamFired = false;
            base.OnExitState();
            Beam.SetActive(false);
            anim.SetBool("Shoot", false) ;
        }
    }
}