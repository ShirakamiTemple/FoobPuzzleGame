using UnityEngine;

public class SaveLoadHandler : Handler<SaveLoadHandler>
{
    public delegate void OnDataSave();
    public OnDataSave Save;
    public delegate void OnDataLoad();
    public OnDataLoad Load;

    private void Start()
    {
        LoadData();
    }
    
    public void SaveData()
    {
        Save?.Invoke();
    }

    public void LoadData()
    {
        Load?.Invoke();
    }

    /// <summary>
    /// Will delete ALL saved data
    /// </summary>
    public void DeleteAllSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Deletes the data of a particular key
    /// </summary>
    /// <param name="key"></param>
    public void DeleteData(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
