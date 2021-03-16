using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Weapons/SuperPlasmaBlade")]
    /// <summary>
    /// A basic melee weapon class, that will activate a "hurt zone" when the weapon is used
    /// </summary>
    public class SuperPlasmaBlade : Weapon
    {
        /// the possible shapes for the melee weapon's damage area
        public enum MeleeDamageAreaShapes { Rectangle, Circle }

        [Header("Damage Area")]
        /// the shape of the damage area (rectangle or circle)
        public MeleeDamageAreaShapes DamageAreaShape = MeleeDamageAreaShapes.Rectangle;
        /// the size of the damage area
        public Vector2 AreaSize = new Vector2(1, 1);
        /// the offset to apply to the damage area (from the weapon's attachment position
        public Vector2 AreaOffset = new Vector2(1, 0);

        [Header("Damage Area Timing")]
        /// the initial delay to apply before triggering the damage area
        public float InitialDelay = 0f;
        /// the duration during which the damage area is active
        public float ActiveDuration = 1f;

        public GameObject MeleeBladeClashEffect;
        public AudioClip MeleeClashEffect;

        [Header("Damage Caused")]
        // the layers that will be damaged by this object
        public LayerMask TargetLayerMask;
        /// The amount of health to remove from the player's health
        public int DamageCaused = 10;
        /// the kind of knockback to apply
        public DamageOnTouch.KnockbackStyles Knockback;
        /// The force to apply to the object that gets damaged
        public Vector2 KnockbackForce = new Vector2(10, 2);

        public Vector2 PlayerKnockbackForce = new Vector2(10, 2);
        /// The duration of the invincibility frames after the hit (in seconds)
        public float InvincibilityDuration = 0.5f;

        protected Collider2D _damageArea;
        protected bool _attackInProgress = false;
        protected bool _airAttackInProgress = false;

        protected PlasmaBladeKnockBacker _plasmaBladeKnockBacker;

        [Header("Spawn")]
        /// the offset position at which the projectile will spawn
        public Vector3 ProjectileSpawnOffset = Vector3.zero;
        /// the number of projectiles to spawn per shot
        public int ProjectilesPerShot = 1;
        /// the spread (in degrees) to apply randomly (or not) on each angle when spawning a projectile
        public Vector3 Spread = Vector3.zero;
        /// whether or not the weapon should rotate to align with the spread angle
        public bool RotateWeaponOnSpread = false;
        /// whether or not the spread should be random (if not it'll be equally distributed)
        public bool RandomSpread = true;

        [ReadOnly]
        public Vector3 SpawnPosition = Vector3.zero;

        public MMObjectPooler ObjectPooler { get; set; }

        protected Vector3 _flippedProjectileSpawnOffset;
        protected Vector3 _randomSpreadDirection;
        protected bool _poolInitialized = false;

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
            CreateDamageArea();
            DisableDamageArea();
            _aimableWeapon = GetComponent<WeaponAim>();

            if (!_poolInitialized)
            {
                if (GetComponent<MMMultipleObjectPooler>() != null)
                {
                    ObjectPooler = GetComponent<MMMultipleObjectPooler>();
                }
                if (GetComponent<MMSimpleObjectPooler>() != null)
                {
                    ObjectPooler = GetComponent<MMSimpleObjectPooler>();
                }
                if (ObjectPooler == null)
                {
                    Debug.LogWarning(this.name + " : no object pooler (simple or multiple) is attached to this Projectile Weapon, it won't be able to shoot anything.");
                    return;
                }
                if (FlipWeaponOnCharacterFlip)
                {
                    _flippedProjectileSpawnOffset = ProjectileSpawnOffset;
                    _flippedProjectileSpawnOffset.y = -_flippedProjectileSpawnOffset.y;
                }
                _poolInitialized = true;
            }
        }

        protected override void Update()
        {
            ApplyOffset();

        }

        /// <summary>
        /// Creates the damage area.
        /// </summary>
        protected virtual void CreateDamageArea()
        {
            GameObject damageArea = new GameObject();

            damageArea.name = this.name + "DamageArea";
            damageArea.transform.position = this.transform.position;
            damageArea.transform.rotation = this.transform.rotation;
            damageArea.transform.SetParent(this.transform);

            if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
            {
                BoxCollider2D boxcollider2d = damageArea.AddComponent<BoxCollider2D>();
                boxcollider2d.offset = AreaOffset;
                boxcollider2d.size = AreaSize;
                _damageArea = boxcollider2d;
            }
            if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
            {
                CircleCollider2D circlecollider2d = damageArea.AddComponent<CircleCollider2D>();
                circlecollider2d.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
                circlecollider2d.radius = AreaSize.x / 2;
                _damageArea = circlecollider2d;
            }
            _damageArea.isTrigger = true;

            Rigidbody2D rigidBody = damageArea.AddComponent<Rigidbody2D>();
            rigidBody.isKinematic = true;

            DamageOnTouch damageOnTouch = damageArea.AddComponent<DamageOnTouch>();
            damageOnTouch.TargetLayerMask = TargetLayerMask;
            damageOnTouch.DamageCaused = DamageCaused;
            damageOnTouch.DamageCausedKnockbackType = Knockback;
            damageOnTouch.DamageCausedKnockbackForce = KnockbackForce;
            damageOnTouch.InvincibilityDuration = InvincibilityDuration;

            _plasmaBladeKnockBacker = damageArea.AddComponent<PlasmaBladeKnockBacker>();
            _plasmaBladeKnockBacker.CorgiController = _controller;
            _plasmaBladeKnockBacker.PlayerKnockbackForce = PlayerKnockbackForce;
            _plasmaBladeKnockBacker.characterPlayer = Owner;
            _plasmaBladeKnockBacker.ClashEffect = MeleeClashEffect;
            _plasmaBladeKnockBacker.BladeClashEffect = MeleeBladeClashEffect;
        }

        /// <summary>
        /// When the weapon is used, we trigger our attack routine
        /// </summary>
        protected override void WeaponUse()
        {
            base.WeaponUse();

            //fire the projectile
            DetermineSpawnPosition();

            for (int i = 0; i < ProjectilesPerShot; i++)
            {
                SpawnProjectile(SpawnPosition, i, ProjectilesPerShot, true);
            }

            //enable damage area from melee attack
            StartCoroutine(MeleeWeaponAttack());

            ModifyMovementWhileAttacking = true;
            MovementMultiplier = 0;


        }

        /// <summary>
        /// Spawns a new object and positions/resizes it
        /// </summary>
        public virtual GameObject SpawnProjectile(Vector3 spawnPosition, int projectileIndex, int totalProjectiles, bool triggerObjectActivation = true)
        {
            /// we get the next object in the pool and make sure it's not null
            GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

            // mandatory checks
            if (nextGameObject == null) { return null; }
            if (nextGameObject.GetComponent<MMPoolableObject>() == null)
            {
                throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
            }
            // we position the object
            nextGameObject.transform.position = spawnPosition;
            // we set its direction

            Projectile projectile = nextGameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetWeapon(this);
                if (Owner != null)
                {
                    projectile.SetOwner(Owner.gameObject);
                }
            }
            // we activate the object
            nextGameObject.gameObject.SetActive(true);


            if (projectile != null)
            {
                if (RandomSpread)
                {
                    _randomSpreadDirection.x = UnityEngine.Random.Range(-Spread.x, Spread.x);
                    _randomSpreadDirection.y = UnityEngine.Random.Range(-Spread.y, Spread.y);
                    _randomSpreadDirection.z = UnityEngine.Random.Range(-Spread.z, Spread.z);
                }
                else
                {
                    if (totalProjectiles > 1)
                    {
                        _randomSpreadDirection.x = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.x, Spread.x);
                        _randomSpreadDirection.y = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.y, Spread.y);
                        _randomSpreadDirection.z = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.z, Spread.z);
                    }
                    else
                    {
                        _randomSpreadDirection = Vector3.zero;
                    }
                }

                Quaternion spread = Quaternion.Euler(_randomSpreadDirection);
                projectile.SetDirection(spread * transform.right * (Flipped ? -1 : 1), transform.rotation, Owner.IsFacingRight);
                if (RotateWeaponOnSpread)
                {
                    this.transform.rotation = this.transform.rotation * spread;
                }
            }

            if (triggerObjectActivation)
            {
                if (nextGameObject.GetComponent<MMPoolableObject>() != null)
                {
                    nextGameObject.GetComponent<MMPoolableObject>().TriggerOnSpawnComplete();
                }
            }

            return (nextGameObject);
        }

        /// <summary>
        /// Determines the spawn position based on the spawn offset and whether or not the weapon is flipped
        /// </summary>
        public virtual void DetermineSpawnPosition()
        {
            if (Flipped)
            {
                if (FlipWeaponOnCharacterFlip)
                {
                    SpawnPosition = this.transform.position - this.transform.rotation * _flippedProjectileSpawnOffset;
                }
                else
                {
                    SpawnPosition = this.transform.position - this.transform.rotation * ProjectileSpawnOffset;
                }
            }
            else
            {
                SpawnPosition = this.transform.position + this.transform.rotation * ProjectileSpawnOffset;
            }
        }

        /// <summary>
        /// Triggers an attack, turning the damage area on and then off
        /// </summary>
        /// <returns>The weapon attack.</returns>
        protected virtual IEnumerator MeleeWeaponAttack()
        {
            if (_attackInProgress) { yield break; }

            _airAttackInProgress = !_controller.State.IsGrounded ? true : false;

            _attackInProgress = true;
            yield return new WaitForSeconds(InitialDelay);
            EnableDamageArea();
            yield return new WaitForSeconds(ActiveDuration);
            DisableDamageArea();
            _attackInProgress = false;
            _airAttackInProgress = false;
        }


        /// <summary>
        /// Enables the damage area.
        /// </summary>
        protected virtual void EnableDamageArea()
        {
            _damageArea.enabled = true;
            //new codes for plasma blade
          //  _characterHorizontalMovement.AbilityPermitted = false;

        }
        /// <summary>
        /// Disables the damage area.
        /// </summary>
        protected virtual void DisableDamageArea()
        {
            _damageArea.enabled = false;
            //new codes for plasma blade
           // _characterHorizontalMovement.AbilityPermitted = true;

        }


        /// <summary>
        /// When the weapon is selected in scene view, draws the damage area's shape
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (_damageArea != null) { return; }

            Gizmos.color = Color.white;
            if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
            {
                Gizmos.DrawWireSphere(this.transform.position + this.transform.rotation * AreaOffset, AreaSize.x / 2);
            }
            if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
            {
                MMDebug.DrawGizmoRectangle(this.transform.position + this.transform.rotation * AreaOffset, AreaSize, Color.white);
            }
        }

    }
}
