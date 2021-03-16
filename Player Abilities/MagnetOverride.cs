using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MoreMountains.CorgiEngine
{
    public class MagnetOverride : MonoBehaviour
    {

        protected LucyMagnet _lucyMagnet;
        // Use this for initialization
        void Start()
        {
            _lucyMagnet = GetComponentInParent<LucyMagnet>();
        }

        public void OnTriggerStay2D(Collider2D _collision)
        {
            if (_collision.gameObject.tag == "MagneticObject")
            {
                Rigidbody2D rb2d = _collision.gameObject.GetComponent<Rigidbody2D>();

                if (_lucyMagnet._magnet.enabled && _lucyMagnet.AttractingMode == true)
                {
                    rb2d.velocity = new Vector2(0, 0);
                    rb2d.isKinematic = true;

                }
                if (_lucyMagnet.AttractingMode == false)
                {
                    rb2d.isKinematic = false;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D _collision)
        {
            if (_collision.gameObject.tag == "MagneticObject")
            {
                Rigidbody2D rb2d = _collision.gameObject.GetComponent<Rigidbody2D>();
                rb2d.isKinematic = false;

            }
        }
    }
}
