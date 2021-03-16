using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class BossNodeChecker : MonoBehaviour
    {
        public BossTwoHealth Nucleus;
        public TripleSwitchNode nodeA;
        public TripleSwitchNode nodeB;

       
        public float TimeChamberStaysOpen = 5f;
        public AudioClip ChamberOpeningSfx;
        public AudioClip ChamberClosingSfx;

        protected Animator chamberAnim;
        protected BoxCollider2D chamberBoxCollider2d;
        protected bool chamberOpen;

        // Use this for initialization
        void Start()
        {
            chamberBoxCollider2d = GetComponent<BoxCollider2D>();
            chamberAnim = GetComponent<Animator>();

            Nucleus.Invulnerable = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(!chamberOpen)
            {
                CheckNodes();
            }
            
        }

        protected virtual void CheckNodes()
        {
            if (nodeA._nodeHit && nodeB._nodeHit)
            {
                StartCoroutine(OpenChamber());
            }
        }

        protected virtual IEnumerator OpenChamber()
        {
            chamberOpen = true;
            PlayChamberOpenSfx();
            chamberBoxCollider2d.enabled = false;
            chamberAnim.SetBool("ChamberOpen", true);
            nodeA.DoorOpened = true;
            nodeB.DoorOpened = true;
            Nucleus.Invulnerable = false;

            yield return new WaitForSeconds(TimeChamberStaysOpen);

            chamberOpen = false;
            PlayChamberClosingSfx();
            chamberBoxCollider2d.enabled = true;
            chamberAnim.SetBool("ChamberOpen", false);
            nodeA.DoorOpened = false;
            nodeB.DoorOpened = false;
            Nucleus.Invulnerable = true;
        }

        protected void PlayChamberOpenSfx()
        {
            SoundManager.Instance.PlaySound(ChamberOpeningSfx, transform.position);
        }

        protected void PlayChamberClosingSfx()
        {
            SoundManager.Instance.PlaySound(ChamberClosingSfx, transform.position);
        }

        protected void NucleusInvulnerable()
        {
            Nucleus.Invulnerable = true;
        }
    }
}