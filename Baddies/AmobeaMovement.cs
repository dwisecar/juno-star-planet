using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.Tools
{
    
    /// <summary>
    /// Add this component to an object and it'll be able to move along a path defined from its inspector.
    /// </summary>
    public class AmobeaMovement : MMPathMovement
    {

        public int SpinSpeed = 4;

        /// <summary>
        /// Moves the object along the path according to the specified movement type.
        /// </summary>
        public override void MoveAlongThePath()
        {
            switch (AccelerationType)
            {
                case PossibleAccelerationType.ConstantSpeed:
                    transform.position = Vector3.MoveTowards(transform.position, _originalTransformPosition + _currentPoint.Current, Time.deltaTime * MovementSpeed);
                    break;

                case PossibleAccelerationType.EaseOut:
                    transform.position = Vector3.Lerp(transform.position, _originalTransformPosition + _currentPoint.Current, Time.deltaTime * MovementSpeed);
                    break;

                case PossibleAccelerationType.AnimationCurve:
                    float distanceBetweenPoints = Vector3.Distance(_previousPoint, _currentPoint.Current);

                    if (distanceBetweenPoints <= 0)
                    {
                        return;
                    }

                    float remappedDistance = 1 - MMMaths.Remap(_distanceToNextPoint, 0f, distanceBetweenPoints, 0f, 1f);
                    float speedFactor = Acceleration.Evaluate(remappedDistance);

                    transform.position = Vector3.MoveTowards(transform.position, _originalTransformPosition + _currentPoint.Current, Time.deltaTime * MovementSpeed * speedFactor);
                    transform.Rotate(Vector3.one * SpinSpeed * Time.deltaTime);
                    break;
            }
        }

    }
}