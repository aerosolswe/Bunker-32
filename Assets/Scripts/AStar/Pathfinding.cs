using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding instance = null;

    [HideInInspector]
    public Grid grid;

    private void Awake() {
        instance = this;

        grid = GetComponent<Grid>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Y)) {
            grid.debugPath = FindPath(Player.instance.transform.position, GameObject.Find("SpawnPoint").transform.position);
            Debug.Log(grid.debugPath.Count + " " + Player.instance.transform.position + " " + GameObject.Find("SpawnPoint").transform.position);
        }
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0) {
            Node currentNode = openSet.RemoveFirst();
            

            closedSet.Add(currentNode);

            if(currentNode == targetNode) {
                return RetracePath(startNode, targetNode);
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)) {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    } else {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    } 
}
