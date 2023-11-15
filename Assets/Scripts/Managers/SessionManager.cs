using System.Collections.Generic;
using UnityEngine;

using Behavior;
using Commons;

namespace Managers
{
    public class SessionManager : MonoBehaviour
    {
        private SelectionManager _selectionManager;
        private MovementManager _movementManager;
        
        
        private static bool _hasCollided;
        private RaycastHit _hit;
        private Rect _rect;

        private void Start()
        {
            _selectionManager = SelectionManager.GetInstance();
            _movementManager = MovementManager.GetInstance();
            SelectionManager.MCamera = Camera.main;
            SelectionManager.RVector2S = new Vector2[2];
            SelectionManager.TextureMat = new Texture2D(1, 1);
        }
        
        private void OnGUI()
        {
            var currentEvent = Event.current;
            switch (currentEvent.type)
            {
                case EventType.MouseDrag:
                    SelectionManager.RVector2S[1] = Input.mousePosition;
                    SelectionManager.SetIsDragging(true);
                    break;
                case EventType.MouseUp:
                    SelectionManager.SetIsDragging(false);
                    break;
            }
            
            _rect = DrawRect();
            if (!SelectionManager.IsDragging) return;
            GUI.DrawTexture(_rect, SelectionManager.TextureMat);
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
            GUI.Label(new Rect(10,10, 150, 50), $"{SelectionManager.RVector2S[0]}\n{SelectionManager.RVector2S[1]}");
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

                SelectionManager.RVector2S[0] = Input.mousePosition;
                
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
            // Read for right click
            if (!Input.GetMouseButtonDown(1)) return;
            {
                var ray = SelectionManager.MCamera.ScreenPointToRay(Input.mousePosition); 
                _hasCollided = Physics.Raycast(ray, out _hit);
                
                // Clicking the Map
                if (!_hasCollided || SelectionManager.GetAllSelected().Count == 0 ||
                    !_hit.collider.CompareTag(ETags.MapGrid.ToString())) return;
                // Set Movement manager _hit position and trigger movement. 
                MovementManager.SetHitPosition(_hit.point);
                _movementManager.TriggerMovement();
            }
        }

        private void RegisterSingleHit(ref ISelectable hit)
        {
            ReleaseHits();
            _selectionManager.AddObjectToDict(ref hit);
        }

        private void RegisterMultipleHit(ref ISelectable hit)
        {
            if (!SelectionManager.ObjectIsSelected(ref hit))
            {
                _selectionManager.AddObjectToDict(ref hit);
            }
            else
            {
                SelectionManager.RemoveObjectFromDict(ref hit);
            }
        }

        private static void ReleaseHits()
        {
            SelectionManager.FreeAllSelected();
        }

        public Dictionary<int, ISelectable> GetAllSelectedObjects()
        {
            return SelectionManager.GetAllSelected();
        }

        private static Rect DrawRect()
        {
            // Move origin from bottom left to top left
            var point1 = new Vector2(SelectionManager.RVector2S[0].x, Screen.height - SelectionManager.RVector2S[0].y);
            var point2 = new Vector2(SelectionManager.RVector2S[1].x, Screen.height - SelectionManager.RVector2S[1].y);
            // Calculate corners
            var topLeft = Vector3.Min( point1, point2 );
            var bottomRight = Vector3.Max( point1, point2 );
            // Create Rect
            return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
        }

        private static void SetSelectionPerimeter()
        {
            // Make Selection by setting viewport
            SelectionManager.SetSelectionBound(GetSelectionBounds(SelectionManager.RVector2S[0], SelectionManager.RVector2S[1]));
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
    }
}
