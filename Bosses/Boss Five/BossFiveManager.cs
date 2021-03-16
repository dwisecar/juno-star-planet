using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	public class BossFiveManager : MonoBehaviour
	{
        public bool HasBeenAttacked;
        public int CoresDown;
        public bool ShieldDown;

        [Header("Buggies")]
        public GameObject BuggyOne;
        public GameObject BuggyTwo;
        public GameObject BuggyThree;
        public GameObject BuggyFour;

        [Header("Core Health and respawners")]
        public CoreHealth C1Health;
        public CoreHealth C2Health;
        public CoreHealth C3Health;
        public AutoRespawn C1Respawn;
        public AutoRespawn C2Respawn;
        public AutoRespawn C3Respawn;
        public AutoRespawn ShieldRespawner;
        public ShieldHealth shieldHealth;

        [Header("Buggy Health")]
        protected BaddieHealth b1Health;
        protected BaddieHealth b2Health;
        protected BaddieHealth b3Health;
        protected BaddieHealth b4Health;

        [Header("Buggy Auto Respawns")]
        protected AutoRespawn b1Respawn;
        protected AutoRespawn b2Respawn;
        protected AutoRespawn b3Respawn;
        protected AutoRespawn b4Respawn;

        [Header("Buggy Fly")]
        protected AIDecisionBuggyFly b1BuggyFly;
        protected AIDecisionBuggyFly b2BuggyFly;
        protected AIDecisionBuggyFly b3BuggyFly;
        protected AIDecisionBuggyFly b4BuggyFly;

        protected bool deathEffectStarted;

        [Header("Core Charging Effects")]
        public ParticleSystem LHChargeFX;
        public ParticleSystem RHChargeFX;
        public ParticleSystem BottomChargeFX;
        public AudioClip ChargingSfx; //no longer than a one second clip
        protected bool runningChargeCycle;

        [Header("Death Effects")]
        public ParticleSystem DeathParticleEffect; //add MM Auto Destroy Particle System to game object.
        public SpriteRenderer HeartSpriteRenderer;
        public AudioClip DeathSfx;
        public GameObject DoorOutOfArea;

        public bool testBossReset;

        private void Start()
        {
            b1Health = BuggyOne.GetComponent<BaddieHealth>();
            b2Health = BuggyTwo.GetComponent<BaddieHealth>();
            b3Health = BuggyThree.GetComponent<BaddieHealth>();
            b4Health = BuggyFour.GetComponent<BaddieHealth>();

            b1Respawn = BuggyOne.GetComponent<AutoRespawn>();
            b2Respawn = BuggyTwo.GetComponent<AutoRespawn>();
            b3Respawn = BuggyThree.GetComponent<AutoRespawn>();
            b4Respawn = BuggyFour.GetComponent<AutoRespawn>();

            b1BuggyFly = BuggyOne.GetComponent<AIDecisionBuggyFly>();
            b2BuggyFly = BuggyTwo.GetComponent<AIDecisionBuggyFly>();
            b3BuggyFly = BuggyThree.GetComponent<AIDecisionBuggyFly>();
            b4BuggyFly = BuggyFour.GetComponent<AIDecisionBuggyFly>();
        }

        void Update()
        {
            if (ShieldDown)
            {
                if (!runningChargeCycle)
                {
                    StartCoroutine(RunChargingCycle());
                }
            }

            if (CoresDown == 3)
            {
                if (!deathEffectStarted)
                {
                    StartCoroutine(PlayDeathEffects());
                }
            }

            if(testBossReset) { RestoreBoss(); }
        }

        //do the same with turn charge off()
        protected void TurnOnChargeFX(string coreLocation)
        {
            if (coreLocation == "LH")
            {
                ParticleSystem.EmissionModule emissionModule = LHChargeFX.emission;
                emissionModule.enabled = true;
            }

            if (coreLocation == "RH")
            {
                ParticleSystem.EmissionModule emissionModule = RHChargeFX.emission;
                emissionModule.enabled = true;
            }

            if (coreLocation == "Bottom")
            {
                ParticleSystem.EmissionModule emissionModule = BottomChargeFX.emission;
                emissionModule.enabled = true;
            }
        }

        //doin the same with turn charge off()
        protected void TurnOffChargeFX(string coreLocation)
        {
            if (coreLocation == "LH")
            {
                ParticleSystem.EmissionModule emissionModule = LHChargeFX.emission;
                emissionModule.enabled = false;
            }

            if (coreLocation == "RH")
            {
                ParticleSystem.EmissionModule emissionModule = RHChargeFX.emission;
                emissionModule.enabled = false;
            }

            if (coreLocation == "Bottom")
            {
                ParticleSystem.EmissionModule emissionModule = BottomChargeFX.emission;
                emissionModule.enabled = false;
            }
        }

        public void TurnOffAllChargeEffects()
        {
            ParticleSystem.EmissionModule emissionModuleLH = LHChargeFX.emission;
            emissionModuleLH.enabled = false;

            ParticleSystem.EmissionModule emissionModuleRH = LHChargeFX.emission;
            emissionModuleRH.enabled = false;

            ParticleSystem.EmissionModule emissionModuleB = LHChargeFX.emission;
            emissionModuleB.enabled = false;
        }

        //run this as soon as the shield drops
        public IEnumerator RunChargingCycle()
        {
            runningChargeCycle = true;

            if(ShieldDown)
            {
                TurnOnChargeFX("LH");
                PlayChargingSfx();
                yield return new WaitForSeconds(1);
                TurnOffChargeFX("LH");
                yield return new WaitForSeconds(2);
            }

            if(ShieldDown)
            {
                TurnOnChargeFX("RH");
                PlayChargingSfx();
                yield return new WaitForSeconds(1);
                TurnOffChargeFX("RH");
                yield return new WaitForSeconds(2);
            }

            if(ShieldDown)
            {
                TurnOnChargeFX("Bottom");
                PlayChargingSfx();
                yield return new WaitForSeconds(1);
                TurnOffChargeFX("Bottom");
                yield return new WaitForSeconds(2);
            }

            runningChargeCycle = false;
        }

        protected IEnumerator PlayDeathEffects()
        {
            deathEffectStarted = true;
            BackgroundMusic.Instance.PlayChillRoomMusic();
            PlayFinalExplosionEffect();
            yield return new WaitForSeconds(3);
            HeartSpriteRenderer.enabled = false;
            DoorOutOfArea.SetActive(false);
        }

        protected void PlayFinalExplosionEffect()
        {
            if (DeathParticleEffect != null)
            {
                ParticleSystem.EmissionModule emissionModule = DeathParticleEffect.emission;
                emissionModule.enabled = true;
            }
        }

        protected void PlayChargingSfx()
        {
            SoundManager.Instance.PlaySound(ChargingSfx, transform.position);
        }


        public void WakeUpBuggys()
        {
            if(CoresDown == 3) { return; }

            b1BuggyFly.FlyBuggy = true;
            b2BuggyFly.FlyBuggy = true;

            b1Respawn.Revive();
            b2Respawn.Revive();

            if(CoresDown > 0)
            {
                b3BuggyFly.FlyBuggy = true;
                b3Respawn.Revive();
            }

            if(CoresDown > 1)
            {
                b4BuggyFly.FlyBuggy = true;
                b4Respawn.Revive();
            }
        }

        public void ResetCoresHealth()
        {
            if(C1Health.CurrentHealth > 0)
            {
                C1Health.ResetHealthToMaxHealth();
            }

            if (C2Health.CurrentHealth > 0)
            {
                C2Health.ResetHealthToMaxHealth();
            }

            if (C3Health.CurrentHealth > 0)
            {
                C3Health.ResetHealthToMaxHealth();
            }
        }

        public void RestoreBoss()
        {
            if(ShieldDown)
            {
                ShieldRespawner.Revive();
            }
            else { shieldHealth.ResetHealthToMaxHealth(); }

            if(C1Health.CurrentHealth <= 0) { C1Respawn.Revive(); }
            else { C1Health.ResetHealthToMaxHealth(); }

            if (C2Health.CurrentHealth <= 0) { C2Respawn.Revive(); }
            else { C2Health.ResetHealthToMaxHealth(); }

            if (C3Health.CurrentHealth <= 0) { C3Respawn.Revive(); }
            else { C3Health.ResetHealthToMaxHealth(); }

            HasBeenAttacked = false;
            CoresDown = 0;
            ShieldDown = false;

        }
    }
}