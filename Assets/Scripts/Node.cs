
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Node> neightbourds;
    private Vector3 position;

    private bool shortcut;

    public List <Node> _neightbourds => neightbourds;
    public Vector3 _position;
    public bool _shortcut;

    public Node (List <Node> neightbourds, Vector3 position, bool shortcut = false)
    {
        this.neightbourds = neightbourds;
        this._position = position;
        this._shortcut = shortcut;
    }
}
