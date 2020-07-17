using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;

namespace Navigation
{    
    public class NavTreeAgent : MonoBehaviour
    {
        private NavTreeManager ntMng;

        private NavTree currentTree;

        private bool waitingForRoute;

        private Vector3 destination;
        public Vector3 Destination {
            get { return destination; }
            set {
                destination = value;
                if (route == null) {
                    // Debug.Log("Requesting Route to: " + destination);
                    RequestRouteAsync();
                } 
                else if (!waitingForRoute) {
                    // Debug.Log("Requesting Route to: " + destination);
                    RequestRouteAsync();
                }
            }
        }

        public float size;

        private Route route;

        private bool atDestination;
        public bool AtDestination { get { return atDestination; } }

        private NavPoint current;
        public Vector3 NextPosition {
            get {
                if (atDestination || current == null) {
                    return destination;
                } else {
                    return current.GetPosition();
                }
            }
        }

        private NavPoint destinationPoint;

        public float SquaredDistanceToTarget {
            get {
                return (destination - transform.position).sqrMagnitude;
            }
        }
        private void Start() {
            ntMng = NavTreeManager.instance;
        }

        public void CheckRoute(Vector3 destinationPosition) {
            // if (waitingForRoute) {
            //     Debug.Log("Waiting");
            //     return;
            // }
            if (route == null) {
                Destination = destinationPosition;
                atDestination = false;
            } else {
                current = route.GetNextPoint(transform.position);
                if (current == null) {
                    route = null;
                    return;
                }
                atDestination = route.AtDestination();
            }
        }

        private void OnDrawGizmos() {
            // if (route == null)
            //         return;
            // for (int i = 0; i < route.points.Count - 1; i++) {
            //     Handles.Label(route.points[i].GetPosition(), i.ToString());
            // }  
        }

        private void Update() {
            // if (route != null && route.points.Count > 0) {
            //     DrawRoute();
            // }
        }

        public void DrawRoute() {
            // Debug.DrawLine(route.points[route.current].GetPosition(), transform.position, Color.green);

            for (int i = 0; i < route.points.Count - 1; i++) {
                if (route == null)
                    return;
                Color32 c = new Color32(255, (byte) (i * 25 + 125), 0, 255);
                Debug.DrawLine(route.points[i].GetPosition(), route.points[i + 1].GetPosition(), c);
            }
        }

        public float GetTargetSpeed() {
            return 1f;
            // float sum = 0f;
            // if (SquaredDistanceToTarget < 200f) {
            //     return 0.3f;
            // }
            // if (route == null) {
            //     return 0.8f;
            // }

            // if (route.current < 0) {
            //     return 0.3f;
            // }
            // Vector3 previous = route.points[route.current].GetPosition();
            // for (int i = 0; i < 3 && route.current - i > 0; i++) {
            //     if (route.points.Count <= route.current - i) {
            //         continue;
            //     }
            //     Vector3 next = route.points[route.current - i].GetPosition();
            //     if (route.current - i - 1 < 0 || route.current - i - 1 > route.points.Count - 1) {
            //         return 0.8f;
            //     }
            //     sum += Vector3.Dot((previous - next).normalized, (next - route.points[route.current - i - 1].GetPosition()).normalized);
            //     previous = next;
            // }
            // return 1f - sum / 3f;
        }

        /// <summary>
        /// Creates a Task that will set the agent's current Route once the route has been found by the NavTreeManager.
        /// When debugging navigation use RequestRoute() instead. Errors finding async routes are not reported as they are not on the main thread.
        /// </summary>
        private void RequestRouteAsync() {
            waitingForRoute = true;
            Task<Route> getRoute = ntMng.GetRouteAsync(transform.position + transform.forward * size, destination, size);
            // the ContinueWith method will be called once the Task has finished.
            getRoute.ContinueWith((task) => { SetRoute(task.Result); }); 
        }

        /// <summary>
        /// Sets the current route, and sets waiting for route to false, allowing the agent to make new route requests
        /// </summary>
        /// <param name="route">The new best route to the destination</param>
        private void SetRoute(Route route) {
            waitingForRoute = false;
            this.route = route;
            destinationPoint = route.points.Last();
        }

        /// <summary>
        /// Requests and sets a route on the main thread, used for testing only (Errors are not reported on other threads)
        /// </summary>
        private void RequestRoute() {
            waitingForRoute = false;
            this.route = ntMng.GetRoute(transform.position, destination, size);
            destinationPoint = route.points.Last();
            DrawRoute();
        }   
    }
}