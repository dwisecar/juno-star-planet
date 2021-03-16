using UnityEngine;
using UnityEditor;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[AddComponentMenu("Corgi Engine/Weapons/ReflectBeam")]
	public class ReflectBeam : Projectile

	{

        protected Vector3 newDirection;

        public virtual void WallBounce(bool sideHit)
        {
            if (sideHit)
            {
                newDirection = new Vector3((Direction.x * -1), Direction.y, Direction.z);
            }
            else
            {
                newDirection = new Vector3(Direction.x, (Direction.y * -1), Direction.z);
            }

            SetDirection(newDirection, transform.rotation);
        }
    }
}