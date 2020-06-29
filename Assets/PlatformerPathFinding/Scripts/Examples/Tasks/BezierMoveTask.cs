using System;
using UnityEngine;

namespace PlatformerPathFinding.Examples {
    class BezierMoveTask : MovementTask {

        BezierCurve _curve;
        readonly float _speed;
        float _t;
        readonly Vector2 _v1, _v2, _v3;

        
        public BezierMoveTask(AiController worker, Action<AiController> onPreUpdate, BezierCurve curve, float speed) :
            base(worker, onPreUpdate) {
            _curve = curve;
            _speed = speed;

            _v1 = -3 * curve.A + 9 * curve.B - 9 * curve.C + 3 * curve.D;
            _v2 = 6 * curve.A - 12 * curve.B + 6 * curve.C;
            _v3 = -3 * curve.A + 3 * curve.B;
        }

        public override bool CanBeCanceled => false;

        protected override bool OnUpdate(float dt) {
            // ReSharper disable once InconsistentNaming
            float L = dt * _speed;
            _t = _t + L / (dt * dt * _v1 * _t * _v2 + _v3).magnitude;

            _t = Mathf.Clamp01(_t);
            
            Worker.SetPosition(_curve.GetValue(_t));

            return _t >= 1;
        }

        public override float GetLength() {
            return (_curve.D - _curve.A).magnitude * 1.5f;
        }
    }
}