using UnityEngine;
using UnityEngine.AI;

using Behaviors;
using Items;
using Managers;


namespace Characters
{
    public class Pawn : MonoBehaviour, IMovable, ISelectable
    {
        public bool IsSelected { get; set; }
        
        private Renderer _renderer;
        private Color _color;
        private NavMeshAgent _navMeshAgent;
        private GameInstanceManager _gim;
        
        // Start is called before the first frame update
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _renderer = GetComponent<Renderer>();
            _color = _renderer.material.color;
            _gim = GameInstanceManager.GetInstance();
            _gim.GetThisMovementManager().MovementTriggered += MoveToTarget;
        }

        private void OnDisable()
        {
            _gim.GetThisMovementManager().MovementTriggered -= MoveToTarget;
        }
        
        private void Update()
        {
            // Listen to see if selector has pick this unit up while dragging
            if (!_gim.GetThisSelectionManager().IsDragging) return;
            ISelectable thisSelectable = this;
            if (_gim.GetThisSelectionManager().SelectionBounds.Contains(SelectionManager.MCamera.WorldToViewportPoint(gameObject.transform.position)))
            {
                _gim.GetThisSelectionManager().AddObjectToDict(ref thisSelectable);
                SelectTarget();
            }
            else
            {
                if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) return;
                if (!_gim.GetThisSelectionManager().ObjectIsSelected(ref thisSelectable)) return;
                _gim.GetThisSelectionManager().RemoveObjectFromDict(ref thisSelectable);
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

        public CharacterRecord GetRecord()
        {
            return new CharacterRecord("Pawn", ECharacterType.VillagerPawn);
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
            _gim.GetThisSelectionManager().AddObjectToDict(ref selectable);
        }
    }
}