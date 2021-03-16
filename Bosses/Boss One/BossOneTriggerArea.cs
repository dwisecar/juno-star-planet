using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class BossOneTriggerArea : MonoBehaviour
    {
        protected AIDecisionFireBeam beam;

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            beam = collision.GetComponent<AIDecisionFireBeam>();

            if (beam!=null)
            {
                StartCoroutine(TurnBeamOn());
            }
        }

        protected IEnumerator TurnBeamOn()
        {
            beam.BeamMode = true;
            
            yield return new WaitForSeconds(4f);
            beam.BeamMode = false;
        }
    }
}