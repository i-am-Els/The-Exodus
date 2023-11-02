using Behavior;
using UnityEngine;
using Commons;
using Navigation;

namespace Managers
{
    public class SessionManager : MonoBehaviour
    {
        
        private Camera _mainCamera;
        private SelectionManager _selectionManager;
        public static bool HasCollided;
        private void Awake()
        {
            _mainCamera = Camera.main;
            _selectionManager = SelectionManager.Initialize();
        }
        private void Update()
        {
            // ================================================= INPUT =================================================
            // Use ray to get point to move to
            if (Input.GetMouseButtonDown(0))
            {
                // Navigation Manager do you trick
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                HasCollided = Physics.Raycast(ray, out hit);

                GameObject target = hit.collider.gameObject;
                // Selection
                // If selecting object that is not a location on the map.
                if (HasCollided && !hit.collider.CompareTag(ETags.MapGrid.ToString()))
                {
                    ISelectableObject selectableComponent = target.GetComponent<ISelectableObject>();
                    
                    if (selectableComponent != null)
                    {
                        if(Input.GetKeyDown(KeyCode.LeftControl))
                        {
                            RegisterMultipleHit(ref selectableComponent);
                        }else
                        {
                            RegisterSingleHit(ref selectableComponent);
                        }
                    }
                }
                // Clicking the Map
                else if (hit.collider.CompareTag(ETags.MapGrid.ToString()))
                {
                    // Movement Manager moves the Players
                    // IMovable movableComponent = target.GetComponent<IMovable>();
                    // if (movableComponent != null)
                    // {
                    //     movableComponent.MoveToTarget(hit);
                    // }
                    
                }
            }
        }

        private void RegisterSingleHit(ref ISelectableObject hit)
        {
            ReleaseHits();
            _selectionManager.AddObjectToList(ref hit);
        }

        private void RegisterMultipleHit(ref ISelectableObject hit)
        {
            if (!_selectionManager.ObjectIsSelected(ref hit))
            {
                _selectionManager.AddObjectToList(ref hit);
            }
            else
            {
                _selectionManager.RemoveObjectFromList(ref hit);
            }
        }

        private void ReleaseHits()
        {
            _selectionManager.FreeAllSelected();
        }
    }
}