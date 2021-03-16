using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

namespace MoreMountains.Tools
{
    public class AIDecisionShieldRestored : AIDecision
    {
        public BossFiveManager B5Manager;

        public override bool Decide()
        {
            if (B5Manager.ShieldDown == false)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}