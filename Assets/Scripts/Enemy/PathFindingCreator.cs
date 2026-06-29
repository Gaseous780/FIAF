using System.Collections.Generic;
using UnityEngine;

public class PathFindingCreator
{
    private Node startNode;
    private Node endNode;

    private ThetaStar theta;

    private AStar aStar;

    public PathFindingCreator(Node endNode = null)
    {
        theta = new ThetaStar();
        aStar = new AStar();
        if (endNode != null)
        {
            this.endNode = endNode;
        }
    }

    public List <Node> SetPathTheta(Node initNode, Node startNode)
    {
        this.startNode = startNode;

        List<Node> path = theta.Run(initNode, ReachEndPoint, GetCosts, HasAShortcut, HasLineOfSight);
    
        List <Vector3> points = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            points.Add(path[i]._position);
        }
    
        return path;
    }

    public List <Node> SetPathAStar(Node initNode)
    {
        List<Node> path = aStar.Run(initNode, ReachOrigin, GetCosts, HasAShortcut);
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            points.Add(path[i]._position);
        }

        return path;
    }

    private bool ReachEndPoint(Node nodeToCompare)
    {
        if (nodeToCompare == endNode)
        {
            return true;
        }

        return false;
    }

    private bool ReachOrigin (Node nodeToCompare)
    {
        if (nodeToCompare == startNode)
        {
            return true;
        }

        return false;
    }

    private float GetCosts(Node actualNode, Node nodeToCompare) 
    {
        return Vector3.Distance(actualNode._position, nodeToCompare._position);
    }

    private float HasAShortcut(Node nodeToGetInfo)
    {
        float cost = 0;

        if (nodeToGetInfo._shortcut == true)
        {
            cost += 100;
        }

        return cost;
    }

    public bool HasLineOfSight(Node actualNode, Node nodeToGo) 
    {
        Vector3 startPosition = actualNode._position + Vector3.up * 0.5f;
        Vector3 endPosition = nodeToGo._position + Vector3.up * 0.5f;

        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;

        return !Physics.Raycast(startPosition, endPosition, distance, LayerMask.GetMask ("Default"));
    }
}
