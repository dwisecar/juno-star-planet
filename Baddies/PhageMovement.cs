using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{

    public class PhageMovement : MonoBehaviour
    {

        public bool AttackMode;
        public bool PlayerSpotted;
        public float Speed = 9f;
        public float WaitTimeBeforeAttack = 2.5f;
        public GameObject SpikeTube;

        //Over Player Vector3 is received from Phage Detector Class
        public virtual void MoveOverPlayer(Vector3 _overPlayer)
        {
            transform.position += _overPlayer * Time.deltaTime * Speed;
        }

        public virtual void AttackPlayer(Vector3 _onPlayerHead)
        {
            transform.position += _onPlayerHead * Time.deltaTime * Speed;
        }

        protected void DropSpike()
        {
            SpikeTube.transform.localPosition = new Vector3(0, -2, 0) * Time.deltaTime * Speed;
        }

        public virtual IEnumerator HoverWaitAttack()
        {
            AttackMode = false;
            DropSpike();
            yield return new WaitForSeconds(WaitTimeBeforeAttack);
            AttackMode = true;
        }
    }
}