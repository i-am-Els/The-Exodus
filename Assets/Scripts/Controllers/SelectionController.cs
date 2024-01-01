using System.Collections.Generic;
using UnityEngine;

using Behaviors;
using Commons;
using Managers;

namespace Controllers
{
    public class SelectionController : MonoBehaviour
    {
        private readonly SelectionManager _selectionManager = new ();
        private readonly MovementManager _movementManager = new ();
            
        private bool _hasCollided;
        private RaycastHit _hit;
        private Rect _rect;

        private void Start()
        {
            SelectionManager.MCamera = Camera.main;
            _selectionManager.RVector2S = new Vector2[2];
            _selectionManager.TextureMat = new Texture2D(1, 1);
        }
        
        private void OnGUI()
        {
            var currentEvent = Event.current;
            switch (currentEvent.type)
            {
                case EventType.MouseDrag:
                    _selectionManager.RVector2S[1] = Input.mousePosition;
                    _selectionManager.SetIsDragging(true);
                    break;
                case EventType.MouseUp:
                    _selectionManager.SetIsDragging(false);
                    break;
            }
            
            _rect = DrawRect();
            if (!_selectionManager.IsDragging) return;
            GUI.DrawTexture(_rect, _selectionManager.TextureMat);
            // Call Selection function
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                // Add group selection to existing Selection
                SetSelectionPerimeter();
            }
            else
            {
                SetSelectionPerimeter();
            }
            GUI.Label(new Rect(10,10, 150, 50), $"{_selectionManager.RVector2S[0]}\n{_selectionManager.RVector2S[1]}");
        }

        private void Update()
        {
            // ================================================= INPUT =================================================
            
            // Initialize a flag to track dragging.
            if (Input.GetMouseButtonDown(0))
            {
                // Navigation Manager do your trick
                // // Use ray to get point to move to
                var ray = SelectionManager.MCamera.ScreenPointToRay(Input.mousePosition); 
                _hasCollided = Physics.Raycast(ray, out _hit);

                _selectionManager.RVector2S[0] = Input.mousePosition;
                
                // Selection
                // If selecting object that is not a location on the map.
                if (_hasCollided && !_hit.collider.CompareTag(ETags.MapGrid.ToString()))
                {
                    var selectableComponent = _hit.collider.gameObject.GetComponent<ISelectable>();
                    if (selectableComponent != null)
                    { 
                        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                        { 
                            RegisterMultipleHit(ref selectableComponent);
                        }
                        else
                        { 
                            RegisterSingleHit(ref selectableComponent);
                        }
                        // Switch to change cursor based on type of Selectable 
                    }

                    // Deselect on contact with obstacle
                    if (_hit.collider.CompareTag(ETags.Obstacle.ToString()))
                    { 
                        // Ray hits obstacles and locations players cannot go to.
                        ReleaseHits();
                    }
                }
                else
                {
                    if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                    {
                        // Ray hits nothing
                        ReleaseHits();
                    }
                }
            }
            
            
            if (_hasCollided)
            {
                var localRay = SelectionManager.MCamera.ScreenPointToRay(Input.mousePosition); 
                _hasCollided = Physics.Raycast(localRay, out _hit);
                
                //  Switch to Know which cursor to display -----[PENDING]-----
                //  Check if object hovered on is a tree, etc to set which cursor to display
                
                
                // Read for right click
                if (!Input.GetMouseButtonDown(1)) return;
                {
                    // Clicking the Map
                    if (!_hasCollided || _selectionManager.GetAllSelected().Count == 0 ||
                        !_hit.collider.CompareTag(ETags.MapGrid.ToString())) return;
                    
                    // Reset Cursor Image to Normal/Standard -----[PENDING]-----
                    
                    // Set Movement manager _hit position and trigger movement. 
                    _movementManager.SetHitPosition(_hit.point);
                    _movementManager.TriggerMovement();
                }
            }
        }

        private void RegisterSingleHit(ref ISelectable hit)
        {
            ReleaseHits();
            _selectionManager.AddObjectToDict(ref hit);
        }

        private void RegisterMultipleHit(ref ISelectable hit)
        {
            if (!_selectionManager.ObjectIsSelected(ref hit))
            {
                _selectionManager.AddObjectToDict(ref hit);
            }
            else
            {
                _selectionManager.RemoveObjectFromDict(ref hit);
            }
        }

        private void ReleaseHits()
        {
            _selectionManager.FreeAllSelected();
        }

        public Dictionary<int, ISelectable> GetAllSelectedObjects()
        {
            return _selectionManager.GetAllSelected();
        }

        private Rect DrawRect()
        {
            // Move origin from bottom left to top left
            var point1 = new Vector2(_selectionManager.RVector2S[0].x, Screen.height - _selectionManager.RVector2S[0].y);
            var point2 = new Vector2(_selectionManager.RVector2S[1].x, Screen.height - _selectionManager.RVector2S[1].y);
            // Calculate corners
            var topLeft = Vector3.Min( point1, point2 );
            var bottomRight = Vector3.Max( point1, point2 );
            // Create Rect
            return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
        }

        private void SetSelectionPerimeter()
        {
            // Make Selection by setting viewport
            _selectionManager.SetSelectionBound(GetSelectionBounds(_selectionManager.RVector2S[0], _selectionManager.RVector2S[1]));
        }

        private static Bounds GetSelectionBounds(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            var v1 = SelectionManager.MCamera.ScreenToViewportPoint( screenPosition1 );
            var v2 = SelectionManager.MCamera.ScreenToViewportPoint( screenPosition2 );
            var min = Vector3.Min( v1, v2 );
            var max = Vector3.Max( v1, v2 );
            min.z = SelectionManager.MCamera.nearClipPlane;
            max.z = SelectionManager.MCamera.farClipPlane;
        
            var bounds = new Bounds();
            bounds.SetMinMax( min, max );
            return bounds;
        }

        public SelectionManager GetSelectionManager()
        {
            return _selectionManager;
        }

        public MovementManager GetMovementManager()
        {
            return _movementManager;
        }
    }
}
