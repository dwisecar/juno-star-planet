using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIDecisionToFireBeam : AIDecision
    {
        public int PathPointToFireBeam;

        protected MMPathMovement _path;

        protected override void Start()
        {
            base.Start();
            _path = GetComponent<MMPathMovement>();
        }

        public override bool Decide()
        {
            if(ComparePathPoint())
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
            if(PathPointToFireBeam == _path.GetCurrentIndexPoint())
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