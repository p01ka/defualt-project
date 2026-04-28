using IdxZero.Base.MVP;

namespace Gameplay.UI.Screens
{
    public class SettingsScreenPresenterFactory
    {
        private readonly SettingsScreenPresenter.Factory _factory;

        public SettingsScreenPresenterFactory(SettingsScreenPresenter.Factory factory)
        {
            _factory = factory;
        }

        public BasePresenter Create(ISettingsScreenView view)
        {
            return _factory.Create(view);
        }
    }
}