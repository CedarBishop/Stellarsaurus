using UnityEngine;

namespace PlatformerPathFinding {
    public struct BezierCurve {
        
        public Vector2 A, B, C, D;

        public BezierCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
            A = a;
            B = b;
            C = c;
            D = d;
        }
        
        public Vector2 GetValue(float t) {
            float oneMinusT = 1 - t;
            return oneMinusT * oneMinusT * oneMinusT * A + 3 * oneMinusT * oneMinusT * t * B +
                   3 * oneMinusT * t * t * C + t * t * t * D;
        }
    }
}