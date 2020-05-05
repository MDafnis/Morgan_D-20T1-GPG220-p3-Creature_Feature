using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : IEquatable<PathFindingNode>
{
    public PathdataNode node;
    public float gCost; // How much it costs to get to the node.
    public float hCost; // How much it costs to get to the destination.
    public PathFindingNode parent; // The way to track back to a previous node

    public PathFindingNode(PathdataNode node, int x, int z)
    {
        this.node = node;
    }

    // The below section is to make comparing nodes easier, by using their world position.
    public override bool Equals(object obj)
    {
        return Equals(obj as PathFindingNode);
    }

    public bool Equals(PathFindingNode other)
    {
        return other != null &&
               EqualityComparer<PathdataNode>.Default.Equals(node, other.node);
    }

    public override int GetHashCode()
    {
        return -231681771 + EqualityComparer<PathdataNode>.Default.GetHashCode(node);
    }

    public static bool operator ==(PathFindingNode left, PathFindingNode right)
    {
        return EqualityComparer<PathFindingNode>.Default.Equals(left, right);
    }

    public static bool operator !=(PathFindingNode left, PathFindingNode right)
    {
        return !(left == right);
    }
    //
}

public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public List<PathdataNode> FindPath(PathdataNode start, PathdataNode end) // Start = start node location, End = end node location.
    {
        List<PathFindingNode> openList = new List<PathFindingNode>(); // if empty there's a problem.
        List<PathFindingNode> closeList = new List<PathFindingNode>();
        PathFindingNode startNode = new PathFindingNode(start, (int)start.WorldLocation.x, (int)start.WorldLocation.y);
        PathFindingNode endNode = new PathFindingNode(end, (int)end.WorldLocation.x, (int)end.WorldLocation.y);
        List<PathdataNode> nodeList = new List<PathdataNode>(); // list of nodes to create a path.
        int loopedIterations = 0;

        startNode.hCost = CalculateCost(start, end);
        openList.Add(startNode);

        while (openList.Count != 0)
        {


            loopedIterations++;
            if (loopedIterations > 1000) // 1000 is a throwaway number but is generally safe.
            {
                Debug.DrawLine(startNode.node.WorldLocation, endNode.node.WorldLocation, Color.magenta);
                for (int i = 0; i < closeList.Count; i++)
                {
                    if(i > 0)
                    {
                        Debug.DrawLine(closeList[i].node.WorldLocation, closeList[i].parent.node.WorldLocation, Color.red);
                    }
                }
                for (int i = 0; i < openList.Count; i++)
                {
                    if(i > 0)
                    {
                        Debug.DrawLine(openList[i].node.WorldLocation, openList[i].parent.node.WorldLocation, Color.yellow);
                    }
                }
                break;
            }

            PathFindingNode bestNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if ((openList[i].gCost + openList[i].hCost) < (bestNode.gCost + bestNode.hCost))
                {
                    bestNode = openList[i];
                }
            }

            openList.Remove(bestNode);
            closeList.Add(bestNode);

            if (bestNode == endNode)
            {
                PathFindingNode currentNode = bestNode;
                while (currentNode.parent != null)
                {
                    nodeList.Add(currentNode.node);
                    currentNode = currentNode.parent;
                }

                nodeList.Reverse();

                return nodeList;
            }
            foreach (PathdataEdge neighbour in bestNode.node.listOfEdges)
            {
                bool continueLoop = false;
                foreach(PathFindingNode withinCloseList in closeList)
                {
                    if(withinCloseList.node == neighbour.link)
                    {
                        continueLoop = true;
                        break;
                    }
                }
                if(continueLoop)
                {
                    continue;
                }
                PathFindingNode locatedNode = null;
                foreach(PathFindingNode withinOpenList in openList)
                {
                    if (withinOpenList.node == neighbour.link)
                    {
                        locatedNode = withinOpenList;
                        continueLoop = true;
                        break;
                    }
                }
                if(!continueLoop)
                {
                    PathFindingNode brotherNode = new PathFindingNode(neighbour.link, (int)neighbour.link.WorldLocation.x, (int)neighbour.link.WorldLocation.y);
                    brotherNode.parent = bestNode;
                    brotherNode.gCost = bestNode.gCost + Vector3.Distance(bestNode.node.WorldLocation, brotherNode.node.WorldLocation);
                    brotherNode.hCost = Vector3.Distance(endNode.node.WorldLocation, brotherNode.node.WorldLocation);
                    openList.Add(brotherNode);
                }
                else
                {
                    float tempGCost = bestNode.gCost + Vector3.Distance(bestNode.node.WorldLocation, neighbour.link.WorldLocation);
                    if (locatedNode.gCost < bestNode.gCost)
                    {
                        if(tempGCost < locatedNode.gCost)
                        {
                            locatedNode.parent = bestNode;
                            locatedNode.gCost = tempGCost;
                        }
                    }
                }
            }
        }

        return nodeList;
    }

    private float CalculateCost(PathdataNode pos1, PathdataNode pos2)
    {
        return Vector3.Distance(pos1.WorldLocation, pos2.WorldLocation);
    }

    // TODO collect the list of the nodes that are neighbours of the best node.
    // the list will include a list of pathfinding nodes.
    // create a getNeighbours function (this will take in a pathfindingnode of bestnode, List openlist, List closelist)
    //   within this create a integer location that'll work on a grid.
    //     if(closedList.contains(neighbourNode) use this to check if a node is contained within a list.
    //   return list of neighbours
    //private List<PathdataNode> getNeighbours(PathFindingNode bestNode, List<PathFindingNode> openList, List<PathFindingNode> closeList)
    //{
    //    int x, z;


    //}

    public PathFindingNode CreatePathNode(Vector3 checkLoc)
    {
        PathdataNode idealNode = null;
        float prevDist = 10000000f;

        foreach(var node in Pathdata.instance.AllNodes)
        {
            var dist = Vector3.Distance(checkLoc, node.WorldLocation);
            if(prevDist > dist)
            {
                prevDist = dist;
                idealNode = node;
            }
        }
        if (idealNode == null)
            return null;
        else
        {
            var pathFindingNode = new PathFindingNode(idealNode, 0, 0);
            return pathFindingNode;
        }
    }
}
