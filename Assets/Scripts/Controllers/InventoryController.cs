using System.Collections.Generic;
using Behaviors;
using Managers;
using UnityEngine;

namespace Controllers
{
    
    /*
     * User selects one or more ISelectables,
     * Selection Controller Notifies the Inventory Controller about the selection,
     * Inventory Controller takes the Dictionary of Selected Items and Sorts them base on the Criteria Below In a Tab-like Manner:
     *      - Movable i.e inheriting from both IMovable & ISelectable:
     *          - Civilian (1 tab).
     *          - Military (1 or more tabs - further sorted by what type of Post/Rank/Type of Soldier they are). 
     *      - Immovable i.e inheriting only from ISelectable:
     *          - Only the last ISelectable item's record will be fetched.
     * The UI is rendered with the records fetched from the ISelectable Records:
     *      - The last ISelectable Item in the Dictionary will be displayed for either category.
     */
    
    public class InventoryController : MonoBehaviour
    {
        private static Dictionary<int, ISelectable> _selected = new();

        public void InitializeGlobalInventory()
        {
            // This function should go into a Non-Component Class { MANAGER CLASS }
        }

        public void UpdateInventorySelection() // updates global inventory
        {
            SelectionManager selectionManager = GameInstanceManager.GetInstance().GetThisSelectionManager();
            if (selectionManager.GetAllSelected().Count != 0)
            {
                _selected = selectionManager.GetAllSelected();
                SortISelectableType();
                DisplayRecordOnInventoryPanel();
            }
            else
            {
                _selected.Clear();
            }
        }

        private void DisplayRecordOnInventoryPanel()
        {
            
        }

        private void SortISelectableType()
        {
            
        }
    }
}