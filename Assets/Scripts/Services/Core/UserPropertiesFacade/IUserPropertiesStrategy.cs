namespace IdxZero.Services.UserProperties
{
    public interface IUserPropertiesStrategy
    {
        void SetUserProperty(string userPropertyKey, string userPropertyValue);
    }
}