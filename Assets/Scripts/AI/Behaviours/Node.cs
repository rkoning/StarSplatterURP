using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviourTree
{
    public enum State {
        Success,
        Failure,
        Running
    }

    public class Sequence : Node {
        public Node[] nodes;
        public int current = 0;
        public Sequence(Node[] nodes) {
            this.nodes = nodes;
        }
        
        public override State Evaluate() {
            State childState = nodes[current].Evaluate();
            // if the current node succeeded, report that success upwards
            // and move to the next node.
            if (childState == State.Success) {
                current++;

                // reset to beginning if we have finished the sequence
                if (current >= nodes.Length) {
                    current = 0;
                    return State.Success;
                } else {
                    return Evaluate();
                }

            // if the current node is still running, keep running
            } else if (childState == State.Running) {
                current = 0;
                return State.Running;

            // if the current node failed, then return a failure and return to the
            // beginning of the sequence
            } else {
                current = 0;
                return State.Failure;
            }
        }
    }

    public class Selector : Node {
        public Node[] nodes;
        public int current = 0;

        public Selector(Node[] nodes) {
            this.nodes = nodes;
        }

        public void AppendNode(Node n) {
            var ns = this.nodes;
            this.nodes = new Node[this.nodes.Length + 1];
            ns.CopyTo(this.nodes, 0);
            this.nodes[this.nodes.Length - 1] = n;
            // for (int i = 0; i < this.nodes.Length; i++) {
            //     Debug.Log(this.nodes[i].ToString());
            // }
        }

        public override State Evaluate() {
            if (nodes.Length < 1) {
                return State.Failure;
            }
            
            State childState = nodes[current].Evaluate(); 

            // if the current child Returns success, report successful
            // and return to start
            if (childState == State.Success) {
                current = 0;
                return State.Success;

            // if the current child is running, pass running and reset
            } else if(childState == State.Running) {
                current = 0;
                return State.Running;

            // if the current child has failed, evaluate the next child
            } else {
                current++;
                // if all children have failed, return a failure and reset
                if (current >= nodes.Length) {
                    current = 0;
                    return State.Failure;
                } else {
                    return Evaluate();
                }
            }
        }
    }

    public class Dynamic : Node {
        public Node node;

        public Dynamic(Node node) {
            this.node = node;
        }

        public void SetNode(Node node) {
            this.node = node;
        }

        public override State Evaluate() {
            return node.Evaluate();
        }
        
        public override string ToString() {
            return "Parallel Node";
        }
    }

    public class Parallel : Node {
        
        public Node[] nodes;

        public Parallel(Node[] nodes) {
            this.nodes = nodes;
        }

        public override State Evaluate() {
            State firstNodeState = nodes[0].Evaluate();
            for(int i = 1; i < nodes.Length; i++) {
                nodes[i].Evaluate();
            }
            return firstNodeState;
        }

        public override string ToString() {
            return "Parallel Node";
        }
    }

    public class Inverter : Node {
        public Inverter(Node child) {
            this.child = child;
        }

        public override State Evaluate() {
            State childState = child.Evaluate();
            if (childState == State.Success) {
                return State.Failure;
            } else if (childState == State.Failure) {
                return State.Success;
            } else {
                return State.Running;
            }
        }

        public override string ToString() {
            return "Inverter containing: " + child.ToString();
        }
    }

    public class Node {
        protected Node child;
        private System.Func<State> action;

        public Node() {}

        public Node(System.Func<State> action) {
            this.action = action;
        }
        
        public Node(System.Func<State> action, Node child) {
            this.action = action;
            this.child = child; 
        }

        public virtual State Evaluate() {
            State state = action.Invoke();
            // Debug.Log(action.Method.Name + " " + state);
            if (child != null && state == State.Success) {
                return child.Evaluate();
            }
            return state;
        }

        public override string ToString() {
            if (action != null) 
                return "Leaf Node containing: " + action.Method.Name;
            else
                return "Leaf Node";
        }
    }
}
