using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class ParticlesOpenDoors : MonoBehaviour
    {
        protected ShotBarrierNode node;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnParticleCollision(GameObject gameObject)
        {
            if (gameObject.GetComponent<ShotBarrierNode>() != null)
            {
                node = gameObject.GetComponent<ShotBarrierNode>();
                node.OpenDoor();
            }
        }
    }
}
