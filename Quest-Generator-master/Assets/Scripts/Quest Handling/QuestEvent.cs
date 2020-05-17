using SideQuestGenerator.InteractableHandling;

namespace SideQuestGenerator.QuestHandling
{
    public abstract class QuestEvent
    {
        public QuestEventDescription Description { get; protected set; }
        public bool IsProgressing { get; protected set; }
        public Quest Quest { get; protected set; }
        public WorldEntity Target { get; protected set; }
        public bool IsActive { get; protected set; }
        public abstract void TriggerEvent(WorldEntity invoker);
        public abstract void SetActive(Quest quest);
        public abstract void SetInactive();
        public abstract bool CanProgressQuest();
        public abstract QuestEventDescription GetQuestEventDescription();
    }
}