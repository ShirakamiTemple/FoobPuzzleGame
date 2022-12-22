using UnityEngine;

public class GameHandler : Handler<GameHandler>
{
    public int CurrentLevel { get; set; }
    public string CurrentPack { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoadHandler.Instance.Load += AssignGameValues;
        SaveLoadHandler.Instance.Save += SaveGameValues;
    }

    private void AssignGameValues()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        CurrentPack = PlayerPrefs.GetString("CurrentPack");
    }

    private void SaveGameValues()
    {
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        PlayerPrefs.SetString("CurrentPack", CurrentPack);
    }

    private void OnDestroy()
    {
        SaveLoadHandler.Instance.Load -= AssignGameValues;
        SaveLoadHandler.Instance.Save -= SaveGameValues;
    }
}
