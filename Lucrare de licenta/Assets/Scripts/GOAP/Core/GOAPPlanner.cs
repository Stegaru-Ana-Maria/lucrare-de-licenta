using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(GameObject agent, List<GOAPAction> actions, Dictionary<string, bool> worldState, Dictionary<string, bool> goal)
    {
        // Resetam toate actiunile
        foreach (var a in actions)
            a.ResetAction();

        List<GOAPAction> usableActions = new List<GOAPAction>();

        foreach (var action in actions)
        {
            if (action.CheckProceduralPrecondition(agent))
                usableActions.Add(action);
        }

        // Nod de start (radacina)
        Node start = new Node(null, 0f, worldState, null);
        List<Node> leaves = new List<Node>();

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("GOAP: Nu s-a gasit niciun plan.");
            return null;
        }

        // Gasim nodul cu costul cel mai mic
        Node cheapest = leaves.OrderBy(n => n.cost).First();

        // Reconstruim planul pornind de la frunza
        List<GOAPAction> result = new List<GOAPAction>();
        while (cheapest != null && cheapest.action != null)
        {
            result.Insert(0, cheapest.action);
            cheapest = cheapest.parent;
        }

        return new Queue<GOAPAction>(result);
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, Dictionary<string, bool> goal)
    {
        bool foundOne = false;

        foreach (GOAPAction action in usableActions)
        {
            if (ArePreconditionsMet(action, parent.state))
            {
                // Creez noua stare rezultata din efectele actiunii
                Dictionary<string, bool> currentState = new Dictionary<string, bool>(parent.state);
                foreach (var effect in action.effects)
                    currentState[effect.Key] = effect.Value;

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // Clonam lista de atiuni fara cea deja folosita
                    List<GOAPAction> remainingActions = new List<GOAPAction>(usableActions);
                    remainingActions.Remove(action);

                    if (BuildGraph(node, leaves, remainingActions, goal))
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    private bool ArePreconditionsMet(GOAPAction action, Dictionary<string, bool> state)
    {
        foreach (var pre in action.preconditions)
        {
            if (!state.ContainsKey(pre.Key) || state[pre.Key] != pre.Value)
                return false;
        }
        return true;
    }

    private bool EffectsMatchGoal(GOAPAction action, Dictionary<string, bool> goal)
    {
        foreach (var g in goal)
        {
            if (action.effects.ContainsKey(g.Key) && action.effects[g.Key] == g.Value)
                return true;
        }
        return false;
    }
    private bool GoalAchieved(Dictionary<string, bool> goal, Dictionary<string, bool> state)
    {
        foreach (var g in goal)
        {
            if (!state.ContainsKey(g.Key) || state[g.Key] != g.Value)
                return false;
        }
        return true;
    }

    private class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<string, bool> state;
        public GOAPAction action;

        public Node(Node parent, float cost, Dictionary<string, bool> state, GOAPAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = state;
            this.action = action;
        }
    }
}
