using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class MagnetPlug : MonoBehaviour
    {

        protected Rigidbody2D rb2d;
        public BoxCollider2D plugCollider;
        public MovingPlatform magnetLockedDoor;


        // Use this for initialization
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerEnter2D(Collider2D _collision)
        {
            rb2d = _collision.GetComponentInParent<Rigidbody2D>();


            if (_collision.gameObject.tag == "Prongs")
            {
                plugCollider.enabled = false;

                rb2d.velocity = Vector2.zero;
                rb2d.angularVelocity = 0f;
                rb2d.MoveRotation(0f);
                rb2d.isKinematic = true;

                magnetLockedDoor.AuthorizeMovement();

            }
        }
    }
}