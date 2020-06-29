using UnityEngine;

namespace PlatformerPathFinding {
    [System.Serializable]
    public class Node : IHeapItem<Node> {

        [SerializeField] bool _isEmpty;
        [SerializeField] Vector2 _worldPosition;
        [SerializeField] int _x;
        [SerializeField] int _y;

        public bool IsEmpty => _isEmpty;
        public Vector2 WorldPosition => _worldPosition;

        public int X => _x;
        public int Y => _y;

        // Distance from starting node
        public int GCost { get; set; }

        // Distance to end node. (Heuristic).
        public int HCost { private get; set; }

        // Total cost.
        int FCost => GCost + HCost;

        public Node Parent { get; set; }
        
        public Node(bool isEmpty, Vector2 worldPosition, int x, int y) {
            _isEmpty = isEmpty;
            _worldPosition = worldPosition;
            _x = x;
            _y = y;
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