using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : MonoBehaviour
{

}

public class PathFinding : MonoBehaviour
{
    Pathdata pd;

    List<PathdataNode> openList = new List<PathdataNode>();
    List<PathdataNode> closeList = new List<PathdataNode>();
    PathFindingNode startNode,endNode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
