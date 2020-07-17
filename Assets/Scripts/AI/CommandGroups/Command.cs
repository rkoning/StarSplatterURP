using System;
using System.Collections.Generic;
using UnityEngine;

using AI.BehaviourTree;

namespace AI.CommandGroups {
   public abstract class Command {
      protected Node commandDefinition;

      protected bool completed;
      protected bool failed;

      protected List<AIShip> ships;
      protected CommandGroup commandGroup;


      protected Node commandNode;

      protected List<PilotCommand> pilotCommands = new List<PilotCommand>();

      protected Guid commandGuid;

      public virtual void Init(CommandGroup commandGroup) {
         this.ships = commandGroup.ships;
         this.commandGroup = commandGroup;
      }

      protected void SetPilotCommands(Node node, Targetable target) {
         pilotCommands.Clear();
         for (int i = 0; i < ships.Count; i++) {
            ships[i].SetTarget(target);
            ships[i].SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(ships[i], node, commandGuid));
         }
      }

      protected void SetPilotCommands(Node node, Vector3 targetPosition) {
         pilotCommands.Clear();
         for (int i = 0; i < ships.Count; i++) {
            ships[i].SetTargetPosition(targetPosition);
            ships[i].SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(ships[i], node, commandGuid));
         }
      }

      protected void SetPilotCommands(Node node) {
         pilotCommands.Clear();
         for (int i = 0; i < ships.Count; i++) {
            ships[i].SetCommandNode(node);
            pilotCommands.Add(new PilotCommand(ships[i], node, commandGuid));
         }
      }
 
      protected bool CurrentCommandNodesSet() {
         if (pilotCommands.Count == 0) {
            return false;
         }
         for (int i = 0; i < pilotCommands.Count; i++) {
            if (pilotCommands[i].CommandEquals(commandGuid)) {
               return true;
            }
         }
         return false;
      }

      public void Evaluate() {
         commandDefinition.Evaluate();
      }

      public bool Completed() {
         return completed;
      }

      public bool Failed() {
         return failed;
      }

      protected virtual State CommandNodeSet() {
         if (CurrentCommandNodesSet()) {
            return State.Success;
         }
         return State.Failure;
      }
      
      public State CompleteCommand() {
         completed = true;
         return State.Success;
      }

      public State FailCommand() {
         failed = true;
         return State.Success;
      }

      protected class PilotCommand {
         AIShip pilot;
         public Node currentCommand;
         Guid guid;

         public PilotCommand(AIShip pilot, Node currentCommand, Guid guid) {
            this.pilot = pilot;
            this.currentCommand = currentCommand;
            this.guid = guid;
         }

         public bool CommandEquals(Guid guid) {
            return this.guid == guid;
         }
      }
   }
}