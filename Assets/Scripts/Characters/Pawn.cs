using UnityEngine;
using UnityEngine.AI;

using Behavior;
using Managers;
using Navigation;

namespace Characters
{
    public class Pawn : MonoBehaviour, IMovable, ISelectable
    {
        public bool IsSelected { get; set; }
        
        private Renderer _renderer;
        private Color _color;
        private NavMeshAgent _navMeshAgent;
        
        // Start is called before the first frame update
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _renderer = GetComponent<Renderer>();
            _color = _renderer.material.color;
        }

        private void OnEnable()
        {
            MovementManager.GetInstance().MovementTriggered += MoveToTarget;
        }

        private void OnDisable()
        {
            MovementManager.GetInstance().MovementTriggered -= MoveToTarget;
        }
        
        private void Update()
        {
            // Listen to see if selector has pick this unit up while dragging
            if (!SelectionManager.IsDragging) return;
            ISelectable thisSelectable = this;
            if (SelectionManager.SelectionBounds.Contains(SelectionManager.MCamera.WorldToViewportPoint(gameObject.transform.position)))
            {
                SelectionManager.GetInstance().AddObjectToDict(ref thisSelectable);
                SelectTarget();
            }
            else
            {
                if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) return;
                if (!SelectionManager.ObjectIsSelected(ref thisSelectable)) return;
                SelectionManager.RemoveObjectFromDict(ref thisSelectable);
                DeselectTarget();
            }
        }

        public virtual void MoveToTarget()
        {
            if (IsSelected)
            {
                _navMeshAgent.destination = MovementManager.GetHitPosition();
            }
        }

        public Record GetRecord()
        {
            return new Record();
        }

        public void SelectTarget()
        {
            IsSelected = true;
            HighlightInScene();            
        }

        public void DeselectTarget()
        {
            IsSelected = false;
            HighlightInScene();
        }

        public void HighlightInScene()
        {
            _renderer.material.color = IsSelected ? Color.green : _color;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        
        public void RegisterSelfHit(ref ISelectable selectable)
        {
            SelectionManager.GetInstance().AddObjectToDict(ref selectable);
        }
    }
}