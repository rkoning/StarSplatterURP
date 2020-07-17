using System.Collections.Generic;
using UnityEngine;

using AI.BehaviourTree;

namespace AI.CommandGroups {
   public class CommandGroup : ComplexTargetable {
      public List<AIShip> ships;

      private Command currentCommand;
      
      protected override void Start() {
         base.Start();

         ships = new List<AIShip>();
         ForChildrenDo((target) => {
            var pilot = target.GetComponent<AIShip>();
            if (pilot) {
               target.OnDestroyed += (Targetable t) => { ships.Remove(pilot); return true; };
               ships.Add(pilot);
            }
            target.Parent = this;
         });
      }

      private void Update() {
         if (HasCommand()) {
            
            currentCommand.Evaluate(); 
            if (Completed() || Failed()) {
               currentCommand = null;
               for (int i = 0; i < ships.Count; i++) {
                  ships[i].SetCommandNode(new Node(() => { return State.Failure; }));
               }
            }
         }
      }

      public void SetCurrentCommand(Command command) {
         currentCommand = command;
         currentCommand.Init(this);
      }

      public bool HasCommand() {
         return currentCommand != null;
      }

      public bool Completed() {
         if (HasCommand()) 
            return currentCommand.Completed();
         return false;
      }

      public bool Failed() {
         if (HasCommand())
            return currentCommand.Failed();
         return false;
      }
   }
}