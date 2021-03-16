using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    //To be put on player magnet ability to send magnetic projectiles back into enemies
    public class Magnetizer : MonoBehaviour
    {
        protected PointEffector2D _magnet;
        protected MagneticProjectile _magProj;
        protected CircleCollider2D _circleCollider;

        // Start is called before the first frame update
        void Start()
        {
            _magnet = GetComponent<PointEffector2D>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckIfColliderTurnedOff();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _magProj = collision.GetComponent<MagneticProjectile>();

            if(_magProj!=null)
            {
                if (_magnet.forceMagnitude > .01f)
                {
                    _magProj.SeekEnemies(true);
                }
                
            }
        }

        protected void CheckIfColliderTurnedOff()
        {
            if(_magProj!=null)
            {
                if(_circleCollider.enabled == false)
                {
                    _magProj.SeekEnemies(false);
                    
                }

                if (_magnet.forceMagnitude < -.01f)
                {
                    _magProj.SeekEnemies(false);
                }
            }


        }
    }
}