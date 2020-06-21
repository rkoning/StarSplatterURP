using System.Collections.Generic;

using UnityEngine;

// using Navigation;

namespace PlanetGeneration {
    [ExecuteInEditMode]
    public class Planet : MonoBehaviour {
        public List<BuildableLocation> orbitalBuildPoints = new List<BuildableLocation>();

        public List<GameObject> deposits = new List<GameObject>();

        // public List<NavTreeAnchor> navTrees = new List<NavTreeAnchor>();

        //    void Update()
        //    {
        //        foreach(var bl in orbitalBuildPoints) {
        //            Debug.DrawLine(bl.position, bl.position + bl.up * 100f);
        //        }
        //    }
        public void Reset() {
            for (int i = 0; i < deposits.Count; i++) {
                DestroyImmediate(deposits[i], true);
            }
            deposits.Clear();
            // for (int i = 0; i < navTrees.Count; i++) {
            //     DestroyImmediate(navTrees[i].gameObject, true);
            // }
            // navTrees.Clear();
            orbitalBuildPoints.Clear();
        }
    }

    public class BuildableLocation {
        public Vector3 position;
        public Vector3 up;
        public bool occupied;

        public BuildableLocation(Vector3 position, Vector3 up) {
            this.position = position;
            this.up = up;
        }
    }
}