using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    public bool walkable;
    public Vector3 worldPosition;

    public int gridX, gridY;

    private int gCost;
    private int hCost;

    private int heapIndex;

    public Node parent;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY) {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if(compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.HCost);
        }

        return -compare;
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int GCost {
        get { return gCost; }
        set { gCost = value; }
    }

    public int HCost {
        get { return hCost; }
        set { hCost = value; }
    }

    public int FCost {
        get { return GCost + HCost; }
    }
}
