using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("SuperBlasterBooster")]

    public class SuperBlasterBooster : MonoBehaviour
    {

        public bool superBlasterObtained = false;
        protected Projectile _projectile;
        protected DamageOnTouch _damageOnTouch;

        // Use this for initialization
        void Start()
        {
            _damageOnTouch = GetComponent<DamageOnTouch>();
            SuperBlasterBonus();
        }

        public virtual void SuperBlasterObtained()
        {
            superBlasterObtained = true;
        }

        public virtual void SuperBlasterBonus()
        {
            if (superBlasterObtained)
            {
                
                _damageOnTouch.DamageCaused = _damageOnTouch.DamageCaused * 2;

            }
        }
    }
}