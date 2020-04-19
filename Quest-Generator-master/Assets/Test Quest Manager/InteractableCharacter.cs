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

    private InvokeQuestEvent m_invokeQuestRef;
    private CompleteQuestEvent m_completeQuestRef;

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

    public bool HasQuestAvailable()
    {
        if (m_invokeQuestRef == null) return false;

        return true;
    }

    public bool CanCompleteQuest()
    {
        if (m_completeQuestRef == null || m_invokeQuestRef != null) return false;
        if (!m_completeQuestRef.IsActive) return false;

        return true;
    }

    public void ActivateQuestMark()
    {
        m_questMarkPlaceholder.SetActive(true);
    }

    public void DeactivateQuestMark()
    {
        m_questMarkPlaceholder.SetActive(false);
    }

    public void AddInvokeQuestEvent(InvokeQuestEvent invokeQuestEvent)
    {
        m_invokeQuestRef = invokeQuestEvent;
    }

    public void AddCompleteQuestEvent(CompleteQuestEvent completeQuestEvent)
    {
        m_completeQuestRef = completeQuestEvent;
    }

    public override void Interact(WorldEntity invoker)
    {
        EntityEventBroker.InteractWithCharacter(invoker, this);
    }

    public void InvokeQuest()
    {
        if (m_invokeQuestRef == null) return;

        m_invokeQuestRef.TriggerEvent(this);
        m_invokeQuestRef = null;
    }

    public void CompleteQuest()
    {
        if (m_completeQuestRef == null) return;
        m_completeQuestRef.TriggerEvent(this);
        m_completeQuestRef = null;
    }

    public void KillCharacter(WorldEntity attacker)
    {
        EntityEventBroker.OnCharactersStatusUpdate -= UpdateCharacterStatus;
        EntityEventBroker.EntityDeath(attacker, this);
    }
}
