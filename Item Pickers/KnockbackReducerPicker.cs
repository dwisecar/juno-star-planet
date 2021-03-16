using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class KnockbackReducerPicker : PickableItem
    {
   
        protected SpriteRenderer _sprite;
        protected LucyInventory characterInventory;
        protected LucyHealth _lucyHealth;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            characterInventory = _collider.GetComponent<LucyInventory>();
            _lucyHealth = _collider.GetComponent<LucyHealth>();

            _lucyHealth.KnockbackReducerAcquired();
            _sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.KnockbackReducerObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.KnockbackReducerObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("KnockbackReducer");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}