using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation {
	public class SpatialNode : IPersitable, INavigable, INavigableCollection
	{
		public SpatialNode[] children;
		public bool obstructed = false;

		private bool containsStatic;
		public bool ContainsStatic {
			get { return containsStatic; }
		}

		private bool isStatic;
		public bool Static {
			get { return isStatic; }
		}

		public float size;
		public Vector3 position;
		public SpatialNode parent;
		public int[] path;

		private NavTree tree;

		private Dictionary<string, Func<int, int>> shift;

		private Dictionary<string, Func<bool>> ancestorPaths;

		
		public static Dictionary<string, int[]> childMap = new Dictionary<string, int[]>() {
			{"y+", new int[] { 0, 1, 4, 5 }},
			{"y-", new int[] { 2, 3, 6, 7 }},
			{"x+", new int[] { 1, 3, 5, 7 }},
			{"x-", new int[] { 0, 2, 4, 6 }},
			{"z+", new int[] { 4, 5, 6, 7 }},
			{"z-", new int[] { 0, 1, 2, 3 }}
		};
		
		public static Dictionary<string, string> invert = new Dictionary<string, string> {
			{"y+", "y-"},
			{"y-", "y+"},
			{"x+", "x-"},
			{"x-", "x+"},
			{"z+", "z-"},
			{"z-", "z+"},
		};

		public SpatialNode(Vector3 position, float size, int[] path, SpatialNode parent, NavTree tree) {
			this.position = position;
			this.size = size;
			this.path = path;
			this.parent = parent;
			this.tree = tree;

			BuildInitial();
				
			ancestorPaths = new Dictionary<string, Func<bool>>() {
				{"y+", Top},
				{"y-", Bottom},
				{"x+", Right},
				{"x-", Left},
				{"z+", Front},
				{"z-", Back},
			};

			shift = new Dictionary<string, Func<int, int>>() {
				{"x+", ShiftX},
				{"x-", ShiftX},
				{"y+", ShiftY},
				{"y-", ShiftY},
				{"z+", ShiftZ},
				{"z-", ShiftZ},
			};
		}


		public SpatialNode(Vector3 position, float size, int[] path, SpatialNode parent, NavTree tree, List<Vector3> vertices) {
			this.position = position;
			this.size = size;
			this.path = path;
			this.parent = parent;
			this.tree = tree;

			Build(vertices);
				
			InitializeAncestors();
		}

		public SpatialNode(NavTreeReader reader, SpatialNode parent, NavTree tree) {
			this.tree = tree;
			if (parent != null) {
				Load(reader, parent);
			} else {
				Load(reader);
			}
			InitializeAncestors();
		}

		/// <summary>
		/// Initializes ancestor paths for NavTree, must be called after the tree is initialized as the methods are non-static
		/// </summary>
		private void InitializeAncestors() {
			ancestorPaths = new Dictionary<string, Func<bool>>() {
				{"y+", Top},
				{"y-", Bottom},
				{"x+", Right},
				{"x-", Left},
				{"z+", Front},
				{"z-", Back},
			};

			shift = new Dictionary<string, Func<int, int>>() {
				{"x+", ShiftX},
				{"x-", ShiftX},
				{"y+", ShiftY},
				{"y-", ShiftY},
				{"z+", ShiftZ},
				{"z-", ShiftZ},
			};
		}

		public void BuildInitial() {
			// check if this node is obstructed
			if (Physics.CheckBox(position, Vector3.one * (size / 2), Quaternion.identity, NavTreeManager.instance.envMask)) { 
					obstructed = true;
				
					// if the node is below the max tree depth then create children 
					if (path.Length < NavTreeManager.instance.maxTreeDepth) {
						children = new SpatialNode[8];
						for (int i = 0; i < 8; i++) {
							// assemble path
						int[] childPath = new int[path.Length + 1];
						for (int k = 0; k < path.Length; k++) {
							childPath[k] = path[k];
						}
						childPath[path.Length] = i;

							// start at center of bottom right front corner( one * size / 4)
							// add size to x if index is even 
							// add size to y if index / 2 is equal to 0 or 2
							// add size to z if index / 4 is greater than 1
							Vector3 childPosition = 
									position 
									- Vector3.one * size / 4 
									+ new Vector3(
										(i % 2) * (size / 2),
										(i / 2) % 2 == 0 || (i / 2) % 2 == 2 ? size / 2 : 0,
										(i / 4) * size / 2
									);

							children[i] = new SpatialNode(
									childPosition, 
									size / 2,
									childPath,
									this,
									tree
							);
						}
					} else {
					isStatic = true;
					parent.SetContainsStaticRecursive(true);
				}
			}
		}

		public void Build(List<Vector3> vertices) {
			List<Vector3> verticesInBounds = new List<Vector3>();
			obstructed = false;
			for (int i = 0; i < vertices.Count; i++) {
				if (InBounds(vertices[i])) {
					verticesInBounds.Add(vertices[i]);
					obstructed = true;
				}
			}

			if (obstructed && path.Length < NavTreeManager.instance.maxTreeDepth) {
				children = new SpatialNode[8];
				for (int i = 0; i < 8; i++) {
					// assemble path
					int[] childPath = new int[path.Length + 1];
					for (int k = 0; k < path.Length; k++) {
						childPath[k] = path[k];
					}
					childPath[path.Length] = i;

					// start at center of bottom right front corner( one * size / 4)
					// add size to x if index is even 
					// add size to y if index / 2 is equal to 0 or 2
					// add size to z if index / 4 is greater than 1
					Vector3 childPosition = 
						position 
						- Vector3.one * size / 4 
						+ new Vector3(
							(i % 2) * (size / 2),
							(i / 2) % 2 == 0 || (i / 2) % 2 == 2 ? size / 2 : 0,
							(i / 4) * size / 2
						);

					children[i] = new SpatialNode(
						childPosition, 
						size / 2,
						childPath,
						this,
						tree,
						verticesInBounds
					);
				}
			}
		}

		public void Save(NavTreeWriter writer) {
			// Save position, size, and obstructed status
			writer.Write(position);
			writer.Write(size);
			writer.Write(obstructed);
			writer.Write(containsStatic);
			writer.Write(isStatic);
			writer.Write(path.Length);
			for (int i = 0; i < path.Length; i++) {
				writer.Write(path[i]);
			}
			// Call Save on all children
			if (children != null && children.Length > 0) {
				writer.Write(true);
				for (int i = 0; i < children.Length; i++)
					children[i].Save(writer);
			} else {
				writer.Write(false);
			}
		}

		public void Load(NavTreeReader reader) {
			// read position and size
			position = reader.ReadVector3();
			size = reader.ReadFloat();

			// read obstructed status
			obstructed = reader.ReadBool();
			isStatic = reader.ReadBool();
			containsStatic = reader.ReadBool();
			// Load path
			int pathLength = reader.ReadInt();
			path = new int[pathLength];
			for (int i = 0; i < pathLength; i++) {
				path[i] = reader.ReadInt();
			}

			bool hasChildren = reader.ReadBool();
			if (hasChildren) {
				children = new SpatialNode[8];
				for (int i = 0; i < 8; i++) {
					children[i] = new SpatialNode(reader, this, tree);
				}
			}
		}

		public void Load(NavTreeReader reader, SpatialNode parent) {
			this.parent = parent;
			Load(reader);
		}

		public void Rebuild(List<Vector3> vertices) {
			if (isStatic) {
				// if a node is static, then it is a leaf node and will always retain it's state
				return;
			}
			if (containsStatic) {
				// if a node contains a static node, then it must be a parent node.
				for (int i = 0; i < 8; i++) {
					children[i].Rebuild(vertices);
				}
			} else {
				// a node that is non-static and does not contain a static node must be a leaf node, or a non-static node that contains a temporary obstruction.
				List<Vector3> verticesInBounds = new List<Vector3>();
				obstructed = false;
				for (int i = 0; i < vertices.Count; i++) {
					if (InBounds(vertices[i])) {
						obstructed = true;
						verticesInBounds.Add(vertices[i]);
					}
				}

				// if this node is obstructed, then it's children must be build downward
				if (obstructed && path.Length < NavTreeManager.instance.maxTreeDepth) {
					children = new SpatialNode[8];
					for (int i = 0; i < 8; i++) {
						// assemble path
						int[] childPath = new int[path.Length + 1];
						for (int k = 0; k < path.Length; k++) {
							childPath[k] = path[k];
						}
						childPath[path.Length] = i;

						// start at center of bottom right front corner( one * size / 4)
						// add size to x if index is even 
						// add size to y if index / 2 is equal to 0 or 2
						// add size to z if index / 4 is greater than 1
						Vector3 childPosition = 
							position 
							- Vector3.one * size / 4 
							+ new Vector3(
								(i % 2) * (size / 2),
								(i / 2) % 2 == 0 || (i / 2) % 2 == 2 ? size / 2 : 0,
								(i / 4) * size / 2
							);

						children[i] = new SpatialNode(
							childPosition, 
							size / 2,
							childPath,
							this,
							tree,
							verticesInBounds
						);
					}
				} else {
					// if this node contains no vertices, delete all of it's children
					children = null;
				}
			}
		}

		public void SetContainsStaticRecursive(bool state) {
			containsStatic = state;
			if (parent != null && !parent.ContainsStatic) {
				parent.SetContainsStaticRecursive(state);
			}
		}

		/// <summary>
		/// Gets the position of this spatial node
		/// </summary>
		/// <returns>position of this spatial node</returns>
		public Vector3 Position() {
			return position;
		}

		public bool Obstructed(float objectSize) {
			return obstructed || size <= objectSize;
		}

		public List<INavigable> Neighbors() {
			var l = new List<INavigable>();
			l.AddRange(NeighborsInDirection("y+"));
			l.AddRange(NeighborsInDirection("y-"));
			l.AddRange(NeighborsInDirection("x+"));
			l.AddRange(NeighborsInDirection("x-"));
			l.AddRange(NeighborsInDirection("z+"));
			l.AddRange(NeighborsInDirection("z-"));
			return l;
		}

		public bool InBounds(Vector3 target) {
			bool withinX = target.x >= position.x - size / 2 && target.x <= position.x + size / 2;
			bool withinY = target.y >= position.y - size / 2 && target.y <= position.y + size / 2;
			bool withinZ = target.z >= position.z - size / 2 && target.z <= position.z + size / 2;
			return withinX && withinY && withinZ;
		}

		public INavigable NodeAt(Vector3 position) {
			if (children == null) {
				return this;
			} else {
				int index = XIndex(position.x) + 2 * YIndex(position.y) + 4 * ZIndex(position.z);
				if (index < 0 || children.Length <= index || children[index] == null) {
					return this;
				}
				return children[index].NodeAt(position);
			}
		}

		public SpatialNode ChildAt(int index) {
			return children == null ? this : children[index];
		}

		private List<SpatialNode> NeighborsInDirection(string direction) {
			int[] parentPath = Ancestor(direction);
			if (parentPath == null) {
				return new List<SpatialNode>();
			}

			int[] neighborPath = new int[path.Length];
			for (int i = 0; i < parentPath.Length - 1; i++) {
				neighborPath[i] = parentPath[i];
			}
			neighborPath[parentPath.Length - 1] = shift[direction].Invoke(parentPath[parentPath.Length - 1]);
			for (int i = parentPath.Length; i < path.Length; i++) {
				neighborPath[i] = shift[direction].Invoke(path[i]);
			}
			var c = tree.NodeAtPath(neighborPath).ChildrenInDirection(SpatialNode.invert[direction]);
			return tree.NodeAtPath(neighborPath).ChildrenInDirection(SpatialNode.invert[direction]);
		}

		private List<SpatialNode> ChildrenInDirection(string direction) {
			if (children == null) {
				return new List<SpatialNode> { this };
			} else {
				int[] ix = SpatialNode.childMap[direction];
					var l = new List<SpatialNode>();
				l.AddRange(children[ix[0]].ChildrenInDirection(direction));
					l.AddRange(children[ix[1]].ChildrenInDirection(direction));
				l.AddRange(children[ix[2]].ChildrenInDirection(direction));
					l.AddRange(children[ix[3]].ChildrenInDirection(direction));
					return l;
			}
		}

		private int[] Ancestor(string direction) {
			if (parent == null) {
				return null;
			} else if (ancestorPaths[direction].Invoke()) {
				return path;
			} else {
				return parent.Ancestor(direction);
			}
		}

		private static int ShiftX(int index) {
			if (index % 2 == 0) {
				return index + 1;
			}
			return index - 1;
		}

		private static int ShiftY(int index) {
			if (index / 2 == 0 || index / 2 == 2) {
				return index + 2;
			}
			return index - 2;
		}
		private static int ShiftZ(int index) {
			if (index < 4) {
				return index + 4;
			} 
			return index - 4;
		}

		private bool Top() { return path[path.Length - 1] / 2 == 1 || path[path.Length - 1] / 2 == 3; }
		private bool Bottom() {	return path[path.Length - 1] / 2 == 0 || path[path.Length - 1] / 2 == 2; }
		private bool Right() { return path[path.Length - 1] % 2 == 0; }
		private bool Left() { return path[path.Length - 1] % 2 == 1; }
		private bool Front() { return path[path.Length - 1] / 4 < 1; }
		private bool Back() { return path[path.Length - 1] / 4 > 0; }

		private int XIndex(float x) {
			return (int) Mathf.Floor((x - position.x) / size) + 1;
		}

		private int YIndex(float y) {
			return (int) Mathf.Floor((position.y - y + size) / size);
		}
		
		private int ZIndex(float z) {
			return (int) Mathf.Floor((size + z - position.z) / size);
		}
	}

}