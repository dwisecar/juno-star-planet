using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[AddComponentMenu("Corgi Engine/Weapons/RemoteDetonator")] 
	/// <summary>
	/// A basic melee weapon class, that will activate a "hurt zone" when the weapon is used
	/// </summary>
	public class RemoteDetonator : MonoBehaviour 
	{
		public enum DamageAreaShapes { Rectangle, Circle }

		[Header("Explosion")]
		public float TimeBeforeExplosion = 2f;
		public GameObject ExplosionEffect;
		public AudioClip ExplosionSfx;

		[Header("Flicker")]
		public bool FlickerSprite = true;
		public float TimeBeforeFlicker = 1f;

		[Header("Damage Area")]
		public Collider2D DamageAreaCollider;
		public float DamageAreaActiveDuration = 1f;

        public Vector2 LineWidth;
        public Material LaserMaterial;

        public bool TestBool;

        protected LineRenderer _line;
        protected RemoteMine[] _mines;

        protected float _timeSinceStart;
		protected Renderer _renderer;
		protected MMPoolableObject _poolableObject;
        protected RemoteMine _remoteMine;

        public Transform _parentPool;

        protected bool _flickering;
		protected bool _damageAreaActive;

		protected Color _initialColor;
		protected Color _flickerColor = new Color32(255, 20, 20, 255);

        protected LucyInventory _characterInventory;


		protected virtual void OnEnable()
		{
			Initialization ();
		}

		protected virtual void Initialization()
		{
			if (DamageAreaCollider == null)
			{
				Debug.LogWarning ("There's no damage area associated to this bomb : " + this.name + ". You should set one via its inspector.");
				return;
			}
			DamageAreaCollider.isTrigger = true;
			DisableDamageArea ();

			_renderer = gameObject.GetComponentNoAlloc<Renderer> ();
			if (_renderer != null)
			{
				if (_renderer.material.HasProperty("_Color"))
				{
					_initialColor = _renderer.material.color;
				}
			}

			_poolableObject = gameObject.GetComponentNoAlloc<MMPoolableObject> ();
			if (_poolableObject != null)
			{
				_poolableObject.LifeTime = 0;
			}

            _remoteMine = gameObject.GetComponentNoAlloc<RemoteMine>();

			_timeSinceStart = 0;
			_flickering = false;
			_damageAreaActive = false;

			//set variable for remote mine's parent so it can come back to the pool on detonate.
			StartCoroutine(SetParent());

		}

		protected IEnumerator SetParent()
        {
			yield return new WaitForEndOfFrame();
			_parentPool = gameObject.transform.parent;

		}

		protected void HandleInput ()
		{
			if (InputManager.Instance.DetonateButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
			{
                //ActivateBeams();
                StartCoroutine (BlowUp ());
			}
		}

		protected virtual void Update()
		{

			HandleInput ();

			_timeSinceStart += Time.deltaTime;
			// flickering
			if (_timeSinceStart >= TimeBeforeFlicker)
			{
				if (!_flickering && FlickerSprite)
				{
					// We make the bomb's sprite flicker
					if (_renderer != null)
					{
						StartCoroutine(MMImage.Flicker(_renderer,_initialColor,_flickerColor,0.05f,(TimeBeforeExplosion - TimeBeforeFlicker)));	
					}
				}
			}

			// activate damage area on input
			if (_timeSinceStart >= TimeBeforeExplosion && !_damageAreaActive)
			{
                StartCoroutine (BlowUp ());
			}
								
		}

		public virtual IEnumerator BlowUp()
		{
            transform.SetParent(_parentPool);
            EnableDamageArea ();
			_renderer.enabled = false;
			InstantiateExplosionEffect ();
			PlayExplosionSound ();
			_damageAreaActive = true;
			yield return new WaitForSeconds(DamageAreaActiveDuration);
			Destroy ();
        }

        protected virtual void ActivateBeams()
        {
            _line = gameObject.AddComponent<LineRenderer>();
            _line.enabled = true;
            _line.loop = true;
            _line.startWidth = LineWidth.x;
            _line.material = LaserMaterial;

            _mines = FindObjectsOfType<RemoteMine>();

            for (int i = 0; i < _mines.Length; i++)
            {
                _line.SetPosition(0, _mines[i - 1].gameObject.transform.position);
                _line.SetPosition(1, _mines[i].gameObject.transform.position);
                _line.SetPosition(2, _mines[i + 1].gameObject.transform.position);
            }

        }

        public virtual void Destroy()
		{
			_renderer.enabled = true;
			_renderer.material.color = _initialColor;
			transform.SetParent(_parentPool);

            if (_poolableObject != null)
			{
				_poolableObject.Destroy ();
            }
			else
			{
				Destroy ();
            }

		}

		protected virtual void InstantiateExplosionEffect()
		{
			// instantiates the destroy effect
			if (ExplosionEffect!=null)
			{
				GameObject instantiatedEffect=(GameObject)Instantiate(ExplosionEffect,transform.position,transform.rotation);
				instantiatedEffect.transform.localScale = transform.localScale;
			}
		}

		protected virtual void PlayExplosionSound()
		{
			if (ExplosionSfx!=null)
			{
				SoundManager.Instance.PlaySound(ExplosionSfx,transform.position);
			}
		}

		/// <summary>
		/// Enables the damage area.
		/// </summary>
		protected virtual void EnableDamageArea()
		{
			DamageAreaCollider.enabled = true;
		}

		/// <summary>
		/// Disables the damage area.
		/// </summary>
		protected virtual void DisableDamageArea()
		{
			DamageAreaCollider.enabled = false;
		}
	}
}