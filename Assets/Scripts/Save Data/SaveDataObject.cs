using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataObject {

    [SerializeField]
    public string name;

    [Serializable]
    public class intSaveEntry {
        public string name;
        public int value;
    }

    [Serializable]
    public class stringSaveEntry {
        public string name;
        public string value;
    }

    [Serializable]
    public class floatSaveEntry {
        public string name;
        public float value;
    }

    [Serializable]
    public class boolSaveEntry {
        public string name;
        public bool value;
    }

    [Serializable]
    public class vector3SaveEntry {
        public string name;
        public Vector3 value;
    }

    [SerializeField]
    private List<intSaveEntry> intSaveEntries;
    [SerializeField]
    private List<stringSaveEntry> stringSaveEntries;
    [SerializeField]
    private List<floatSaveEntry> floatSaveEntries;
    [SerializeField]
    private List<boolSaveEntry> boolSaveEntries;
    [SerializeField]
    private List<vector3SaveEntry> vector3SaveEntries;

    [SerializeField]
    private Dictionary<string, int> intSaveData;
    [SerializeField]
    private Dictionary<string, string> stringSaveData;
    [SerializeField]
    private Dictionary<string, float> floatSaveData;
    [SerializeField]
    private Dictionary<string, bool> boolSaveData;
    [SerializeField]
    private Dictionary<string, Vector3> vector3SaveData;

    public void LoadFlags() {
        if (intSaveEntries == null) intSaveEntries = new List<intSaveEntry>();
        if (stringSaveEntries == null) stringSaveEntries = new List<stringSaveEntry>();
        if (floatSaveEntries == null) floatSaveEntries = new List<floatSaveEntry>();
        if (boolSaveEntries == null) boolSaveEntries = new List<boolSaveEntry>();
        if (vector3SaveEntries == null) vector3SaveEntries = new List<vector3SaveEntry>();

        intSaveData = new Dictionary<string, int>();
        foreach (intSaveEntry item in intSaveEntries) {
            intSaveData.Add(item.name, item.value);
        }

        stringSaveData = new Dictionary<string, string>();
        foreach (stringSaveEntry item in stringSaveEntries) {
            stringSaveData.Add(item.name, item.value);
        }

        floatSaveData = new Dictionary<string, float>();
        foreach (floatSaveEntry item in floatSaveEntries) {
            floatSaveData.Add(item.name, item.value);
        }

        boolSaveData = new Dictionary<string, bool>();
        foreach (boolSaveEntry item in boolSaveEntries) {
            boolSaveData.Add(item.name, item.value);
        }

        vector3SaveData = new Dictionary<string, Vector3>();
        foreach (vector3SaveEntry item in vector3SaveEntries) {
            vector3SaveData.Add(item.name, item.value);
        }
    }

    public int GetIntFlag(string name) {
        if (intSaveData.ContainsKey(name))
            return intSaveData[name];
        else
            return 0;
    }

    public void SetIntFlag(string name, int value) {
        intSaveData[name] = value;
        bool foundEntry = false;
        foreach (intSaveEntry item in intSaveEntries) {
            if (item.name == name) {
                item.value = value;
                foundEntry = true;
            }
        }

        if (!foundEntry) {
            intSaveEntry item = new intSaveEntry();
            item.name = name;
            item.value = value;
            intSaveEntries.Add(item);
        }
    }

    public string GetStringFlag(string name) {
        if (stringSaveData.ContainsKey(name))
            return stringSaveData[name];
        else
            return null;
    }

    public void SetStringFlag(string name, string value) {
        stringSaveData[name] = value;
        bool foundEntry = false;
        foreach (stringSaveEntry item in stringSaveEntries) {
            if (item.name == name) {
                item.value = value;
                foundEntry = true;
            }
        }

        if (!foundEntry) {
            stringSaveEntry item = new stringSaveEntry();
            item.name = name;
            item.value = value;
            stringSaveEntries.Add(item);
        }
    }

    public float GetFloatFlag(string name) {
        if (floatSaveData.ContainsKey(name))
            return floatSaveData[name];
        else
            return float.NaN;
    }

    public void SetFloatFlag(string name, float value) {
        floatSaveData[name] = value;
        bool foundEntry = false;
        foreach (floatSaveEntry item in floatSaveEntries) {
            if (item.name == name) {
                item.value = value;
                foundEntry = true;
            }
        }

        if (!foundEntry) {
            floatSaveEntry item = new floatSaveEntry();
            item.name = name;
            item.value = value;
            floatSaveEntries.Add(item);
        }
    }

    public bool GetBoolFlag(string name) {
        if (boolSaveData.ContainsKey(name))
            return boolSaveData[name];
        else
            return false;
    }

    public void SetBoolFlag(string name, bool value) {
        boolSaveData[name] = value;
        bool foundEntry = false;
        foreach (boolSaveEntry item in boolSaveEntries) {
            if (item.name == name) {
                item.value = value;
                foundEntry = true;
            }
        }

        if (!foundEntry) {
            boolSaveEntry item = new boolSaveEntry();
            item.name = name;
            item.value = value;
            boolSaveEntries.Add(item);
        }
    }

    public Vector3 GetVector3Flag(string name) {
        if (vector3SaveData.ContainsKey(name))
            return vector3SaveData[name];
        else
            return Vector3.zero;
    }

    public void SetVector3Flag(string name, Vector3 value) {
        vector3SaveData[name] = value;
        bool foundEntry = false;
        foreach (vector3SaveEntry item in vector3SaveEntries) {
            if (item.name == name) {
                item.value = value;
                foundEntry = true;
            }
        }

        if (!foundEntry) {
            vector3SaveEntry item = new vector3SaveEntry();
            item.name = name;
            item.value = value;
            vector3SaveEntries.Add(item);
        }
    }

    [Serializable]
    public enum FlagType {
        Int,
        String,
        Float
    }
}
