using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class TripleNodeBarrier : MonoBehaviour
    {
        public float FadeOutTime = 1f;
        public GameObject NodeAObject;
        public GameObject NodeBObject;
        public GameObject NodeCObject;
        protected TripleSwitchNode nodeA;
        protected TripleSwitchNode nodeB;
        protected TripleSwitchNode nodeC;
        protected BoxCollider2D boxCollider2d;


        protected SpriteRenderer _barrierSprite;

        // Use this for initialization
        void Start()
        {
            nodeA = NodeAObject.GetComponent<TripleSwitchNode>();
            nodeB = NodeBObject.GetComponent<TripleSwitchNode>();
            nodeC = NodeCObject.GetComponent<TripleSwitchNode>();
            _barrierSprite = GetComponent<SpriteRenderer>();
            boxCollider2d = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckNodes();
        }

        protected virtual void CheckNodes()
        {
            if (nodeA._nodeHit && nodeB._nodeHit && nodeC._nodeHit)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            StartCoroutine(MMFade.FadeSprite(this.GetComponent<SpriteRenderer>(), FadeOutTime, new Color(1f, 1f, 1f, 0f)));
            //StartCoroutine(MMFade.FadeSprite(nodeA.GetComponent<SpriteRenderer>(), FadeOutTime, new Color(1f, 1f, 1f, 0f)));
            //StartCoroutine(MMFade.FadeSprite(nodeB.GetComponent<SpriteRenderer>(), FadeOutTime, new Color(1f, 1f, 1f, 0f)));
            //StartCoroutine(MMFade.FadeSprite(nodeC.GetComponent<SpriteRenderer>(), FadeOutTime, new Color(1f, 1f, 1f, 0f)));
            boxCollider2d.enabled = false;

            nodeA.DoorOpened = true;
            nodeB.DoorOpened = true;
            nodeC.DoorOpened = true;
        }
    }
}