using UnityEngine;

namespace PlatformerPathFinding {
    [System.Serializable]
    public class Node : IHeapItem<Node> {

        public bool isEmpty;
        public Vector2 worldPosition;
        public int x;
        public int y;
        public Node cameFromNode;

        public bool IsEmpty => isEmpty;
        public Vector2 WorldPosition => worldPosition;

        public int X => x;
        public int Y => y;

        // Distance from starting node
        public int GCost { get; set; }

        // Distance to end node. (Heuristic).
        public int HCost { get; set; }

        // Total cost.
        int FCost => GCost + HCost;

        public Node Parent { get; set; }
        
        public Node(bool _isEmpty, Vector2 _worldPosition, int _x, int _y) {
            this.isEmpty = _isEmpty;
            this.worldPosition = _worldPosition;
            this.x = _x;
            this.y = _y;
        }

        public TransitionType Transition { get; set; }

        public int HeapIndex { get; set; }

        public int CompareTo(Node nodeToCompare) {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0) {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }

            return -compare;
        }
    }
}