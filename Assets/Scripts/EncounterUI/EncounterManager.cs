using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndOfWorld.EncounterSystem;
using System.IO;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

public class EncounterManager : MonoBehaviour
{
    [SerializeField]
    private short chaperIndex;

    [SerializeField]
    private List<EncounterFile> unusedEncounterFileList;

    [SerializeField]
    private readonly List<EncounterFile> usedEncounterFileList = new List<EncounterFile>();

    [SerializeField]
    private SaveEncounterFileList saveEncounterFileList;

    [SerializeField]
    private EncounterFileListForAcquiredFiles encounterFileListForAcquiredFiles;

    private EncounterFile encounterFile;

    private PrintManager printManager;

    private List<Item> itemList;

    private List<TextAndEncounterFile> choiceItemList;

    private int thisProgressLevel = 0;

    private int encounterFileIndex;

    private void Awake()
    {
        printManager = GameObject.FindWithTag("PrintManager").GetComponent<PrintManager>();
    }

    private void Start()
    {
        BringAcquiredFiles();
        SelectRandomEncounterFile();
        StartCoroutine(PrintEncounter());
    }


    /// <summary>
    /// 특정한 조건에 의해 추가된 EncounterFile을 가져오는 함수
    /// </summary>
    private void BringAcquiredFiles()
    {
        if (encounterFileListForAcquiredFiles.acquiredEncounterFileList == null) return;
        int listLength = encounterFileListForAcquiredFiles.acquiredEncounterFileList.Count;

        for (short i = 0; i < listLength; i++)
        {
            //한번도 추가 된 적 없다면 && EncounterFile의 chaperIndex가 EncounterManager의 chaperIndex와 같다면 
            if (!encounterFileListForAcquiredFiles.IsItAdded(i) && encounterFileListForAcquiredFiles.GetChaperIndex(i) == this.chaperIndex) 
            {   //리스트에 추가
                unusedEncounterFileList.Add(encounterFileListForAcquiredFiles.acquiredEncounterFileList[i].GetEncounterFile());
            }
        }

        //unusedEncounterFileList = unusedEncounterFileList.Distinct().ToList(); //중복 제거
        SaveData();
    }

    public void SelectRandomEncounterFile()
    {
        ShuffleList();

        int i;

        for(i = 0; i < unusedEncounterFileList.Count; i++)
        {
            if(unusedEncounterFileList[i].progressLevel == thisProgressLevel)
            {
                encounterFile = unusedEncounterFileList[i];
                encounterFileIndex = i;
                return;
            }
        }

        if (i == unusedEncounterFileList.Count)
            Debug.LogError("No more encounter file that match with this progress level at EncounterManager.cs");
    }

    IEnumerator PrintEncounter()
    {
        //인카운터 파일 내 리스트에 요소가 없을 경우(special encounter만 저장하고 싶을 경우)
        if(encounterFile.itemList == null)
        {
            CheckAndSaveSpecialEncounter();
            yield return null;
        }

        CopyItems();

        CheckAndSaveSpecialEncounter();

        foreach (var item in itemList)
        {
            CallPrintManager(item);

            yield return new WaitUntil(() => printManager.isPrintDone == true);
            yield return null;

            printManager.isPrintDone = false;
            yield return null;
        }

        yield return null;
    }

    //선택지에서 골랐을 시
    public void TakeAChoice(int index)
    {
        void ContinueToNextEncounterFile()
        {
            encounterFile = choiceItemList[index].encounterFile;
        }

        ConveyToUsedList();

        if (choiceItemList[index].encounterFile != null)
        {
            ContinueToNextEncounterFile();
            CopyItems();
            ConnectEncounter();
        }
        else //인카운터 파일이 null일 경우 
        {
            thisProgressLevel += 1;

            SelectRandomEncounterFile();
            CopyItems();
            SkipEncounter();
        }

        SaveData();
    }

    /// <summary>
    /// SpecialEncounter가 있는지 확인하고 저장
    /// </summary>
    private void CheckAndSaveSpecialEncounter()
    {
        bool CheckIsHavingSpecialEncounter()
        {
            return this.encounterFile.specialEncounterFile != null;
        }

        if (CheckIsHavingSpecialEncounter())
        {
            encounterFileListForAcquiredFiles.SaveToAcquiredFileList(this.encounterFile.specialEncounterFile);
        }
    }
    
    private void SkipEncounter()
    {
        printManager.ReturnObjects();
        StartCoroutine(PrintEncounter());
    }

    private void ConnectEncounter()
    {
        printManager.ReturnChoiceObjects();
        StartCoroutine(PrintEncounter());
    }

    private void CopyItems()
    {
        this.itemList = new List<Item>(encounterFile.itemList);
    }

    private void CallPrintManager(Item item)
    {
        switch(item.ItemType)
        {
            case ItemType.Text:
                TextItem textItem = item as TextItem;
                string text = textItem.text;

                printManager.PrintContent(text);
                break;

            case ItemType.Sprite:
                SpriteItem spriteItem = item as SpriteItem;
                Sprite sprite = spriteItem.sprite;

                printManager.PrintContent(sprite);
                break;

            case ItemType.Choice:
                ChoiceItem choiceItem = item as ChoiceItem;
                choiceItemList = choiceItem.choiceList;

                printManager.PrintContent(choiceItemList);
                break;
        }


        Debug.Log("Print " + item.ItemType);
    }

    private static System.Random random = new System.Random();
    private void ShuffleList()
    {
        //Fisher Yates algorithm (Knuth Shuffle)
        //리스트 길이를 하나씩 줄여가며 그 안에서 랜덤한 요소와 리스트의 끝에 있는 요소를 바꾼다.

        int listLength = unusedEncounterFileList.Count;

        int n = unusedEncounterFileList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            EncounterFile value = unusedEncounterFileList[k];
            unusedEncounterFileList[k] = unusedEncounterFileList[n];
            unusedEncounterFileList[n] = value;
        }
    }

    private void ConveyToUsedList()
    {
        usedEncounterFileList.Add(unusedEncounterFileList[encounterFileIndex]);
        unusedEncounterFileList.RemoveAt(encounterFileIndex);
    }

    private void SaveData()
    {
        saveEncounterFileList.unusedEncounterFiles = new List<EncounterFile>(unusedEncounterFileList);
        saveEncounterFileList.usedEncounterFiles = new List<EncounterFile>(usedEncounterFileList);

        EditorUtility.SetDirty(saveEncounterFileList);
        encounterFileListForAcquiredFiles.SaveData();
    }
}
