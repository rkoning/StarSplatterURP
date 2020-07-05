using UnityEngine;
using System.Collections;

public class BurstFireMode : FireMode {
    public int shotsPerBurst = 3;
    public float burstDuration = 1f;

    private Coroutine firing;

    public FireMode nextFireMode;

    public override void Fire() {
        if (firing == null) {
            firing = StartCoroutine(Burst(shotsPerBurst, burstDuration));
        }
    }

    private IEnumerator Burst(int numShots, float duration) {
        float delay = duration / (float) numShots;
        for (int i = 0; i < numShots; i++) {
            if (nextFireMode)
                nextFireMode.Fire();
            yield return new WaitForSeconds(delay);
        }
        firing = null;
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }
}