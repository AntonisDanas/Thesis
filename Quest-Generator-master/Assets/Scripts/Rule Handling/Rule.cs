using SideQuestGenerator.QuestHandling;
using SideQuestGenerator.GraphHandling;
using System.Collections.Generic;
using SideQuestGenerator.InteractableHandling;

namespace SideQuestGenerator.RuleHandling
{
    public abstract class Rule
    {
        public float RuleMultiplier { get; protected set; }

        protected string ruleName;
        public Rule()
        {
            ruleName = null;
        }
        public Rule(string ruleName)
        {
            this.ruleName = ruleName;
        }
        public void SetRuleName(string ruleName)
        {
            this.ruleName = ruleName;
        }
        public string GetRuleName()
        {
            return ruleName;
        }

        public abstract List<Graph> ImplementRule(Graph graph);

        public abstract Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies);

        public abstract float GetAverageCost(Graph graph);
    }
}