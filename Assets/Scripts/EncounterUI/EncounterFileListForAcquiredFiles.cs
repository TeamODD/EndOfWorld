using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "AcquiredEncounterFileList", menuName = "EncounterSystem/AcquiredEncounterFileList")]
[System.Serializable]
public class EncounterFileListForAcquiredFiles : ScriptableObject
{
    public List<AcquiredEncounterFile> AcquiredEncounterFileList = new List<AcquiredEncounterFile>();

    public bool IsItAdded(short index)
    {
        return AcquiredEncounterFileList[index].isItAdded;
    }

    public short GetChaperIndex(short index)
    {
        return AcquiredEncounterFileList[index].encounterFile.ChaperLevel;
    }

    public void SaveToAcquiredFileList(EncounterFile encounterFile)
    {
        AcquiredEncounterFile acquiredEncounterFile = new AcquiredEncounterFile(encounterFile);

        AcquiredEncounterFileList.Add(acquiredEncounterFile);
        SaveData();
    }

    public void SaveData()
    {
        EditorUtility.SetDirty(this);
    }
}

public class AcquiredEncounterFile
{
    public AcquiredEncounterFile(EncounterFile encounterFile)
    {
        isItAdded = false;
        this.encounterFile = encounterFile;
    }

    [HideInInspector]
    public bool isItAdded { get; private set; }

    public EncounterFile encounterFile;


    public EncounterFile GetEncounterFile()
    {
        CheckAdded();
        return this.encounterFile;
    }

    public void CheckAdded()
    {
        this.isItAdded = true;
    }
}
