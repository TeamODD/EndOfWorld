using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterFilesSavingObject", menuName = "EncounterSystem/EncounterFilesSavingObject")]
[System.Serializable]
public class SaveEncounterFileList : ScriptableObject
{
    public List<EncounterFile> unusedEncounterFiles;

    public List<EncounterFile> usedEncounterFiles;
}
