using UnityEngine;

namespace Behavior
{

    public record Record
    {
        
    }
    
    public interface ISelectable
    {
        public bool IsSelected { get; set; }
        public Record GetRecord();

        public void SelectTarget();
        public void DeselectTarget();

        public void HighlightInScene();

        public GameObject GetGameObject();

        public void RegisterSelfHit(ref ISelectable selectable);

    }
}
