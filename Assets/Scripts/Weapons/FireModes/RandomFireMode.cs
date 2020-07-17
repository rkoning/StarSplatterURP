using System;
using UnityEngine;

public class RandomFireMode : FireMode {
   public override void Fire() {
      int current = UnityEngine.Random.Range(0, weapon.projectors.Length);
      weapon.Projectors[current].Emit();
   }
}
