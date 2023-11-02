using System.Collections.Generic;
using Behavior;
using UnityEngine;

namespace Managers
{
    
    public sealed class SelectionManager
    {
        private static List<ISelectableObject> _selectedGameObjects = new ();
        
        private static readonly SelectionManager Instance = new ();
        private SelectionManager()
        {
        }

        public static SelectionManager Initialize()
        {
            return Instance;
        }
        
        public List<ISelectableObject> GetAllSelected()
        {
            return _selectedGameObjects;
        }

        public void FreeAllSelected()
        {
            foreach (ISelectableObject item in _selectedGameObjects)
            {
                item.IsSelected = false;
            }
            _selectedGameObjects.Clear();
        }

        public void AddObjectToList(ref ISelectableObject newObject)
        {
            _selectedGameObjects.Add(newObject);
            newObject.IsSelected = true;
        }

        public void RemoveObjectFromList(ref ISelectableObject oldObject)
        {
            _selectedGameObjects.Remove(oldObject);
            oldObject.IsSelected = false;
        }

        public bool ObjectIsSelected(ref ISelectableObject oldObject)
        {
            if (_selectedGameObjects.Contains(oldObject))
                return true;
            return false;
        }
    }
}