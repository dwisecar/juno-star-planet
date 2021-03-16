using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
    public class AIDecisionBuggyShoot : AIDecision
    {
        public int FirstPointToShootAt;
        public int SecondPointToShootAt;

        protected MMPathMovement _path;

        protected bool alreadyAtFirstPoint;
        protected bool alreadyAtSecondPoint;


        public override void Initialization()
        {
            base.Initialization();
            _path = GetComponent<MMPathMovement>();
        }

        public override bool Decide()
        {
            if(!alreadyAtFirstPoint)
            {
                if (_path.GetCurrentIndexPoint() == FirstPointToShootAt)
                {
                    alreadyAtFirstPoint = true;
                    alreadyAtSecondPoint = false;
                    return true;
                }
                else { return false; }
            }

            if (!alreadyAtSecondPoint)
            {
                if (_path.GetCurrentIndexPoint() == SecondPointToShootAt)
                {
                    alreadyAtFirstPoint = false;
                    alreadyAtSecondPoint = true;
                    return true;
                }
                else { return false; }
            }

            else { return false; }
        }

        
    }
}
