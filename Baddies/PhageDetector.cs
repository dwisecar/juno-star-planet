using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{

    public class PhageDetector : MonoBehaviour
    {
        [Header("Speed and Measurements")]
        public float Speed = 3f;
        public float HoverDistance = 2f;
        public float PlayerHeadHeight;
        public float HoveringHeight;
        public GameObject SpikeTube;
        public Transform _phage;

        [Header("Wobble Rotation")]
        public float _Angle = 10f;
        public float _Period = 2f;
        protected float _Time;
        

        [Header("Testing Bools")]
        public bool _hoverMode;
        public bool _preAttackMode;
        public bool _attackMode;

        /// the radius to search our target in
        public float Radius = 3f;
        /// the center of the search circle
        public Vector3 DetectionOriginOffset = new Vector3(0, 0, 0);
        /// the layer(s) to search our target on
        public LayerMask TargetLayer;

        //for the detector
        protected Vector2 _raycastOrigin;
        protected Collider2D _detectionCollider = null;
        protected Color _gizmoColor = Color.yellow;
        protected bool _init = false;

        protected Character _player;
        protected Vector3 pos1; //hover mode position 1
        protected Vector3 pos2; //hover kode pos 2
        protected Vector3 _overTarget;
        protected Vector3 _attackingPosition;
        protected Animator _anim;

        // Start is called before the first frame update
        void Start()
        {
           
            _hoverMode = true;
            _preAttackMode = false;
            _attackMode = false;
            _anim = GetComponentInParent<Animator>();
            _gizmoColor.a = 0.25f;
            _init = true;
            _detectionCollider = null;

            pos1 = new Vector3(_phage.position.x, _phage.position.y + HoverDistance, _phage.position.z);
            pos2 = new Vector3(_phage.position.x, _phage.position.y - HoverDistance, _phage.position.z);
        }

        protected void Update()
        {
            DetectTarget();
            if (_hoverMode) { HoverMode(); }
            if (_preAttackMode) { PreAttackMode(); }
            if (_attackMode) { AttackMode(); }
        }

        protected virtual void DetectTarget()
        {
            // we cast a ray to the left of the agent to check for a Player
            _raycastOrigin.x = transform.position.x + DetectionOriginOffset.x;
            _raycastOrigin.y = transform.position.y + DetectionOriginOffset.y;

            _detectionCollider = Physics2D.OverlapCircle(_raycastOrigin, Radius, TargetLayer);
            if (_detectionCollider != null)
            {
                _hoverMode = false;
                _preAttackMode = true;
                if (!_attackMode) { StartCoroutine(HoverWaitAttack()); }

                _overTarget = new Vector3(_detectionCollider.gameObject.transform.position.x, _detectionCollider.gameObject.transform.position.y + HoveringHeight);
                _attackingPosition = new Vector3(_detectionCollider.gameObject.transform.position.x, _detectionCollider.gameObject.transform.position.y + PlayerHeadHeight);
            }
           
        }

        /*public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponent<Character>() != null)
            {
                _player = collision.GetComponent<Character>();

                if (_player != null)
                {
                    //For each frame that the player is in the circle, we set the vector3 for over the player's head, high and low.
                    if (_player.CharacterType == Character.CharacterTypes.Player)
                    {
                        _hoverMode = false;
                        _preAttackMode = true;
                        if(!_attackMode) { StartCoroutine(HoverWaitAttack()); }

                        _overTarget = new Vector3(collision.transform.position.x, collision.transform.position.y + HoveringHeight);
                        _attackingPosition = new Vector3(collision.transform.position.x, collision.transform.position.y + PlayerHeadHeight);
                    }
                }
            }
        }*/

        protected virtual void HoverMode()
        {
            //for vertical movement
            _phage.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Speed * Time.time) + 1.0f) / 2.0f);

            //for rotational wobble
            _Time = _Time + Time.deltaTime;
            float phase = Mathf.Sin(_Time / _Period);
            _phage.localRotation = Quaternion.Euler(new Vector3(0, 0, phase * _Angle));
        }

        protected virtual void PreAttackMode()
        {
            _phage.rotation = Quaternion.Slerp(_phage.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * Speed); //rotate upright
            float step = (Speed*2) * Time.deltaTime; // calculate distance to move
            _phage.position = Vector3.MoveTowards(_phage.position, _overTarget, step);
        }

        protected virtual void AttackMode()
        {
            _anim.SetBool("Attacking", true);
            _phage.rotation = Quaternion.Slerp(_phage.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * Speed); //rotate upright
            float stepFast = (Speed*5) * Time.deltaTime; // calculate distance to move
            _phage.position = Vector3.MoveTowards(_phage.position, _attackingPosition, stepFast);
        }

        protected virtual IEnumerator HoverWaitAttack()
        {
            _attackMode = false;
            yield return new WaitForSeconds(1.5f);
            _preAttackMode = false;
            _attackMode = true;
        }

        /// <summary>
        /// Draws gizmos for the detection circle
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            _raycastOrigin.x = transform.position.x + DetectionOriginOffset.x;
            _raycastOrigin.y = transform.position.y + DetectionOriginOffset.y;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_raycastOrigin, Radius);
            if (_init)
            {
                Gizmos.color = _gizmoColor;
                Gizmos.DrawSphere(_raycastOrigin, Radius);
            }
        }
    }
}