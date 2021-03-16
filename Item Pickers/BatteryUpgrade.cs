using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

namespace MoreMountains.CorgiEngine
{
    public class BatteryUpgrade : PickableItem
    {
        public int PowerNodeNumber;
        protected LucyHealth _health;
        protected SpriteRenderer _sprite;
        protected LucyInventory _inventory;
     
        /// Override this to describe what happens when the object gets picked
        /// </summary>
        protected override void Pick()
        {
            _health = _collider.GetComponent<LucyHealth>();
            _inventory = _collider.GetComponent<LucyInventory>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
         
            //Starts the process of increasing heath count and size of health bar
            _health.BatteryObtained();
            _sprite.enabled = false;

            //Tell the inventory to update the GUI on power nodes collected
            _inventory.PowerNodesCollected++;
            _inventory.UpdateGUIPowerNodes();

            CheckPowerNodeNumber(PowerNodeNumber);
        }

        protected override void RunSplashScreen()
        {
            //base.RunSplashScreen();

        }

        protected void CheckPowerNodeNumber(int powerNode)
        {
            switch (powerNode)
            {
                case 1:
                    if (!GameManager.Instance.PowerNodeOneObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeOneObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 2:
                    if (!GameManager.Instance.PowerNodeTwoObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeTwoObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 3:
                    if (!GameManager.Instance.PowerNodeThreeObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeThreeObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 4:
                    if (!GameManager.Instance.PowerNodeFourObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeFourObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 5:
                    if (!GameManager.Instance.PowerNodeFiveObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeFiveObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 6:
                    if (!GameManager.Instance.PowerNodeSixObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeSixObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 7:
                    if (!GameManager.Instance.PowerNodeSevenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeSevenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 8:
                    if (!GameManager.Instance.PowerNodeEightObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeEightObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 9:
                    if (!GameManager.Instance.PowerNodeNineObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeNineObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                case 10:
                    if (!GameManager.Instance.PowerNodeTenObtained)
                    {
                        RunSplashScreen();
                        GameManager.Instance.PowerNodeTenObtained = true;
                        RetroAdventureProgressManager.Instance.SaveProgress();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}