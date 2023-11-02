using UnityEngine;
using UnityEngine.AI;

using Behavior;
using Managers;
using Navigation;

namespace Characters
{
    public class Pawn : MonoBehaviour, IMovable, ISelectableObject
    {
        public bool IsSelected { get; set; }
        
        private NavMeshAgent _navMeshAgent;
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            MovementManager.Initialize().MovementTriggered += MoveToTarget;
        }

        private void OnDisable()
        {
            MovementManager.Initialize().MovementTriggered -= MoveToTarget;
        }

        public virtual void MoveToTarget()
        {
            if(IsSelected)
                _navMeshAgent.destination = MovementManager.GetHitPosition();
        }

        public Record GetRecord()
        {
            return new Record();
        }

        public void SelectTarget()
        {
            IsSelected = true;
        }

        public void DeselectTarget()
        {
            IsSelected = false;
        }

        public void HighlightInScene()
        {
            
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}