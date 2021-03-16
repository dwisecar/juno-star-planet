using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class PhageWobbler : MonoBehaviour
    {
        [Header("Wobble Rotation")]
        public float _Angle = 10f;
        public float _Period = .15f;
        protected float _Time;
        protected Transform _phage;
        public bool WobbleMode;

        // Start is called before the first frame update
        void Start()
        {
            _phage = GetComponent<Transform>();
            WobbleMode = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(WobbleMode)
            {
                //for rotational wobble
                _Time = _Time + Time.deltaTime;
                float phase = Mathf.Sin(_Time / _Period);
                _phage.localRotation = Quaternion.Euler(new Vector3(0, 0, phase * _Angle));
            }
            else
            {
                _phage.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
    }
}