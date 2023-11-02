using System.Collections.Generic;
using Behavior;

namespace Managers
{
    public class SelectionManager
    {
        public static bool IsMoving { get; set; }
        private static SelectionManager _instance;
        private List<ISelectableObject> _selectedGameObjects;
        private SelectionManager()
        {
            IsMoving = false;
        }
        
        public static SelectionManager Initialize()
        {
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }
            
            return _instance;
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