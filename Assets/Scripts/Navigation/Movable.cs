using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    public interface IMovable
    {
        public void MoveToTarget(RaycastHit hit);
        public GameObject GetGameObject();
    }
}
