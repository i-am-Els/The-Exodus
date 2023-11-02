using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    public interface IMovable
    {
        public void MoveToTarget();
        public GameObject GetGameObject();
    }
}
