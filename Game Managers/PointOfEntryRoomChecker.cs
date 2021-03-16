using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class PointOfEntryRoomChecker : MonoBehaviour
    {
        public bool EntryPoint0;
        public bool EntryPoint1;
        public bool EntryPoint2;

        protected Character _player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character>() != null)
            {
                _player = collision.GetComponent<Character>();

            }
        }
    }
}