using System.IO;
using UnityEngine;
using PlayerData;

public static class SaveManager
{
    public static bool SaveFileExists(int slotNumber)
    {
        string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
        return File.Exists(path);
    }
    public static void LoadGameData(int slotNumber)
    {
        Stats stats = Stats.LoadStats(slotNumber);
        Debug.Log($"Save Slot {slotNumber} loaded.");
    }
    public static void CreateNewSaveData(int slotNumber)
    {
        Stats newStats = new Stats();
        newStats.SaveStats(slotNumber);
        Debug.Log($"New Save Slot {slotNumber} created.");
    }
}