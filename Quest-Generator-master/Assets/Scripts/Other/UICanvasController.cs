using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;
using UnityEngine;
using UnityEngine.UI;

namespace SideQuestGenerator
{
    public class UICanvasController : MonoBehaviour
    {
        public GameObject QuestBoard;
        public GameObject StartQuestButton;
        public Text QuestBoardCharacterName;
        public Text QuestDescription;

        private WorldEntity m_invoker;
        private InteractableCharacter m_selectedCharacter;

        // Start is called before the first frame update
        void Start()
        {
            QuestBoard.SetActive(false);
            StartQuestButton.SetActive(false);
            EntityEventBroker.OnCharacterInteract += CharacterInteracted;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CloseWindow()
        {
            QuestBoard.SetActive(false);
            m_invoker = null;
            m_selectedCharacter = null;
        }

        public void TriggerEvent()
        {
            QuestBoard.SetActive(false);

            m_selectedCharacter.TriggerQuestEvent(m_invoker);

            m_invoker = null;
            m_selectedCharacter = null;
        }

        private void CharacterInteracted(WorldEntity invoker, InteractableCharacter character)
        {
            Debug.Log("Interacted");
            QuestBoard.SetActive(true);
            QuestBoardCharacterName.text = character.CharacterName;
            m_invoker = invoker;
            m_selectedCharacter = character;

            if (!character.HasQuestEvent())
            {
                QuestDescription.text = "No Quest Available...";
                StartQuestButton.SetActive(false);
            }
            else
            {
                var des = m_selectedCharacter.GetQuestEventDescription();

                QuestDescription.text = des.DescriptionLabel;
                StartQuestButton.SetActive(true);
                StartQuestButton.GetComponentInChildren<Text>().text = des.ButtonLabel;
            }
        }
    }

    public class QuestEventDescription
    {
        public string DescriptionLabel;
        public string ButtonLabel;
    }
}

