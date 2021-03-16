using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{

	public class CoreHealth : Health
	{

        public AutoRespawn ShieldRespawner;
        public BossFiveManager bossFiveManager;


        public override void Kill()
        {
            
            bossFiveManager.CoresDown++;

            bossFiveManager.WakeUpBuggys();

            bossFiveManager.TurnOffAllChargeEffects();

            bossFiveManager.ResetCoresHealth();

            if(bossFiveManager.CoresDown < 3)
            {
                ShieldRespawner.Revive();
                bossFiveManager.ShieldDown = false;
                bossFiveManager.TurnOffAllChargeEffects();
            }



            base.Kill();
        }
    }
}