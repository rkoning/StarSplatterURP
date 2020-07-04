using System.Collections.Generic;
using UnityEngine;
// using Navigation;

namespace PlanetGeneration
{
    [ExecuteInEditMode]
    public class PlanetGenerator : MonoBehaviour
    {

        public List<Biome> biomes;
        public bool hasRings;
        public bool hasAtmosphere;
        public bool hasWater;
        private GameObject water;
        private Material waterMaterial;

        private MeshGenerator generator;
        private NoiseDensity density;

        public Transform realPlanet;
        public Transform scaledPlanet;

        private RingGenerator ringGenerator;
        private GameObject ringChunkHolder;

        public GameObject waterPrefab;

        public Biome selected;

        public float orbitalRadius;
        public float orbitalRadiusScale = 1.25f;

        public float ringInnerRadiusScale = 3.5f;
        public float ringOuterRadiusScale = 4.5f;

        public GameObject planetPrefab;

        // public float navTreeSize;
        // public GameObject navTreeAnchorPrefab;
        // public List<GameObject> navTrees;
        // private Planet planet;

        public LayerMask envMask;

        private TextureMinMax textureMinMax;

        private void Awake() {
            generator = GetComponent<MeshGenerator>();    
            density = GetComponent<NoiseDensity>();
            ringGenerator = GetComponentInChildren<RingGenerator>();
        }

        private void Start() {
            // Setup();
            // Generate();
            var a = generator.chunkHolder.GetComponentsInChildren<MeshCollider>();
            for (int i = 0; i < a.Length; i++) {
                a[i].enabled = false;
                a[i].enabled = true;
            }
            // PlacePrefabs();
        }

        public void GenerateFull() {
            var planetGO = GameObject.Instantiate(planetPrefab);
            var planet = planetGO.GetComponent<Planet>();
            scaledPlanet = planet.scaledChunkHolder;
            realPlanet = planet.realChunkHolder;
            Setup();
            GenerateScaled();
            GenerateReal();
            realPlanet.SetParent(null);
            realPlanet.gameObject.SetActive(false);
        }

        public void Setup() {
            // set planet biome
            selected = biomes[Random.Range(0, biomes.Count)];
            // set planet params
            density.numOctaves = Random.Range(selected.minOctaves, selected.maxOctaves);
            density.lacunarity = Random.Range(selected.minLacunarity, selected.maxLacunarity);
            density.persistence = Random.Range(selected.minPersistence, selected.maxPersistence);
            density.noiseScale = Random.Range(selected.minNoiseScale, selected.maxNoiseScale);
            density.noiseWeight = Random.Range(selected.minNoiseWeight, selected.maxNoiseWeight);
            density.weightMultiplier = Random.Range(selected.minWeightMultipiler, selected.maxWeightMultipiler);
            density.radius = Random.Range(selected.minRadius, selected.maxRadius);

            orbitalRadius = density.radius * orbitalRadiusScale;
            
            // set ring params
            hasRings = Random.Range(0f, 1f) <= selected.ringsChance;

            // roll water
            hasWater = Random.Range(0f, 1f) <= selected.waterChance;
        
            // roll atmosphere
            hasAtmosphere = Random.Range(0f, 1f) <= selected.atmosphereChance;

            density.seed = Random.Range(int.MinValue, int.MaxValue);
        }

        private Material CreateSurfaceMaterial(Material baseMaterial, float min, float max) {
            Material mat = new Material(Shader.Find("Shader Graphs/Cel-shaded Planet"));
            mat.SetTexture("_Texture2D", baseMaterial.GetTexture("_Texture2D"));
            mat.SetTexture("_LightRamp", baseMaterial.GetTexture("_LightRamp"));
            mat.SetTexture("_NormalTex", baseMaterial.GetTexture("_NormalTex"));
            mat.SetFloat("_Min_Dist", min);
            mat.SetFloat("_Max_Dist", max);
            return mat;
        }
        
        public void GenerateScaled() {
            textureMinMax = new TextureMinMax();
            generator.generateColliders = false;
            scaledPlanet.localScale = Vector3.one / OriginManager.skyboxScale;
            Debug.Log(scaledPlanet.localScale);
            generator.chunkHolder = scaledPlanet.gameObject;


            generator.Run(textureMinMax);
            generator.mat = CreateSurfaceMaterial(selected.material, Mathf.Sqrt(textureMinMax.Min), Mathf.Sqrt(textureMinMax.Max));
        }
        
        public void GenerateReal() {
            textureMinMax = new TextureMinMax();
            // Reset();
            generator.generateColliders = true;
            generator.chunkHolder = realPlanet.gameObject;
            generator.Run(textureMinMax);

            
            // set planet colors
            generator.mat = CreateSurfaceMaterial(selected.material, Mathf.Sqrt(textureMinMax.Min), Mathf.Sqrt(textureMinMax.Max));

            // // set water
            // if (hasWater) {
            //     waterMaterial = selected.waterMaterials[Random.Range(0, selected.waterMaterials.Length)];

            //     // instantiate a water prefab with scale set to slightly larger than scaleModifier * radius   
            //     water = GameObject.Instantiate(waterPrefab);
            //     water.layer = 9;
            //     water.transform.position = realPlanet.position;
            //     water.transform.rotation = realPlanet.rotation;
            //     water.transform.localScale = Vector3.one * scaleModifier * density.radius * selected.waterHeight;
            // }

            // if (hasAtmosphere) {
            //     // instantiate an atmosphere prefab with scale set to slightly larger than scaleModifier * radius
            // }

            // if (hasRings) {
            //     ringChunkHolder = new GameObject("Ring Chunks Holder");
            //     ringChunkHolder.layer = 9;
            //     ringGenerator.chunkHolder = ringChunkHolder;
            //     ringChunkHolder.transform.position = realPlanet.position;
            //     ringChunkHolder.transform.rotation = realPlanet.rotation;

            //     ringGenerator.mat = selected.ringMaterials[Random.Range(0, selected.ringMaterials.Length)];
            //     RingDensity ringDensity = (RingDensity) ringGenerator.densityGenerator;
            //     ringDensity.innerRadius = density.radius * ringInnerRadiusScale;
            //     ringDensity.outerRadius = density.radius * ringOuterRadiusScale;
            //     ringDensity.seed = density.seed;
            //     ringGenerator.Run();
            //     ringChunkHolder.transform.localScale = Vector3.one * scaleModifier;
            // }
 

            // planet = realPlanet.gameObject.AddComponent<Planet>();
            // List<Vector3> loc = GetOrbitalLocations(density.radius * scaleModifier * 2f);
            // planet.orbitalBuildPoints = new List<BuildableLocation>();
            // foreach (Vector3 l in loc) {
            //     planet.orbitalBuildPoints.Add(
            //         new BuildableLocation(l, (l - realPlanet.position).normalized)
            //     );
            // }
            
            // BuildNavTrees();

        }

        public void BuildNavTrees() {
            // subdivide the planet into 5000 unit cubes, use planet radius to find size of cube needed.
            Vector3 planetSize = Vector3.one * density.radius * 1.5f;
            // if the planet has rings then add to the x and z size of the planet.
            if (hasRings) {
                planetSize.x = density.radius * (ringOuterRadiusScale + 0.5f);
                planetSize.z = density.radius * (ringOuterRadiusScale + 0.5f);
            }

            // navTrees = new List<GameObject>();
            // System.Guid planetGuid = System.Guid.NewGuid();
            // // build and save each NavTree by index
            // for(float x = -planetSize.x + (navTreeSize / 2); x < planetSize.x - (navTreeSize / 2); x += navTreeSize) {
            //     for (float z = -planetSize.z + (navTreeSize / 2); z < planetSize.z - (navTreeSize / 2); z += navTreeSize) {
            //         for(float y = -planetSize.y; y < planetSize.y; y += navTreeSize) {
            //             var nT = GameObject.Instantiate(navTreeAnchorPrefab, new Vector3(x, y, z), Quaternion.identity);
            //             var anchor = nT.GetComponent<NavTreeAnchor>();
            //             anchor.maxNodeSize = navTreeSize;
            //             anchor.maxTreeDepth = 6;
            //             anchor.fileName = planetGuid.ToString() + x.ToString() + y.ToString() + z.ToString();
            //             nT.GetComponent<BoxCollider>().size = Vector3.one * navTreeSize;
            //             anchor.BuildAndSave();
            //         }
            //     }
            // }
        }

        public void PlacePrefabs() {
            for (int i = 0; i < 20; i++) {
                var sp = SampleSurface();
                if (sp != null) {
                    // Get a random prefab from the biome
                    var prefab = selected.prefabs[Random.Range(0, selected.prefabs.Length)];
                    // instantiate it at the position
                    var deposit = GameObject.Instantiate(prefab.prefab, sp.point, sp.rotation);
                }
            }
        }

        /// <summary>
        ///  Gets a list of locations in a dodecaheron at a given radius
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public List<Vector3> GetOrbitalLocations(float radius) {
            float phi = (Mathf.Sqrt(5) - 1f) / 2f;
            float a = 1 / Mathf.Sqrt(3);
            float b = a / phi;
            float c = a * phi;

            var vertices = new List<Vector3>();
            foreach (var i in new[] { -1, 1 })
            {
                foreach (var j in new[] { -1, 1 })
                {
                    vertices.Add(new Vector3(
                                        0,
                                        i * c * radius,
                                        j * b * radius));
                    vertices.Add(new Vector3(
                                        i * c * radius,
                                        j * b * radius,
                                        0));
                    vertices.Add(new Vector3(
                                        i * b * radius,
                                        0,
                                        j * c * radius));

                    foreach (var k in new[] { -1, 1 })
                        vertices.Add(new Vector3(
                                            i * a * radius,
                                            j * a * radius,
                                            k * a * radius));
                }
            }
            return vertices;
        }
        
        public void Sample() {
            // for (int i = 0; i < 100; i++) {
            SampleSurface();
            // }
        }

        public void Reset() {
            if (Application.isEditor) {
                if (water) {
                    DestroyImmediate(water, true);
                }
                if (ringChunkHolder) {
                    DestroyImmediate(ringChunkHolder, true);
                }
            } else {
                Destroy(water);
            }
            // if (planet) {
            //     planet.Reset();
            //     DestroyImmediate(planet, true);
            //     planet = null;
            // }
        }

        private SurfacePosition SampleSurface() {
            Vector3 orbitalPosition = Random.onUnitSphere * orbitalRadius * 4 + realPlanet.position;
            RaycastHit hit;
            Debug.DrawLine(orbitalPosition, orbitalPosition + Vector3.up * 100f, Color.green, 20f);
            Debug.DrawLine(
                (realPlanet.position - orbitalPosition).normalized * orbitalRadius * 4, 
                (realPlanet.position - orbitalPosition).normalized * orbitalRadius * 4 + Vector3.up * 100f, 
                Color.red, 
                20f
            );
            Debug.DrawLine(orbitalPosition, (realPlanet.position - orbitalPosition).normalized * orbitalRadius * 4, Color.cyan, 20f);

            if (Physics.Raycast(orbitalPosition, realPlanet.position - orbitalPosition, out hit, orbitalRadius * 1000)) {
                if (hit.collider.gameObject.tag == "Surface") {
                    Debug.DrawLine(orbitalPosition, hit.point, Color.green, 10f);
                    Debug.DrawLine(hit.point + Vector3.up * 50f, hit.point + Vector3.up * -50f, Color.yellow);
                    Debug.DrawLine(hit.point + Vector3.right * 50f, hit.point + Vector3.right * -50f, Color.yellow);
                    Debug.DrawLine(hit.point + Vector3.forward * 50f, hit.point + Vector3.forward * -50f, Color.yellow);
                    Debug.Log(hit.collider.gameObject);
                    return new SurfacePosition(hit.point, Quaternion.LookRotation(orbitalPosition - realPlanet.position));
                } else {
                    Debug.DrawLine(orbitalPosition, hit.point, Color.white, 10f);
                }
            }
            return null;
        }

    }

    public class SurfacePosition {
        public Vector3 point;
        public Quaternion rotation;
        public SurfacePosition(Vector3 point, Quaternion rotation) {
            this.point = point;
            this.rotation = rotation;
        }
    }
    
}
