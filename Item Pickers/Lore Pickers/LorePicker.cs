using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class LorePicker : PickableItem
    {
        public int LoreNumber;
        protected SpriteRenderer _sprite;
        protected LucyInventory characterInventory;

        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            base.Pick();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            characterInventory = _collider.GetComponent<LucyInventory>();
            _characterPause = _collider.GetComponent<CharacterPause>();

            _sprite.enabled = false;

            CheckLoreNumber(LoreNumber); 
            
        }

        protected override void RunSplashScreen()
        {
            if(!GameManager.Instance.DebugMode)
            {
                base.RunSplashScreen();
                GUIManager.Instance.StartLoreCollected(LoreNumber);
                //tell the character pause class that the button input will remove the pop up
                _characterPause.StartPopUpClearingCoroutine();
            }
        }

        protected void CheckLoreNumber(int loreNumber)
        {
            switch (loreNumber)
            {
                case 1:
                    if (!GameManager.Instance.LoreOneObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreOneObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 2:
                    if (!GameManager.Instance.LoreTwoObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreTwoObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 3:
                    if (!GameManager.Instance.LoreThreeObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreThreeObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 4:
                    if (!GameManager.Instance.LoreFourObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreFourObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 5:
                    if (!GameManager.Instance.LoreFiveObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreFiveObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 6:
                    if (!GameManager.Instance.LoreSixObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreSixObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 7:
                    if (!GameManager.Instance.LoreSevenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreSevenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 8:
                    if (!GameManager.Instance.LoreEightObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreEightObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 9:
                    if (!GameManager.Instance.LoreNineObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreNineObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 10:
                    if (!GameManager.Instance.LoreTenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreTenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 11:
                    if (!GameManager.Instance.LoreElevenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreElevenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 12:
                    if (!GameManager.Instance.LoreTwelveObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreTwelveObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 13:
                    if (!GameManager.Instance.LoreThirteenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.LoreThirteenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                default:
                    break;
            }
        }


    }


}