namespace IdxZero.Base.Screens
{
    public interface IScreenPresenter
    {
        bool IsActive { get; }

        void ActiveScreen();

        void DeActiveScreen(bool toDisable = false);
    }
}