using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerPathFinding {
    public class PlatformerRules : IPathFindingRules {
        static readonly Func<Node, bool> IsGround = n => n == null || !n.IsEmpty;
        static readonly Func<Node, bool> IsAir = n => n != null && n.IsEmpty;

        public int GetHeuristic(Node node, Node goal, PathFindingAgent agent) {
            return Mathf.Abs(node.X - goal.X) + Mathf.Abs(node.Y - goal.Y);
        }

        int GetCost(PathFindingAgent agent, Node fromNode, Node toNode, TransitionType transitionType) {
            int dy;

            switch (transitionType) {
                case TransitionType.Walk:
                    return 1;
                case TransitionType.Jump:
                    dy = toNode.Y - fromNode.Y;
                    int dx = toNode.X - fromNode.X;
                    return agent.JumpStrength + Mathf.Abs(dx) /*+ agent.JumpStrength - dy*/;
                case TransitionType.Fall:
                    dy = fromNode.Y - toNode.Y;
                    return dy;
                case TransitionType.None:
                    Debug.LogError("Transition type is None.");
                    return -1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null);
            }
        }

        public List<TransitionData> GetTransitions(PathFindingGrid grid, PathFindingAgent agent, Node node) {

            var neighbours = new List<TransitionData>();
            
            bool isGrounded = AnyNode(grid, node.X, node.Y - 1, agent.Width, 1, IsGround);
            if (isGrounded) {
                if (AllInColumn(grid, node.X - 1, node.Y, agent.Height, IsAir)) {
                    var neighbourNode = grid.GetNode(node.X - 1, node.Y);
                    var neighbour = new TransitionData(neighbourNode, TransitionType.Walk, 1);
                    neighbours.Add(neighbour);
                }

                if (AllInColumn(grid, node.X + agent.Width, node.Y, agent.Height, IsAir)) {
                    var neighbourNode = grid.GetNode(node.X + 1, node.Y);
                    var neighbour = new TransitionData(neighbourNode, TransitionType.Walk, 1);
                    neighbours.Add(neighbour);
                }

                for (var dx = 2; dx <= agent.JumpStrength; dx++)
                    CheckJump(grid, agent, neighbours, node, dx);

                for (var dx = -2; dx >= -agent.JumpStrength; --dx)
                    CheckJump(grid, agent, neighbours, node, dx);
            }
            else {
                Node landing = GetFallOnGroundNode(grid, agent, node.X, node.Y);
                if (landing != null) {
                    int fallHeight = node.Y - landing.Y;

                    if (fallHeight <= agent.FallLimit) {
                        const TransitionType fall = TransitionType.Fall;
                        var neighbour = new TransitionData(landing, fall, GetCost(agent, node, landing, fall));
                        neighbours.Add(neighbour);
                    }
                }
            }

            return neighbours;
        }

        void CheckJump(PathFindingGrid grid, PathFindingAgent agent, List<TransitionData> neighbours, 
            Node node, int dx) {
            
            Node landing = GetFallOnGroundNode(grid, agent, node.X + dx, node.Y + agent.JumpStrength);
            if (landing == null)
                return;

            int fallHeight = node.Y + agent.JumpStrength - landing.Y;
            if (fallHeight > agent.FallLimit)
                return;

            if (CheckTrajectory(grid, agent, node, landing)) {
                const TransitionType jump = TransitionType.Jump;
                var neighbour = new TransitionData(landing, jump, GetCost(agent, node, landing, jump));
                neighbours.Add(neighbour);
            }
        }
        
        static Node GetFallOnGroundNode(PathFindingGrid grid, PathFindingAgent agent, int x, int y) {
            if (!AllInRow(grid, x, y, agent.Width, node => node != null))
                return null;

            while (AllInRow(grid, x, y - 1, agent.Width, IsAir))
                --y;
            
            return grid.GetNode(x, y);
        }

        static bool CheckTrajectory(PathFindingGrid grid, PathFindingAgent agent, Node jumpStart, Node jumpEnd) {
            int dx = jumpEnd.X - jumpStart.X,
                dy = jumpEnd.Y - jumpStart.Y;
            
            var offsets = agent.GetCheckNodes(dx, dy);

            foreach (var offset in offsets) {
                Node toCheck = grid.GetNode(jumpStart.X + offset.x, jumpStart.Y + offset.y);
                if (!IsAir(toCheck))
                    return false;
            }

            return true;
        }

        static bool AllInRow(PathFindingGrid grid, int xStart, int yStart, int xCount, 
            Func<Node, bool> checkFunc) {
            for (int x = 0; x < xCount; x++) {
                if (!checkFunc(grid.GetNode(xStart + x, yStart)))
                    return false;
            }

            return true;
        }

        static bool AllInColumn(PathFindingGrid grid, int xStart, int yStart, int yCount, 
            Func<Node, bool> checkFunc) {
            for (int y = 0; y < yCount; y++) {
                if (!checkFunc(grid.GetNode(xStart, yStart + y)))
                    return false;
            }

            return true;
        }

        static bool AnyNode(PathFindingGrid grid, int xStart, int yStart, int xCount, int yCount, 
            Func<Node, bool> checkFunc) {

            for (int x = 0; x < xCount; x++) {
                for (int y = 0; y < yCount; y++) {
                    if (checkFunc(grid.GetNode(xStart + x, yStart + y)))
                        return true;    
                }
            }

            return false;
        }
    }
}