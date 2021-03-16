using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    //to be put on magnetic projectiles so they can be "magnetized" and hurt enemies too.
    public class MagneticProjectile : MonoBehaviour
    {
        public bool CanHurtEnemies; //for the boss 3 shield to see if it is being targeted and if so, will be disabled.

        protected Health _health;
        protected SmartMissile2D _missile;
        protected string EnemyTag = "Enemies";
        protected string PlayerTag = "Player";
        protected Animator anim;


        // Start is called before the first frame update
        void Start()
        {
            _health = GetComponent<Health>();
            _missile = GetComponent<SmartMissile2D>();
            anim = GetComponent<Animator>();
        }

        //called from the boss shield to disable the projectile.
        public void KillMagenticProjectile()
        {
            _health.Kill();
        }

        //called from the magnetizer class on the player to redirect the projectile towards an enemy while the magnet is on.
        public void SeekEnemies(bool it)
        {
           
            if(it)
            {
                if (!CanHurtEnemies)
                {
                    _missile.TargetTag = EnemyTag;
                    _missile.FindNewTarget();
                    CanHurtEnemies = true;
                    anim.SetBool("Reversed", true);
                }
                
            }
            else
            {
                if(CanHurtEnemies)
                {
                    _missile.TargetTag = PlayerTag;
                    _missile.FindNewTarget();
                    CanHurtEnemies = false;
                    anim.SetBool("Reversed", false);

                }
            }
        }
        


    }
}