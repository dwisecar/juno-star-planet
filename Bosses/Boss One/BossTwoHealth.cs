using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class BossTwoHealth : BossHealth
{
        [Header("Objects to Deactivate")]
        public GameObject[] objectsToBlowUp;

        public GameObject Chamber;

        public void OpenChamberOnDeath()
        {
            Animator anim = Chamber.GetComponent<Animator>();
            anim.SetBool("Death", true);

        }

        protected void BlowUpObjects()
        {
            foreach (GameObject objects in objectsToBlowUp)
            {
                if (objects == null) { return; }
                objects.SetActive(false);

            }
        }

        public override void Kill()
        {
            BlowUpObjects();
            OpenChamberOnDeath();
            base.Kill();
        }
    }
}