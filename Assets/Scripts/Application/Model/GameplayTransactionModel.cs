using IdxZero.Gameplay.States;

namespace IdxZero.Application.Model
{
    public class GameplayTransactionModel : IGameplayTransactionModelSetter, IGameplayTransactionModelGetter
    {
        public GameplayStateMachineConditionManager.StateConfig StateConfig
        {
            get; set;
        }

        public bool UseCustomerGameplayStateConfig
        {
            get; set;
        }

        public bool UseDebugMode
        {
            get; set;
        }
    }

    public interface IGameplayTransactionModelSetter
    {
        GameplayStateMachineConditionManager.StateConfig StateConfig { get; set; }
        bool UseCustomerGameplayStateConfig { get; set; }
        bool UseDebugMode { get; set; }
    }

    public interface IGameplayTransactionModelGetter
    {
        GameplayStateMachineConditionManager.StateConfig StateConfig { get; set; }
        bool UseCustomerGameplayStateConfig { get; set; }
    }
}