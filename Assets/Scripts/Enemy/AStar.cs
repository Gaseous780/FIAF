using System;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public List<Node> Run(Node initialNode, Func<Node, bool> isSatisfied, Func<Node, Node, float> getCosts, Func<Node, float> heuristic)
    {
        PriorityQueue<Node> pending = new PriorityQueue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
        Dictionary<Node, float> costs = new Dictionary<Node, float>();
        costs[initialNode] = 0;

        pending.Enqueue(initialNode, 0);
        visited.Add(initialNode);

        while (!pending.IsEmpty)
        {

            Node node = pending.Dequeue();
            if (isSatisfied(node))
            {
                List<Node> path = new List<Node>();
                path.Add(node);
                Node current = node;

                while (parents.ContainsKey(current))
                {
                    path.Add(parents[current]);
                    current = parents[current];
                }

                Debug.Log(path.Count);

                path.Reverse();
                return path;
            }
            else
            {
                List<Node> children = node._neightbourds;

                for (int i = 0; i < children.Count; ++i)
                {
                    if (visited.Contains(children[i]))
                    {
                        continue;
                    }
                    float currentCosts = costs[node] + getCosts(node, children[i]);
                    if (costs.ContainsKey(children[i]) && currentCosts > costs[children[i]])
                    {
                        continue;
                    }
                    costs[children[i]] = currentCosts;
                    pending.Enqueue(children[i], currentCosts + heuristic(children[i]));
                    visited.Add(children[i]);
                    parents[children[i]] = node;
                }
            }

        }
        return new List<Node>();

    }
}
