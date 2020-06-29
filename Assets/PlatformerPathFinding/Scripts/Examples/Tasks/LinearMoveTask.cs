using System;
using UnityEngine;

namespace PlatformerPathFinding.Examples {
    class LinearMoveTask : MovementTask {
        Vector2 _start;
        readonly Vector2 _end;
        Vector2 _cur;

        readonly float _speed;

        public LinearMoveTask(AiController worker, Action<AiController> onPreUpdate, Vector2 start, Vector2 end,
            float speed, bool canBeCanceled) : base(worker, onPreUpdate) {
            _start = start;
            _cur = start;
            _end = end;
            _speed = speed;
            CanBeCanceled = canBeCanceled;
        }

        public override bool CanBeCanceled { get; }

        protected override bool OnUpdate(float dt) {
            float maxDistanceDelta = dt * _speed;

            Vector2 a = _end - _cur;
            float magnitude = a.magnitude;

            if (magnitude <= maxDistanceDelta) {
                Worker.SetPosition(_end);
                return true;
            }

            _cur = _cur + a / magnitude * maxDistanceDelta;
            Worker.SetPosition(_cur);
            return false;
        }

        public override float GetLength() {
            return (_end - _cur).magnitude;
        }
    }
}