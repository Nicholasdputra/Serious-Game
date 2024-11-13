using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Node : IHeapItem<AStar_Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public AStar_Node parent;
    int heapIndex;

    public AStar_Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY){
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fcost(){
        return gCost + hCost;   
    }

    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

    public int CompareTo(AStar_Node nodeToCompare){
        int compare = fcost().CompareTo(nodeToCompare.fcost());
        if(compare == 0){
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}