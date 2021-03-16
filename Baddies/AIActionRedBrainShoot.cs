﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// An Action that shoots using the currently equipped weapon. If your weapon is in auto mode, will shoot until you exit the state, and will only shoot once in SemiAuto mode. You can optionnally have the character face (left/right) the target, and aim at it (if the weapon has a WeaponAim component).
    /// </summary>
    public class AIActionRedBrainShoot : AIAction
    {
        public AudioClip EnterStateSfx;
        public ParticleSystem ParticleEmitter;

        protected Animator anim;

        /// if true, the Character will face the target (left/right) when shooting
        public bool FaceTarget = true;
        /// if true the Character will aim at the target when shooting
        public bool AimAtTarget = false;

        protected Character _character;
        protected CharacterHandleWeapon _characterHandleWeapon;
        protected WeaponAim _weaponAim;
        protected ProjectileWeapon _projectileWeapon;
        protected Vector3 _weaponAimDirection;
        protected int _numberOfShoots = 0;
        protected bool _shooting = false;

        /// <summary>
        /// On init we grab our CharacterHandleWeapon ability
        /// </summary>
        protected override void Initialization()
        {
            _character = GetComponent<Character>();
            _characterHandleWeapon = this.gameObject.GetComponent<CharacterHandleWeapon>();
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// On PerformAction we face and aim if needed, and we shoot
        /// </summary>
        public override void PerformAction()
        {
            TestFaceTarget();
            TestAimAtTarget();
            Shoot();
        }

        /// <summary>
        /// Sets the current aim if needed
        /// </summary>
        protected virtual void Update()
        {
            if (_characterHandleWeapon.CurrentWeapon != null)
            {
                if (_weaponAim != null)
                {
                    if (_shooting)
                    {
                        _weaponAim.SetCurrentAim(_weaponAimDirection);
                    }
                    else
                    {
                        if (_character.IsFacingRight)
                        {
                            _weaponAim.SetCurrentAim(Vector3.right);
                        }
                        else
                        {
                            _weaponAim.SetCurrentAim(Vector3.left);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aims at the target if required
        /// </summary>
        protected virtual void TestAimAtTarget()
        {
            if (!AimAtTarget)
            {
                return;
            }

            if (_characterHandleWeapon.CurrentWeapon != null)
            {
                if (_weaponAim == null)
                {
                    _weaponAim = _characterHandleWeapon.CurrentWeapon.gameObject.GetComponentNoAlloc<WeaponAim>();
                }

                if (_weaponAim != null)
                {
                    //******* New code to stop shooting at player's feet ********************
                    Vector3 targetPosition = new Vector3(_brain.Target.position.x, _brain.Target.position.y + 1.3f, 0);

                    if (_projectileWeapon != null)
                    {
                        _projectileWeapon.DetermineSpawnPosition();
                        _weaponAimDirection = targetPosition - (_projectileWeapon.SpawnPosition); //NEW CODE FOR FEET SHOOTING FIX

                        //_weaponAimDirection = _brain.Target.position - (_projectileWeapon.SpawnPosition); //OLD CODE FOR FEET SHOOTING FIX
                    }
                    else
                    {
                        _weaponAimDirection = targetPosition - _characterHandleWeapon.CurrentWeapon.transform.position; //NEW CODE FOR FEET FIX
                        //_weaponAimDirection = _brain.Target.position - _characterHandleWeapon.CurrentWeapon.transform.position; //OLD CODE FOR FEET FIX
                    }
                }
            }
        }


        /// <summary>
        /// Faces the target if required
        /// </summary>
        protected virtual void TestFaceTarget()
        {
            if (!FaceTarget)
            {
                return;
            }

            if (this.transform.position.x > _brain.Target.position.x)
            {
                _character.Face(Character.FacingDirections.Left);
            }
            else
            {
                _character.Face(Character.FacingDirections.Right);
            }
        }

        /// <summary>
        /// Activates the weapon
        /// </summary>
        protected virtual void Shoot()
        {
            if (_numberOfShoots < 1)
            {
                TurnOnChargeEffect();
                PlaySfx();

                anim.speed = 2;
                _characterHandleWeapon.ShootStart();
                _numberOfShoots++;
            }
        }

        /// <summary>
        /// When entering the state we reset our shoot counter and grab our weapon
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();
            _numberOfShoots = 0;
            _shooting = true;
            _weaponAim = _characterHandleWeapon.CurrentWeapon.gameObject.GetComponentNoAlloc<WeaponAim>();
            _projectileWeapon = _characterHandleWeapon.CurrentWeapon.gameObject.GetComponentNoAlloc<ProjectileWeapon>();
        }

        /// <summary>
        /// When exiting the state we make sure we're not shooting anymore
        /// </summary>
        public override void OnExitState()
        {
            base.OnExitState();

            if (anim != null)
            {
                anim.speed = 1;
                anim.SetBool("Charging", false);
            }

            TurnOffChargeEffect();

            _characterHandleWeapon.ShootStop();
            _shooting = false;
        }

        protected void PlaySfx()
        {
            if (EnterStateSfx != null)
            {
                SoundManager.Instance.PlaySound(EnterStateSfx, transform.position);
            }
        }

        protected void TurnOnChargeEffect()
        {
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = true;

            }

            if(anim!=null)
            {
                anim.SetBool("Charging", true);
                
            }
        }

        protected void TurnOffChargeEffect()
        {
            if (ParticleEmitter != null)
            {
                ParticleSystem.EmissionModule emissionModule = ParticleEmitter.emission;
                emissionModule.enabled = false;

            }

        }


    }
}