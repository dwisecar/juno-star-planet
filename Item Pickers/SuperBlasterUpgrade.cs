using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class SuperBlasterUpgrade : PickableItem {

        protected Animator _animator;
        protected SpriteRenderer _sprite;
        protected CharacterHandleEMP _characterHandleEMP;


        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _animator = _collider.GetComponent<Animator> ();
            _sprite = gameObject.GetComponent<SpriteRenderer> ();
            _characterHandleEMP = _collider.GetComponent<CharacterHandleEMP>();

           
            _animator.runtimeAnimatorController = Resources.Load ("SuperSuitSuperBlasterAnimator") as RuntimeAnimatorController;
            _sprite.enabled = false;
            _characterHandleEMP.AbilityPermitted = true;

            //if we haven't already obtained it
            if (!GameManager.Instance.EMPObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.EMPObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            Debug.Log("RunSplash being called");
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("EMP");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}