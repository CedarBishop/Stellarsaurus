
namespace PlatformerPathFinding {
    public struct TransitionData {
        public Node Node { get; }
        public TransitionType Transition { get; }
        public int Cost { get; }

        public TransitionData(Node node, TransitionType transition, int cost) {
            Node = node;
            Transition = transition;
            Cost = cost;
        }
    }
}

