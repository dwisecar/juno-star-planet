using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class BossOneFireBeamCheck : MonoBehaviour
    {
        protected MMPathMovement _path;
        public GameObject Beam;
        public Animator anim;
        public float TimeWithBeamOn = 3f;
        public float TimeBeforeBeamOn = 1.5f;

        public AudioClip BeamChargeSfx;
        public AudioClip BeamOnSfx;

        protected Animator beamAnim;
        protected Character character;
        public bool firingBeam;
        protected bool flipped;
        public float startingHeight;

        // Start is called before the first frame update
        void Start()
        {
            _path = GetComponent<MMPathMovement>();
            character = GetComponent<Character>();
            startingHeight = transform.position.y;
            beamAnim = Beam.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {


            if ((startingHeight - transform.position.y) > 4.9f)
            {
                if (!firingBeam)
                {
                    StartCoroutine(FireBeam());
                }
            }

        }

        protected IEnumerator FireBeam()
        {
            firingBeam = true;
            anim.SetBool("Idle", false);
            anim.SetTrigger("Firing");
            PlayChargeSfx();
            yield return new WaitForSeconds(TimeBeforeBeamOn);
            Beam.SetActive(true);
            beamAnim.SetBool("BeamOn", true);
            PlayBeamSfx();
            yield return new WaitForSeconds(TimeWithBeamOn);
            Beam.SetActive(false);
            anim.SetBool("Idle", true);
            
            beamAnim.SetBool("BeamOn", false);
            firingBeam = false;
        }

        protected virtual void PlayChargeSfx()
        {
            if (BeamChargeSfx != null)
            {
                SoundManager.Instance.PlaySound(BeamChargeSfx, transform.position);
            }
        }

        protected virtual void PlayBeamSfx()
        {
            if (BeamOnSfx != null)
            {
                SoundManager.Instance.PlaySound(BeamOnSfx, transform.position);
            }
        }
    }
}