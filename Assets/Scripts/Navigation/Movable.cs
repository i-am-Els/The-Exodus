using UnityEngine;

namespace Navigation
{
    public interface IMovable
    {
        public void MoveToTarget();
        public GameObject GetGameObject();
    }
}
