using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionShootMagnet : AIAction
    {
        [Header("Objects")]
        public GameObject MagnetBomb;
        public Transform WeaponAttachment;
        public GameObject LightningRods;

        [Header("Timing Variables")]
        public float TimeBeforeLightningAttack = 15f;
        public float TimeBeforeMagnetAttack = 15f;
        public float LightningActiveTime = 3f;
        public float PauseBetweenBolts = 1.5f;

        [Header("Sfx")]
        public AudioClip RoarSfx;
        public AudioClip FireMagnetSfx;
        public AudioClip LightningSfx;
        public _2dxFX_LightningBolt _2DxFX_LightningBolt;

        public bool StartingCinematicHasHappened;
        public bool CycleInProgress;

        protected Animator anim;

        protected override void Initialization()
        {
            base.Initialization();
            anim = GetComponentInChildren<Animator>();
        }

        public override void PerformAction()
        {
            if(!StartingCinematicHasHappened)
            {
                StartCoroutine(SeeTargetRoar());
            }

            if(!CycleInProgress)
            {
                StartCoroutine(ToggleAttackModes());
            }
        }

        protected IEnumerator SeeTargetRoar()
        {
            StartingCinematicHasHappened = true;
            CycleInProgress = true;
            yield return new WaitForSeconds(2f);
            anim.SetBool("Roar", true);
            PlayRoarSfx();
            yield return new WaitForSeconds(1.167f);
            BackgroundMusic.Instance.PlayBossMusic();
            anim.SetBool("Roar", false);
            CycleInProgress = false;
        }

        protected void DeployMagnetBomb()
        {
            Instantiate(MagnetBomb, WeaponAttachment.position, WeaponAttachment.rotation);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public IEnumerator ToggleAttackModes()
        {
            //Have cycle start once
            CycleInProgress = true;

            //Shoot magnet and wait a while before lighting spinner attack starts
            DeployMagnetBomb();
            PlayMagnetShotSfx();
            yield return new WaitForSeconds(TimeBeforeLightningAttack);

            anim.SetBool("Roar", true);
            PlayRoarSfx();
            yield return new WaitForSeconds(1.167f); //time for roar aninmation to play

            anim.SetBool("Roar", false);
            LightningRods.SetActive(true);
            _2DxFX_LightningBolt.enabled = true;
            PlayLightningSfx();
            yield return new WaitForSeconds(LightningActiveTime);

            LightningRods.SetActive(false);
            _2DxFX_LightningBolt.enabled = false;
            yield return new WaitForSeconds(PauseBetweenBolts);

            LightningRods.SetActive(true);
            _2DxFX_LightningBolt.enabled = true;
            PlayLightningSfx();
            yield return new WaitForSeconds(LightningActiveTime);

            LightningRods.SetActive(false);
            _2DxFX_LightningBolt.enabled = false;
            yield return new WaitForSeconds(TimeBeforeMagnetAttack);

            CycleInProgress = false;


        }

        protected void PlayRoarSfx()
        {
            SoundManager.Instance.PlaySound(RoarSfx, transform.position);
        }

        protected void PlayLightningSfx()
        {
            SoundManager.Instance.PlaySound(LightningSfx, transform.position);
        }

        protected void PlayMagnetShotSfx()
        {
            SoundManager.Instance.PlaySound(FireMagnetSfx, transform.position);
        }

    }
}