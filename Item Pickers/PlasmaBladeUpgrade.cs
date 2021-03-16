using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
	public class PlasmaBladeUpgrade : PickableItem {

		protected CharacterHandleMelee _characterHandleMelee;
		protected SpriteRenderer _sprite;
        protected LucyInventory _characterInventory;

		/// Override this to describe what happens when the object gets picked
		/// </summary>
		protected override void Pick()
		{
            base.Pick();
            _characterHandleMelee = _collider.GetComponent<CharacterHandleMelee> ();
			_sprite = gameObject.GetComponent<SpriteRenderer> ();
            _characterInventory = _collider.GetComponent<LucyInventory>();

			_characterHandleMelee.AbilityPermitted = true;
			_sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.PlasmaBladeObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.PlasmaBladeObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("PlasmaBlade");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}