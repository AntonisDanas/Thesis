using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace SideQuestGenerator.QuestHandling
{
    public class CustomReactionQuestEvent : QuestEvent
    {

        public CustomReactionQuestEvent(WorldEntity target)
        {
            Target = target;
            IsActive = false;
            IsProgressing = false;
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

        public override void TriggerEvent(WorldEntity invoker)
        {
            EntityEventBroker.SendCustomQuestEventReaction(Test);
        }

        public override bool CanProgressQuest()
        {
            return true;    // No condition to progress quest
        }

        private void Test(string t)
        {
            Debug.Log(t);
        }

        public override QuestEventDescription GetQuestEventDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}