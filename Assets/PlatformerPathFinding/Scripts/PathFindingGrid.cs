using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PathFindingGrid : MonoBehaviour {
        [SerializeField] int _gridSizeX;
        [SerializeField] int _gridSizeY;
        [SerializeField] float _nodeSize;
        [SerializeField] LayerMask _collisionLayerMask;
        [SerializeField] bool _drawGrid = true;

        [SerializeField, HideInInspector] Node[] _grid;

        IPathFindingRules _pathFindingRules;
        AStarSearch _search;

        public float NodeSize => _nodeSize;
        public int MaxSize => _gridSizeX * _gridSizeY;

        public void Init(IPathFindingRules pathFindingRules, AStarSearch search) {
            _pathFindingRules = pathFindingRules;
            _search = search;
        }

        bool IsInsideGrid(int x, int y) {
            return x >= 0 && y >= 0 && x < _gridSizeX && y < _gridSizeY;
        }

        public Node GetNode(int x, int y) {
            return IsInsideGrid(x, y) ? _grid[x * _gridSizeY + y] : null;
        }

        public void Build() {
            _grid = new Node[_gridSizeX * _gridSizeY];
            Vector2 bottomLeftCell = GetBottomLeftNodePosition();
            for (var x = 0; x < _gridSizeX; ++x) {
                for (var y = 0; y < _gridSizeY; ++y) {
                    Vector2 cellCenter = bottomLeftCell + new Vector2(x * _nodeSize, y * _nodeSize);
                    bool isOccupiedCell = IsOccupiedCell(cellCenter);
                    _grid[x * _gridSizeY + y] = new Node(!isOccupiedCell, cellCenter, x, y);
                }
            }
        }

        public void UnBuild() {
            _grid = null;
        }

        public List<Node> FindPath(PathFindingAgent agent, Vector2 goalPosition) {
            Node start = WorldPositionToNode(agent.transform.position);
            Node goal = WorldPositionToNode(goalPosition);

            return _search.Search(start, goal, _pathFindingRules, agent);
        }

        Vector2 GetBottomLeftNodePosition() {
            var pos = transform.position;
            return new Vector2(pos.x - (_gridSizeX / 2f - .5f) * _nodeSize,
                pos.y - (_gridSizeY / 2f - .5f) * _nodeSize);
        }

        void OnDrawGizmos() {
            if (!_drawGrid || _grid == null)
                return;

            Vector2 scale = Vector2.one * _nodeSize;
            foreach (Node node in _grid) {
                Gizmos.color = node.IsEmpty ? new Color(0, 0.7f, 0, 0.2f) : new Color(0.7f, 0f, 0f, 0.2f);
                Gizmos.DrawWireCube(node.WorldPosition, scale);
            }
        }

        bool IsOccupiedCell(Vector2 worldPos) {
            return Physics2D.OverlapBox(worldPos, new Vector2(_nodeSize, _nodeSize) - Vector2.one * 0.05f, 0,
                _collisionLayerMask);
        }

        Node WorldPositionToNode(Vector2 worldPos) {
            Vector2 bottomLeftNode = GetBottomLeftNodePosition();
            int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x - bottomLeftNode.x) / _nodeSize), 0, _gridSizeX - 1),
                y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - bottomLeftNode.y) / _nodeSize), 0, _gridSizeY - 1);

            return GetNode(x, y);
        }
    }
}