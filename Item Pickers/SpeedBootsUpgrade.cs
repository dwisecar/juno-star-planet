using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class SpeedBootsUpgrade : PickableItem
    {
        protected CharacterRun characterRun;
        protected SpriteRenderer _sprite;
        protected CharacterInventory characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            characterRun = _collider.GetComponent<CharacterRun>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            characterInventory = _collider.GetComponent<CharacterInventory>();

            characterRun.AbilityPermitted = true;
            _sprite.enabled = false;
            //if we haven't already obtained it
            if (!GameManager.Instance.SpeedBootsObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.SpeedBootsObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("SpeedBoots");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}
