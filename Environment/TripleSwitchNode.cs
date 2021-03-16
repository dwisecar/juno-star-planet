using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine{

    [AddComponentMenu("Prefabs/Envirnoment/TripleSwitchNode")]
    public class TripleSwitchNode : MonoBehaviour
    {
        protected Color _initialColor;
        public float TimeNodeIsActive = .5f;
        public bool _nodeHit;
        protected Animator anim;

        public BossNodeChecker Chamber;

        public bool DoorOpened;

        protected void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            CheckNode();
        }

        public void OnTriggerEnter2D(Collider2D _collision)
        {
            if (_collision.gameObject.tag == "PlayerProjectiles")
            {
               _nodeHit = true;
            }
        }

        protected virtual void CheckNode()
        {
            if(_nodeHit)
            {
                if(DoorOpened)
                {
                    StartCoroutine(ChamberOpen());
                }
                else
                {
                    StartCoroutine(SwitchHit());
                }
            }
        }

        public virtual IEnumerator SwitchHit()
        {
            anim.SetBool("NodeOpened", true);
            yield return new WaitForSeconds(TimeNodeIsActive);
            _nodeHit = false;

            if (Chamber == null)
            {
                anim.SetBool("NodeOpened", false);
                
            }
            else
            {
                yield return new WaitForSeconds(1f);

                anim.SetBool("NodeOpened", false);
            }
          
        }

        protected virtual IEnumerator ChamberOpen()
        {
            if(Chamber == null)
            {
                anim.SetBool("DoorOpened", true);
            }
            else
            {
                anim.SetBool("NodeOpened", true);
                yield return new WaitForSeconds(Chamber.TimeChamberStaysOpen);
                _nodeHit = false;
                anim.SetBool("NodeOpened", false);
            }

           
        }
    }
}
