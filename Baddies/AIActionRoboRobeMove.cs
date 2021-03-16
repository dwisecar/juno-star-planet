using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class AIActionRoboRobeMove : AIAction
    {
        protected Transform RoboRobe;
        public Transform pos1;
        public Transform pos2;
        public Transform pos3;
        public Transform pos4;
        public float Speed = 10f;

        protected Vector3 currentPoint;

        protected bool _moveMode;

        protected int previousNumber = 0;
        protected int randomNumber;

        protected AIDecisionRoboRobeDoneMoving _decideMoveDone;

        protected override void Start()
        {
            base.Start();
            RoboRobe = gameObject.transform;
            _decideMoveDone = GetComponent<AIDecisionRoboRobeDoneMoving>();
        }

        /// <summary>
        /// On PerformAction we fly
        /// </summary>
        public override void PerformAction()
        {
            if(_moveMode)
            {
                MoveToNextSpot();
            }
        }

        protected void GenerateNumber()
        {
            randomNumber = Random.Range(0, 3);
        }

        protected virtual void FindNextSpot()
        {
            GenerateNumber();

            while (previousNumber == randomNumber) { GenerateNumber(); }

            previousNumber = randomNumber;

            if (randomNumber == 0) { currentPoint = pos1.position; }
            if (randomNumber == 1) { currentPoint = pos2.position; }
            if (randomNumber == 2) { currentPoint = pos3.position; }
            if (randomNumber == 3) { currentPoint = pos4.position; }

        }

        protected virtual void MoveToNextSpot()
        {
            float step = Speed * Time.deltaTime; // calculate distance to move
            RoboRobe.position = Vector3.MoveTowards(RoboRobe.position, currentPoint, step);

            float dist = Vector3.Distance(RoboRobe.position, currentPoint);

            if (RoboRobe.position == currentPoint)
            {
                _moveMode = false;
                _decideMoveDone.MoveCompleted = true;
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            FindNextSpot();
            _moveMode = true;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            _moveMode = false;
        }
    }
}