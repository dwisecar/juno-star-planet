using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class LucySuperRemoteMines : MonoBehaviour
    {
        public Vector2 LineWidth;
        public Material LaserMaterial;

        protected LineRenderer _line;
        protected RemoteMine[] _mines;


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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}