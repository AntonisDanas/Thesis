using SideQuestGenerator.InteractableHandling;

namespace SideQuestGenerator.QuestHandling
{
    public class StealQuestEvent : QuestEvent
    {
        public StealQuestEvent(InteractableObject target)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
        }

        public StealQuestEvent(InteractableObject target, QuestEventDescription description)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
            Description = description;
        }

        public override void SetActive(Quest quest)
        {
            IsActive = true;
            Quest = quest;
            //(Target as InteractableCharacter).ActivateQuestMark();
        }

        public override void TriggerEvent(WorldEntity invoker)
        {
            //The stealing is handled by the EntityEventBroker
        }

        public override void SetInactive()
        {
            IsActive = false;
            //(Target as InteractableCharacter).DeactivateQuestMark();
        }

        public override bool CanProgressQuest()
        {
            return true;    // No condition to progress quest
        }

        public override QuestEventDescription GetQuestEventDescription()
        {
            var des = new QuestEventDescription();
            des.ButtonLabel = ""; // no button required
            des.DescriptionLabel = "Steal " + (Target as InteractableObject).ObjectName;

            return des;
        }
    }
}