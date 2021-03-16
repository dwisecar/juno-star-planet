using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class VacuumBootsUpgrade : PickableItem {

        protected LucyWallJump _walljump;
        protected LucyWallClinging _wallcling;
        protected SpriteRenderer _sprite;
        protected CharacterInventory characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _walljump = _collider.GetComponent<LucyWallJump> ();
            _wallcling = _collider.GetComponent<LucyWallClinging> ();
            _sprite = gameObject.GetComponent<SpriteRenderer> ();
            characterInventory = _collider.GetComponent<CharacterInventory>();
            _characterPause = _collider.GetComponent<CharacterPause>();


            _wallcling.AbilityPermitted = true;
            _walljump.AbilityPermitted = true;
            _sprite.enabled = false;

            //if we haven't already obtained it
            if (!GameManager.Instance.WallJumpBootsObtained)
            {
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.WallJumpBootsObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();
            
            GUIManager.Instance.StartItemCollected("WallJumpBoots");

            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}