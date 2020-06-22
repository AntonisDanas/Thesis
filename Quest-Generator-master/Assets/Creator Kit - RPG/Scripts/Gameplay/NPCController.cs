using RPGM.Core;
using RPGM.Gameplay;
using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects.
    /// </summary>
    public class NPCController : MonoBehaviour
    {
        public ConversationScript[] conversations;

        Quest activeQuest = null;

        Quest[] quests;

        GameModel model = Schedule.GetModel<GameModel>();

        void OnEnable()
        {
            quests = gameObject.GetComponentsInChildren<Quest>();
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Interact");
            InteractableCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractableCharacter>();
            InteractableCharacter npc = GetComponent<InteractableCharacter>();
            EntityEventBroker.InteractWithCharacter(player, npc);

            //var c = GetConversation();
            //if (c != null)
            //{
            //    var ev = Schedule.Add<Events.ShowConversation>();
            //    ev.conversation = c;
            //    ev.npc = this;
            //    ev.gameObject = gameObject;
            //    ev.conversationItemKey = "";
            //}
        }

        public void CompleteQuest(Quest q)
        {
            if (activeQuest != q) throw new System.Exception("Completed quest is not the active quest.");
            foreach (var i in activeQuest.requiredItems)
            {
                model.RemoveInventoryItem(i.item, i.count);
            }
            activeQuest.RewardItemsToPlayer();
            activeQuest.OnFinishQuest();
            activeQuest = null;
        }

        public void StartQuest(Quest q)
        {
            if (activeQuest != null) throw new System.Exception("Only one quest should be active.");
            activeQuest = q;
        }

        // Edited
        ConversationScript GetConversation()
        {
            InteractableCharacter ic = GetComponent<InteractableCharacter>();

            if (ic == null)
                return null;

            if (!ic.HasQuestEvent())
            {
                int r = Random.Range(0, conversations.Length);
                Debug.Log(r + "    " + conversations.Length);
                return conversations[r];
            }



            return null;

            //if (activeQuest == null)
            //    return conversations[0];
            //foreach (var q in quests)
            //{
            //    if (q == activeQuest)
            //    {
            //        if (q.IsQuestComplete())
            //        {
            //            CompleteQuest(q);
            //            return q.questCompletedConversation;
            //        }
            //        return q.questInProgressConversation;
            //    }
            //}
            //return null;
        }
    }
}