using System;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviourTree;
using AI.Factions;

namespace AI.CommandGroups {
   public class CommandGroup : ComplexTarget {

      // used for debugging
      public Target targetTransform;

      public Transform pickUp;
      public Transform dropOff;
      
      public List<AIPilot> pilots;

      private Command currentCommand;
      public Specialization specialization;

      protected override void Start() {
         base.Start();

         pilots = new List<AIPilot>();
         ForChildrenDo((target) => {
            var pilot = target.GetComponent<AIPilot>();
            if (pilot) {
               target.OnDestroyed += (Target t) => { pilots.Remove(pilot); return true; };
               pilots.Add(pilot);
            }
            target.Parent = this;
         });
      }

      private void Update() {
         if (HasCommand()) {
            
            currentCommand.Evaluate(); 
            if (Completed() || Failed()) {
               currentCommand = null;
               for (int i = 0; i < pilots.Count; i++) {
                  pilots[i].SetCommandNode(new Node(() => { return State.Failure; }));
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