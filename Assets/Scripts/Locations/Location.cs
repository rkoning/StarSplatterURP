using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Factions;

public class Location : MonoBehaviour
{
    public Dictionary<MajorFaction, List<Target>> localTargets = new Dictionary<MajorFaction, List<Target>>();


    public MajorFaction owner;
    
    public delegate void TargetEnterAction(MajorFaction majorFaction, Target target);
    public event TargetEnterAction OnTargetEnter;

    public float patrolRadius = 500f;

    private void Start() {
        OnTargetEnter += (MajorFaction majorFaction, Target target) => {};
    }

    public Vector3[] GetPatrolPoints() {
        Vector3[] points = new Vector3[Random.Range(3, 8)];
        for (int i = 0, count = points.Length; i < count; i++) {
            points[i] = Random.onUnitSphere * patrolRadius;
        }
        return points;
    }

    public bool FactionInLocation(MajorFaction faction) {
        return localTargets.ContainsKey(faction);
    }

    public Target GetClosestLocalTarget(MajorFaction faction, Vector3 position, float range) {
        if (!localTargets.ContainsKey(faction)) {
            return null;
        }
        Target closest = null;
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

    public Target GetClosestLocalTarget(MajorFaction faction, Vector3 position) {
        return GetClosestLocalTarget(faction, position, Mathf.Infinity);
    }

    private void OnTriggerEnter(Collider other) {
        // when a target enters the location check if it's major faction already exists, and if so add it.
        var t = other.GetComponent<Target>();
        if (t && t.majorFaction) {
            if (localTargets.ContainsKey(t.majorFaction)) {
                localTargets[t.majorFaction].Add(t);
                // Debug.Log("Added: " + t);
            } else {
                var targetList = new List<Target>();
                targetList.Add(t);
                localTargets.Add(t.majorFaction, targetList);
                // Debug.Log("Added: " + t);
            }
            OnTargetEnter(t.majorFaction, t);
        }    
    }

    private void OnTriggerExit(Collider other) {
        // when a target exits the location remove it
        var t = other.GetComponent<Target>();
        if (t && t.majorFaction) {
            if (localTargets.ContainsKey(t.majorFaction)) {
                localTargets[t.majorFaction].Remove(t);
                Debug.Log("Removed: " + t);
                // if this was the last target present in the faction, then remove that faction's presense from the local targets
                if (localTargets[t.majorFaction].Count < 1) {
                    localTargets.Remove(t.majorFaction);
                    Debug.Log("Removed: " + t.majorFaction);
                }
            }
        }
    }
}
