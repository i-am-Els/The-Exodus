using System.Collections.Generic;
using Behavior;
using UnityEngine;
using Commons;

namespace Managers
{
    public class SessionManager : MonoBehaviour
    {
        
        private Camera _mainCamera;
        private SelectionManager _selectionManager;
        private MovementManager _movementManager;
        private static bool _hasCollided;
        private void Awake()
        {
            _mainCamera = Camera.main;
            _selectionManager = SelectionManager.Initialize();
            _movementManager = MovementManager.Initialize();
        }

        private void Start()
        {
            
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
                _hasCollided = Physics.Raycast(ray, out hit);

                GameObject target = hit.collider.gameObject;
                // Selection
                // If selecting object that is not a location on the map.
                if (_hasCollided && !hit.collider.CompareTag(ETags.MapGrid.ToString()))
                {
                    ISelectableObject selectableComponent = target.GetComponent<ISelectableObject>();
                    
                    if (selectableComponent != null)
                    {
                        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
                        {
                            RegisterMultipleHit(ref selectableComponent);
                        }else
                        {
                            RegisterSingleHit(ref selectableComponent);
                        }
                    }
                }
                // Clicking the Map
                else if (_hasCollided && hit.collider.CompareTag(ETags.MapGrid.ToString()))
                {
                    // Movement Manager moves the Players
                    // IMovable movableComponent = target.GetComponent<IMovable>();
                    // if (movableComponent != null)
                    // {
                    //     movableComponent.MoveToTarget(hit);
                    // }
                    MovementManager.SetHitPosition(hit.point);
                    _movementManager.TriggerMovement();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReleaseHits();
            }
        }

        private void RegisterSingleHit(ref ISelectableObject hit)
        {
            _selectionManager.FreeAllSelected();
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

        public List<ISelectableObject> GetAllSelectedObjects()
        {
            return _selectionManager.GetAllSelected();
        }
        
        
    }
}