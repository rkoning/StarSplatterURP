using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Navigation {
    public class NavTreeAnchor : MonoBehaviour
    {

        public float maxNodeSize = 1000;
        public int maxTreeDepth = 7;

        private NavTree tree;

        public string fileName;
        private string savePath;

        public bool tryLoad = false;

        void Start()
        {
            // if (tryLoad && FileExists()) {
            //     LoadFromFile();
            // } else {
            //     tree = new NavTree(maxNodeSize, maxTreeDepth, transform.position);
            // }
            tree = new NavTree(maxNodeSize, maxTreeDepth, transform.position);
        }

        private void Update() {
            if (!tree.building) {
                tree.building = true;
                tree.RequestRebuild();
            }
        }

        private void OnDestroy() {
            NavTreeManager.instance.RemoveNavTree(tree);
        }

        private void OnTriggerEnter(Collider other) {
            Obstacle obs = GetObstacleInOtherOrParent(other.gameObject);
            if (obs) {
                tree.AddObstacle(obs);  
            }
        }

        private void OnTriggerExit(Collider other) {
            Obstacle obs = GetObstacleInOtherOrParent(other.gameObject);
            if (obs) {
                tree.RemoveObstacle(obs);  
            }
        }

        private Obstacle GetObstacleInOtherOrParent(GameObject other) {
            var obs = other.GetComponent<Obstacle>();
            if (obs) {
                return obs;
            } else {
                return other.GetComponentInParent<Obstacle>();
            }
        }

        public void BuildAndSave() {
            savePath = Navigation.Config.NavTreeSavePath + fileName + Navigation.Config.NavTreeExtention;
            Debug.Log("Saving NavTree " + name + " to file: " + savePath);
            tree = new NavTree(maxNodeSize, maxTreeDepth, transform.position);
            using (
                var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
            ) {
                tree.Save(new NavTreeWriter(writer));
                Debug.Log("NavTree Saved!");
            }
        }
        
        public void LoadFromFile() {
            Debug.Log("loading tree from file...");
            using (
                var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
            ) {
                tree = new NavTree(maxNodeSize, maxTreeDepth, transform.position, new NavTreeReader(reader));
                Debug.Log("NavTree loaded from file: " + savePath + "!");
            }
        }

        public bool FileExists() {
            savePath = Navigation.Config.NavTreeSavePath + fileName + Navigation.Config.NavTreeExtention;
            return File.Exists(savePath);
        }
    }
}