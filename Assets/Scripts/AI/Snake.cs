using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

using AI;

public class Snake : MonoBehaviour {
   public List<GameObject> sections;

   public AnimationCurve xAxisCurve = new AnimationCurve();
   public AnimationCurve yAxisCurve = new AnimationCurve();
   public AnimationCurve zAxisCurve = new AnimationCurve();

   private PointArray points; 

   public int buffer = 10;
   private int keyFrameLength;

   public float maxDist = 5f;
   private Vector3 lastPosition;
   public float speed = 10f;
   public float initialDist = 0.1f;

   public GameObject bodySectionPrefab;
   public GameObject headPrefab;
   public GameObject tailPrefab;

   public GameObject bodyParentPrefab;
   private Transform body;


   public int initialSections;
   private int currentSections;

   public GameObject fighterPrefab;

   public bool buildOnStart;

   private Action<Health> sectionDeathAction;

   private Health mainHealth;

   private void Start() {
      mainHealth = GetComponent<Health>();
      buffer = 3 * initialSections;
      if (!buildOnStart) {
         keyFrameLength = buffer + initialSections;
         lastPosition = transform.position;
         return;
      }
      body = GameObject.Instantiate(bodyParentPrefab, Vector3.zero, Quaternion.identity).transform;
      sections = BuildSnake(initialSections);
      var turrets = GetComponent<TurretSystemComponent>();
      turrets.turretParent = body;
      currentSections = initialSections;

      keyFrameLength = buffer + sections.Count;
      points = new PointArray(keyFrameLength);

      for (int i = 0; i < keyFrameLength; i++) {
         points.AddPoint(lastPosition + (lastPosition - Vector3.one).normalized * i * 5);
      }
      SetKeys();
   }

   public List<GameObject> BuildSnake(int sections) {
      var newSections = new List<GameObject>();
      var head = GameObject.Instantiate(headPrefab, transform.position, transform.rotation, body);
      var laser = head.GetComponentInChildren<Weapon>();
      GetComponent<AI.AIFighter>().primary = laser;
      newSections.Add(head);
      for (int i = 0; i < sections; i++) {
         var section = GameObject.Instantiate(bodySectionPrefab, transform.position + transform.forward * maxDist * i, transform.rotation, body);
         newSections.Add(section);
         int current = i;
         section.GetComponent<SnakeSection>().SetDeathAction((Health h) => { SplitAt(current); });
      }

      var tail = GameObject.Instantiate(tailPrefab, transform.position + transform.forward * maxDist * (sections + 1), transform.rotation, body);
      newSections.Add(tail);
      return newSections;
   }

   public void SetPoints(PointArray points) {
      this.points = points;
   }

   public void SplitAt(int index) {
      Debug.Log(" Splitting at: " + index);
      if (index < 2 || index > sections.Count - 1) {
         Debug.Log("Can not split at: " + index);
         if (sections.Count <= 3) {
            Debug.Log("Final Health Section destroyed, dying");
            foreach(var s in sections) {
               Destroy(s);
            }
            mainHealth.Die();
         } else {
            var removed = sections[index];
            sections.RemoveAt(index);
            Destroy(removed);
         }
         return;
      }

      if (sections.Count <= 3) {
         Debug.Log("I Should be destroyed, BOOM!");
         return;
      }
      // split sections into 2 lists
      Transform splitTransform = sections[index].transform;
      Transform newBody = GameObject.Instantiate(bodyParentPrefab, Vector3.zero, Quaternion.identity).transform;
      var newSnake = GameObject.Instantiate(fighterPrefab, splitTransform.position, splitTransform.rotation, newBody).GetComponent<Snake>();

      Debug.Log("KeyFrameLength: " + keyFrameLength);
      int pointsIndex = (int) (((float) index / (float) sections.Count) * keyFrameLength);
      
      var end = sections.GetRange(index, sections.Count - index);

      Debug.Log("End count: " + end.Count);
      // add split events
      for (int i = 0; i < end.Count; i++) {
         var section = end[i].GetComponentInChildren<SnakeSection>();
         if (section) {
            section.SetDeathAction((Health h) => { SplitAt(i); });
            section.transform.SetParent(newBody);
         }
      }

      newSnake.GetComponent<TurretSystemComponent>().SetTurretParent(newBody);

      // add new head to the end
      var head = GameObject.Instantiate(headPrefab, splitTransform.position, splitTransform.rotation, body);

      var laser = head.GetComponentInChildren<Weapon>();
      newSnake.GetComponent<AI.AIFighter>().primary = laser;
      end = new List<GameObject>(end.Prepend(head));
      
      newSnake.sections = end;
      newSnake.initialSections = end.Count;
      newSnake.buildOnStart = false;

      var endPoints = points.GetRange(pointsIndex, keyFrameLength - pointsIndex - 1);
      newSnake.keyFrameLength = (int) (index / (float) sections.Count) * keyFrameLength;
      newSnake.SetPoints(endPoints);
      newSnake.SetKeys();
      

      newSnake.GetComponent<AI.AIFighter>().target = GetComponent<AI.AIFighter>().currentTarget;
      sections = sections.GetRange(0, index);
      var tail = GameObject.Instantiate(tailPrefab, splitTransform.position, splitTransform.rotation, body);
      sections.Add(tail);
      buffer = sections.Count * 3;
      keyFrameLength = buffer + sections.Count;
      SetPoints(points.GetRange(0, pointsIndex));
      SetKeys();    
   }

   private void FixedUpdate() {
      if (mainHealth.Dead)
         return;
      if ((transform.position - lastPosition).sqrMagnitude > maxDist) {
         lastPosition = transform.position;
         points.AddPoint(lastPosition);
         SetKeys();
      }
      float fCount = (float) sections.Count;
      for (int i = 0; i < sections.Count; i++) {
         if (i == 0) {
            if (sections[i].transform.position - transform.position != Vector3.zero)
               sections[i].transform.rotation = Quaternion.LookRotation(sections[i].transform.position - transform.position);
         } else {
            if (sections[i].transform.position - sections[i - 1].transform.position != Vector3.zero)
               sections[i].transform.rotation = Quaternion.LookRotation(sections[i].transform.position - sections[i - 1].transform.position);
         }
         float time = i / (fCount - initialDist) + initialDist;
         sections[i].transform.position = Vector3.LerpUnclamped(sections[i].transform.position, new Vector3(xAxisCurve.Evaluate(time), yAxisCurve.Evaluate(time), zAxisCurve.Evaluate(time)), Time.fixedDeltaTime * speed);
      }
   }

   private void SetKeys() {
      xAxisCurve.keys = points.x;
      yAxisCurve.keys = points.y;
      zAxisCurve.keys = points.z;
   }

   public class PointArray {
      public int size;
      public Keyframe[] x;
      public Keyframe[] y;
      public Keyframe[] z;

      public PointArray(int size) {
         this.size = size;
         x = new Keyframe[size];
         y = new Keyframe[size];
         z = new Keyframe[size];
      }

      public void AddPoint(Vector3 point) {
         x = ShiftArray(x);
         x[0] = new Keyframe(0f, point.x);
         y = ShiftArray(y);
         y[0] = new Keyframe(0f, point.y);
         z = ShiftArray(z);
         z[0] = new Keyframe(0f, point.z);
      }

      private Keyframe[] ShiftArray(Keyframe[] arr) {
         int len = arr.Length;
         float fLen = (float) len;
         Keyframe[] temp = new Keyframe[len];
         for (int i = 1; i < len; i++) {
            temp[i] = arr[i - 1];
            temp[i].time = i / fLen;
         }
         temp[0] = arr[len - 1];
         return temp;
      }

      public PointArray GetRange(int index, int count) {
         var range = new PointArray(count);
         for (int i = index; i < index + count - 1 && i < x.Length; i++) {
            // Debug.Log(i - index + " " + count);
            range.AddPoint(new Vector3(x[i].value, y[i].value, z[i].value));
         }
         return range;
      }
   }
}