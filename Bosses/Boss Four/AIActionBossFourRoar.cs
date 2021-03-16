using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionBossFourRoar : AIAction
    {
        protected bool CycleInProgress;
        protected bool timeToFadeIn;
        protected bool fadeInStarted;
        protected Animator anim;
        protected CharacterFollowPath _characterFollowPath;

        public float TimeBeforeRoar = 3f;
        public float RoarAnimationTime;
        public float pathSpeed = 10f;
        public AudioClip RoarSfx;
        public SpriteRenderer Fader;
        public GameObject DoorToTrapPlayerIn;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            anim = GetComponentInChildren<Animator>();
            _characterFollowPath = GetComponent<CharacterFollowPath>();
        }
       

        public override void PerformAction()
        {
            if(!CycleInProgress)
            {
                StartCoroutine(SeeTargetRoar());
            }

            if(timeToFadeIn)
            {
                if(!fadeInStarted)
                {
                    StartCoroutine(FadeBackgroundIn(0f, 1f));
                }
            }
        }

        protected IEnumerator SeeTargetRoar()
        {
            CycleInProgress = true;
            yield return new WaitForSeconds(TimeBeforeRoar);
            timeToFadeIn = true;
            anim.SetBool("Roar", true);
            PlayRoarSfx();
            yield return new WaitForSeconds(RoarAnimationTime);
            BackgroundMusic.Instance.PlayBossMusic();
            anim.SetBool("Roar", false);
            _characterFollowPath.FollowPathSpeed = pathSpeed;

            BackgroundMusic.Instance.PlayBossMusic();

            if (DoorToTrapPlayerIn!=null)
            {
                DoorToTrapPlayerIn.SetActive(true);

            }
        }


        protected IEnumerator FadeBackgroundIn(float aValue, float aTime)
        {
            fadeInStarted = true;
            float alpha = Fader.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                Fader.material.color = newColor;
                yield return null;
            }
        }

        protected void PlayRoarSfx()
        {
            SoundManager.Instance.PlaySound(RoarSfx, transform.position);
        }
    }
}