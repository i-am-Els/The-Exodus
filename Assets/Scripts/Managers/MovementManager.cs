using System;
using UnityEngine;

namespace Managers
{
    public sealed class MovementManager 
    {
        public event Action MovementTriggered;

        private static Vector3 _hitPosition;
        
        public void  TriggerMovement()
        {
            MovementTriggered?.Invoke();
        }

        public static Vector3 GetHitPosition()
        {
            return _hitPosition;
        }
        
        public void SetHitPosition(Vector3 position)
        {
            _hitPosition = position;
        }
    }
}