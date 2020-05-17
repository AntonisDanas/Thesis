using SideQuestGenerator.InteractableHandling;

namespace SideQuestGenerator.QuestHandling
{
    public class AssassinationQuestEvent : QuestEvent
    {

        public AssassinationQuestEvent(WorldEntity target)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
        }

        public AssassinationQuestEvent(WorldEntity target, QuestEventDescription description)
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
            (Target as InteractableCharacter).ActivateQuestMark();
        }

        public override void TriggerEvent(WorldEntity invoker)
        {
            // The kill is handled by the EntityEventBroker
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
            des.ButtonLabel = "Kill"; // no button required
            des.DescriptionLabel = "Kill " + (Target as InteractableCharacter).CharacterName + "?";

            return des;
        }
    }
}