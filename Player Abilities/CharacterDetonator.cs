using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Add this component to a character and it'll be able to activate mines.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Detonator")] 
	public class CharacterDetonator : CharacterAbility 
	{
		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "Allows this character (and the player controlling it) to detonate all the mines planted."; }

        public bool SuperMinesObtained;
		public bool detonate = false;
        public Vector2 LineWidth;
        public Material LaserMaterial;

        protected LineRenderer _line;
        protected RemoteMine[] _mines;

        protected override void HandleInput()
		{
			if (_inputManager.DetonateButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed)				
			{
				detonate = true;
			}

            if (SuperMinesObtained == true)
            {
                ActivateBeams();
            }
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
    }
}
