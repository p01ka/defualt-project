namespace IdxZero.Base.Screens
{
    public interface IScreensController<in T> where T : System.Enum
    {
        void AddScreen(T screenEnum, IScreenPresenter screenPresenter);
    }
}