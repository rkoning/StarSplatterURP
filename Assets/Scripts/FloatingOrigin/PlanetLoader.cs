using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetLoader : MonoBehaviour
{

   public string sceneName;
   private FloatingOrigin floatingOrigin;
   public int buildIndex = -1;

   private AsyncOperation loading;
   public Unloader unloader;
   private void Start() {
      floatingOrigin = OriginManager.instance.floatingOrigin;
      unloader.loader = this;
      unloader.gameObject.SetActive(false);
   }

   private void OnTriggerEnter(Collider other) {
      loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
      // load scene async
      loading.completed += (AsyncOperation op) => {
         if (buildIndex < 0) {
            buildIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
         }
         floatingOrigin.enabled = false;

         // move player to relative location
         Vector3 playerPosition = (other.transform.position - transform.position) * OriginManager.skyboxScale;
         OriginManager.instance.player.ship.transform.position = playerPosition;

         // move skybox such that this location is the new 0,0,0
         Vector3 skyboxOrigin = transform.position;
         Vector3 shiftedSkyboxPosition = (OriginManager.instance.skybox.transform.position - skyboxOrigin);
         OriginManager.instance.skybox.transform.position = shiftedSkyboxPosition;
         unloader.gameObject.SetActive(true);
         gameObject.SetActive(false);
      };
   }
}
