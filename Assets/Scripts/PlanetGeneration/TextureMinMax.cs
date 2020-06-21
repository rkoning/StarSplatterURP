using UnityEngine;

public class TextureMinMax {
   public float Min { get; private set; }
   public float Max { get; private set; }
   
   public TextureMinMax() {
      Min = float.MaxValue;
      Max = float.MinValue;
   }

   public void SetValue(float value) {
      if (value < Min) {
         Min = value;
      } else if (value > Max) {
         Max = value;
      }
   }
}