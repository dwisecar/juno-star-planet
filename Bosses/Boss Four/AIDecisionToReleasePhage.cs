using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIDecisionToReleasePhage : AIDecision
    {
        public int PathPointToReleasePhage;

        protected MMPathMovement _path;

        protected override void Start()
        {
            base.Start();
            _path = GetComponent<MMPathMovement>();
        }

        public override bool Decide()
        {
            if (ComparePathPoint())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            ComparePathPoint();
        }

        protected bool ComparePathPoint()
        {
            if (PathPointToReleasePhage == _path.GetCurrentIndexPoint())
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