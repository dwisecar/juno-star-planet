using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class ArtifactPickerC : PickableItem
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
            _characterInventory.ArtifactCObtained = true;
            _characterInventory.CheckArtifacts();


            //if we haven't already obtained it
            if (!GameManager.Instance.ArtifactCObtained)
            {
                GameManager.Instance.NumberOfArtifactsObtained += 1;
                if (!GameManager.Instance.DebugMode) { RunSplashScreen(); }
                GameManager.Instance.ArtifactCObtained = true;
                RetroAdventureProgressManager.Instance.SaveProgress();
            }
        }

        protected override void RunSplashScreen()
        {
            base.RunSplashScreen();

            if (GameManager.Instance.NumberOfArtifactsObtained < 5)
            {
                GUIManager.Instance.StartItemCollected("Artifact");
            }
            //tell the character pause class that the button input will remove the pop up
            _characterPause.StartPopUpClearingCoroutine();
        }
    }
}