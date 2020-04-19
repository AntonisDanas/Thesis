using System;
using System.Collections.Generic;

public abstract class Rule
{
    protected String ruleName;
    public Rule()
    {
        this.ruleName = null;
    }
    public Rule(String ruleName)
    {
        this.ruleName = ruleName;
    }
    public void SetRuleName(String ruleName)
    {
        this.ruleName = ruleName;
    }
    public String GetRuleName()
    {
        return this.ruleName;
    }

    public abstract List<Graph> ImplementRule(Graph graph);

    public abstract Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies);
}

