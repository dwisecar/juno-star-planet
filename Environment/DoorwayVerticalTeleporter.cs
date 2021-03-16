using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.MMInterface;
using Com.LuisPedroFonseca.ProCamera2D;


namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Exoplanet/Scripts/Environment/DoorwayVerticalTeleporter")]
    /// <summary>
    /// Add this script to a trigger collider2D to teleport objects from that object to its destination
    /// </summary>

    public class DoorwayVerticalTeleporter : MonoBehaviour
    {
        [Header("Teleporter")]
        /// the teleporter's destination
        public Transform Destination;
        /// if true, this won't teleport non player characters
        public bool OnlyAffectsPlayer = true;
        /// a gameobject to instantiate when teleporting
        public GameObject TeleportEffect;

        [Header("Teleporter Camera")]
        /// if this is true, the camera will teleport instantly to the teleporter's destination when activated
        public bool TeleportCamera = false;
        /// if this is true, a fade to black will occur when teleporting
        public bool FadeToBlack = false;
        /// the duration (in seconds) of the fade to black
        public float FadeDuration;

        [Header("Enemies to Respawn")]
        public AutoRespawn[] autoRepawn;

        [Header("Rooms to Activate and Deactivate")]
        public GameObject RoomToTurnOn;
        public GameObject RoomToTurnOff;
        public GameObject SecondaryRoomToTurnOn;
        public GameObject SecondaryRoomToTurnOff;

        protected Character _player;
        protected List<Transform> _ignoreList;
        protected LucyHorizontalMovement lucyHorizontalMovement;

        public float TimeWithoutControl = 1f;
       
        protected CorgiController _controller;

        public bool TopSideTeleporter = false;
        public bool BottomSideTeleporter = false;

        protected bool StandingOnTopSide = false;
        protected bool EnteringUnderside = false;

        protected Vector3 cameraLocation;

        protected Collider2D _thePlayerCollider = null;

        public bool EnteringBossRoom;
        public bool LeavingBossRoom;
        public bool EnteringChillRoom;
        public bool LeavingChillRoom;

        public bool EnteringBonusRoom1;
        public bool EnteringBonusRoom2;
        public bool EnteringBonusRoom3;

        /// <summary>
        /// On start we initialize our ignore list
        /// </summary>
        protected virtual void Start()
        {
            _ignoreList = new List<Transform>();
        }

        protected void Update()
        {
            CheckIfCanPassThrough(_thePlayerCollider);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
             // if the object that collides with the teleporter is on its ignore list, we do nothing and exit.
             if (_ignoreList.Contains(collider.transform))
             {
                 return;
             }

             if (collider.GetComponent<Character>() != null)
             {
                 _player = collider.GetComponent<Character>();
                 _controller = collider.GetComponent<CorgiController>();
                lucyHorizontalMovement = collider.GetComponent<LucyHorizontalMovement>();
            }


            //if this teleporter is on top of the doorway, set a bool to true and set the player collider to the value that has collided with this trigger.
            if (TopSideTeleporter == true)
             {
                if (collider.GetComponent<Character>() != null)
                {
                    StandingOnTopSide = true;
                    _thePlayerCollider = collider;
                }

             }
            if (BottomSideTeleporter == true)
            {
                if (collider.GetComponent<Character>() != null)
                {
                    EnteringUnderside = true;
                    _thePlayerCollider = collider;
                }
            }


             
         }
        //If the player is standing on the top and 
        protected virtual void CheckIfCanPassThrough (Collider2D collider)
        {
            if(collider != null)
            {
                if (StandingOnTopSide)
                {
                    if (InputManager.Instance.PrimaryMovement.y <= -.5f)
                    {
                        Teleport(collider);
                    }
                }
                if (EnteringUnderside)
                {
                    if(InputManager.Instance.PrimaryMovement.y >= .5f)
                    {
                        Teleport(collider);
                    }
                }
            }

        }

        /// <summary>
        /// Teleports whatever enters the portal to a new destination
        /// </summary>
        protected virtual void Teleport(Collider2D collider)
        {
            // if the teleporter has a destination, we move the colliding object to that destination
            if (Destination != null)
            {
                LevelManager.Instance.FadeIn();

                collider.transform.position = Destination.transform.position;
                _ignoreList.Remove(collider.transform);

                // we trigger splashs at both portals locations
                // Splash();
                // Destination.Splash();

                _player.Freeze();

                StartCoroutine(TeleportEnd());

                //respawn enemies a few rooms away
                //RespawnEnemies();


                if (EnteringBossRoom)
                {
                    BackgroundMusic.Instance.PlayBossMusic();
                }

                if (EnteringChillRoom)
                {
                    BackgroundMusic.Instance.PlayChillRoomMusic();
                }

                if (EnteringBonusRoom1)
                {
                    BackgroundMusic.Instance.PlayBonusRoom1Music();
                }
                if (EnteringBonusRoom2)
                {
                    BackgroundMusic.Instance.PlayBonusRoom2Music();
                }
                if (EnteringBonusRoom3)
                {
                    BackgroundMusic.Instance.PlayBonusRoom3Music();
                }

                if (LeavingBossRoom || LeavingChillRoom)
                {
                    BackgroundMusic.Instance.PlayBackgroundMusic(); 
                    
                }
            }
        }

        protected virtual IEnumerator TeleportEnd()
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
                TurnRoomsOffAndOnAsAppropriate();
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
            _player.UnFreeze();

            if (lucyHorizontalMovement != null)
            {
                lucyHorizontalMovement.JustTeleported();
            }
            //Moving the parallax root position to the new room
            cameraLocation = Camera.main.transform.position;

            var ParallaxClass = FindObjectOfType<ProCamera2DParallax>();
            if (ParallaxClass != null)
            {
                ParallaxClass.RootPosition = cameraLocation;
            }

            StartCoroutine(HitTheGroundThenFadeOut());
        }

        //don't fade back until the player has hit the ground after teleporting. About .15 seconds.
        protected virtual IEnumerator HitTheGroundThenFadeOut()
        {
            //Deactivate the room we just left
            if (RoomToTurnOff != null)
            {
                RoomToTurnOff.SetActive(false);
            }
            if (SecondaryRoomToTurnOff != null)
            {
                SecondaryRoomToTurnOff.SetActive(false);
            }
            if (RoomToTurnOn != null)
            {
                RoomToTurnOn.SetActive(true);
            }
            if (SecondaryRoomToTurnOn != null)
            {
                SecondaryRoomToTurnOn.SetActive(true);
            }
            yield return new WaitForSeconds(.15f);
            LevelManager.Instance.FadeOut();
        }

        protected void TurnRoomsOffAndOnAsAppropriate()
        {
            //Deactivate the room we just left
            if (RoomToTurnOff != null)
            {
                RoomToTurnOff.SetActive(false);
            }
            if (SecondaryRoomToTurnOff != null)
            {
                SecondaryRoomToTurnOff.SetActive(false);
            }
            if (RoomToTurnOn != null)
            {
                RoomToTurnOn.SetActive(true);
            }
            if (SecondaryRoomToTurnOn != null)
            {
                SecondaryRoomToTurnOn.SetActive(true);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            if (_ignoreList.Contains(collider.transform))
            {
                _ignoreList.Remove(collider.transform);
            }

            StandingOnTopSide = false;
            EnteringUnderside = false;
            _thePlayerCollider = null;

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