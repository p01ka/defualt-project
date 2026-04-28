using UnityEngine;

namespace IdxZero.DataBase
{
    public class PlayerPrefsUserDatabaseManager : IUserDatabaseManager
    {
        public void ClearDatabase()
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.Debug.Log("PLAYER PREFS DELETED");
        }
    }
}