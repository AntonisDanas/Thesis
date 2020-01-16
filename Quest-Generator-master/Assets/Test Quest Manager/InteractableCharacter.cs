using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public string CharacterName { get { return m_characterName; } }


    [SerializeField] private string m_characterName;
    [SerializeField] private GameObject m_questMarkPlaceholder;

    private InvokeQuestEvent m_invokeQuestRef;
    private CompleteQuestEvent m_completeQuestRef;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        EntityEventBroker.EntityDeath(attacker, this);
    }
}
