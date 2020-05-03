using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public string CharacterName { get { return m_characterName; } }

    public List<Relationship> OutgoingRelationships;

    [SerializeField] private SG_SpaceNode m_graphInstance;
    [SerializeField] private string m_characterName;
    [SerializeField] private GameObject m_questMarkPlaceholder;

    private QuestEvent m_currentQuestEvent;

    protected override void Start()
    {
        base.Start();

        if (m_graphInstance == null)
        {
            Debug.Log("No Graph Instance for Character");
            return;
        }

        m_characterName = m_graphInstance.NodeName;
        OutgoingRelationships = FindObjectOfType<GraphHandler>().GetAllOutgoingRelationships(m_graphInstance.Index);
        EntityEventBroker.OnCharactersStatusUpdate += UpdateCharacterStatus;
    }

    public void UpdateCharacterStatus(Dictionary<int, CharacterStatus> changedCharacters)
    {
        if (!changedCharacters.ContainsKey(m_graphInstance.Index))
            return;

        m_graphInstance.Labels[0] = changedCharacters[m_graphInstance.Index].Label;
        m_graphInstance.Attributes = changedCharacters[m_graphInstance.Index].Attributes;
        OutgoingRelationships = changedCharacters[m_graphInstance.Index].OutgoingRelationships;
    }

    public int GetIndexOfGraphInstance()
    {
        return m_graphInstance.Index;
    }

    public bool HasQuestEvent()
    {
        if (m_currentQuestEvent == null)
            return false;

        return true;
    }

    public QuestEventDescription GetQuestEventDescription()
    {
        return m_currentQuestEvent.GetQuestEventDescription();
    }

    public void AddQuestEvent(QuestEvent qe)
    {
        m_currentQuestEvent = qe;
    }

    public void TriggerQuestEvent(WorldEntity invoker)
    {
        if (m_currentQuestEvent == null || !m_currentQuestEvent.IsActive)
            return;

        m_currentQuestEvent.TriggerEvent(invoker);
    }

    public void ActivateQuestMark()
    {
        m_questMarkPlaceholder.SetActive(true);
    }

    public void DeactivateQuestMark()
    {
        m_questMarkPlaceholder.SetActive(false);
    }

    public override void Interact(WorldEntity invoker)
    {
        EntityEventBroker.InteractWithCharacter(invoker, this);
    }

    public void KillCharacter(WorldEntity attacker)
    {
        EntityEventBroker.OnCharactersStatusUpdate -= UpdateCharacterStatus;
        EntityEventBroker.EntityDeath(attacker, this);
    }
}
