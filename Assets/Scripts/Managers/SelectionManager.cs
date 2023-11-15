using System.Collections.Generic;
using Behavior;
using UnityEngine;

namespace Managers
{
    
    public sealed class SelectionManager
    {
        public static bool IsDragging { get; private set; }
        
        public static Bounds SelectionBounds;
        public static Camera MCamera;
        public static Vector2[] RVector2S;
        public static Texture TextureMat;
        
        private static readonly Dictionary<int, ISelectable> SelectedGameObjects = new ();
        private static readonly SelectionManager Instance = new ();
        private SelectionManager()
        {
        }

        public static SelectionManager GetInstance()
        {
            return Instance;
        }
        
        public static Dictionary<int, ISelectable> GetAllSelected()
        {
            return SelectedGameObjects;
        }

        public static void FreeAllSelected()
        {
            foreach (var item in SelectedGameObjects.Values)
            {
                item.DeselectTarget();
            }
            SelectedGameObjects.Clear();
        }

        public void AddObjectToDict(ref ISelectable selectable)
        {
            var id = selectable.GetGameObject().GetInstanceID();
            if (SelectedGameObjects.ContainsKey(id)) return;
            SelectedGameObjects.Add(id, selectable);
            selectable.SelectTarget();
        }

        public static void RemoveObjectFromDict(ref ISelectable selectable)
        {
            SelectedGameObjects.Remove(selectable.GetGameObject().GetInstanceID());
            selectable.DeselectTarget();
        }

        public static bool ObjectIsSelected(ref ISelectable selectable)
        {
            return SelectedGameObjects.ContainsKey(selectable.GetGameObject().GetInstanceID());
        }

        public static void SetIsDragging(bool isDragging)
        {
            IsDragging = isDragging;
        }

        public static void SetSelectionBound(Bounds bounds)
        {
            SelectionBounds = bounds;
        }
    }
}