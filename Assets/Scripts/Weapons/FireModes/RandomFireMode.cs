using System;
using UnityEngine;

public class RandomFireMode : FireMode {
   public override void Fire() {
      int current = UnityEngine.Random.Range(0, weapon.projectors.Length);
      if (weapon.Projectors[current] != null) {
         weapon.Projectors[current].Emit();
      }
   }
}
