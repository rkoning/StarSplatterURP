using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour {
   public Faction owner;

   public Structure structure;

   public Dictionary<Faction, List<Targetable>> localTargets = new Dictionary<Faction, List<Targetable>>();

   public delegate void TargetEnterAction(Faction faction, Targetable target);
   public event TargetEnterAction OnTargetEnter;

   private void Start() {
      OnTargetEnter += (Faction f, Targetable t) => {};
   }

   public bool FactionPresent(Faction faction) {
      return localTargets.ContainsKey(faction);
   }

   public Targetable GetClosestLocalTarget(Faction faction, Vector3 position, float range) {
      if (!localTargets.ContainsKey(faction)) {
         return null;
      }
      Targetable closest = null;
      float closestDist = Mathf.Infinity;
      var targets = localTargets[faction];
      for (int i = 0; i < targets.Count; i++) {
         float squaredDist = (targets[i].transform.position - position).sqrMagnitude;
         if (squaredDist < closestDist && squaredDist < range * range) {
            closest = targets[i];
            closestDist = squaredDist;
         }
      }
      return closest;
   }

   public Targetable GetClosestLocalTarget(Faction faction, Vector3 position) {
        return GetClosestLocalTarget(faction, position, Mathf.Infinity);
   }

   
   private void OnTriggerEnter(Collider other) {
      // when a target enters the location check if it's major faction already exists, and if so add it.
      var t = other.GetComponent<Targetable>();
      if (t && t.faction) {
         if (localTargets.ContainsKey(t.faction)) {
            localTargets[t.faction].Add(t);
            // Debug.Log("Added: " + t);
         } else {
            var targetList = new List<Targetable>();
            targetList.Add(t);
            localTargets.Add(t.faction, targetList);
            // Debug.Log("Added: " + t);
         }
         OnTargetEnter(t.faction, t);
      }    
   }


   private void OnTriggerExit(Collider other) {
      // when a target exits the location remove it
      var t = other.GetComponent<Targetable>();
      if (t && t.faction) {
         if (localTargets.ContainsKey(t.faction)) {
            localTargets[t.faction].Remove(t);
            // Debug.Log("Removed: " + t);
            // if this was the last target present in the faction, then remove that faction's presense from the local targets
            if (localTargets[t.faction].Count < 1) {
               localTargets.Remove(t.faction);
               // Debug.Log("Removed: " + t.faction);
            }
         }
      }
   }
}