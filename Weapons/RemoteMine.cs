using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("Corgi Engine/Weapons/RemoteMine")]
	public class RemoteMine : Projectile 
	{
		protected Rigidbody2D _rigidBody2D;
		protected Transform _transform;
		protected Vector2 _throwingForce;
		protected bool _forceApplied = false;
		protected float _torqueApplied = 15f;
		protected bool hit;

        public Transform _originalParent;

		protected override void Initialization()
		{
			base.Initialization();
			_rigidBody2D = this.GetComponent<Rigidbody2D>();
			_transform = this.GetComponent<Transform> ();			
		}

		/// <summary>
		/// On enable, we reset the object's speed
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			_forceApplied = false;
			_rigidBody2D.freezeRotation = false;
			_rigidBody2D.isKinematic = false;
			_rigidBody2D.angularDrag = .05f;
			_rigidBody2D.drag = 0;
			_rigidBody2D.gravityScale = 1;
			hit = false;

			_originalParent = gameObject.transform.parent;
		}

		/// <summary>
		/// Handles the projectile's movement, every frame
		/// </summary>
		public override void Movement()
		{
			if (!_forceApplied && (Direction != Vector3.zero) && !hit)
			{
				
					_throwingForce = Direction * Speed;
					_rigidBody2D.AddForce (_throwingForce);
					_rigidBody2D.AddTorque (_torqueApplied);
					_forceApplied = true;

			}
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			hit = true;
			_rigidBody2D.freezeRotation = true;
			_rigidBody2D.angularDrag = 1000000;
			_rigidBody2D.drag = 1000000;
			_rigidBody2D.gravityScale = 0;
			
            if (col.gameObject.CompareTag("Enemies"))
            {
                gameObject.transform.parent = col.gameObject.transform;
            }
		}

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemies"))
            {
                if(collision.enabled == true)
                {
                    _rigidBody2D.freezeRotation = true;
                    _rigidBody2D.angularDrag = 1000000;
                    _rigidBody2D.drag = 1000000;
                    _rigidBody2D.gravityScale = 0;
                    gameObject.transform.parent = collision.gameObject.transform;
                }

                if (collision.enabled == false)
                {
                    gameObject.transform.parent = null;
                }
                
            }
        }



    }
}
