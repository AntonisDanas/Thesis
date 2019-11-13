using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEntity : WorldEntity
{
    [SerializeField] private Transform m_markerPlaceholder;
    [SerializeField] private GameObject m_questMarkerPrefab;

    private GameObject m_questMarker;

    public void SetQuestMarker()
    {
        m_questMarker = Instantiate(m_questMarkerPrefab,
                                    m_markerPlaceholder.transform.position,
                                    Quaternion.identity);
        m_questMarker.SetActive(true);
    }

    public void DestroyQuestMarker()
    {
        Destroy(m_questMarker);
    }

    private void OnTriggerEnter(Collider other)
    {
        WorldEntity invoker = other.gameObject.GetComponent<WorldEntity>();
        if (invoker == null)
        {
            Debug.Log("Something strange killed " + gameObject.name);
        }
        else
        {
            WorldEntity recepient = gameObject.GetComponent<WorldEntity>();
            EntityEventBroker.EntityDeath(invoker, recepient);
        }
    }

    // TODO impliment death condition
    // TODO impliment interaction with player
}
