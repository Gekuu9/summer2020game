using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDataManager : MonoBehaviour {

    public static SaveDataManager instance;

    public static string saveDataPath = "/Saves/";

#pragma warning disable 0649
    [SerializeField]
    private SaveDataObject currentSaveData;
#pragma warning restore 0649

    private void Awake() {
        instance = this;

        LoadFile();
    }

    public void SaveFile() {
        BinaryFormatter bf = new BinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + saveDataPath)) {
            Directory.CreateDirectory(Application.persistentDataPath + saveDataPath);
        }

        FileStream fileStream = File.Create(Application.persistentDataPath + saveDataPath + currentSaveData.name + ".save");

        bf.Serialize(fileStream, currentSaveData);

        fileStream.Close();
    }

    public void LoadFile() {
        if (File.Exists(Application.persistentDataPath + saveDataPath + currentSaveData.name + ".save")) {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fileStream = File.Open(Application.persistentDataPath + saveDataPath + currentSaveData.name + ".save", FileMode.Open);

            currentSaveData = bf.Deserialize(fileStream) as SaveDataObject;

            fileStream.Close();

            currentSaveData.LoadFlags();
        } else {
            Debug.LogWarning("No save data found");
        }
    }

    public void LoadFile(string saveDataName) {
        if (File.Exists(Application.persistentDataPath + saveDataPath + saveDataName + ".save")) {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fileStream = File.Open(Application.persistentDataPath + saveDataPath + saveDataName + ".save", FileMode.Open);

            currentSaveData = bf.Deserialize(fileStream) as SaveDataObject;

            fileStream.Close();
        }
        else {
            Debug.LogWarning("No save data found");
        }
    }

    public void ResetSaveFile() {
        string saveName = currentSaveData.name;
        currentSaveData = new SaveDataObject();
        currentSaveData.name = saveName;

        SaveFile();
    }

    public int GetIntFlag(string flagName) {
        return currentSaveData.GetIntFlag(flagName);
    }

    public int GetIntFlag(string flagName, SaveDataObject save) {
        return save.GetIntFlag(flagName);
    }

    public string GetStringFlag(string flagName) {
        return currentSaveData.GetStringFlag(flagName);
    }

    public string GetStringFlag(string flagName, SaveDataObject save) {
        return save.GetStringFlag(flagName);
    }
    public float GetFloatFlag(string flagName) {
        return currentSaveData.GetFloatFlag(flagName);
    }

    public float GetFloatFlag(string flagName, SaveDataObject save) {
        return save.GetFloatFlag(flagName);
    }

    public bool GetBoolFlag(string flagName) {
        return currentSaveData.GetBoolFlag(flagName);
    }

    public Vector3 GetVector3Flag(string flagName, SaveDataObject save) {
        return save.GetVector3Flag(flagName);
    }

    public Vector3 GetVector3Flag(string flagName) {
        return currentSaveData.GetVector3Flag(flagName);
    }

    public bool GetBoolFlag(string flagName, SaveDataObject save) {
        return save.GetBoolFlag(flagName);
    }

    public void SetIntFlag(string flagName, int value) {
        currentSaveData.SetIntFlag(flagName, value);
        SaveFile();
    }

    public void SetIntFlag(string flagName, int value, SaveDataObject save) {
        save.SetIntFlag(flagName, value);
        SaveFile();
    }

    public void SetStringFlag(string flagName, string value) {
        currentSaveData.SetStringFlag(flagName, value);
        SaveFile();
    }

    public void SetStringFlag(string flagName, string value, SaveDataObject save) {
        save.SetStringFlag(flagName, value);
        SaveFile();
    }

    public void SetFloatFlag(string flagName, float value) {
        currentSaveData.SetFloatFlag(flagName, value);
        SaveFile();
    }

    public void SetFloatFlag(string flagName, float value, SaveDataObject save) {
        save.SetFloatFlag(flagName, value);
        SaveFile();
    }

    public void SetBoolFlag(string flagName, bool value) {
        currentSaveData.SetBoolFlag(flagName, value);
        SaveFile();
    }

    public void SetBoolFlag(string flagName, bool value, SaveDataObject save) {
        save.SetBoolFlag(flagName, value);
        SaveFile();
    }

    public void SetVector3Flag(string flagName, Vector3 value) {
        currentSaveData.SetVector3Flag(flagName, value);
        SaveFile();
    }

    public void SetVector3Flag(string flagName, Vector3 value, SaveDataObject save) {
        save.SetVector3Flag(flagName, value);
        SaveFile();
    }
}
