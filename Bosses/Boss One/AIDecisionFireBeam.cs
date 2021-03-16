using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This Decision will return true if the current Brain's Target is within the specified range, false otherwise.
    /// </summary>
    public class AIDecisionFireBeam : AIDecision
    {

        public bool BeamMode; 

        /// <summary>
        /// On Decide we check our distance to the Target
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            return EvaluateAttackMode();
        }

        public bool EvaluateAttackMode()
        {
            if (BeamMode == true) { return true; }
            if (BeamMode == false) { return false; }

            return false;
        }




    }
}