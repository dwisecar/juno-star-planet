using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{

    public class BossFiveTeleporter : DoorwayTeleport
    {
        public GameObject BossFivePrefab;
        public Transform BossLocation;

        protected bool _hasFacedTheBossAlready;
        protected Transform _existingBoss;

        protected override void Teleport(Collider2D collider)
        {
            DestroyExistingBoss();
            base.Teleport(collider);

           
        }


        protected void DestroyExistingBoss()
        {
            _existingBoss = BossLocation.GetChild(0);

            if (_existingBoss != null)
            {
                Destroy(_existingBoss.gameObject);
            }

           Instantiate(BossFivePrefab, BossLocation);
        }

    }
}