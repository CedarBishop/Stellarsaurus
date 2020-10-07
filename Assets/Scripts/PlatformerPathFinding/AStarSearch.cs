using System.Collections.Generic;

namespace PlatformerPathFinding {
    public class AStarSearch {
        
        readonly Heap<Node> _openSet;
        readonly HashSet<Node> _closedSet = new HashSet<Node>();

        readonly PathFindingGrid _pathFindingGrid;
        
        public AStarSearch(PathFindingGrid pathFindingGrid) {
            _pathFindingGrid = pathFindingGrid;
            _openSet = new Heap<Node>(pathFindingGrid.MaxSize);
        }

        public List<Node> Search(Node start, Node goal, IPathFindingRules rules, PathFindingAgent agent) {

            _openSet.Clear();
            _closedSet.Clear();
            
            _openSet.Add(start);
            var foundGoal = false;

            while (_openSet.Count > 0) {
                Node node = _openSet.RemoveFirst();
                _closedSet.Add(node);

                if (node == goal) {
                    foundGoal = true;     
                    break;                    
                }

                var transitions = rules.GetTransitions(_pathFindingGrid, agent, node);
                foreach (var transition in transitions) {
                    Node neighbour = transition.Node;
                    
                    if ( /*!neighbour.IsWalkable ||*/ _closedSet.Contains(neighbour))
                        continue;

                    int newCost = node.GCost + transition.Cost;
                    if (newCost < neighbour.GCost || !_openSet.Contains(neighbour)) {
                        neighbour.GCost = newCost;
                        neighbour.HCost = rules.GetHeuristic(neighbour, goal, agent);
                        neighbour.Parent = node;
                        neighbour.Transition = transition.Transition;

                        if (_openSet.Contains(neighbour)) {
                            _openSet.UpdateItem(neighbour);
                        }
                        else {
                            _openSet.Add(neighbour);
                        }
                    }
                }
            }

            return foundGoal ? RetracePath(start, goal) : null;
        }

        static List<Node> RetracePath(Node start, Node goal) {

            var path = new List<Node>();
            Node currentNode = goal;

            TransitionType previousTransition = TransitionType.None;
            
            while (currentNode != start) {
                TransitionType currentTransition = currentNode.Transition;
                
                if (!(previousTransition == TransitionType.Walk && currentTransition == TransitionType.Walk))
                    path.Add(currentNode);

                previousTransition = currentTransition;
                currentNode = currentNode.Parent;
            }
            path.Add(start);
            
            path.Reverse();
            return path;
        }
    }
}