using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class HomingBeamPlusPicker : PickableItem
    {

        protected SpriteRenderer _sprite;
        protected LucyInventory _characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = _collider.GetComponent<LucyInventory>();


            _sprite.enabled = false;
            _characterInventory.EquipWeaponOnPickup("InventoryHomingBeamPlus");
            _characterInventory.DestroyWeaponOnPickup("InventoryHomingBeam");

            //if we haven't already obtained it
            if (!GameManager.Instance.HomingBeamPlusObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.HomingBeamPlusObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }

            
        }
        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("HomingBeamPlus");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}