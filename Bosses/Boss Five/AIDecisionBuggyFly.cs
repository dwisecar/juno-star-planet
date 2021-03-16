using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoreMountains.Tools
{
    public class AIDecisionBuggyFly : AIDecision
    {
        //fly buggy called from boss state manager
        public bool FlyBuggy;

        public override bool Decide()
        {
            if(FlyBuggy)
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