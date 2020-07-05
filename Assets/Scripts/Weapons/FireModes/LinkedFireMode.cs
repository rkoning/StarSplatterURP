using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedFireMode : FireMode
{
    public override void Fire() {
        foreach (var projector in weapon.Projectors) {
            projector.Emit();
        }
    }
}
