using System;

namespace PlatformerPathFinding.Examples {
    public abstract class MovementTask {
        protected readonly AiController Worker;
        readonly Action<AiController> _onPreUpdate;
        bool _actionFired;

        public abstract bool CanBeCanceled { get; }
        
        protected MovementTask(AiController worker, Action<AiController> onPreUpdate) {
            Worker = worker;
            _onPreUpdate = onPreUpdate;
        }

        public bool Update(float dt) {
            if (!_actionFired) {
                _onPreUpdate?.Invoke(Worker);
                _actionFired = true;
            }

            return OnUpdate(dt);
        }
        
        protected abstract bool OnUpdate(float dt);

        public abstract float GetLength();
    }
}