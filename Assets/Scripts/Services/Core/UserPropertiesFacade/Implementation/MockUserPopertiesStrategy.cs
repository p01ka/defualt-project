
namespace IdxZero.Services.UserProperties
{
    public class MockUserPopertiesStrategy : IUserPropertiesStrategy
    {
        public void SetUserProperty(string userPropertyKey,
                                    string userPropertyValue)
        {
            UnityEngine.Debug.Log("USER PROPERTY " + userPropertyKey + " USER PROPERTY VALUE " + userPropertyValue);
        }
    }
}