﻿using SideQuestGenerator.GraphEditor;
using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.RuleHandling;
using SideQuestGenerator.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator.GraphHandling
{
    public class GraphHandler : MonoBehaviour
    {
        public SG_Graph SpaceGraph;

        private Graph m_graph;

        // Start is called before the first frame update
        void Awake()
        {
            InitGraph();
            SubscribeToEvents();
        }

        private void InitGraph()
        {
            if (SpaceGraph == null) return;

            if (SpaceGraph.nodes == null || SpaceGraph.nodes.Count == 0) return;

            List<SG_NodeBase> snodes = SpaceGraph.nodes;
            List<SG_Edge> sedges = SpaceGraph.edges;

            m_graph = new Graph();

            foreach (var node in snodes)
            {
                Vertex v = new Vertex();
                v.SetIndex(node.Index);
                v.SetLabel(node.Label);
                v.SetAttribute("Name", node.nodeName);
                m_graph.AddVertex(v);
            }

            foreach (var edge in sedges)
            {
                m_graph.SetRelation(edge.StartNode.Index,
                                      edge.EndNode.Index,
                                      edge.Label);
            }
        }

        private void SubscribeToEvents()
        {
            EntityEventBroker.OnEntityDeath += EntityKilled;
            EntityEventBroker.OnObjectPickUpSuccess += ObjectPickedUp;
            EntityEventBroker.OnObjectTransfer += ObjectTransfer;
            EntityEventBroker.OnCustomQuestEventReactionSent += CustomQuestEventReactionSent;
        }


        private void CustomQuestEventReactionSent(Action<string> customReaction)
        {
            customReaction("Custom Reaction ran by Graph Handler");
        }

        public List<Graph> SearchForPattern(Rule rule)
        {
            return rule.ImplementRule(m_graph);
        }

        public void SetNewIncomingRelationships(int target, List<int> sources, Relationship newRel)
        {
            List<Vertex> sv = new List<Vertex>();
            Dictionary<int, CharacterStatus> result = new Dictionary<int, CharacterStatus>();

            foreach (int sourceIndex in sources)
            {
                m_graph.SetRelation(sourceIndex, target, newRel.Relation, newRel.Reason);

                var vertex = m_graph.GetVertex(sourceIndex);
                var status = new CharacterStatus();

                status.Label = vertex.GetLabel();
                status.Attributes = vertex.GetAttributes();
                status.OutgoingRelationships = GetAllOutgoingRelationships(sourceIndex);

                result.Add(sourceIndex, status);
            }

            EntityEventBroker.CharacterStatusChanged(result);
            return;
        }

        public void SetIncomingRelationshipsWithCondition(int target, List<int> sources, Relationship newRel, Relationship condition, bool replace = true)
        {
            List<Vertex> sv = new List<Vertex>();
            Dictionary<int, CharacterStatus> result = new Dictionary<int, CharacterStatus>();

            List<int> newSources = new List<int>();
            Vertex tv = m_graph.GetVertex(target);

            if (condition.Reason == null || condition.Reason == "")
                sv = m_graph.GetIncomingVerticesByRelationLabel(tv, condition.Relation);
            else
                sv = m_graph.GetIncomingVerticesByRelationReason(tv, condition.Relation, condition.Reason);

            foreach (var source in sv)
            {
                int sourceIndex = source.GetIndex();
                Edge edge;

                if (condition.Reason == null || condition.Reason == "")
                    edge = new Edge(sourceIndex, target, condition.Relation);
                else
                    edge = new Edge(sourceIndex, target, condition.Relation, condition.Reason);

                if (replace)
                    m_graph.DeleteRelation(edge);

                m_graph.SetRelation(sourceIndex, target, newRel.Relation, newRel.Reason);

                var vertex = m_graph.GetVertex(sourceIndex);
                var status = new CharacterStatus();

                status.Label = vertex.GetLabel();
                status.Attributes = vertex.GetAttributes();
                status.OutgoingRelationships = GetAllOutgoingRelationships(sourceIndex);

                result.Add(sourceIndex, status);
            }

            EntityEventBroker.CharacterStatusChanged(result);
            return;
        }

        public void SetOutgoingRelationship(Relationship newRel, Relationship oldRelToReplace = null)
        {
            if (oldRelToReplace != null)
            {

                m_graph.DeleteRelation(new Edge(oldRelToReplace.SourceNodeIndex,
                                                oldRelToReplace.DestinationNodeIndex,
                                                oldRelToReplace.Relation,
                                                oldRelToReplace.Reason == "" || oldRelToReplace.Reason == null ? "" : oldRelToReplace.Reason));
            }

            m_graph.SetRelation(newRel.SourceNodeIndex,
                                                newRel.DestinationNodeIndex,
                                                newRel.Relation,
                                                newRel.Reason == "" || newRel.Reason == null ? "" : newRel.Reason);
        }

        public List<Relationship> GetAllOutgoingRelationships(int vertexIndex)
        {
            List<Relationship> rels = new List<Relationship>();
            List<Edge> edges = m_graph.GetOutgoingEdges(m_graph.GetVertex(vertexIndex));

            foreach (var edge in edges)
            {
                var rel = new Relationship();
                rel.SourceNodeIndex = Convert.ToInt32(edge.GetEdgeIndex().Split('-')[0]);
                rel.DestinationNodeIndex = Convert.ToInt32(edge.GetEdgeIndex().Split('-')[1]);
                rel.Relation = edge.GetRelationLabel();
                rel.Reason = edge.GetReasonLabel();

                rels.Add(rel);
            }

            return rels;
        }

        public List<Relationship> GetAllIncomingRelationships(int vertexIndex)
        {
            List<Relationship> rels = new List<Relationship>();
            List<Edge> edges = m_graph.GetIncomingEdges(m_graph.GetVertex(vertexIndex));

            foreach (var edge in edges)
            {
                var rel = new Relationship();
                rel.SourceNodeIndex = Convert.ToInt32(edge.GetEdgeIndex().Split('-')[0]);
                rel.DestinationNodeIndex = Convert.ToInt32(edge.GetEdgeIndex().Split('-')[1]);
                rel.Relation = edge.GetRelationLabel();
                rel.Reason = edge.GetReasonLabel();

                rels.Add(rel);
            }

            return rels;
        }

        public string GetRelationshipLabelBetweenNodes(int src, int dst)
        {
            var edges = m_graph.GetOutgoingEdges(m_graph.GetVertex(src));

            foreach (var edge in edges)
            {
                int dstIndex = Convert.ToInt32(edge.GetEdgeIndex().Split('-')[1]);

                if (dstIndex != dst)
                    continue;

                return edge.GetRelationLabel();
            }

            return "";
        }

        public int GetPlayerIndex()
        {
            return m_graph.GetPlayerVertex() != null ? m_graph.GetPlayerVertex().GetIndex() : -1;
        }

        public float GetRuleCost(Rule r)
        {
            return r.GetAverageCost(m_graph);
        }

        private void ObjectPickedUp(WorldEntity entity, InteractableObject obj)
        {
            if (!IsObjectOwned(obj))
                return;

            Debug.Log(obj.ObjectName + " is stolen by Player");

            Dictionary<int, CharacterStatus> result = new Dictionary<int, CharacterStatus>();
            CharacterStatus playerStatus = new CharacterStatus();
            CharacterStatus ownerStatus = new CharacterStatus();

            int objIndex = obj.GetIndexOfGraphInstance();
            int playerIndex = m_graph.GetPlayerVertex().GetIndex();
            int objOwnerIndex = m_graph.GetIncomingVerticesByRelationLabel(m_graph.GetVertex(objIndex), "Owns")[0].GetIndex();

            Vertex player = m_graph.GetPlayerVertex();
            Vertex owner = m_graph.GetVertex(objOwnerIndex);

            m_graph.DeleteRelation(objOwnerIndex + "-" + objIndex, "Owns");
            m_graph.DeleteRelation(objOwnerIndex + "-" + playerIndex);
            m_graph.SetRelation(playerIndex, objIndex, "Owns", "Stolen");
            m_graph.SetRelation(objOwnerIndex, playerIndex, "Dislikes", "Stolen " + obj.ObjectName);

            playerStatus.Label = player.GetLabel();
            playerStatus.Attributes = player.GetAttributes();
            playerStatus.OutgoingRelationships = GetAllOutgoingRelationships(playerIndex);

            ownerStatus.Label = owner.GetLabel();
            ownerStatus.Attributes = owner.GetAttributes();
            ownerStatus.OutgoingRelationships = GetAllOutgoingRelationships(objOwnerIndex);

            result.Add(playerIndex, playerStatus);
            result.Add(objOwnerIndex, ownerStatus);

            EntityEventBroker.CharacterStatusChanged(result);
        }

        private void ObjectTransfer(InteractableCharacter target, InteractableObject obj)
        {
            Debug.Log(obj.ObjectName + " is transfered");

            Dictionary<int, CharacterStatus> result = new Dictionary<int, CharacterStatus>();
            CharacterStatus newOwnerStatus = new CharacterStatus();

            int objIndex = obj.GetIndexOfGraphInstance();
            int targetIndex = target.GetIndexOfGraphInstance();
            int playerIndex = m_graph.GetIncomingVerticesByRelationLabel(m_graph.GetVertex(objIndex), "Owns")[0].GetIndex();

            Vertex newOwner = m_graph.GetVertex(targetIndex);

            m_graph.DeleteRelation(playerIndex + "-" + objIndex, "Owns");
            m_graph.SetRelation(targetIndex, objIndex, "Owns");

            newOwnerStatus.Label = newOwner.GetLabel();
            newOwnerStatus.Attributes = newOwner.GetAttributes();
            newOwnerStatus.OutgoingRelationships = GetAllOutgoingRelationships(targetIndex);

            result.Add(targetIndex, newOwnerStatus);

            EntityEventBroker.CharacterStatusChanged(result);
        }

        private bool IsObjectOwned(InteractableObject obj)
        {
            int index = obj.GetIndexOfGraphInstance();

            var result = m_graph.GetIncomingVerticesByRelationLabel(m_graph.GetVertex(index), "Owns");

            if (result.Count > 0)
                return true;

            return false;
        }

        private void EntityKilled(WorldEntity invoker, WorldEntity receiver)
        {
            Debug.Log("Graph Handler: Entity Killed");

            if (!(invoker is InteractableCharacter) || !(receiver is InteractableCharacter))
                return;

            InteractableCharacter inv = invoker as InteractableCharacter;
            InteractableCharacter rec = receiver as InteractableCharacter;

            Debug.Log(inv.CharacterName + "    " + rec.CharacterName);

            List<Relationship> allIncomingRelationshipsOfTarget = GetAllIncomingRelationships(rec.GetIndexOfGraphInstance());
            List<int> charsWhoLoveTarget = new List<int>();
            List<int> charsWhoLikeTarget = new List<int>();
            List<int> charsWhoDislikeTarget = new List<int>();
            List<int> charsWhoHateTarget = new List<int>();

            Dictionary<int, CharacterStatus> changedStatuses = new Dictionary<int, CharacterStatus>();

            foreach (var rel in allIncomingRelationshipsOfTarget)
            {
                if (rel.Relation == "Loves")
                    charsWhoLoveTarget.Add(rel.SourceNodeIndex);
                else if (rel.Relation == "Likes")
                    charsWhoLikeTarget.Add(rel.SourceNodeIndex);
                else if (rel.Relation == "Dislikes")
                    charsWhoDislikeTarget.Add(rel.SourceNodeIndex);
                else if (rel.Relation == "Hates")
                    charsWhoHateTarget.Add(rel.SourceNodeIndex);
            }

            foreach (var c in charsWhoLoveTarget)
            {
                Relationship oldRel = new Relationship();
                Relationship newRel = new Relationship();
                newRel.SourceNodeIndex = oldRel.SourceNodeIndex = c;
                newRel.DestinationNodeIndex = oldRel.DestinationNodeIndex = inv.GetIndexOfGraphInstance();
                newRel.Relation = "Hates";
                newRel.Reason = "Killed " + rec.CharacterName;

                oldRel.Relation = GetRelationshipLabelBetweenNodes(c, inv.GetIndexOfGraphInstance());

                if (oldRel.Relation == "") // no relationship between the two
                    SetOutgoingRelationship(newRel);
                else
                    SetOutgoingRelationship(newRel, oldRel);

                CharacterStatus newStatus = new CharacterStatus();
                newStatus.OutgoingRelationships = GetAllOutgoingRelationships(c);
                changedStatuses.Add(c, newStatus);
            }

            foreach (var c in charsWhoLikeTarget)
            {
                Relationship oldRel = new Relationship();
                Relationship newRel = new Relationship();
                newRel.SourceNodeIndex = oldRel.SourceNodeIndex = c;
                newRel.DestinationNodeIndex = oldRel.DestinationNodeIndex = inv.GetIndexOfGraphInstance();
                newRel.Relation = "Dislikes";
                newRel.Reason = "Killed " + rec.CharacterName;

                oldRel.Relation = GetRelationshipLabelBetweenNodes(c, inv.GetIndexOfGraphInstance());

                if (oldRel.Relation == "") // no relationship between the two
                    SetOutgoingRelationship(newRel);
                else
                    SetOutgoingRelationship(newRel, oldRel);

                CharacterStatus newStatus = new CharacterStatus();
                newStatus.OutgoingRelationships = GetAllOutgoingRelationships(c);
                changedStatuses.Add(c, newStatus);
            }

            foreach (var c in charsWhoDislikeTarget)
            {
                Relationship oldRel = new Relationship();
                Relationship newRel = new Relationship();
                newRel.SourceNodeIndex = oldRel.SourceNodeIndex = c;
                newRel.DestinationNodeIndex = oldRel.DestinationNodeIndex = inv.GetIndexOfGraphInstance();
                newRel.Relation = "Likes";
                newRel.Reason = "Killed " + rec.CharacterName;

                oldRel.Relation = GetRelationshipLabelBetweenNodes(c, inv.GetIndexOfGraphInstance());

                if (oldRel.Relation == "") // no relationship between the two
                    SetOutgoingRelationship(newRel);
                else
                    SetOutgoingRelationship(newRel, oldRel);

                CharacterStatus newStatus = new CharacterStatus();
                newStatus.OutgoingRelationships = GetAllOutgoingRelationships(c);
                changedStatuses.Add(c, newStatus);
            }

            foreach (var c in charsWhoHateTarget)
            {
                Relationship oldRel = new Relationship();
                Relationship newRel = new Relationship();
                newRel.SourceNodeIndex = oldRel.SourceNodeIndex = c;
                newRel.DestinationNodeIndex = oldRel.DestinationNodeIndex = inv.GetIndexOfGraphInstance();
                newRel.Relation = "Loves";
                newRel.Reason = "Killed " + rec.CharacterName;

                oldRel.Relation = GetRelationshipLabelBetweenNodes(c, inv.GetIndexOfGraphInstance());

                if (oldRel.Relation == "") // no relationship between the two
                    SetOutgoingRelationship(newRel);
                else
                    SetOutgoingRelationship(newRel, oldRel);

                CharacterStatus newStatus = new CharacterStatus();
                newStatus.OutgoingRelationships = GetAllOutgoingRelationships(c);
                changedStatuses.Add(c, newStatus);
            }

            // Delete all relationships of dead entity
            m_graph.DeleteAllRelationsOfVertex(rec.GetIndexOfGraphInstance());
            CharacterStatus receiverStatus = new CharacterStatus();
            receiverStatus.OutgoingRelationships = GetAllOutgoingRelationships(rec.GetIndexOfGraphInstance());
            changedStatuses.Add(rec.GetIndexOfGraphInstance(), receiverStatus);

            // Update new relationships to Interactable characters
            EntityEventBroker.CharacterStatusChanged(changedStatuses);
        }

    }

    public class Relationship
    {
        public int SourceNodeIndex;
        public int DestinationNodeIndex;
        public string Relation;
        public string Reason;
    }

    public class CharacterStatus
    {
        public string Label;
        public StringObjectDictionary Attributes = new StringObjectDictionary();
        public List<Relationship> OutgoingRelationships = new List<Relationship>();
    }
}