
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Node> neightbourds;
    private Vector3 position;

    private string name;

    private bool shortcut;

    private GameObject gameObjectToDisable;

    public Vector3 _position;
    public bool _shortcut;
    public string _name => name;

    public List<Node> _neightbourds { get => neightbourds; set => neightbourds = value; }

    public GameObject _gameObjectToDisable => gameObjectToDisable;

    public Node (Vector3 position,string name, GameObject gameObject,bool shortcut = false)
    {
        this._position = position;
        _shortcut = shortcut;
        this.name = name;
        gameObjectToDisable = gameObject;
    }
}
