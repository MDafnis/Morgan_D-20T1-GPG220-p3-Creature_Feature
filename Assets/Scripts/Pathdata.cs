using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathdataNode : IEquatable<PathdataNode>
{
    public Vector2Int GridLocation;
    public Vector3 WorldLocation;
    public List<PathdataEdge> listOfEdges = new List<PathdataEdge>(8);
    public bool blocking = false;

    public PathdataNode(int x, int z, Vector3 nodePosition)
    {
        GridLocation = new Vector2Int(x, z);
        WorldLocation = nodePosition;
    }

    // The below section is to make comparing nodes easier, by using their world position.
    public override bool Equals(object obj)
    {
        return Equals(obj as PathdataNode);
    }

    public bool Equals(PathdataNode other)
    {
        return other != null &&
               GridLocation.Equals(other.GridLocation);
    }

    public override int GetHashCode()
    {
        return 703087130 + EqualityComparer<Vector2Int>.Default.GetHashCode(GridLocation);
    }

    public static bool operator ==(PathdataNode left, PathdataNode right)
    {
        return EqualityComparer<PathdataNode>.Default.Equals(left, right);
    }

    public static bool operator !=(PathdataNode left, PathdataNode right)
    {
        return !(left == right);
    }
    //
}

public class Pathdata : MonoBehaviour
{
    public Vector2Int WorldSize;
    public List<PathdataNode> AllNodes = new List<PathdataNode>();
    public static Pathdata instance;

    public bool DEBUG_DrawPathdata = false;

    void ShowPathdata()
    {
        foreach(var node in AllNodes)
        {
            foreach(var edge in node.listOfEdges)
            {
                if(edge.link != null)
                {
                    Debug.DrawLine(node.WorldLocation, edge.link.WorldLocation, Color.blue);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DEBUG_DrawPathdata)
        {
            ShowPathdata();
        }
    }

    public PathdataNode CreateNode(int x, int z, Vector3 nodePosition)
    {
        PathdataNode newNode = new PathdataNode(x, z, nodePosition);

        AllNodes.Add(newNode);

        return newNode;
    }

    public PathdataNode FindNode(Vector2 checkLoc)
    {
        Vector2Int gridLoc = WorldToGrid(checkLoc);
        int index = gridLoc.x * WorldSize.y + gridLoc.y;

        if(gridLoc.x < 0 || gridLoc.y < 0 || gridLoc.x >= WorldSize.x || gridLoc.y >= WorldSize.y)
        {
            return null;
        }

        if(index < 0 || index > AllNodes.Count)
        {
            Debug.Log(index + "" + gridLoc + "" + WorldSize);
        }

        return AllNodes[index];
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector2Int gridLoc = Vector2Int.zero;
        gridLoc.y = Mathf.RoundToInt(worldPos.x / TerrainGenerator.instance.terrain.terrainData.heightmapScale.z);
        gridLoc.x = Mathf.RoundToInt(worldPos.y / TerrainGenerator.instance.terrain.terrainData.heightmapScale.x);
        return gridLoc;
    }
}

public class PathdataEdge
{
    public PathdataNode link;

    public PathdataEdge(PathdataNode link)
    {
        this.link = link;
    }
}
