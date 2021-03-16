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
    public class AreaThreeManager : RoomManager
    {


        public override void Start()
        {
            _rooms = FindObjectOfType<ProCamera2DRooms>();

            TurnRoomsOff();
            //CheckForRoom();

        }

        //Check if player spawned in initial room, if not, turn it off
        protected override void CheckForRoom()
        {
            Vector3 player = _rooms.TriggerTarget.position;
            var roomPlayerIsIn = _rooms.ComputeCurrentRoom(player);

            if (roomPlayerIsIn != 0)
            {
                Room0.SetActive(false);
            }
        }

        //called from teleporters, add newly entered rooms to map
        public override void AddRoomToMap()
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
        public override void TurnRoomsOff()
        {

            //Deactivates all rooms except the point of entry and save rooms.
            if (Room0 != null)
            {
                //Room0.SetActive(false);
            }
            if (Room1 != null)
            {
                Room1.SetActive(false);
            }
            if (Room2 != null)
            {
                Room2.SetActive(false);
            }
            if (Room3 != null)
            {
                Room3.SetActive(false);
            }
            if (Room4 != null)
            {
                Room4.SetActive(false);
            }
            if (Room5 != null)
            {
                Room5.SetActive(false);
            }
            if (Room6 != null)
            {
                Room6.SetActive(false);
            }
            if (Room7 != null)
            {
                Room7.SetActive(false);
            }
            if (Room8 != null)
            {
                //Room8.SetActive(false);
            }
            if (Room9 != null)
            {
                Room9.SetActive(false);
            }
            if (Room10 != null)
            {
                Room10.SetActive(false);
            }
            if (Room11 != null)
            {
                Room11.SetActive(false);
            }
            if (Room12 != null)
            {
                Room12.SetActive(false);
            }
            if (Room13 != null)
            {
                Room13.SetActive(false);
            }
            if (Room14 != null)
            {
                Room14.SetActive(false);
            }
            if (Room15 != null)
            {
                Room15.SetActive(false);
            }
            if (Room16 != null)
            {
                Room16.SetActive(false);
            }
            if (Room17 != null)
            {
                //Room17.SetActive(false);
            }
            if (Room18 != null)
            {
                Room18.SetActive(false);
            }
            if (Room19 != null)
            {
                Room19.SetActive(false);
            }
            if (Room20 != null)
            {
                Room20.SetActive(false);
            }
            if (Room21 != null)
            {
                //Room21.SetActive(false);
            }
            if (Room22 != null)
            {
                //Room22.SetActive(false);
            }
            if (Room23 != null)
            {
                Room23.SetActive(false);
            }
            if (Room24 != null)
            {
                Room24.SetActive(false);
            }
            if (Room25 != null)
            {
                Room25.SetActive(false);
            }
            if (Room26 != null)
            {
                //Room26.SetActive(false);
            }
            


        }
    }


}