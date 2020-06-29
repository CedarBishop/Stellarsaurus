using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace PlatformerPathFinding {
    public class PathFindingAgent : MonoBehaviour {
        [Range(1, 20)]
        [SerializeField] int _height;
        [Range(1, 20)]
        [SerializeField] int _width;
        [Range(3, 50)]
        [SerializeField] int _jumpStrength = 5;
        [Range(4, 100)]
        [SerializeField] int _fallLimit = 10;
        [SerializeField] bool _drawPathGizmos;

        PathFindingGrid _pathFindingGrid;

        public int JumpStrength => _jumpStrength;

        public int Height => _height;
        public int Width => _width;
        public int FallLimit => _fallLimit;
        
        Vector2 TopLeftOffset => new Vector2(-.5f, _height - .5f);

        List<Node> _path;
        
        public void Init(PathFindingGrid pathFindingGrid) {
            _pathFindingGrid = pathFindingGrid;
        }
        
        void Awake() {
            PrecomputeBezier();
        }

        public List<Node> FindPath(Vector2 position) {
            _path = _pathFindingGrid.FindPath(this, position);
            return _path;
        }
        
        HashSet<Vector2Int>[,] _rightNodes;
        HashSet<Vector2Int>[,] _leftNodes;

        public HashSet<Vector2Int> GetCheckNodes(int xOffset, int yOffset) {
            int beginY = _jumpStrength - _fallLimit;
            return  xOffset > 0 ? _rightNodes[xOffset, yOffset - beginY] : _leftNodes[-xOffset, yOffset - beginY];
        }

        void PrecomputeBezier() {
            int beginY = _jumpStrength - _fallLimit;
            _rightNodes = new HashSet<Vector2Int>[_jumpStrength + 1, _fallLimit + 1];
            _leftNodes = new HashSet<Vector2Int>[_jumpStrength + 1, _fallLimit + 1];
            
            var a = new Vector2(-.5f, _height - .51f);
            var b = TopLeftOffset + Vector2.up * _jumpStrength;

            var bezierCurve = new BezierCurve {A = a, B = b};


            for (int x = 2; x <= _jumpStrength; x++) {
                bezierCurve.C = b + Vector2.right * x;

                for (int y = 0; y >= -_fallLimit; y--) {
                    bezierCurve.D = bezierCurve.C + Vector2.up * y;

                    var overlappingNodes = GetOverlappingNodes(bezierCurve);
                    var nodesCopy = new HashSet<Vector2Int>(overlappingNodes);

                    foreach (var node in nodesCopy) {
                        for (int h = 0; h >= -_height; h--) {
                            for (int w = 0; w <= _width; w++)
                                overlappingNodes.Add(node + new Vector2Int(w, h));
                        }
                    }
                    
                    var landing = new Vector2Int(x, _jumpStrength + y);
                    for (int h = 0; h < _height; h++) {
                        for (int w = 0; w < _width; w++)
                            overlappingNodes.Add(landing + new Vector2Int(w, h));
                    }
                    _rightNodes[landing.x, landing.y - beginY] = overlappingNodes;
                    
                    
                    var leftOverlappingNodes = new HashSet<Vector2Int>();
                    foreach (var n in overlappingNodes) {
                        leftOverlappingNodes.Add(new Vector2Int(-n.x, n.y));
                    }
                    _leftNodes[landing.x, landing.y - beginY] = leftOverlappingNodes;
                }
            }
        }

        static HashSet<Vector2Int> GetOverlappingNodes(BezierCurve curve) {
            var overlappingNodes = new HashSet<Vector2Int>();
            // Exclude the last point.
            SplitCurveInHalf(curve, 0f, 0.99f, overlappingNodes);
            return overlappingNodes;
        }

        static void SplitCurveInHalf(BezierCurve curve, float tBegin, float tEnd, HashSet<Vector2Int> outHash) {
            
            Vector2 beginValue = curve.GetValue(tBegin),
                endValue = curve.GetValue(tEnd);

            int endX = Mathf.RoundToInt(endValue.x),
                endY = Mathf.RoundToInt(endValue.y);

            int beginX = Mathf.RoundToInt(beginValue.x),
                beginY = Mathf.RoundToInt(beginValue.y);

            int dxAbs = Mathf.Abs(endX - beginX),
                dyAbs = Mathf.Abs(endY - beginY);
            
            bool endSplit = dxAbs == 1 && dyAbs == 0 || dyAbs == 1 && dxAbs == 0 
                            || Vector2.Distance(beginValue, endValue) <= 0.25f;

            if (!endSplit) {
                float mid = Mathf.Lerp(tBegin, tEnd, 0.5f);
                SplitCurveInHalf(curve, tBegin, mid, outHash);
                SplitCurveInHalf(curve, mid, tEnd, outHash);

                return;
            }
            
            var point = new Vector2Int(endX, endY);
            outHash.Add(point);
        }

        public BezierCurve GetBezier(Node from, Node to) {
            Vector2 a = from.WorldPosition;
            Vector2 b = a + Vector2.up * (_jumpStrength * _pathFindingGrid.NodeSize);
            Vector2 c = b + Vector2.right * ((to.X - from.X) * _pathFindingGrid.NodeSize);
            Vector2 d = to.WorldPosition;

            return new BezierCurve(a, b, c, d);    
        }

        void OnDrawGizmos() {
            if(_drawPathGizmos)
                DrawPathGizmos();
            //DrawTestTrajectoryGizmos();
        }

        void DrawBezier(Node from, Node to, int numPoints = 55) {
            var bezierCurve = GetBezier(from, to);
            Vector2 lastPoint = bezierCurve.A;
            for (float i = 1; i <= numPoints; i++) {
                Vector2 curPoint = bezierCurve.GetValue(i / numPoints);
                Gizmos.DrawLine(lastPoint, curPoint);

                lastPoint = curPoint;
            }
        }

        void DrawTestTrajectoryGizmos() {

            if (_rightNodes == null)
                return;
            
            for (int x = -10; x < 10; x++) {
                for (int y = -10; y < 10; y++) {
                    Gizmos.color = GetCheckNodes(-5, -3).Contains(new Vector2Int(x, y))
                        ? Color.blue : Color.green;

                    Gizmos.DrawWireCube(new Vector2(x, y), Vector3.one * 0.95f);
                }
            }
        }

        void DrawPathGizmos() {
            if (_path == null || _path.Count == 0)
                return;

            Gizmos.color = Color.magenta;
            //Gizmos.DrawLine(transform.position, _path[0].WorldPosition);

            for (int i = 0; i < _path.Count - 1; i++) {
                Gizmos.color = Gizmos.color == Color.blue ? Color.magenta : Color.blue;

                if (_path[i + 1].Transition == TransitionType.Jump)
                    DrawBezier(_path[i], _path[i + 1]);
                else
                    Gizmos.DrawLine(_path[i].WorldPosition, _path[i + 1].WorldPosition);
            }
        }
    }
}