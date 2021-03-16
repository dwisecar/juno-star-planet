using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class PlasmaBladeKnockBacker : MonoBehaviour
    {
        public CorgiController CorgiController;
        public Character characterPlayer;
        public Vector2 PlayerKnockbackForce;
        public GameObject BladeClashEffect;
        public AudioClip ClashEffect;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((collision.gameObject.layer == 13) || (collision.gameObject.layer == 28))
            {
                if(characterPlayer.IsFacingRight)
                {
                    CorgiController.SetForce(PlayerKnockbackForce);
                }
                else
                {
                    CorgiController.SetForce(-PlayerKnockbackForce);
                }

                Instantiate(BladeClashEffect, collision.transform.position, collision.transform.rotation);
                SoundManager.Instance.PlaySound(ClashEffect, collision.transform.position);
            }
        }
    }

    
}