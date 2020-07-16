using UnityEngine;

[CreateAssetMenu(fileName = "NewShipStats", menuName = "Inventory/ShipStats", order = 2)]
public class ShipStats : ScriptableObject {
   public float acceleration;
   public float topSpeed;
   public float maneuverability;
   public float maxShield;
   public float shieldRecharge;
   public float maxHealth;

   public int primaryHardpoints;
   public int secondaryHardpoints;
   public int equipmentHardpoints;
}