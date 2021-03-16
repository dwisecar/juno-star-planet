using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Character/Abilities/Lucy Inventory")]
    public class LucyInventory : CharacterInventory, MMEventListener<MMInventoryEvent>
    {
        public string PowerNodeInventoryName;         public string SuitInventoryName;         public string SuitUpgradeInventoryName;         public string BlasterUpgradeInventoryName;         public string ArtifactInventoryName;         public string MeleeInventoryName;         public string LoreInventoryName;

        public Inventory PowerNodeInventory { get; set; }         public Inventory SuitInventory { get; set; }         public Inventory SuitUpgradeInventory { get; set; }         public Inventory BlasterUpgradeInventory { get; set; }         public Inventory ArtifactInventory { get; set; }         public Inventory MeleeInventory { get; set; }         public Inventory LoreInventory { get; set; }

        public GameObject hyperBeamPicker;

        protected CharacterHandleEMP _characterHandleEMP;         protected LucyHealth _lucyHealth;         protected LucyMagnet _magnetEffect;         protected CharacterHandleMelee _characterHandleMelee;         protected LucyWallJump _walljump;         protected LucyWallClinging _wallcling;         protected LucyFlamethrower _lucyFlamethrower;
        protected CharacterRun _characterRun;
        protected _2dxFX_PlasmaRainbow _plasmaRainbow;

        public int PowerNodesCollected = 0;

        public bool ArtifactAObtained = false;         public bool ArtifactBObtained = false;         public bool ArtifactCObtained = false;         public bool ArtifactDObtained = false;         public bool ArtifactEObtained = false;

        [Header("Item Pickers To Drop On Load")] 
        public GameObject BlasterPicker;
        public GameObject BlasterPlusPicker;
        public GameObject ReflectOrbPicker;
        public GameObject ReflectOrbPlusPicker;
        public GameObject RemoteMinePicker;
        public GameObject RemoteMinePlus;
        public GameObject HomingBeam;
        public GameObject HomingBeamPlus;
        public GameObject MuonBeam;
        public GameObject MuonBeamPlus;
        public GameObject HyperBeam;
        public GameObject WallJumpBoots;
        public GameObject MaglockBoots;
        public GameObject SpeedBoots;
        public GameObject MagnetSuit;
        public GameObject UltraSuit;
        public GameObject PlasmaBlade;
        public GameObject SuperPlasmaBlade;
        public GameObject Flamethrower;
        public GameObject SuperFlamethrower;
        public GameObject EMPDetonator;
        public GameObject KnockbackReducer;
        public GameObject RechargeBooster;
        public GameObject ArtifactA;
        public GameObject ArtifactB;
        public GameObject ArtifactC;
        public GameObject ArtifactD;
        public GameObject ArtifactE;
        public GameObject PowerNode1;
        public GameObject PowerNode2;
        public GameObject PowerNode3;
        public GameObject PowerNode4;
        public GameObject PowerNode5;
        public GameObject PowerNode6;
        public GameObject PowerNode7;
        public GameObject PowerNode8;
        public GameObject PowerNode9;
        public GameObject PowerNode10;
        public GameObject Lore1;
        public GameObject Lore2;
        public GameObject Lore3;
        public GameObject Lore4;
        public GameObject Lore5;
        public GameObject Lore6;
        public GameObject Lore7;
        public GameObject Lore8;
        public GameObject Lore9;
        public GameObject Lore10;
        public GameObject Lore11;
        public GameObject Lore12;
        public GameObject Lore13;

        protected override void Setup()
        {
            base.Setup();
            _animator = GetComponent<Animator>();             _characterHandleEMP = GetComponent<CharacterHandleEMP>();             _lucyHealth = GetComponent<LucyHealth>();             _magnetEffect = GetComponent<LucyMagnet>();             _characterHandleMelee = GetComponent<CharacterHandleMelee>();             _walljump = GetComponent<LucyWallJump>();             _wallcling = GetComponent<LucyWallClinging>();             _lucyFlamethrower = GetComponent<LucyFlamethrower>();
            _characterRun = GetComponent<CharacterRun>();
            _plasmaRainbow = GetComponent<_2dxFX_PlasmaRainbow>();

            PowerNodesCollected = 0;

            StartCoroutine(AddItemsToInventory());

            StartCoroutine(GetTheRightSuitOn());
        }

        protected override void GrabInventories()
        {
            base.GrabInventories();
            if (PowerNodeInventory == null)             {                 GameObject powerNodeInventoryTmp = GameObject.Find(PowerNodeInventoryName);                 if (powerNodeInventoryTmp != null) { PowerNodeInventory = powerNodeInventoryTmp.GetComponent<Inventory>(); }             }             if (SuitInventory == null)             {                 GameObject suitInventoryTmp = GameObject.Find(SuitInventoryName);                 if (suitInventoryTmp != null) { SuitInventory = suitInventoryTmp.GetComponent<Inventory>(); }             }             if (SuitUpgradeInventory == null)             {                 GameObject suitUpgradeInventoryTmp = GameObject.Find(SuitUpgradeInventoryName);                 if (suitUpgradeInventoryTmp != null) { SuitUpgradeInventory = suitUpgradeInventoryTmp.GetComponent<Inventory>(); }             }             if (BlasterUpgradeInventory == null)             {                 GameObject blasterUpgradeInventoryTmp = GameObject.Find(BlasterUpgradeInventoryName);                 if (blasterUpgradeInventoryTmp != null) { BlasterUpgradeInventory = blasterUpgradeInventoryTmp.GetComponent<Inventory>(); }             }             if (ArtifactInventory == null)             {                 GameObject artifactInventoryTmp = GameObject.Find(ArtifactInventoryName);                 if (artifactInventoryTmp != null) { ArtifactInventory = artifactInventoryTmp.GetComponent<Inventory>(); }             }             if (MeleeInventory == null)             {                 GameObject meleeInventoryTmp = GameObject.Find(MeleeInventoryName);                 if (meleeInventoryTmp != null) { MeleeInventory = meleeInventoryTmp.GetComponent<Inventory>(); }             }             if (LoreInventory == null)             {                 GameObject loreInventoryTmp = GameObject.Find(LoreInventoryName);                 if (loreInventoryTmp != null) { LoreInventory = loreInventoryTmp.GetComponent<Inventory>(); }             }

            if (PowerNodeInventory != null) { PowerNodeInventory.SetOwner(this.gameObject); PowerNodeInventory.TargetTransform = this.transform; }             if (SuitInventory != null) { SuitInventory.SetOwner(this.gameObject); SuitInventory.TargetTransform = this.transform; }             if (SuitUpgradeInventory != null) { SuitUpgradeInventory.SetOwner(this.gameObject); SuitUpgradeInventory.TargetTransform = this.transform; }             if (BlasterUpgradeInventory != null) { BlasterUpgradeInventory.SetOwner(this.gameObject); BlasterUpgradeInventory.TargetTransform = this.transform; }             if (ArtifactInventory != null) { ArtifactInventory.SetOwner(this.gameObject); ArtifactInventory.TargetTransform = this.transform; }             if (MeleeInventory != null) { MeleeInventory.SetOwner(this.gameObject); MeleeInventory.TargetTransform = this.transform; }             if (LoreInventory != null) { LoreInventory.SetOwner(this.gameObject); LoreInventory.TargetTransform = this.transform; }
        }

        //called by the item picker to equip what you just picked up
        public virtual void EquipWeaponOnPickup(string weaponID)         {             for (int i = 0; i < MainInventory.Content.Length; i++)             {                 if (InventoryItem.IsNull(MainInventory.Content[i]))                 {                     continue;                 }                 if (MainInventory.Content[i].ItemID == weaponID)                 {                     MMEventManager.TriggerEvent(new MMInventoryEvent(MMInventoryEventType.EquipRequest, null, MainInventory.name, MainInventory.Content[i], 0, i));                 }             }         }

        //called by the item picker to destroy the old beam when you pick up the upgraded version
        public virtual void DestroyWeaponOnPickup(string weaponID)
        {
            for (int i = 0; i < MainInventory.Content.Length; i++)
            {
                if (InventoryItem.IsNull(MainInventory.Content[i]))
                {
                    continue;
                }
                if (MainInventory.Content[i].ItemID == weaponID)
                {
                    MMEventManager.TriggerEvent(new MMInventoryEvent(MMInventoryEventType.Destroy, null, MainInventory.name, MainInventory.Content[i], 0, i));
                }

            }
        }

        //Run at the start of level to see what abilities have been unlocked and basically instansiate the collectable in
        //the players pocket as they load into the level.
        //All a way around using buggy inventory system for data persistance.
        public virtual IEnumerator AddItemsToInventory()
        {
            if (GameManager.Instance.BlasterObtained) { Instantiate(BlasterPicker, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ReflectOrbObtained && !GameManager.Instance.ReflectOrbPlusObtained) { Instantiate(ReflectOrbPicker, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.RemoteMinesObtained && !GameManager.Instance.RemoteMinePlusObtained) { Instantiate(RemoteMinePicker, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.HomingBeamObtained && !GameManager.Instance.HomingBeamPlusObtained) { Instantiate(HomingBeam, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ParticleAcceleratorObtained && !GameManager.Instance.ParticleAcceleratorPlusObtained) { Instantiate(MuonBeam, transform.position, transform.rotation); }

            yield return new WaitForEndOfFrame();
            _plasmaRainbow.enabled = false;
            if (GameManager.Instance.ReflectOrbPlusObtained) { Instantiate(ReflectOrbPlusPicker, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.RemoteMinePlusObtained) { Instantiate(RemoteMinePlus, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.HomingBeamPlusObtained) { Instantiate(HomingBeamPlus, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ParticleAcceleratorPlusObtained) { Instantiate(MuonBeamPlus, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.HyperBeamObtained) { Instantiate(HyperBeam, transform.position, transform.rotation); }

            yield return new WaitForEndOfFrame();

            if (GameManager.Instance.BlasterPlusObtained)
            {
                Instantiate(BlasterPlusPicker, transform.position, transform.rotation);

            }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.WallJumpBootsObtained) { Instantiate(WallJumpBoots, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.MaglockBootsObtained) { Instantiate(MaglockBoots, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.SpeedBootsObtained) { Instantiate(SpeedBoots, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.PlasmaBladeObtained) { Instantiate(PlasmaBlade, transform.position, transform.rotation); }

            yield return new WaitForEndOfFrame();

            if (GameManager.Instance.MagnetSuitObtained) { Instantiate(MagnetSuit, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.FlamethrowerObtained) { Instantiate(Flamethrower, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.SuperPlasmaBladeObtained) { Instantiate(SuperPlasmaBlade, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.RechargeBoosterObtained) { Instantiate(RechargeBooster, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.KnockbackReducerObtained) { Instantiate(KnockbackReducer, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.EMPObtained) { Instantiate(EMPDetonator, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ArtifactAObtained) { Instantiate(ArtifactA, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ArtifactBObtained) { Instantiate(ArtifactB, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ArtifactCObtained) { Instantiate(ArtifactC, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ArtifactDObtained) { Instantiate(ArtifactD, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.ArtifactEObtained) { Instantiate(ArtifactE, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            

            if (GameManager.Instance.LoreOneObtained) { Instantiate(Lore1, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreTwoObtained) { Instantiate(Lore2, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreThreeObtained) { Instantiate(Lore3, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreFourObtained) { Instantiate(Lore4, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreFiveObtained) { Instantiate(Lore5, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreSixObtained) { Instantiate(Lore6, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreSevenObtained) { Instantiate(Lore7, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.UltraSuitObtained) { Instantiate(UltraSuit, transform.position, transform.rotation); ; }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreEightObtained) { Instantiate(Lore8, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreNineObtained) { Instantiate(Lore9, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreTenObtained) { Instantiate(Lore10, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreElevenObtained) { Instantiate(Lore11, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreTwelveObtained) { Instantiate(Lore12, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.LoreThirteenObtained) { Instantiate(Lore13, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();


            if (GameManager.Instance.PowerNodeOneObtained) { Instantiate(PowerNode1, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeTwoObtained) { Instantiate(PowerNode2, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeThreeObtained) { Instantiate(PowerNode3, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeFourObtained) { Instantiate(PowerNode4, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeFiveObtained) { Instantiate(PowerNode5, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeSixObtained) { Instantiate(PowerNode6, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeSevenObtained) { Instantiate(PowerNode7, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeEightObtained) { Instantiate(PowerNode8, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeNineObtained) { Instantiate(PowerNode9, transform.position, transform.rotation); }
            if (GameManager.Instance.PowerNodeTenObtained) { Instantiate(PowerNode10, transform.position, transform.rotation); }
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.SuperFlamethrowerObtained) { Instantiate(SuperFlamethrower, transform.position, transform.rotation); }

            UpdateGUIPowerNodes();

            LoadPreviouslyEquippedWeapon();
            RetroAdventureProgressManager.Instance.SaveProgress();
        }

        public virtual void CheckArtifacts()         {             if (ArtifactAObtained && ArtifactBObtained && ArtifactCObtained && ArtifactDObtained && ArtifactEObtained)             {                 if(!GameManager.Instance.HyperBeamObtained)
                {                     SpawnHyperBeam();                 }             }         }          protected virtual void SpawnHyperBeam()         {             Instantiate(hyperBeamPicker, transform.position, transform.rotation);         }          public virtual void RemoveUnarmed()         {             for (int i = 0; i < MainInventory.Content.Length; i++)             {                 if (InventoryItem.IsNull(MainInventory.Content[i]))                 {                     continue;                 }                 if (MainInventory.Content[i].ItemID == "InventoryUnarmed")                 {                     MainInventory.DestroyItem(i);                 }             }         }

        public void UpdateGUIPowerNodes() //passes the value of the power node array length into the GUI manager
        {
            GUIManager.Instance.UpdatePowerNodeCounter(PowerNodesCollected);
        }

        protected override void EquipWeapon(string weaponID)
        {
            base.EquipWeapon(weaponID);

            //setting persistant variable for weapon that was equipped while leaving a level.
            GameManager.Instance.CurrentlyEquippedWeapon = weaponID;
        }

        //On load this will equip the wepaon that the player last had equipped when leaving an area.
        public void LoadPreviouslyEquippedWeapon()
        {
            string _weaponToEquip = GameManager.Instance.CurrentlyEquippedWeapon;
            EquipWeapon(_weaponToEquip);
        }

        public void StartItemCollectedEffect()
        {
            StartCoroutine(PlayItemCollectionPlayerEffect());
        }

        protected IEnumerator PlayItemCollectionPlayerEffect()
        {
            _2dxFX_Shiny_Reflect _shinyReflect = GetComponent<_2dxFX_Shiny_Reflect>();
            _shinyReflect.enabled = true;
            yield return new WaitForSeconds(3f);
            _shinyReflect.enabled = false;
        }

        protected IEnumerator GetTheRightSuitOn()
        {
            yield return new WaitForSeconds(1);
            if(GameManager.Instance.BlasterObtained && !GameManager.Instance.MagnetSuitObtained)
            {
                _animator.runtimeAnimatorController = Resources.Load("OrangeSuitAnimator") as RuntimeAnimatorController;
            }
            if(GameManager.Instance.MagnetSuitObtained && !GameManager.Instance.EMPObtained)
            {
                _animator.runtimeAnimatorController = Resources.Load("SuperSuitAnimator") as RuntimeAnimatorController;
            }
            if (GameManager.Instance.EMPObtained && !GameManager.Instance.UltraSuitObtained)
            {
                _animator.runtimeAnimatorController = Resources.Load("SuperSuitSuperBlasterAnimator") as RuntimeAnimatorController;
            }
            if (GameManager.Instance.UltraSuitObtained && !GameManager.Instance.SuperFlamethrowerObtained)
            {
                _animator.runtimeAnimatorController = Resources.Load("UltraSuitAnimator") as RuntimeAnimatorController;
            }
            if (GameManager.Instance.SuperFlamethrowerObtained)
            {
                _animator.runtimeAnimatorController = Resources.Load("SFTAnimator") as RuntimeAnimatorController;
            }
        }

    }
}
