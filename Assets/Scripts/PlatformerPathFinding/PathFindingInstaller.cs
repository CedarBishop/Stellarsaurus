using PlatformerPathFinding;
using UnityEngine;

public class PathFindingInstaller : MonoBehaviour {
    [SerializeField] PathFindingGrid _pathFindingGrid;
    [SerializeField] PathFindingAgent[] _pathFindingAgents;
    
    void Awake() {
        IPathFindingRules rules = new PlatformerRules();
        AStarSearch search = new AStarSearch(_pathFindingGrid);
        foreach(var agent in _pathFindingAgents)
            agent.Init(_pathFindingGrid);
        _pathFindingGrid.Init(rules, search);
    }
}
