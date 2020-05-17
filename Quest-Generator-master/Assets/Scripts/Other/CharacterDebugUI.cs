using SideQuestGenerator.GraphHandling;
using SideQuestGenerator.InteractableHandling;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SideQuestGenerator
{
    public class CharacterDebugUI : MonoBehaviour
    {
        public bool Debug;

        private InteractableCharacter m_ic;
        private List<Relationship> m_rels;

        private Dictionary<int, Transform> m_icTransf;

        void Start()
        {
            m_ic = GetComponent<InteractableCharacter>();
            m_rels = FindObjectOfType<GraphHandler>().GetAllOutgoingRelationships(m_ic.GetIndexOfGraphInstance());
            var allIC = FindObjectsOfType<InteractableCharacter>();
            m_icTransf = new Dictionary<int, Transform>();
            foreach (var rel in m_rels)
            {
                foreach (var ic in allIC)
                {
                    if (rel.DestinationNodeIndex == ic.GetIndexOfGraphInstance() &&
                        !m_icTransf.ContainsKey(rel.DestinationNodeIndex))
                    {
                        m_icTransf.Add(ic.GetIndexOfGraphInstance(), ic.transform);
                    }
                }
            }
        }

        private void Update()
        {
            m_ic = GetComponent<InteractableCharacter>();
            m_rels = FindObjectOfType<GraphHandler>().GetAllOutgoingRelationships(m_ic.GetIndexOfGraphInstance());
            var allIC = FindObjectsOfType<InteractableCharacter>();
            m_icTransf = new Dictionary<int, Transform>();
            foreach (var rel in m_rels)
            {
                foreach (var ic in allIC)
                {
                    if (rel.DestinationNodeIndex == ic.GetIndexOfGraphInstance() &&
                        !m_icTransf.ContainsKey(rel.DestinationNodeIndex))
                    {
                        m_icTransf.Add(ic.GetIndexOfGraphInstance(), ic.transform);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Debug && m_ic.CharacterName != null && m_ic.CharacterName != "")
            {
                Handles.BeginGUI();

                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                Handles.Label(transform.position + Vector3.up * 1.5f, m_ic.CharacterName, style);

                List<Relationship> rels = m_ic.OutgoingRelationships;
                Dictionary<int, int> accessedRels = new Dictionary<int, int>();
                foreach (var re in rels)
                {
                    if (!m_icTransf.ContainsKey(re.DestinationNodeIndex))
                        continue;

                    if (!accessedRels.ContainsKey(re.DestinationNodeIndex))
                        accessedRels.Add(re.DestinationNodeIndex, 0);

                    var endPos = m_icTransf[re.DestinationNodeIndex].position;
                    Handles.DrawLine(transform.position, endPos);

                    Vector3 labelPos = new Vector3((endPos.x - transform.position.x) / 3 + transform.position.x + 0.05f * accessedRels[re.DestinationNodeIndex],
                                            (endPos.y - transform.position.y) / 3 + transform.position.y + 0.05f * accessedRels[re.DestinationNodeIndex],
                                            (endPos.z - transform.position.z) / 3 + transform.position.z + 0.05f * accessedRels[re.DestinationNodeIndex]);

                    accessedRels[re.DestinationNodeIndex] += 1;

                    string labelWithArrow = re.Relation;
                    if (transform.position.x <= endPos.x)
                        labelWithArrow = labelWithArrow + '\u25BA';
                    else
                        labelWithArrow = '\u25C4' + labelWithArrow;

                    Handles.Label(labelPos, labelWithArrow);
                }

                Handles.EndGUI();
            }
        }
    }
}