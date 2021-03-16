using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class AIDecisionRoboRobeDoneMoving : AIDecision
    {
        public bool MoveCompleted;

        protected override void Start()
        {
            base.Start();
        }

        public override bool Decide()
        {
            if(MoveCompleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();
            MoveCompleted = false;
        }
    }
}