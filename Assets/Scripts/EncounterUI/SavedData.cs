using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterFilesSavingObject", menuName = "EncounterSystem/EncounterFilesSavingObject")]
[System.Serializable]
public class SavedData : ScriptableObject
{
    public List<EncounterFile> UnusedEncounterFiles;

    public List<EncounterFile> UsedEncounterFiles;

    public int ProgressLevel;


    [ContextMenu("AddText")]
    public void ResetField()
    {
        this.UnusedEncounterFiles = new List<EncounterFile>();
        this.UsedEncounterFiles = new List<EncounterFile>();
        this.ProgressLevel = 0;
    }
}
