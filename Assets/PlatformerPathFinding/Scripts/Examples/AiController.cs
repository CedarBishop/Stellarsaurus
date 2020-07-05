using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlatformerPathFinding.Examples {
    /// <summary>
    /// The example AI Controller, which can be used in your game,
    /// use this or write your own.
    /// The most important thing is agent.FindPath method.
    /// </summary>
    public class AiController : MonoBehaviour {
        public Transform _goal;
        [SerializeField] PathFindingAgent _agent;
        [SerializeField] float _stopDistance;
        [SerializeField] LayerMask _groundMask;
        [SerializeField] float _nodeSize;

        private Perception perception;
        private SpriteRenderer spriteRenderer;

        public float _walkSpeed;

        public float _jumpSpeed;
        public float _fallSpeed;

        [SerializeField] float _pathUpdateFrequency = 10f;

        Queue<MovementTask> _movementTasks;
        MovementTask _pendingTask;

        float _elapsedTime;
        float _updatePathTime;

        void Start() 
        {
            UpdatePath();
            perception = GetComponent<Perception>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        Vector2 GetAgentFloorPos() 
        {
            RaycastHit2D hit = Physics2D.Raycast(_goal.position, Vector2.down, float.PositiveInfinity, _groundMask);

            Vector2 point = hit.point;


            return point + Vector2.up * _nodeSize / 2;
        }

        void UpdatePath() 
        {
            var path = _agent.FindPath(GetAgentFloorPos());
            if (path == null) 
            {
                Debug.Log("There is no path there.");
                GetComponent<AI>().SetRandomGoal();
                _movementTasks = null;
                _pendingTask = null;
                
                return;
            }
            
            _movementTasks = NodesToMoveTasks(path);

            var distance = EstimatedDistance(_movementTasks);
            _updatePathTime = distance * (1 / _pathUpdateFrequency);
            
            if(_movementTasks.Count > 0)
                _pendingTask = _movementTasks.Dequeue();
            
            //Debug.Log("UpdatePath(), time until next update: " + _updatePathTime);
        }

        static float EstimatedDistance(IEnumerable<MovementTask> movementTasks) 
        {
            return movementTasks.Sum(task => task.GetLength());
        }

        public void SetPosition(Vector2 position) 
        {
            transform.position = position;
        }

        public void SetLookDir(bool right) 
        {
            perception.isFacingRight = right;
            spriteRenderer.flipX = !right;
        }

        Queue<MovementTask> NodesToMoveTasks(IReadOnlyList<Node> path) 
        {
            var result = new Queue<MovementTask>(path.Count);

            Action<AiController> turnRight = aiController => aiController.SetLookDir(true);
            Action<AiController> turnLeft = aiController => aiController.SetLookDir(false);

            for (var i = 1; i < path.Count; i++) 
            {
                var cur = path[i];
                var prev = path[i - 1];
                bool rightDir = cur.X - prev.X > 0;
                MovementTask task;
                switch (cur.Transition) 
                {
                    case TransitionType.Jump:
                        result.Enqueue(new WaitTask(this, rightDir ? turnRight : turnLeft, 0.2f));

                        var bezier = _agent.GetBezier(prev, cur);
                        task = new BezierMoveTask(this, null, bezier, _jumpSpeed);
                        result.Enqueue(task);

                        result.Enqueue(new WaitTask(this, null, 0.2f));
                        break;
                    case TransitionType.Walk:
                        task = new LinearMoveTask(this, rightDir ? turnRight : turnLeft, prev.WorldPosition,
                            cur.WorldPosition, _walkSpeed, true);
                        result.Enqueue(task);
                        break;
                    case TransitionType.Fall:
                        task = new LinearMoveTask(this, null, prev.WorldPosition, cur.WorldPosition, _fallSpeed, false);
                        result.Enqueue(task);

                        result.Enqueue(new WaitTask(this, null, 0.2f));
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            return result;
        }

        void Update() 
        {
            if (_goal == null)
            {
                GetComponent<AI>().SetRandomGoal();
            }
            float dt = Time.deltaTime;
            _elapsedTime += dt;

            var distance = (transform.position - _goal.position).magnitude;
            if (distance <= _stopDistance && (_pendingTask == null || _pendingTask.CanBeCanceled))
                return;

            if (_elapsedTime >= _updatePathTime && (_pendingTask == null || _pendingTask.CanBeCanceled)) {
                _elapsedTime = 0;

                UpdatePath();
            }

            if (_pendingTask == null)
                return;
            
            var ended = _pendingTask.Update(dt);
            if (ended)
                _pendingTask = _movementTasks.Count > 0 ? _movementTasks.Dequeue() : null;
        }
    }
}