using System;
using System.Collections.Generic;

public abstract class Rule
{
    public float RuleMultiplier { get; protected set; }

    protected string ruleName;
    public Rule()
    {
        this.ruleName = null;
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
        return this.ruleName;
    }

    public abstract List<Graph> ImplementRule(Graph graph);

    public abstract Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies);

    public abstract float GetAverageCost(Graph graph);
}

