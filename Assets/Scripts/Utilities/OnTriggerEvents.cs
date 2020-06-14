using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnTriggerEnterEvent : UnityEvent<GameObject> {}

[System.Serializable]
public class OnTriggerStayEvent : UnityEvent<GameObject> {}

[System.Serializable]
public class OnTriggerExitEvent : UnityEvent<GameObject> {}

public class OnTriggerEvents : MonoBehaviour
{

    public OnTriggerEnterEvent TriggerEnter;
    public OnTriggerStayEvent TriggerStay;
    public OnTriggerExitEvent TriggerExit;

    private void OnTriggerEnter(Collider other) {
        TriggerEnter.Invoke(other.gameObject);
    }

    private void OnTriggerStay(Collider other) {
        TriggerStay.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        TriggerExit.Invoke(other.gameObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, -1, transform.position, transform.rotation, transform.localScale);
    }
}