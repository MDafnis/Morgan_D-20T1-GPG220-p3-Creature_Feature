using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathdataNode
{
    Vector2Int GridLocation;
    Vector3 WorldLocation;

    public PathdataNode(int x, int z, Vector3 nodePosition)
    {
        GridLocation = new Vector2Int(x, z);
        WorldLocation = nodePosition;
    }
}

public class Pathdata : MonoBehaviour
{
    public Vector2Int WorldSize;
    public List<PathdataNode> AllNodes = new List<PathdataNode>();

    public bool DEBUG_DrawPathdata = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DEBUG_DrawPathdata)
        {
            
        }
    }

    public PathdataNode CreateNode(int x, int z, Vector3 nodePosition)
    {
        PathdataNode newNode = new PathdataNode(x, z, nodePosition);

        AllNodes.Add(newNode);

        return newNode;
    }
}
