using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIActionTurtleDuck : AIAction
    {
        protected BaddieHealth health;
        protected Animator anim;
        protected CharacterHorizontalMovement movement;


        protected override void Start()
        {
            base.Start();
            health = GetComponent<BaddieHealth>();
            anim = GetComponent<Animator>();
            movement = GetComponent<CharacterHorizontalMovement>();
        }

        

        protected void DuckMode()
        {
            health.Invulnerable = true;
            anim.SetBool("Ducking", true);
            movement.MovementSpeed = 0f;
        }

        public override void PerformAction()
        {
            DuckMode();
        }

        public override void OnExitState()
        {
            health.Invulnerable = false;
            anim.SetBool("Ducking", false) ;
            movement.MovementSpeed = 3f;

            base.OnExitState();

        }
    }
}