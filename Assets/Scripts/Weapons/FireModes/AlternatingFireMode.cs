using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingFireMode : FireMode
{

    private int current = 0;
    public override void Fire() {
        weapon.Projectors[current].Emit();
        current = (current + 1) % (weapon.Projectors.Length);
    }

}
