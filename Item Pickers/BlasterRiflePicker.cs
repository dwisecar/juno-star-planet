using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class BlasterRiflePicker : PickableItem
    {

        protected Animator _animator;
        protected SpriteRenderer _sprite;
        protected LucyInventory _characterInventory;
        

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _animator = _collider.GetComponent<Animator>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = _collider.GetComponent<LucyInventory>();
            

            _animator.runtimeAnimatorController = Resources.Load("OrangeSuitAnimator") as RuntimeAnimatorController;
            _sprite.enabled = false;
            _characterInventory.EquipWeaponOnPickup("InventoryBlasterRifle");
            _characterInventory.RemoveUnarmed();

            //if we haven't gotten it before run the splash and effects.
            if(GameManager.Instance.BlasterObtained == false)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.BlasterObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

      protected override void RunSplashScreen()
      {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("BlasterRifle");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}