using System;

namespace PlatformerPathFinding.Examples {
    class WaitTask : MovementTask {
        readonly float _time;
        float _elapsedTime = 0;

        public override bool CanBeCanceled => true;
        
        public WaitTask(AiController worker, Action<AiController> onPreUpdate, float time) 
            : base(worker, onPreUpdate) {
            _time = time;
        }

        protected override bool OnUpdate(float dt) {
            _elapsedTime += dt;
            return _elapsedTime >= _time;
        }

        public override float GetLength() {
            return 0;
        }
    }
}