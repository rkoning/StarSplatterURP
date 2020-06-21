
using UnityEngine;

[ExecuteInEditMode]
public class RingGenerator : MeshGenerator {
   public RingDensity ringDensityGenerator;

   protected override void Awake() {
      base.Awake();
      this.densityGenerator = ringDensityGenerator;
   }
}