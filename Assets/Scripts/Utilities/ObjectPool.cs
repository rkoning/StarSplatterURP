using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool {
   private static Dictionary<string, Queue<GameObject>> objects = new Dictionary<string, Queue<GameObject>>();

   public static void Register(string objectType, GameObject prefab, int count) {
      // check if there is an existing pool for this object type
      bool existing = objects.ContainsKey(objectType);

      Queue<GameObject> pool;
      if (existing) {
         pool = objects[objectType];
      } else {
         // create a new queue with the object type if we don't already have it
         pool = new Queue<GameObject>();
         objects.Add(objectType, pool);
      }

      // and 'count' number of objects into the new or existing queue
      for (int i = 0; i < count; i++) {
         var go = GameObject.Instantiate(prefab);
         go.name.Replace(" (Clone)", "");
         go.SetActive(false);
         pool.Enqueue(go);
      }
   }
   
   public static GameObject Create(string objectType, Vector3 position, Quaternion rotation, Transform parent) {
      // get the oldest object on the queue and activate it
      var go = objects[objectType].Dequeue();
      go.SetActive(true);
      go.transform.position = position;
      go.transform.rotation = rotation;
      go.transform.SetParent(parent);
      // add the object to the back of the queue, so it will be reused after all other objects have been
      objects[objectType].Enqueue(go);
      return go;
   }

   public static bool ContainsKey(string objectType) {
      return objects.ContainsKey(objectType);
   }
}