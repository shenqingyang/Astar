using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { 
   undefined,normal, barrier,gress,sand
}

public class AstarNode 
{
    public int x;
    public int y;
    public float f;
    public float g;
    public float h;
    public AstarNode father;
    public NodeType type;

    public AstarNode(int x,int y,NodeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
