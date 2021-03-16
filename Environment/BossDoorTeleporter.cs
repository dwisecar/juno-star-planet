using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.MMInterface;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/DoorwayTeleport")]
    /// <summary>
    /// Add this script to a trigger collider2D to teleport objects from that object to its destination
    /// </summary>

    public class BossDoorTeleport : Teleporter
    {
        public float TimeWithoutControl = 1f;
        protected Character _character;
        protected CorgiController _controller;

        public bool TopSideTeleporter = false;
        public bool BottomSideTeleporter = false;
        protected bool StandingOnDoorTop = false;

        protected Vector3 cameraLocation;

        protected LucyHorizontalMovement lucyHorizontalMovement;

        [Header("Enemies to Respawn")]
        public AutoRespawn[] autoRepawn;

        [Header("Rooms to Activate and Deactivate")]
        public GameObject RoomToTurnOn;
        public GameObject RoomToTurnOff;
        public GameObject SecondaryRoomToTurnOn;
        public GameObject SecondaryRoomToTurnOff;

        [Header("Bosses to Activate and Deactivate")]
        public GameObject PreBoss;
        protected BossOpeningCinematic bossOpeningCinematic;

        protected override void Start()
        {
            base.Start();
            bossOpeningCinematic = PreBoss.GetComponent<BossOpeningCinematic>();
        }

        /// <summary>
        /// Triggered when something enters the teleporter
        /// </summary>
        /// <param name="collider">Collider.</param>
        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            // if the object that collides with the teleporter is on its ignore list, we do nothing and exit.
            if (_ignoreList.Contains(collider.transform))
            {
                return;
            }

            //if the player is on the top side, set this to true
            StandingOnDoorTop |= TopSideTeleporter;

            if (collider.GetComponent<Character>() != null)
            {
                _player = collider.GetComponent<Character>();
                _controller = collider.GetComponent<CorgiController>();
                lucyHorizontalMovement = collider.GetComponent<LucyHorizontalMovement>();

            }

            if (_player != null)
            {
                //Set active the room we are about to enter 
                if (_player.CharacterType == Character.CharacterTypes.Player)
                {
                    if (RoomToTurnOn != null)
                    {
                        RoomToTurnOn.SetActive(true);
                    }
                    if (SecondaryRoomToTurnOn != null)
                    {
                        SecondaryRoomToTurnOn.SetActive(true);
                    }
                }
            }


            // if the teleporter is supposed to only affect the player (well, corgiControllers), we do nothing and exit
            if (OnlyAffectsPlayer || !AutoActivation)
            {
                base.OnTriggerEnter2D(collider);
            }
            else
            {
                if ((TopSideTeleporter == false) && (BottomSideTeleporter == false))
                {
                    Teleport(collider);
                }
            }
        }

        /// <summary>
        /// Teleports whatever enters the portal to a new destination
        /// </summary>
        protected override void Teleport(Collider2D collider)
        {
            // if the teleporter has a destination, we move the colliding object to that destination
            if (Destination != null)
            {
                LevelManager.Instance.FadeIn();

                collider.transform.position = Destination.transform.position;
                _ignoreList.Remove(collider.transform);
                Destination.AddToIgnoreList(collider.transform);

                // we trigger splashs at both portals locations
                // Splash();
                // Destination.Splash();

                _character = collider.GetComponent<Character>();

                _character.Freeze();

                StartCoroutine(TeleportEnd());

                //Deactivate the room we just left
                if (RoomToTurnOff != null)
                {
                    RoomToTurnOff.SetActive(false);
                }
                if (SecondaryRoomToTurnOff != null)
                {
                    SecondaryRoomToTurnOff.SetActive(false);
                }

                //respawn enemies a few rooms away
                RespawnEnemies();
            }
        }

        protected override IEnumerator TeleportEnd()
        {
            if (FadeToBlack)
            {
                if (TeleportCamera)
                {
                    LevelManager.Instance.LevelCameraController.FollowsPlayer = false;
                }
                MMEventManager.TriggerEvent(new MMFadeInEvent(FadeDuration));
                yield return new WaitForSeconds(FadeDuration);
                if (TeleportCamera)
                {
                    LevelManager.Instance.LevelCameraController.TeleportCameraToTarget();
                    LevelManager.Instance.LevelCameraController.FollowsPlayer = true;

                }
                MMEventManager.TriggerEvent(new MMFadeOutEvent(FadeDuration));
            }
            else
            {
                if (TeleportCamera)
                {
                    LevelManager.Instance.LevelCameraController.TeleportCameraToTarget();
                }
            }

            yield return new WaitForSeconds(TimeWithoutControl);
            _character.UnFreeze();

            //mutes the 'touch the ground' sfx for a second
            lucyHorizontalMovement.JustTeleported();

            //Moving the parallax root position to the new room
            cameraLocation = Camera.main.transform.position;

            var ParallaxClass = FindObjectOfType<ProCamera2DParallax>();
            if (ParallaxClass != null)
            {
                ParallaxClass.RootPosition = cameraLocation;
            }

            //fade back after the player has hit the ground 
            StartCoroutine(HitTheGroundThenFadeOut());

            //Start the boss room cinematic
            if(bossOpeningCinematic!=null)
            {
                StartCoroutine(bossOpeningCinematic.BossCinematic());
            }
            
        }

        /// <summary>
        /// When something exits the teleporter, if it's on the ignore list, we remove it from it, so it'll be considered next time it enters.
        /// </summary>
        /// <param name="collider">Collider.</param>
        protected override void OnTriggerExit2D(Collider2D collider)
        {
            if (_ignoreList.Contains(collider.transform))
            {
                _ignoreList.Remove(collider.transform);
            }
            base.OnTriggerExit2D(collider);
            StandingOnDoorTop = false;
        }

        //don't fade back until the player has hit the ground after teleporting. About .15 seconds.
        protected virtual IEnumerator HitTheGroundThenFadeOut()
        {
            yield return new WaitForSeconds(.15f);
            LevelManager.Instance.FadeOut();
        }

        protected void RespawnEnemies()
        {
            foreach (AutoRespawn enemy in autoRepawn)
            {
                if (enemy == null) { return; }
                enemy.Revive();

            }
        }
    }
}