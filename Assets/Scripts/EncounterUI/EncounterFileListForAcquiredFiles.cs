using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "AcquiredEncounterFileList", menuName = "EncounterSystem/AcquiredEncounterFileList")]
[System.Serializable]
public class EncounterFileListForAcquiredFiles : ScriptableObject
{
    public List<EncounterFile> EncounterFiles;
    public void SaveToAcquireFileList(EncounterFile encounterFile)
    {
        EncounterFiles.Add(encounterFile);
        SaveData();
    }

    public void SaveData()
    {
        EditorUtility.SetDirty(this);
    }
}
