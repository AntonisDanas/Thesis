using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;

namespace SideQuestGenerator.QuestHandling
{
    public class CompleteQuestEvent : QuestEvent
    {
        public CompleteQuestEvent(WorldEntity target)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
        }

        public CompleteQuestEvent(WorldEntity target, QuestEventDescription description)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
            Description = description;
        }

        public override void TriggerEvent(WorldEntity invoker)
        {
            if (!IsActive) return;
            if (Quest == null) return;

            EntityEventBroker.QuestCompleted(Quest);
        }

        public override void SetActive(Quest quest)
        {
            IsActive = true;
            Quest = quest;
            (Target as InteractableCharacter).ActivateQuestMark();
        }

        public override void SetInactive()
        {
            IsActive = false;
            (Target as InteractableCharacter).DeactivateQuestMark();
        }

        public override bool CanProgressQuest()
        {
            return true;    // No condition to progress quest
        }

        public override QuestEventDescription GetQuestEventDescription()
        {
            var des = new QuestEventDescription();
            des.ButtonLabel = "Complete Quest!";
            des.DescriptionLabel = "Great! Here's your reward!";

            return des;
        }
    }
}