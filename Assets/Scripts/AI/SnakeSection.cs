using System;
using UnityEngine;

public class SnakeSection : MonoBehaviour {
   public Health health;
   private Health.DeathAction onDeathAction;

   public Projector turret;
   
   public void SetDeathAction(Health.DeathAction onDeathAction) {
      if (this.onDeathAction != null) {
         health.OnDeath -= this.onDeathAction;
      }
      this.onDeathAction = onDeathAction;
      health.OnDeath += this.onDeathAction;
   }
}