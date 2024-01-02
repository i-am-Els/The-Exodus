using Items;
using UnityEngine;

namespace Behaviors
{
    public interface ISelectable
    {
        public bool IsSelected { get; set; }
        public CharacterRecord GetRecord();

        public void SelectTarget();
        public void DeselectTarget();

        public void HighlightInScene();

        public GameObject GetGameObject();

        public void RegisterSelfHit(ref ISelectable selectable);

    }
}
