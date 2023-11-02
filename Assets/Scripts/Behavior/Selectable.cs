using UnityEngine;

namespace Behavior
{

    public record Record
    {
        
    }
    
    public interface ISelectableObject
    { 
        public bool IsSelected { get; set; }
        public Record GetRecord();

        public void SelectTarget();
        public void DeselectTarget();

        public void HighlightInScene();

        public GameObject GetGameObject();
    }
}
