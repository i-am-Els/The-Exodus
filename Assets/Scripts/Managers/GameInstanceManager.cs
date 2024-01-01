using Controllers;
using UnityEngine;

namespace Managers
{
    public class GameInstanceManager : MonoBehaviour
    {
        private SelectionController _selectionController;
        private InventoryController _inventoryController;
        private static GameObject _gameObj;

        private void Awake()
        {
            _selectionController = gameObject.AddComponent<SelectionController>();
            _inventoryController = gameObject.AddComponent<InventoryController>();
            _gameObj = gameObject;
        }

        public static GameInstanceManager GetInstance()
        {
            
            return _gameObj.GetComponent<GameInstanceManager>();
        }

        public SelectionManager GetThisSelectionManager()
        {
            return _selectionController.GetSelectionManager();
        }
        
        public MovementManager GetThisMovementManager()
        {
            return _selectionController.GetMovementManager();
        }
    }
}