using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

namespace MoreMountains.Tools
{
    public class AIActionFireLightning : AIAction
    {
        public GameObject TopLightning;
        public GameObject FloorLightning;
        public AudioClip LightningSfx1;
        public AudioClip LightningSfx2;

        protected bool FiringStarted;

        public override void PerformAction()
        {
            if(!FiringStarted)
            {
                StartCoroutine(FireLightning());
            }
        }

        protected IEnumerator FireLightning()
        {
            FiringStarted = true;
            yield return new WaitForSeconds(7);
            TopLightning.SetActive(true);
            SoundManager.Instance.PlaySound(LightningSfx1, transform.position);
            yield return new WaitForSeconds(.25f);
            FloorLightning.SetActive(true);
            SoundManager.Instance.PlaySound(LightningSfx2, transform.position);
            yield return new WaitForSeconds(1.75f);
            TopLightning.SetActive(false);
            FloorLightning.SetActive(false);
            FiringStarted = false;
        }
    }
}