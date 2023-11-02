using System;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

namespace Managers
{
    public sealed class MovementManager 
    {
        public event Action MovementTriggered;
            
        private static readonly MovementManager Instance = new();

        private static Vector3 _hitPosition;
            
        public static MovementManager Initialize()
        {
            return Instance;
        }
        
        public void  TriggerMovement()
        {
            MovementTriggered?.Invoke();
        }

        public void GetAllMovableObjects(List<IMovable> movableSelectedObjects)
        {
            
        }

        public static Vector3 GetHitPosition()
        {
            return _hitPosition;
        }
        
        public static void SetHitPosition(Vector3 position)
        {
            _hitPosition = position;
        }
    }
}