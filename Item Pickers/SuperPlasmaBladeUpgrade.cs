using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class SuperPlasmaBladeUpgrade : PickableItem
    {

        protected CharacterHandleMelee _characterHandleMelee;
        protected SpriteRenderer _sprite;
        protected CharacterInventory _characterInventory;


        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _characterHandleMelee = _collider.GetComponent<CharacterHandleMelee>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _characterInventory = _collider.GetComponent<CharacterInventory>();


            _characterHandleMelee.AbilityPermitted = true;
            _characterHandleMelee.ObtainedSuperPlasmaBlade();
            _sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.SuperPlasmaBladeObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.SuperPlasmaBladeObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            GUIManager.Instance.StartItemCollected("SuperPlasmaBlade");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}