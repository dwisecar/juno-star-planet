using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    //Attached to shield of boss 3
    public class ProjectileBlocker : MonoBehaviour
    {
        protected Health _projectileHealth;
        protected BaddieHealth _shieldHealth;
        protected MagneticProjectile _magProj;
        protected Animator anim;
        protected AutoRespawn _autoRespawn;
        protected float respawnTime;

        public LayerMask PlayerProjectileLayerMask;
        public LayerMask MagneticLayerMask;
        public GameObject LightningFx;

        private void Start()
        {
            _shieldHealth = GetComponent<BaddieHealth>();
            anim = GetComponent<Animator>();
            _autoRespawn = GetComponent<AutoRespawn>();

            respawnTime = _autoRespawn.AutoRespawnDuration;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            /*if(collision.gameObject.layer == PlayerProjectileLayerMask.value)
            {
                _projectileHealth = collision.GetComponent<Health>();
                _projectileHealth.Kill();
            }*/

            _magProj = collision.GetComponent<MagneticProjectile>();

            if(_magProj!=null)
            {
                if(_magProj.CanHurtEnemies)
                {
                    StartCoroutine(PlayLightningFx());
                    _shieldHealth.Kill();
                    _magProj.KillMagenticProjectile();
                }
            }
        }

        protected IEnumerator PlayLightningFx()
        {
            LightningFx.SetActive(true);
            yield return new WaitForSeconds(2f);
            LightningFx.SetActive(false);

        }

        protected IEnumerator PlayFadeAnimations()
        {
            anim.SetBool("Dead", true);
            yield return new WaitForSeconds(respawnTime);
            anim.SetBool("Dead", false);

        }
    }
}