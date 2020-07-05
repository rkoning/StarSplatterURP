using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour {
   public List<Health> bodySections;

   public AnimationCurve xAxisCurve = new AnimationCurve();
   public AnimationCurve yAxisCurve = new AnimationCurve();
   public AnimationCurve zAxisCurve = new AnimationCurve();

   private PointArray points; 

   public int buffer = 10;
   private int keyFrameLength;

   public float maxDist = 5f;
   private Vector3 lastPosition;
   public float speed = 10f;

   private void Start() {
      lastPosition = transform.position;
      keyFrameLength = buffer + bodySections.Count;
      points = new PointArray(keyFrameLength);

      for (int i = 0; i < keyFrameLength; i++) {
         Debug.Log(lastPosition + (lastPosition - Vector3.one).normalized * i * 5);
         points.AddPoint(lastPosition + (lastPosition - Vector3.one).normalized * i * 5);
      }
      SetKeys();
   }
   private void FixedUpdate() {
      if ((transform.position - lastPosition).sqrMagnitude > maxDist) {
         lastPosition = transform.position;
         points.AddPoint(lastPosition);
         SetKeys();
      }
      float fCount = (float) bodySections.Count;
      for (int i = 0; i < bodySections.Count; i++) {
         if (i == 0) {
            if (bodySections[i].transform.position - transform.position != Vector3.zero)
               bodySections[i].transform.rotation = Quaternion.LookRotation(bodySections[i].transform.position - transform.position);
         } else {
            if (bodySections[i].transform.position - bodySections[i - 1].transform.position != Vector3.zero)
               bodySections[i].transform.rotation = Quaternion.LookRotation(bodySections[i].transform.position - bodySections[i - 1].transform.position);
         }
         float time = i / fCount;
         bodySections[i].transform.position = Vector3.LerpUnclamped(bodySections[i].transform.position, new Vector3(xAxisCurve.Evaluate(time), yAxisCurve.Evaluate(time), zAxisCurve.Evaluate(time)), Time.fixedDeltaTime * speed);
      }
   }

   private void SetKeys() {
      xAxisCurve.keys = points.x;
      yAxisCurve.keys = points.y;
      zAxisCurve.keys = points.z;
   }

   private class PointArray {
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
   }
}