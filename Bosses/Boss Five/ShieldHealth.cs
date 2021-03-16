using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class ShieldHealth : Health
    {
        public GameObject BuggyOne;
        public GameObject BuggyTwo;
        public GameObject BuggyThree;
        public GameObject BuggyFour;

        public BossFiveManager BossFiveManager;

        protected AIDecisionBuggyFly b1BuggyFly;
        protected AIDecisionBuggyFly b2BuggyFly;
        protected AIDecisionBuggyFly b3BuggyFly;
        protected AIDecisionBuggyFly b4BuggyFly;

        protected override void Initialization()
        {
            base.Initialization();

            b1BuggyFly = BuggyOne.GetComponent<AIDecisionBuggyFly>();
            b2BuggyFly = BuggyTwo.GetComponent<AIDecisionBuggyFly>();
            b3BuggyFly = BuggyThree.GetComponent<AIDecisionBuggyFly>();
            b4BuggyFly = BuggyFour.GetComponent<AIDecisionBuggyFly>();


        }

        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration)
        {
            if (!BossFiveManager.HasBeenAttacked)
            {
                WakeUpBugs();
                BossFiveManager.HasBeenAttacked = true;
            }

            base.Damage(damage, instigator, flickerDuration, invincibilityDuration);
        }

        public override void Kill()
        {
            BossFiveManager.ShieldDown = true;
            base.Kill();
        }

        public virtual void WakeUpBugs()
        {
            b1BuggyFly.FlyBuggy = true;
            b2BuggyFly.FlyBuggy = true;
        }
    }
}
