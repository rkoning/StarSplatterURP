using UnityEngine;

public class SnakeWeapon : MonoBehaviour {
   private Snake snake;

   private Weapon bombLauncher;

   public float fireDelay = 8f;
   private float nextFire = 8f;

   public float range = 20f;

   private void Start() {
      snake = GetComponent<Snake>();
      bombLauncher = GetComponentInChildren<Weapon>();
   }

   private void Update() {
      float now = Time.fixedTime;
      if (now > nextFire) {
         nextFire = now + fireDelay;
         var current = snake.sections[Random.Range(1, snake.sections.Count - 1)];
         if (current) {
            Vector3 rand = Random.onUnitSphere * range;
            Vector3 newPosition = current.transform.position + current.transform.rotation * new Vector3(rand.x, rand.y, 0f);

            bombLauncher.projectors[0].transform.position = newPosition;
            bombLauncher.projectors[0].transform.rotation = Quaternion.LookRotation(current.transform.position - newPosition);
            bombLauncher.Fire();
         }
      }
   }
}