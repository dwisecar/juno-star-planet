using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class UltraSuitUpgrade : PickableItem
    {

        protected Animator _animator;
        protected LucyHealth _health;
        protected SpriteRenderer _sprite;
        protected CharacterInventory _characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _animator = _collider.GetComponent<Animator>();
            _health = _collider.GetComponent<LucyHealth>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = _collider.GetComponent<CharacterInventory>();


            _health.UltraSuitAquired();
            _animator.runtimeAnimatorController = Resources.Load("UltraSuitAnimator") as RuntimeAnimatorController;
            _sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.UltraSuitObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.UltraSuitObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("UltraSuit");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}