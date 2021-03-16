using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIPhageDoNothing : AIActionDoNothing
    {
        protected Transform _phage;
        public float Speed = 3f;
        protected Vector3 pos1; //hover mode position 1
        protected Vector3 pos2; //hover kode pos 2
        protected PhageWobbler wobbler;



        protected override void Initialization()
        {
            _phage = GetComponent<Transform>();
            pos1 = new Vector3(_phage.position.x, _phage.position.y + 1.5f, _phage.position.z);
            pos2 = new Vector3(_phage.position.x, _phage.position.y - 1.5f, _phage.position.z);
            wobbler = GetComponent<PhageWobbler>();
        }


        public override void PerformAction()
        {
            //for vertical movement
            _phage.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Speed * Time.time) + 1.0f) / 2.0f);
            wobbler.WobbleMode = true;

        }
    }
}