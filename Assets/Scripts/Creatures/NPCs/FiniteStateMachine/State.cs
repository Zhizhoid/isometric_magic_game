using System;

namespace Creatures.NPCs.FiniteStateMachine {
    public interface State {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
        public Type ShouldExit(); // Returns type of the next state if the current state should be exited, otherwise returns null
    }
}
