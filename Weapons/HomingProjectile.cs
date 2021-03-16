using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Exoplanet/Scripts/Weapons")]
    public class HomingProjectile : Projectile
    {
        protected Rigidbody2D _rigidBody2D;
        protected Transform _transform;
        protected Vector2 _throwingForce;
        protected bool _forceApplied = false;
        protected float _torqueApplied = 35f;
        protected bool hit;

        protected override void Initialization()
        {
            base.Initialization();
            _rigidBody2D = this.GetComponent<Rigidbody2D>();
            _transform = this.GetComponent<Transform>();
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
            _rigidBody2D.angularDrag = 0;
            _rigidBody2D.drag = 0;
            _rigidBody2D.gravityScale = 0;
            hit = false;

        }

        /// <summary>
        /// Handles the projectile's movement, every frame
        /// </summary>
        public override void Movement()
        {
            if (!_forceApplied && (Direction != Vector3.zero) && !hit)
            {

                _throwingForce = Direction * Speed;
                _rigidBody2D.AddForce(_throwingForce);
                _rigidBody2D.AddTorque(_torqueApplied);
                _forceApplied = true;

            }
        }

    }
}
