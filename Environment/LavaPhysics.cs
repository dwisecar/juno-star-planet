using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    public class LavaPhysics : MonoBehaviour
    {
        protected LucyHealth lucyHealth;
        protected CorgiController controller;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            lucyHealth = collision.gameObject.GetComponent<LucyHealth>();
            if (lucyHealth != null)
            {
                controller = collision.gameObject.GetComponent<CorgiController>();
                controller.Parameters.MaxVelocity = new Vector2(5, 50);
                controller.Parameters.FallMultiplier = .5f;
                controller.Parameters.Gravity = -10f;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            lucyHealth = collision.gameObject.GetComponent<LucyHealth>();
            if (lucyHealth != null)
            {
                controller = collision.gameObject.GetComponent<CorgiController>();
                controller.Parameters.MaxVelocity = new Vector2(50, 50);
                controller.Parameters.FallMultiplier = 1f;
                controller.Parameters.Gravity = -30f;

            }
        }
    }
}