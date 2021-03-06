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

   public GameObject fighterPrefab;

   public bool buildOnStart;

   private Action<Health> sectionDeathAction;

   private Health mainHealth;

   public int splitIndex;

   private void Start() {
      mainHealth = GetComponent<Health>();
      if (!buildOnStart) {
         // keyFrameLength = initialSections * 4;
         lastPosition = transform.position;
         return;
      }
      body = GameObject.Instantiate(bodyParentPrefab, Vector3.zero, Quaternion.identity).transform;
      sections = BuildSnake(initialSections);
      var turrets = GetComponent<TurretSystemComponent>();
      turrets.turretParent = body;

      keyFrameLength = initialSections * 4;
      points = new PointArray(keyFrameLength);

      for (int i = 0; i < keyFrameLength; i++) {
         points.AddPoint(lastPosition);
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
         var section = GameObject.Instantiate(bodySectionPrefab, transform.position, transform.rotation, body);
         newSections.Add(section);
         int current = i;
         section.GetComponent<SnakeSection>().SetDeathAction((Health h) => { SplitAt(current); });
      }

      var tail = GameObject.Instantiate(tailPrefab, transform.position, transform.rotation, body);
      newSections.Add(tail);
      return newSections;
   }

   public void SetPoints(PointArray points) {
      this.points = points;
   }

   public void SplitAt(int index) {
      index = index - 1;
      if (index < 2 || index > sections.Count - 1) {
         if (sections.Count <= 3) {
            foreach(var s in sections) {
               Destroy(s);
            }
            mainHealth.Die();
         } else if (index > 0 && index < sections.Count) {
            var removed = sections[index];
            sections.RemoveAt(index);
            Destroy(removed);
         }
         Debug.Log("did not split" + index + " " + sections.Count);
         return;
      }

      int pointsIndex = (int) (((float) index / (float) sections.Count) * keyFrameLength);
      if (points.size - pointsIndex < 1) {
         return;
      }

      // split sections into 2 lists
      Transform splitTransform = sections[index].transform;
      Transform newBody = GameObject.Instantiate(bodyParentPrefab, Vector3.zero, Quaternion.identity).transform;
      var newSnake = GameObject.Instantiate(fighterPrefab, splitTransform.position, Quaternion.Inverse(splitTransform.rotation), null).GetComponent<Snake>();
      var turrets = newSnake.GetComponent<TurretSystemComponent>();
      turrets.turretParent = newBody;


      var end = sections.GetRange(index, sections.Count - index);

      // add split events
      for (int i = 0; i < end.Count; i++) {
         var section = end[i].GetComponentInChildren<SnakeSection>();
         if (section) {
            section.SetDeathAction((Health h) => { newSnake.SplitAt(i); });
            section.transform.SetParent(newBody);
         }
      }

      newSnake.GetComponent<TurretSystemComponent>().SetTurretParent(newBody);

      // add new head to the end
      var head = GameObject.Instantiate(headPrefab, splitTransform.position, splitTransform.rotation, newBody);

      var laser = head.GetComponentInChildren<Weapon>();
      newSnake.GetComponent<AI.AIFighter>().primary = laser;
      end = new List<GameObject>(end.Prepend(head));
      
      newSnake.sections = end;
      newSnake.initialSections = end.Count;
      newSnake.buildOnStart = false;
      var endPoints = points.GetRange(pointsIndex, points.size - pointsIndex);

      newSnake.keyFrameLength = (int) (index / (float) sections.Count) * keyFrameLength;
      newSnake.SetPoints(endPoints);
      newSnake.SetKeys();
      
      newSnake.GetComponent<AI.AIFighter>().target = GetComponent<AI.AIFighter>().currentTarget;
      
      sections = sections.GetRange(0, index);
      var tail = GameObject.Instantiate(tailPrefab, splitTransform.position, splitTransform.rotation, body);
      sections.Add(tail);
      initialSections = sections.Count;
      keyFrameLength = initialSections * 4;
      SetPoints(points.GetRange(1, pointsIndex));
      SetKeys();
   }

   private void Update() {
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
         if (size == 0) {
            return;
         }
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
         for (int i = index + count - 1; i > index - 1; i--) {
            range.AddPoint(new Vector3(x[i].value, y[i].value, z[i].value));
         }
         return range;
      }
   }
}