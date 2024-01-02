using System.Collections.Generic;
using Behaviors;
using UnityEngine;

namespace Managers
{
    
    public sealed class SelectionManager
    {
        public bool IsDragging { get; private set; }
        
        public Bounds SelectionBounds;
        public static Camera MCamera;
        public Vector2[] RVector2S;
        public Texture TextureMat;
        
        private readonly Dictionary<int, ISelectable> _selectedGameObjects = new ();
        
        public Dictionary<int, ISelectable> GetAllSelected()
        {
            return _selectedGameObjects;
        }

        public void FreeAllSelected()
        {
            foreach (var item in _selectedGameObjects.Values)
            {
                item.DeselectTarget();
            }
            _selectedGameObjects.Clear();
        }

        public void AddObjectToDict(ref ISelectable selectable)
        {
            var id = selectable.GetGameObject().GetInstanceID();
            if (_selectedGameObjects.ContainsKey(id)) return;
            _selectedGameObjects.Add(id, selectable);
            selectable.SelectTarget();
        }

        public void RemoveObjectFromDict(ref ISelectable selectable)
        {
            _selectedGameObjects.Remove(selectable.GetGameObject().GetInstanceID());
            selectable.DeselectTarget();
        }

        public bool ObjectIsSelected(ref ISelectable selectable)
        {
            return _selectedGameObjects.ContainsKey(selectable.GetGameObject().GetInstanceID());
        }

        public void SetIsDragging(bool isDragging)
        {
            IsDragging = isDragging;
        }

        public void SetSelectionBound(Bounds bounds)
        {
            SelectionBounds = bounds;
        }
    }
}