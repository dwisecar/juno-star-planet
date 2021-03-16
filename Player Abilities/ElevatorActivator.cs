using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class ElevatorActivator : MonoBehaviour
    {

        public bool StandingOnElevator = false;

        protected LucyFlamethrower _lucyFlamethrower;
        protected Elevator _elevator;

        // Use this for initialization
        void Start()
        {
            _lucyFlamethrower = GetComponentInParent<LucyFlamethrower>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerStay2D(Collider2D _collision)
        {

            _elevator = _collision.GetComponent<Elevator>();

            if (_collision.gameObject.tag == "Elevator")
            {
                StandingOnElevator = true;

                if (_lucyFlamethrower.ShootingUp || _lucyFlamethrower.ShootingUpward)
                {
                    _elevator.AuthorizeMovement();
                }
                else
                {
                    _elevator.ForbidMovement();
                }
            }
        }
    }
}