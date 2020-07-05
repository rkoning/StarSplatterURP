using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
    public class Navigator {

        public Route GetRoute(INavigableCollection tree, INavigable start, INavigable end, float minSize = 0) {
            
            Vector3 goalPosition = end.Position();
            Vector3 startPosition = start.Position();

            List<NavPoint> routePoints = new List<NavPoint>();

            PriorityQueue<INavigable> fringe = new PriorityQueue<INavigable>();
            fringe.Enqueue(start, 0);
            Dictionary<INavigable, INavigable> visited = new Dictionary<INavigable, INavigable>();
            visited.Add(start, null);
            Dictionary<INavigable, float> visitedCosts = new Dictionary<INavigable, float>();
            visitedCosts.Add(start, 0);
            INavigable current = null;

            while (!fringe.Empty()) {
                current = fringe.Dequeue();
                // Debug.DrawLine(current.Position(), current.Position() + Vector3.up * 50f, Color.cyan, 10f);
                if (current.InBounds(goalPosition)) {
                    break;
                }

                foreach (INavigable next in current.Neighbors()) {

                    float cost = (next.Position() - startPosition).sqrMagnitude;
                    if (next.Obstructed(minSize)) {
                        continue;
                    }
                    
                    if (!visited.ContainsKey(next)) {
                        visited.Add(next, current);
                    } else if (cost >= visitedCosts[next]) {
                        continue;
                    }
                    visitedCosts[next] = cost;
                    
                    float priority = cost + (next.Position() - goalPosition).sqrMagnitude;
                    fringe.Enqueue(next, priority);
                }
            }

            while(current != null) {
                routePoints.Add(new NavPoint(current, tree));
                if (current.InBounds(startPosition))
                    break;
                current = visited[current];
            }

            // routePoints.Reverse();
            var route = new Route();
            route.points = routePoints;
            return route;
        }

        public Route GetRoute(INavigableCollection tree, Vector3 startPosition, Vector3 goalPosition, float minSize = 0) {
            return GetRoute(tree, tree.NodeAt(startPosition), tree.NodeAt(goalPosition), minSize);
        }

        private float DistToNode(Vector3 position, SpatialNode node) {
            return (position - node.position).sqrMagnitude;
        }
    }

    public class NavPoint {
        private Vector3 position;
        public int[] path;
        public NavTree tree;

        public NavPoint(INavigable node, INavigableCollection tree) {
            if (node.GetType() == typeof(SpatialNode)) {
                this.path = ((SpatialNode) node).path;
                this.tree = (NavTree) tree;
                this.position = node.Position();
            } else {
                this.position = node.Position(); 
            }
        }

        public NavPoint(Vector3 position) {
            this.position = position;
        }

        public NavPoint(int[] path, NavTree tree) {
            this.path = path;
            this.tree = tree;
            position = this.tree.NodeAtPath(this.path).position;
        }

        public Vector3 GetPosition() {
            if (tree != null) {
                return this.tree.NodeAtPath(this.path).position;
            }
            return position;
        }

        public bool AtPoint(Vector3 point) {
            if (tree != null) {
                return this.tree.NodeAtPath(this.path).InBounds(point);
            }
            return (point - position).sqrMagnitude < 50 * 50;
        }
    }

    public class Route {
        public List<NavPoint> points = new List<NavPoint>();
        public int current;

        public Route() {}

        public NavPoint GetNextPoint(Vector3 position) {
            if (points.Count > 0) {
                NavPoint next = points[current];
                if (!next.AtPoint(position)) {
                    return next;
                } else if (!AtDestination()) {
                    if (points.Count - 1 == current) {
                        return null;
                    }
                    current++;
                    return points[current];
                }
            }
            return null;
        }

        public NavPoint GetDestination() {
            return points.Last();
        }
        public bool AtDestination() {
            return current >= points.Count;
        }

        public float GetTargetSpeed(int nodeCount) {
            float speed = 0f;
            if (points.Count < 1) {
                return 0.5f;
            }
            SpatialNode c = points[current].tree.NodeAtPath(points[current].path);
            for (int i = 0; i < nodeCount && current - i > 0; i++) {
                SpatialNode next = points[current].tree.NodeAtPath(points[current].path);
                speed += Vector3.Dot(c.position, next.position);
                c = next;
            }
            speed /= nodeCount;
            return speed;
        }

        public void Append(Route route) {
            // var points = route.points;
            // points.AddRange(this.points);
            // this.points = points;
            this.points.AddRange(route.points);
            // this.points.Reverse();
            // current = this.points.Count - 1;
        }
    }
}