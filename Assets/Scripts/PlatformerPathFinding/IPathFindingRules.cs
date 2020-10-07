using System.Collections.Generic;

namespace PlatformerPathFinding {
    public interface IPathFindingRules {
        int GetHeuristic(Node node, Node goal, PathFindingAgent agent);
        List<TransitionData> GetTransitions(PathFindingGrid pathFindingGrid, PathFindingAgent agent, Node node);
    }
}