namespace IdxZero.Base.States
{
    public class StateTransition<T, TU> where T : System.Enum where TU : System.Enum
    {
        private readonly T _currentState;
        private readonly TU _currentCommand;

        public StateTransition(T currentState, TU currentCommand)
        {
            _currentState = currentState;
            _currentCommand = currentCommand;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * _currentState.GetHashCode() + 31 * _currentCommand.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition<T, TU> other = (StateTransition<T, TU>)obj;
            bool isNull = other == null;
            bool isSameState = other != null && _currentState.Equals(other._currentState);
            bool isSameCommand = other != null && _currentCommand.Equals(other._currentCommand);
            return !isNull && isSameState && isSameCommand;
        }
    }
}