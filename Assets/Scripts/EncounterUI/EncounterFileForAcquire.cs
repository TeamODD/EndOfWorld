using EndOfWorld.EncounterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterFileForAcquire", menuName = "EncounterSystem/EncounterFileForAcquire")]
[System.Serializable]
public class EncounterFileForAcquire : ScriptableObject
{
    [SerializeField]
    private EncounterFileListForAcquiredFiles specialEncounterFileList;

    [SerializeField]
    private EncounterFile encounterFile;

    public void AcquireEncounterFile()
    {
        specialEncounterFileList.SaveToAcquireFileList(encounterFile);
    }

}
