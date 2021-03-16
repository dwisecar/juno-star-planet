using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class AIPhageHoverOver : AIActionFlyTowardsTarget
    {
        protected bool _attackTime;
        public float TimeBeforeAttacking = 2f;
        public float HoverOverSpeed = 7f;
        public float AttackSpeed = 11f;
        protected PhageWobbler wobbler;
        protected CorgiController _controller;
        protected Transform _phage;
        public bool _latchMode;
        public bool _hoverOverMode;
		protected float _hoverPoint;
		protected float _attackingPoint;
		protected Vector3 _targetLocation;
        

        /// <summary>
        /// On init we grab our CharacterFly ability
        /// </summary>
        protected override void Initialization()
        {
            _characterFly = this.gameObject.GetComponent<CharacterFly>();
            _controller = this.gameObject.GetComponent<CorgiController>();
            _phage = this.gameObject.GetComponent<Transform>();

            wobbler = this.gameObject.GetComponent<PhageWobbler>();
        }

        /// <summary>
        /// On PerformAction we fly
        /// </summary>
        public override void PerformAction()
        {
            if (!_latchMode && !_hoverOverMode) { FlyAbove(); }
            if (_latchMode) { LatchMode(); }
            if (_hoverOverMode) { HoverOverMode(); }
        }

        /// <summary>
        /// Moves the character towards the target if needed
        /// </summary>
        protected virtual void FlyAbove()
        {
            if (_brain.Target == null)
            {
                return;
            }

			_hoverPoint = _brain.Target.position.y + 6f;
			_attackingPoint = _brain.Target.position.y + 2.8f;


			if (!_attackTime)
            {
                _characterFly.FlySpeed = HoverOverSpeed;

                if (this.transform.position.x < _brain.Target.position.x)
                {
                    _characterFly.SetHorizontalMove(1f);
                }
                else
                {
                    _characterFly.SetHorizontalMove(-1f);
                }

                if (this.transform.position.y < _hoverPoint)
                {
                    _characterFly.SetVerticalMove(1f);
                }
                else
                {
                    _characterFly.SetVerticalMove(-1f);
                }

                if (Mathf.Abs(this.transform.position.x - _brain.Target.position.x) < MinimumDistance)
                {
                    _characterFly.SetHorizontalMove(0f);
                    StartCoroutine(AttackTimer());
                }

                if (Mathf.Abs(this.transform.position.y - _hoverPoint) < MinimumDistance)
                {
                    _characterFly.SetVerticalMove(0f);
                    //_hoverOverMode = true;
                }
            }

            if (_attackTime)
            {

                _characterFly.FlySpeed = AttackSpeed;
                wobbler.WobbleMode = false;

                if (this.transform.position.x < _brain.Target.position.x)
                {
                    _characterFly.SetHorizontalMove(1f);
                }
                else
                {
                    _characterFly.SetHorizontalMove(-1f);
                }

                if (this.transform.position.y < _attackingPoint)
                {
                    _characterFly.SetVerticalMove(1f);
                }
                else
                {
                    _characterFly.SetVerticalMove(-1f);
                }

                if (Mathf.Abs(this.transform.position.x - _brain.Target.position.x) < MinimumDistance)
                {
                    _characterFly.SetHorizontalMove(0f);
                }

                if (Mathf.Abs(this.transform.position.y - _attackingPoint) < MinimumDistance)
                {
                    _characterFly.SetVerticalMove(0f);
                    //_hoverOverMode = false;
                    //_latchMode = true;

                }

            }
        }

        protected virtual void HoverOverMode()
        {
            _targetLocation = new Vector3(_brain.Target.position.x, _brain.Target.position.y + 6f, 0);
            _phage.position = Vector3.MoveTowards(_phage.position, _targetLocation, (AttackSpeed * Time.deltaTime));

        }

        protected virtual void LatchMode()
        {
            _targetLocation = new Vector3(_brain.Target.position.x, _brain.Target.position.y + 2.8f, 0);
            _phage.position = Vector3.MoveTowards(_phage.position, _targetLocation, (AttackSpeed * Time.deltaTime));

        }

        protected virtual IEnumerator AttackTimer()
        {
            if(!_attackTime)
            {
                yield return new WaitForSeconds(TimeBeforeAttacking);
                _attackTime = true;
               

            }
            
        }

    }


    
}