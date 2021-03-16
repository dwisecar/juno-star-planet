using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class WallFlower : MonoBehaviour
    {
        public Transform WhereToFart;
        public float TimeBetweenFarts = 4;

        public GameObject PoisonGas;
        public AudioClip FartSfx;

        protected Animator anim;
        protected bool ableToFart;
        protected Character _player;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponentInParent<Animator>();
            ableToFart = true;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character>() != null)
            {
                _player = collision.GetComponent<Character>();
            }

            if (_player != null)
            {
                //Set active the room we are about to enter 
                if (_player.CharacterType == Character.CharacterTypes.Player)
                {
                    if (ableToFart)
                    {
                        StartCoroutine(Fart());
                    }
                }
            }

        }

        protected IEnumerator Fart()
        {
            Instantiate(PoisonGas, WhereToFart.position, transform.rotation);
            SoundManager.Instance.PlaySound(FartSfx, WhereToFart.position);

            anim.SetBool("Fart", true);
            ableToFart = false;
            yield return new WaitForSeconds(TimeBetweenFarts);
            ableToFart = true;
            anim.SetBool("Fart", false);

        }
    }
}