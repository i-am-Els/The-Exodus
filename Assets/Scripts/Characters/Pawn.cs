using UnityEngine;
using UnityEngine.AI;

using Behavior;
using Navigation;

namespace Characters
{
    public class Pawn : MonoBehaviour, IMovable, ISelectableObject
    {
        private NavMeshAgent _navMeshAgent;
        private Collider _collider; 
        public bool IsSelected { get; set; }
        
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            
        }
        
        public void MoveToTarget( RaycastHit hit)
        {
            _navMeshAgent.destination = hit.point;
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