using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class ItemPickerGUIEffect : MonoBehaviour
    {
        public bool LoreItem;
        public bool AbilityItem;

        public float FreezeTime = 8f;
        public Image Dimmer;
        public Image LoreTablet;

        protected Character _player;
        protected CharacterPause _characterPause;
        protected CorgiController _controller;


        // Start is called before the first frame update
        void Start()
        {

            Dimmer.CrossFadeAlpha(0, 0.1f, false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {

            if (collider.GetComponent<Character>() != null)
            {
                _player = collider.GetComponent<Character>();
                _characterPause = collider.GetComponent<CharacterPause>();
                _controller = collider.GetComponent<CorgiController>();
            }

            if (_player != null)
            {
                if (_player.CharacterType == Character.CharacterTypes.Player)
                {
                    FreezeGame();
                    Dimmer.CrossFadeAlpha(100, .5f, false);
                    LoreTablet.CrossFadeAlpha(100, .5f, false);

                }
            }
        }

        protected void FreezeGame()
        {
            MMFreezeFrameEvent.Trigger(FreezeTime);
        }
    }
}