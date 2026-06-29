using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ThetaStar
{
    public List<Node> Run(Node initialNode, Func<Node, bool> isSatisfied, Func<Node, Node, float> getCosts, Func<Node, float> heuristic, Func<Node, Node, bool> hasLineOfSight)
    {
        PriorityQueue<Node> pending = new PriorityQueue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
        Dictionary<Node, float> costs = new Dictionary<Node, float>();

        costs[initialNode] = 0;
        parents[initialNode] = initialNode;

        pending.Enqueue(initialNode, 0);

        //int counter = 0;

        while (!pending.IsEmpty)
        {
            //counter++;
            //if (counter > watchDog) break;

            Node node = pending.Dequeue();
            if (visited.Contains(node))
            {
                continue;
            }

            visited.Add(node);

            Debug.Log(node._name);

            if (isSatisfied(node))
            {
                List<Node> path = new List<Node>();
                path.Add(node);
                Node current = node;

                int t = 0;
                while (parents.ContainsKey(current) && parents[current] != current)
                {
                    path.Add(parents[current]);
                    current = parents[current];
                    Debug.Log(parents[current]._name);
                }

                path.Reverse();
                Debug.Log("Complete");

                for (int i = 0; i < path.Count; i++)
                {
                    Debug.Log(path[i]._name);
                }
                return path;
            }
            else
            {
                List<Node> children = node._neightbourds;

                for (int i = 0; i < children.Count; ++i)
                {
                    Node child = children[i];

                    if (visited.Contains(child))
                    {
                        continue;
                    }

                    Node parent = parents[node];

                    float currentCosts;
                    Node newParent;

                    if (parent != node && hasLineOfSight(parent, child))
                    {
                        currentCosts = costs[parent] + getCosts(parent, child);
                        newParent = parent;
                    }
                    else
                    {
                        currentCosts = costs[node] + getCosts(node, child);
                        newParent = node;
                    }

                    if (costs.ContainsKey(child) && currentCosts >= costs[child])
                    {
                        continue;
                    }

                    costs[child] = currentCosts;
                    parents[child] = newParent;
                    pending.Enqueue(child, currentCosts + heuristic(child));
                }
            }
        }

        return new List<Node>();
    }
}
