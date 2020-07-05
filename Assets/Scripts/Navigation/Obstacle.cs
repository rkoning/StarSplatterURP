using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Navigation {
    public class Obstacle : MonoBehaviour
    {

        private MeshFilter f;

        private List<Vector3> vertices;
        public List<Vector3> Vertices {
            get { return vertices; }
        }

        private List<NavTree> currentTrees = new List<NavTree>();

        private new Collider collider;
        private NavTreeManager ntm;

        void Start() {
            ntm = NavTreeManager.instance;
            collider = GetComponent<Collider>();
            f = GetComponent<MeshFilter>();
            if (f) {
                vertices = new List<Vector3>(f.mesh.vertices);
            } else {
                vertices = new List<Vector3>();
            }
            for (int i = 0; i < vertices.Count; i++) {
                vertices[i] = new Vector3(vertices[i].x * transform.localScale.x, vertices[i].y * transform.localScale.y, vertices[i].z * transform.localScale.z);
            }

            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
            foreach(MeshFilter m in meshes) {
                if (m == f) {
                    continue;
                }
                Vector3[] childVertices = m.mesh.vertices;
                Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, m.transform.forward);
                for (int i = 0; i < childVertices.Length; i++) {
                    childVertices[i] =
                        rotationDelta * new Vector3((m.transform.localPosition.x + childVertices[i].x * m.transform.localScale.x),
                                    (m.transform.localPosition.y + childVertices[i].y * m.transform.localScale.y), 
                                    (m.transform.localPosition.z + childVertices[i].z * m.transform.localScale.z));
                }
                vertices.AddRange(childVertices);
            }

            // Uncomment to draw the vertices of the obstace on start
            // for(int i = 0; i < vertices.Count - 1; i++) {
            //     Debug.DrawLine(vertices[i], vertices[i + 1], Color.yellow, 10f);
            // }
        }
    }
}