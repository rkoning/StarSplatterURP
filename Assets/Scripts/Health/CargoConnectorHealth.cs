using UnityEngine;

public class CargoConnectorHealth : ComponentHealth {

    public Transform cargoAnchor;
    public GameObject cargoPrefab;

    private Transform cargo;

    /// <summary>
    /// Sets the cargo of this connector to its default prefab if it has one and calls base.Start()
    /// If a cargo anchor is not set try to get the first child of this Transform
    /// </summary>
    protected override void Start() {
        base.Start();
        if (!cargoAnchor) {
            cargoAnchor = transform.GetChild(0);
        }
        if (cargoPrefab) {
            SetCargo(cargoPrefab);
        }
    }

    /// <summary>
    /// Instantiates a prefab parented to this connector's anchor. Drops cargo that is already being carried.
    /// </summary>
    /// <param name="cargoPrefab"></param>
    public void SetCargo(GameObject cargoPrefab) {
        if (cargoAnchor) {
            if (cargo) {
                DropCargo();
            }
            cargo = GameObject.Instantiate(cargoPrefab, cargoAnchor.position, cargoAnchor.rotation, cargoAnchor).GetComponent<Transform>();
            cargo.GetComponent<Rigidbody>().isKinematic = true;
        } else {
            Debug.LogWarning("Warning: Attepming to SetCargo() to component with no cargoAnchor set! on: " + gameObject.name);
        }
    }

    /// <summary>
    /// Drops the cargo container
    /// </summary>
    public void DropCargo() {
        if (cargo) {
            cargo.SetParent(null);
            cargo.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    /// <summary>
    /// Drops cargo the runs ComponentHealth.Die()
    /// </summary>
    public override void Die() {
        DropCargo();
        base.Die();
    }
}