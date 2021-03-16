using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class VineEye : MonoBehaviour
    {
        protected Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponentInParent<Animator>();

        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                anim.SetBool("EyeOpen", true);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                anim.SetBool("EyeOpen", false);
            }
        }
    }
}