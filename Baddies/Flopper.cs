using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{
    public class Flopper : MonoBehaviour
    {
        protected CharacterHandleWeapon _characterShoot;
        protected Character _player;
        protected Vector3 pos1;
        protected Vector3 pos2;
        protected bool _playerSpotted;
        protected bool _detectorActive = true;

        //parent transform
        public Transform _Flopper;
        public float TimeUpsideDown = 1f;
        public float Speed = 4f;
        public float RotationSpeed = 7f;
        //Distance it moves side to side
        public float HoverDistance = 2f;
        public float TimeItTakesToRotate=1f;
        public float RotationAngle = 180;

        //raycast details
        public float ShootDistance = 10f;
        public Vector2 LHRaycastOriginOffset = new Vector2(-1.5f, 6);
        public Vector2 RHRaycastOriginOffset = new Vector2(1.5f, 6);
        public LayerMask TargetLayerMask;

        protected Vector2 _raycastLHOrigin;
        protected Vector2 _raycastRHOrigin;
        protected RaycastHit2D _raycastLH;
        protected RaycastHit2D _raycastRH;


        protected bool FlipBackMode;

        // Start is called before the first frame update
        void Start()
        {
            _characterShoot = GetComponentInParent<CharacterHandleWeapon>();
            _detectorActive = true;
            pos1 = new Vector3(_Flopper.position.x + HoverDistance, _Flopper.position.y, _Flopper.position.z);
            pos2 = new Vector3(_Flopper.position.x - HoverDistance, _Flopper.position.y, _Flopper.position.z);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_playerSpotted)
            {
                HoverMode();
                
            }

            if (_playerSpotted)
            {
                _detectorActive = false;
                _Flopper.rotation = Quaternion.Slerp(_Flopper.rotation, Quaternion.Euler(0, 0, RotationAngle), Time.deltaTime * RotationSpeed);
                StartCoroutine(DropProjectile());
            }

            if (FlipBackMode)
            {
                _Flopper.rotation = Quaternion.Slerp(_Flopper.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * RotationSpeed);
            }
        }

        protected virtual void DetectTarget()
        {
            // we cast a ray in front of the agent to check for a Player
            _raycastLHOrigin.x = transform.position.x + LHRaycastOriginOffset.x;
            _raycastLHOrigin.y = transform.position.y + LHRaycastOriginOffset.y;
            _raycastLH = MMDebug.RayCast(_raycastLHOrigin, Vector2.down, ShootDistance, TargetLayerMask, Color.yellow, true);

            _raycastRHOrigin.x = transform.position.x + RHRaycastOriginOffset.x;
            _raycastRHOrigin.y = transform.position.y + RHRaycastOriginOffset.y;
            _raycastRH = MMDebug.RayCast(_raycastRHOrigin, Vector2.down, ShootDistance, TargetLayerMask, Color.yellow, true);

            if (_raycastLH) { _playerSpotted = true; }
            if (_raycastRH) { _playerSpotted = true; }
        }

        protected virtual IEnumerator DropProjectile()
        {
            _characterShoot.ShootStart(); //Start shooter, ****weapon must have delay before firing****
            yield return new WaitForSeconds(TimeItTakesToRotate);  //time it takes to flip roughly
            
            yield return new WaitForSeconds(TimeUpsideDown);  //time to wait before rotating back.
            _characterShoot.ShootStop(); //Turn off weapon
            _playerSpotted = false; //disable spotted mode
            FlipBackMode = true; //enable flip back mode
            yield return new WaitForSeconds(TimeItTakesToRotate*3f);  //time it takes to flip plus some extra time to allow for a little hover mode.
            
            FlipBackMode = false;
            _detectorActive = true;
        }

        protected void HoverMode()
        {
            _Flopper.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Speed * Time.time) + 1.0f) / 2.0f);

            if(_detectorActive) { DetectTarget(); }
        }

    }
}
