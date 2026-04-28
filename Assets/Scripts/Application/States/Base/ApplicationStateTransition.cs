using IdxZero.Base.States;

namespace IdxZero.Application.States.Base
{
    public class ApplicationStateTransition : StateTransition<ApplicationState, ApplicationCommand>
    {
        public ApplicationStateTransition(ApplicationState currentState,
                                          ApplicationCommand currentCommand) : base(currentState, currentCommand)
        {
        }
    }
}