using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class Cilia : MonoBehaviour
    {

        protected Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine(WiggleCilia());
        }

        protected IEnumerator WiggleCilia()
        {
            anim.SetBool("Wiggle", true);
            yield return new WaitForSeconds(.5f);
            anim.SetBool("Wiggle", false);

        }
    }
}