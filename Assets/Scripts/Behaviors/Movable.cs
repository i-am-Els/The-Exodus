using UnityEngine;

namespace Behaviors
{
    public interface IMovable
    {
        public void MoveToTarget();
        public GameObject GetGameObject();
    }
}
