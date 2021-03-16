using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	public class AIActionFireBeam : AIAction
	{
		public GameObject Beam;
		public AudioClip BeamSfx;
		public AudioClip ChargingSfx;
        public float TimeBeforeBeamOn = 1.5f;
        public int PathPointBeamFiresOn;
		public ParticleSystem ParticleEmitter;

		protected MMPathMovement _path;
		public Animator anim;
		protected bool beamFired;
        protected Animator beamAnim;

        protected override void Start()
		{
			base.Start();
			_path = GetComponent<MMPathMovement>();
            beamAnim = Beam.GetComponent<Animator>();
        }

		public override void PerformAction()
		{
			if (_path.GetCurrentIndexPoint() == PathPointBeamFiresOn)
			{
				if (!beamFired)
				{
					StartCoroutine(FireBeam());
				}
			}
		}

		protected IEnumerator FireBeam()
		{
			beamFired = true;

			PlayChargingSfx();
            anim.SetBool("Idle", false);
            anim.SetTrigger("Firing");
            TurnChargingEmitterOn();

			yield return new WaitForSeconds(TimeBeforeBeamOn);
            beamAnim.SetBool("BeamOn", true);
            Beam.SetActive(true);
			PlayBeamSfx();
            yield return new WaitForSeconds(4f);
            TurnChargingEmitterOff();
            
            Beam.SetActive(false);
            anim.SetBool("Idle", true);
            beamAnim.SetBool("BeamOn", false);
            beamFired = false;
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
			
			TurnChargingEmitterOff();
			beamFired = false;
            Beam.SetActive(false);
            anim.SetBool("Idle", true);
            beamAnim.SetBool("BeamOn", false);
            base.OnExitState();

        }
	}
}