using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class SFTUpgrade : PickableItem
    {

        protected Animator _animator;
        protected LucyFlamethrower _lucyFlamethrower;
        protected SpriteRenderer _sprite;
        protected LucyInventory _characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _animator = _collider.GetComponent<Animator>();
            _lucyFlamethrower = _collider.GetComponent<LucyFlamethrower>();
             _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = _collider.GetComponent<LucyInventory>();

            _lucyFlamethrower.SuperFlamethrowerObtained = true;
            _animator.runtimeAnimatorController = Resources.Load("SFTAnimator") as RuntimeAnimatorController;
            _sprite.enabled = false;
            //if we haven't already obtained it
            if (!GameManager.Instance.SuperFlamethrowerObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.SuperFlamethrowerObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("SuperFlamethrower");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}