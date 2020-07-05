using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Navigation {
	public class NavTree : IPersitable, INavigableCollection {

		public float maxNodeSize = 1000;
		public int maxTreeDepth = 7;
		public bool building;
		public delegate void RebuildEvent();
		public event RebuildEvent OnRebuild;

		public Vector3 position;

		[HideInInspector]
		public SpatialNode root;

		public Dictionary<Obstacle, int> obstacles = new Dictionary<Obstacle, int>();
		
		private HashSet<SpatialNode> nodeBuffer = new HashSet<SpatialNode>();

		public NavTree(float maxNodeSize, int maxTreeDepth, Vector3 position) {
			this.maxNodeSize = maxNodeSize;
			this.maxTreeDepth = maxTreeDepth;
			this.position = position;
			NavTreeManager.instance.AddNavTree(this);

			root = new SpatialNode(position, maxNodeSize, new int[] {0}, null, this);
		}

		public NavTree(float maxNodeSize, int maxTreeDepth, Vector3 position, NavTreeReader reader) {
			this.maxNodeSize = maxNodeSize;
			this.maxTreeDepth = maxTreeDepth;
			this.position = position;

			Load(reader);
			NavTreeManager.instance.AddNavTree(this);
		}

		/// <summary>
		/// Returns true if the point is within bounds of this NavTree
		/// </summary>
		/// <param name="point">Point to check if in bounds</param>
		/// <returns>true if the point is within bounds of this NavTree</returns>
		public bool InTree(Vector3 point) {
			bool x = point.x < position.x + maxNodeSize / 2 && point.x > position.x - maxNodeSize / 2;
			bool y = point.y < position.y + maxNodeSize / 2 && point.y > position.y - maxNodeSize / 2;
			bool z = point.z < position.z + maxNodeSize / 2 && point.z > position.z - maxNodeSize / 2;
			return x && y && z;
		}

		public void Save(NavTreeWriter writer) {
			root.Save(writer);
		}

		public void Load(NavTreeReader reader) {
			root = new SpatialNode(reader, null, this);
		}

		public INavigable NodeAt(Vector3 position) {
			return root.NodeAt(position);
		}

		public SpatialNode NodeAtPath(int[] path) {
			SpatialNode current = root;
			for(int i = 1; i < path.Length; i++) {
				SpatialNode last = current;
				current = current.ChildAt(path[i]);
				if (current == null) {
					// DrawNode(last);
					return last;
				}
			}
			// DrawNode(current);

			return current;
		}

		public SpatialNode NodeAtPath(int[] path, SpatialNode root) {
			SpatialNode current = root;
			for(int i = 1; i < path.Length; i++) {
				SpatialNode last = current;
				current = current.ChildAt(path[i]);
				if (current == null) {
					return last;
				}
			}
			return current;
		}

		public void RequestRebuild() {
			// if (obstacles.Count < 1) {
			// 	return;
			// }
			RebuildRequest[] rebuilds = new RebuildRequest[obstacles.Count];
			// Debug.Log(obstacles.Count);
			int i = 0;
			foreach(KeyValuePair<Obstacle, int> kvp in obstacles) {
				var obstacle = kvp.Key;
				rebuilds[i].vertices = obstacle.Vertices;
				rebuilds[i].position = obstacle.transform.position;
				rebuilds[i].rotation = obstacle.transform.rotation;
				i++;
			}
			// Task<SpatialNode> getRebuild = RebuildAsync(rebuilds);
			// getRebuild.ContinueWith((task) => { Rebuilt(task.Result); } );
			Rebuilt(Rebuild(rebuilds)); // Uncomment to test Rebuilds on main thread
		}

		public void Rebuilt(SpatialNode rebuiltRoot) {
			this.root = rebuiltRoot;
			building = false;
		}

		public Task<SpatialNode> RebuildAsync(RebuildRequest[] obstacles) {
			return Task.Factory.StartNew(() => {
				return Rebuild(obstacles);
			});
		}

		public SpatialNode Rebuild(RebuildRequest[] obstacleRequests) {

			SpatialNode rootCopy = root;
			HashSet<SpatialNode> currentNodes = new HashSet<SpatialNode>();
			List<Vector3> allVertices = new List<Vector3>();
			foreach (var o in obstacleRequests) {
				List<Vector3> worldSpaceVertices = new List<Vector3>(o.vertices);
				for (int i = 0; i < o.vertices.Count; i++) {
					currentNodes.Add((SpatialNode) rootCopy.NodeAt(o.position + o.rotation * o.vertices[i]));
					worldSpaceVertices[i] = o.position + o.rotation * o.vertices[i];
				}
				allVertices.AddRange(worldSpaceVertices);
			}
			HashSet<SpatialNode> newNodes = new HashSet<SpatialNode>(currentNodes);
			newNodes.ExceptWith(nodeBuffer);
			HashSet<SpatialNode> old = new HashSet<SpatialNode>(nodeBuffer);
			old.ExceptWith(currentNodes);

			newNodes.UnionWith(old);
			HashSet<SpatialNode> parents = new HashSet<SpatialNode>();
			foreach (var n in newNodes) {
				parents.Add(LargestNonStaticParent(n));
			}

			foreach (var n in parents) {
				n.Rebuild(allVertices);
			}

			currentNodes = new HashSet<SpatialNode>();
			foreach (var o in obstacleRequests) {
				for (int i = 0; i < o.vertices.Count; i++) {
					currentNodes.Add((SpatialNode) rootCopy.NodeAt(o.position + o.rotation * o.vertices[i]));
				}
				allVertices.AddRange(o.vertices);
			}
			nodeBuffer = currentNodes;
			return rootCopy;
		}
		
		private SpatialNode LargestNonStaticParent(SpatialNode n) {
			if (n.parent == null) {
					return n;
			} else if (n.parent.ContainsStatic) {
				return n.parent;
			} else {
					return LargestNonStaticParent(n.parent);
			}
		}

		public Vector3[] GetIntersectPoints(Vector3 p1, Vector3 p2) {
			Vector3 minBounds = position - maxNodeSize / 2 * Vector3.one;
			Vector3 maxBounds = position + maxNodeSize / 2 * Vector3.one;
			int INSIDE = 0;
			int LEFT = 1;
			int RIGHT = 2;
			int BOTTOM = 4;
			int TOP = 8;
			int FRONT = 16;
			int BACK = 32;

			int ComputeCode(Vector3 point) {
				int code = INSIDE;
				if (point.x < minBounds.x) {
					code |= LEFT;
				} else if (point.x > maxBounds.x) {
					code |= RIGHT;
				}
				if (point.y < minBounds.y) {
					code |= BOTTOM;
				} else if (point.y > maxBounds.y) {
					code |= TOP;
				}
				if (point.z < minBounds.z) {
					code |= FRONT;
				} else if (point.z > maxBounds.z) {
					code |= BACK;
				}
				return code;
			}

			int p1Code = ComputeCode(p1);
			int p2Code = ComputeCode(p2);

			bool accept = false;
			
			while (true) {
				if (p1Code == 0 && p2Code == 0) {
					// both inside
					accept = true;
					break;
				}

				if ((p1Code & p2Code) != 0) {
					// both outside and in same region
					break;
				}

				int codeOut;
				Vector3 point = Vector3.zero;

				if (p1Code != 0)
					codeOut = p1Code;
				else
					codeOut = p2Code;

				if ((codeOut & TOP) != 0) {
					point.x = p1.x + (p2.x - p1.x) * (maxBounds.y - p1.y) / (p2.y - p1.y);
					point.y = maxBounds.y - 0.5f;
					point.z = p1.z + (p2.z - p1.z) * (maxBounds.y - p1.y) / (p2.y - p1.y);
				} else if ((codeOut & BOTTOM) != 0) {
					point.x = p1.x + (p2.x - p1.x) * (minBounds.y - p1.y) / (p2.y - p1.y);
					point.y = minBounds.y + 0.5f;
					point.z = p1.z + (p2.z - p1.z) * (minBounds.y - p1.y) / (p2.y - p1.y);
				} else if ((codeOut & RIGHT) != 0) {
					point.x = maxBounds.x - 0.5f;
					point.y = p1.y + (p2.y - p1.y) * (maxBounds.x - p1.x) / (p2.x - p1.x);
					point.z = p1.z + (p2.z - p1.z) * (maxBounds.x - p1.x) / (p2.x - p1.x);
				} else if ((codeOut & LEFT) != 0) {
					point.x = minBounds.x + 0.5f;
					point.y = p1.y + (p2.y - p1.y) * (minBounds.x - p1.x) / (p2.x - p1.x);
					point.z = p1.z + (p2.z - p1.z) * (minBounds.x - p1.x) / (p2.x - p1.x);
				} else if ((codeOut & FRONT) != 0) {
					point.x = p1.x + (p2.x - p1.x) * (minBounds.z - p1.z) / (p2.z - p1.z);
					point.y = p1.y + (p2.y - p1.y) * (minBounds.z - p1.z) / (p2.z - p1.z);
					point.z = minBounds.z + 0.5f;
				} else if ((codeOut & BACK) != 0) {
					point.x = p1.x + (p2.x - p1.x) * (maxBounds.z - p1.z) / (p2.z - p1.z);
					point.y = p1.y + (p2.y - p1.y) * (maxBounds.z - p1.z) / (p2.z - p1.z);
					point.z = maxBounds.z - 0.5f;
				}

				if (codeOut == p1Code) {
					p1 = point;
					p1Code = ComputeCode(p1);
				} else  {
					p2 = point;
					p2Code = ComputeCode(p2);
				}
			}
			if (accept) {
				return new Vector3[] {p1, p2};
			} else {
				return new Vector3[] {};
			}
		}

		public void AddObstacle(Obstacle o) {
			if (obstacles.ContainsKey(o)) {
				obstacles[o] += 1;
			} else {
				obstacles.Add(o, 1);
			}
		}

		public void RemoveObstacle(Obstacle o) {
			if (obstacles[o] > 1) {
				obstacles[o] -= 1;
			} else {
				obstacles.Remove(o);
			}
		}

		public void DrawNode(SpatialNode node) {
			if (node == null) { return; }
			DrawNode(node, node.obstructed ? Color.black : Color.white);
		}

		public void DrawNode(SpatialNode node, Color color) {
			if (node == null) {
				return;
			}
			var position = node.position;
			var size = node.size;
			var botCorner = position - Vector3.one * size / 2;
			var topCorner = position + Vector3.one * size / 2;
			
			float dT = Time.deltaTime + 0.05f;
			Debug.DrawLine(botCorner, botCorner + Vector3.up * size, color, dT);
			
			Debug.DrawLine(botCorner, botCorner + Vector3.right * size, color, dT);
			
			Debug.DrawLine(botCorner, botCorner + Vector3.forward * size, color, dT);
			
			Debug.DrawLine(topCorner, topCorner - Vector3.up * size, color, dT);
			
			Debug.DrawLine(topCorner, topCorner - Vector3.right * size, color, dT);
			
			Debug.DrawLine(topCorner, topCorner - Vector3.forward * size, color, dT);

			var topRight = position + new Vector3(1, 1, -1) * size / 2;
			Debug.DrawLine(topRight, topRight - Vector3.up * size, color, dT);
			Debug.DrawLine(topRight, topRight - Vector3.right * size, color, dT);
			
			var topLeft = position + new Vector3(-1, 1, 1) * size / 2;
			Debug.DrawLine(topLeft, topLeft - Vector3.up * size, color, dT);
			Debug.DrawLine(topLeft, topLeft - Vector3.forward * size, color, dT);

			var botForward = position + new Vector3(1, -1, 1) * size / 2;
			Debug.DrawLine(botForward, botForward - Vector3.forward * size, color, dT);
			Debug.DrawLine(botForward, botForward - Vector3.right * size, color, dT);
		}

		public void VisualizeBounds() {
			DrawNode(root, Color.cyan);
		}
	}

	public struct RebuildRequest {
		public List<Vector3> vertices;
		public Vector3 position;
		public Quaternion rotation;
	}
}