using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

namespace MoreMountains.CorgiEngine
{

    public class RoboRobeDetector : MonoBehaviour
    {
        public Transform pos1;
        public Transform pos2;
        public Transform pos3;
        public Transform pos4;

        public float Speed = 5f;
        public Transform RoboRobe;

        protected BoxCollider2D _collider;
        protected Character _player;
        protected Character _robeCharacter;
        public bool _playerSpotted;
        protected CharacterHandleWeapon _characterShoot;
        protected Vector3 currentPoint;
        protected Animator _anim;
        protected BaddieHealth _baddieHealth;

        public bool _moveMode;

        protected int previousNumber = 0;
        protected int randomNumber;

        public bool coroutineRunning;


        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponentInParent<BoxCollider2D>();
            _anim = GetComponentInParent<Animator>();
            _characterShoot = GetComponentInParent<CharacterHandleWeapon>();
            _robeCharacter = GetComponentInParent<Character>();
            _baddieHealth = GetComponentInParent<BaddieHealth>();
            _characterShoot.enabled = false;
            currentPoint = pos2.position;
            
        }

        // Update is called once per frame
        void Update()
        {
            CheckHealth();

            if (_playerSpotted)
            {
                if (_moveMode && !coroutineRunning)
                {
                    _anim.SetBool("Moving", true);
                    _anim.SetBool("Shooting", false);
                    MoveToNextSpot();
                }
                else
                {
                    CheckForFlip();

                    if (!coroutineRunning)
                    {
                        

                        _anim.SetBool("Shooting", true);
                        _anim.SetBool("Moving", false);
                        StartCoroutine(PhaseAndShoot());
                        _characterShoot.ShootStop();
                    }
                }
            }
        }

        protected void CheckHealth()
        {
            if(_baddieHealth.CurrentHealth <= 0)
            {
                coroutineRunning = false;
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character>() != null)
            {
                _player = collision.GetComponent<Character>();

                if (_player != null)
                {
                    if (_player.CharacterType == Character.CharacterTypes.Player)
                    {
                        if (!_playerSpotted)
                        {
                            _playerSpotted = true;
                            _moveMode = true;
                        }
                    }
                }
            }
        }

        protected virtual void CheckForFlip()
        {
            if(_robeCharacter.enabled == true)
            {
                if (_player.transform.position.x < RoboRobe.position.x)
                {
                    if (_robeCharacter.IsFacingRight) { _robeCharacter.Flip(); }
                }
                if (_player.transform.position.x >= RoboRobe.position.x)
                {
                    if (!_robeCharacter.IsFacingRight) { _robeCharacter.Flip(); }
                }
            }
            
        }

        protected virtual IEnumerator PhaseAndShoot()
        {
            coroutineRunning = true;

            //_collider.enabled = true;

            yield return new WaitForSeconds(2f); //wait before shooting so animation lines up
            _characterShoot.enabled = true;
            _characterShoot.ShootStart(); //turn on weapon and right back off so that only one shot fires.

            StartCoroutine(PhaseOut());

        }

        protected virtual IEnumerator PhaseOut()
        {
            FindNextSpot();
            yield return new WaitForSeconds(1f);
            _characterShoot.ShootStop();
            _moveMode = true;
            //_collider.enabled = false;

            coroutineRunning = false;
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
            }
        }
    }
}
