using UnityEngine;

namespace AI.Factions {
    public abstract class Objective {    

        public delegate void CompletedAction();
        public delegate void FailedAction();

        public event CompletedAction OnCompleted;
        public event FailedAction OnFailed;

        public abstract bool IsComplete();
        public virtual void Complete() {
            OnCompleted();
        }

        public virtual void Fail() {
            OnFailed();
        }

        public abstract void Perform(MinorFaction f);
    }
}