using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class FlamethrowerUpgrade : PickableItem
    {
        protected LucyFlamethrower _lucyFlamethrower;
        protected SpriteRenderer _sprite;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _lucyFlamethrower = _collider.GetComponent<LucyFlamethrower>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();

            _lucyFlamethrower.FlameThrowerObtained = true;
            _sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.FlamethrowerObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.FlamethrowerObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("Flamethrower");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}