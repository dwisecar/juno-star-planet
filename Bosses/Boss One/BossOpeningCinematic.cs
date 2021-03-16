using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.MMInterface;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MoreMountains.CorgiEngine
{
    public class BossOpeningCinematic : MonoBehaviour
    {

        public GameObject ActiveBoss;
        public AudioClip BossRoarSfx;
        protected Animator anim;
        public float BeforeRoaringTime = 2f;
        public float RoaringTime = 2f;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public virtual IEnumerator BossCinematic()
        {
            ActiveBoss.SetActive(false);
            anim.SetBool("Roar", false);
            yield return new WaitForSeconds(BeforeRoaringTime);
            PlayBossRoarSfx();
            anim.SetBool("Roar", true);
            yield return new WaitForSeconds(RoaringTime);
            ActiveBoss.SetActive(true);
            Destroy(gameObject);
        }

        protected virtual void PlayBossRoarSfx()
        {
            if (BossRoarSfx != null)
            {
                SoundManager.Instance.PlaySound(BossRoarSfx, transform.position);
            }
        }
    }
}