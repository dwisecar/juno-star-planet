using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class StuckInWallFixer : Teleporter
    {
        public LayerMask TargetLayerMask;
        public float TeleportDistance = 2;

        protected CorgiController _controller;
        protected Vector3 _destination;

        public enum WallSide
        {
            Top,
            Left,
            Right,
            Bottom
        }

        public WallSide wallSide;

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            
            if (wallSide == WallSide.Bottom)
            {
                _destination = new Vector3(collider.transform.position.x, collider.transform.position.y + TeleportDistance);
            }

            if (wallSide == WallSide.Top)
            {
                _destination = new Vector3(collider.transform.position.x, collider.transform.position.y - TeleportDistance);
            }

            if (wallSide == WallSide.Left)
            {
                _destination = new Vector3(collider.transform.position.x + TeleportDistance, collider.transform.position.y);
            }

            if (wallSide == WallSide.Right)
            {
                _destination = new Vector3(collider.transform.position.x - TeleportDistance, collider.transform.position.y);
            }

            base.OnTriggerEnter2D(collider);
        }
            

        /// <summary>
        /// Teleports whatever enters the portal to a new destination
        /// </summary>
        protected override void Teleport(Collider2D collider)
        {
           

            // if the teleporter has a destination, we move the colliding object to that destination
            if (_destination != null)
            {
                collider.transform.position = _destination;

                // we trigger splashs at both portals locations
                Splash();

                StartCoroutine(TeleportEnd());
            }
        }


    }
}