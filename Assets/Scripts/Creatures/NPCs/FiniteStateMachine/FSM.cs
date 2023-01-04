using System;
using System.Collections.Generic;

namespace Creatures.NPCs.FiniteStateMachine {
    public class FSM {
        private readonly State initState;
        private State currentState;
        private readonly Dictionary<Type, State> allStates = new Dictionary<Type, State>();

        public FSM(State _initState, State[] _allStates) {
            initState = _initState;
            foreach (State s in _allStates) {
                allStates[s.GetType()] = s;
            }
        }

        

        public void Start() {
            currentState = initState;
            currentState.OnEnter();
        }

        public void UpdateTick() {
            Type next = currentState.ShouldExit();
            if (next != null) {
                currentState.OnExit();
                currentState = allStates[next];
                currentState.OnEnter();
            }
            
            currentState.OnUpdate();
        }
    }
}
