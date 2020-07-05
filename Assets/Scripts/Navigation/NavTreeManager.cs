using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Navigation {
    [ExecuteInEditMode]
    public class NavTreeManager : MonoBehaviour
    {
        public static NavTreeManager instance;

        private Navigator navigator;

        public LayerMask envMask;
        public int maxTreeDepth = 7;

        public List<NavTree> trees = new List<NavTree>();

        private Obstacle[] obstacles;

        [SerializeField]
        private bool debugMode;

        public Transform start;
        public Transform goal;
        
        private void Awake() {
            instance = this;
            navigator = new Navigator();
            obstacles = FindObjectsOfType<Obstacle>();
        }

        private void OnEnable() {
            instance = this;        
        }
        public Task<Route> GetRouteAsync(Vector3 start, Vector3 goal, float minSize) {
            return Task.Factory.StartNew(() => {
                return GetRoute(start, goal, minSize);
            });
        }

        public Route GetRoute(Vector3 start, Vector3 goal, float minSize) {
            Route route = new Route();

            NavTree[] sorted = new NavTree[trees.Count];
            trees.CopyTo(sorted);
            sorted = sorted.OrderBy(t => (t.position - start).sqrMagnitude).ToArray();

            for (int i = 0; i < sorted.Length; i++) {
                NavTree t = sorted[i];
                Vector3[] clipped = sorted[i].GetIntersectPoints(start, goal);
                if (clipped.Length > 0) {
                    Route clipRoute = navigator.GetRoute(t, clipped[0], clipped[1], minSize);
                    clipRoute.points.Reverse();
                    route.Append(clipRoute);
                    // t.DrawNode(t.root, new Color32((byte) (75 + i * 50), 125, 125, 255));

                }
            }

            NavTree endTree = GetTreeAt(goal);
            if (endTree == null) {
                Route offTreeEnd = new Route();
                offTreeEnd.points.Add(new NavPoint(goal));
                route.Append(offTreeEnd);
            }

            return route;
        }

        private void Update() {
            if (debugMode) {
                foreach (var t in trees) {
                    HighlightChildren(t, t.root);                      
                }

                if (start && goal) {
                    foreach (NavTree t in trees) {
                        Vector3[] clipped = t.GetIntersectPoints(start.position, goal.position);
                        if (clipped.Length > 0) {
                            Debug.DrawLine(clipped[0] + Vector3.up * 10, clipped[0] - Vector3.up * 10, Color.green);
                            Debug.DrawLine(clipped[0] + Vector3.right * 10, clipped[0] - Vector3.right * 10, Color.green);
                            Debug.DrawLine(clipped[1] + Vector3.up * 10, clipped[1] - Vector3.up * 10, Color.red);
                            Debug.DrawLine(clipped[1] + Vector3.right * 10, clipped[1] - Vector3.right * 10, Color.red);
                            Debug.DrawLine(clipped[0], clipped[1], Color.yellow);
                        }
                    }
                }
            }
        }

        public void HighlightChildren(NavTree t, SpatialNode n) {
            t.DrawNode(n);
            if (n != null && n.children != null) {
                for (int i = 0; i < n.children.Length; i++) {
                    HighlightChildren(t, n.children[i]);
                }	
            } 
        }

        public NavTree GetTreeAt(Vector3 point) {
            foreach(NavTree t in trees) {
                if (t.InTree(point)) {
                    return t;
                }
            }
            return null;
        }

        public void AddNavTree(NavTree tree) {
            trees.Add(tree);
        }

        public void RemoveNavTree(NavTree tree) {
            trees.Remove(tree);
        }
    }
}