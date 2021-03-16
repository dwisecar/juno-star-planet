using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

namespace MoreMountains.Tools
{
	public class AIActionBuggyFly : AIAction
	{
        public float FlySpeed = 6;
        protected MMPathMovement _path;
        protected CharacterFollowPath _followPath;
        protected bool speedSet;
        protected Animator _anim;

        protected override void Initialization()
        {
            base.Initialization();
            _path = GetComponent<MMPathMovement>();
            _anim = GetComponent<Animator>();
            _followPath = GetComponent<CharacterFollowPath>();
        }

        public override void PerformAction()
        {
            if (!speedSet)
            {
                StartFlying();
            }
        }

        protected void StartFlying()
        {
            _path.MovementSpeed = FlySpeed;
            _followPath.FollowPathSpeed = FlySpeed;
            //speedSet = true;
        }

        public override void OnEnterState()
        {
            _anim.SetBool("Awake", true);
            
            speedSet = false;
            base.OnEnterState();
            speedSet = false;
        }

        public override void OnExitState()
        {
            speedSet = false;
            base.OnExitState();
            speedSet = false;
        }
    }
}