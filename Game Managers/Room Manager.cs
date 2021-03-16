using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using MoreMountains.MMInterface;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MoreMountains.CorgiEngine
{
    public class RoomManager : MonoBehaviour
    {

        protected ProCamera2DRooms _rooms;
        public GameObject MapSquare0;

        [Space(10)]         [Header("Area Boss Objects")]
        public BossHealth BossHealth;
        public GameObject BossRoomToTurnOffOnPlayerDeath;
        public GameObject BossDoorToOpen; //kinda just for area 4 boss

        [Header("Point Of Entry Rooms")]
        public GameObject Room0;
        public GameObject Room1;
        public GameObject Room2;
        public GameObject Room3;
        public GameObject Room4;
        public GameObject Room5;
        public GameObject Room6;
        public GameObject Room7;
        public GameObject Room8;
        public GameObject Room9;
        public GameObject Room10;
        public GameObject Room11;
        public GameObject Room12;
        public GameObject Room13;
        public GameObject Room14;
        public GameObject Room15;
        public GameObject Room16;
        public GameObject Room17;
        public GameObject Room18;
        public GameObject Room19;
        public GameObject Room20;
        public GameObject Room21;
        public GameObject Room22;
        public GameObject Room23;
        public GameObject Room24;
        public GameObject Room25;
        public GameObject Room26;
        public GameObject Room27;
        public GameObject Room28;
        public GameObject Room29;
        public GameObject Room30;
        public GameObject Room31;
        public GameObject Room32;


        public virtual void Start()
        {
            _rooms = FindObjectOfType<ProCamera2DRooms>();

            TurnRoomsOff();
            CheckForRoom();

        }

        //Check if player spawned in initial room, if not, turn it off
        protected virtual void CheckForRoom()
        {
            if(_rooms.TriggerTarget!=null)
            {
                Vector3 player = _rooms.TriggerTarget.position;
                var roomPlayerIsIn = _rooms.ComputeCurrentRoom(player);

                if (roomPlayerIsIn != 0)
                {
                    Room0.SetActive(false);
                }
            }           
        }

        //called from teleporters, add newly entered rooms to map
        public virtual void AddRoomToMap()
        {
            Vector3 player = _rooms.TriggerTarget.position;
            var roomPlayerIsIn = _rooms.ComputeCurrentRoom(player);

            if (roomPlayerIsIn == 0)
            {
                if (MapSquare0 != null)
                {
                    MapSquare0.SetActive(true);
                }
            }
        }

        public virtual void TurnRoomsOff()
        {
            //meant to be overidden
        }
    }


}