using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.MMInterface;

namespace MoreMountains.CorgiEngine
{
    public class SaveStation : MonoBehaviour
    {

        protected Character _player;
        protected CharacterPause _characterPause;
        protected CorgiController _controller;
        protected List<Transform> _ignoreList;
        protected Animator stationAnimator;
        protected Animator playerAnimator;
        protected BoxCollider2D _box2D;
        protected LucyHealth _lucyHealth;

        public float TimeWithoutControl = 1f;
        public ParticleSystem SaveParticleEmitter;
        public GameObject SaveGameTextBox;
        public AudioClip SaveGamesfx;

        // Start is called before the first frame update
        void Start()
        {
            _ignoreList = new List<Transform>();
            stationAnimator = GetComponent<Animator>();

            _box2D = GetComponent<BoxCollider2D>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            // if the object that collides with the teleporter is on its ignore list, we do nothing and exit.
            if (_ignoreList.Contains(collider.transform))
            {
                return;
            }

            if (collider.GetComponent<Character>() != null)
            {
                _player = collider.GetComponent<Character>();
                playerAnimator = collider.GetComponent<Animator>();
                _characterPause = collider.GetComponent<CharacterPause>();
                _controller = collider.GetComponent<CorgiController>();
                _lucyHealth = collider.GetComponent<LucyHealth>();

            }

            if(_player!=null)
            {
                if (_player.CharacterType == Character.CharacterTypes.Player)
                {

                    if(_controller.State.IsGrounded)
                    {
                        StartCoroutine(SaveGameEffect());
                        _lucyHealth.ResetHealthToMaxHealth();
                    }
                }

                AddToIgnoreList(collider.transform);
            }

        }

        protected virtual IEnumerator SaveGameEffect()
        {

            playerAnimator.SetBool("ThumbsUp", true);
            _player.Freeze();
            _player.Disable();
            stationAnimator.SetBool("SaveGameEffect", true);


            if (SaveParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = SaveParticleEmitter.emission;
                emissionModule.enabled = true;
            }

            PlaySaveGameSfx();

            yield return new WaitForSeconds(TimeWithoutControl);
            _player.EnableCharacterControl();
            _player.UnFreeze();

            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("ThumbsUp", false);

            if (SaveParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = SaveParticleEmitter.emission;
                emissionModule.enabled = false;
            }

            StartCoroutine(SaveGameText());
        }

        protected virtual IEnumerator SaveGameText()
        {
            SaveGameTextBox.SetActive(true);
           
            yield return new WaitForSeconds(3f);
            SaveGameTextBox.SetActive(false);
        }

        /// <summary>
        /// Adds an object to the ignore list, which will prevent that object to be moved by the teleporter while it's in that list
        /// </summary>
        /// <param name="objectToIgnore">Object to ignore.</param>
        public virtual void AddToIgnoreList(Transform objectToIgnore)
        {
            _ignoreList.Add(objectToIgnore);
        }

        /// <summary>
        /// When something exits the teleporter, if it's on the ignore list, we remove it from it, so it'll be considered next time it enters.
        /// </summary>
        /// <param name="collider">Collider.</param>
        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            if (_ignoreList.Contains(collider.transform))
            {
                _ignoreList.Remove(collider.transform);
            }
            stationAnimator.SetBool("SaveGameEffect", false);

        }

        protected virtual void PlaySaveGameSfx()
        {
            if (SaveGamesfx != null) { SoundManager.Instance.PlaySound(SaveGamesfx, transform.position); }
        }

    }
}