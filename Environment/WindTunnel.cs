using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class WindTunnel : MonoBehaviour
    {
        public float HorizontalForce = 1f;
        public float VerticalForce = 1f;

        protected CorgiController _controller;
        protected Vector2 _windDirection;


        // Use this for initialization
        void Start()
        {
            _windDirection = new Vector2(HorizontalForce, VerticalForce);
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerStay2D(Collider2D _collider)
        {
            _controller = _collider.GetComponent<CorgiController>();

            if(_controller != null)
            {
                if (_controller.State.IsGrounded)
                {
                    _controller.AddForce(_windDirection);
                }
                else
                {
                    _controller.AddForce(_windDirection / 2);

                }

            }
        }
    }
}