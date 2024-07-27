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


    private void Awake()
    {
        printManager = GameObject.FindWithTag("PrintManager").GetComponent<PrintManager>();
    }

    private void Start()
    {
        BringAcquiredFiles();
        //ShuffleList();
        encounterFile = unusedEncounterFileList[0];
        //StartCoroutine(PrintEncounter());
    }

    private void BringAcquiredFiles()
    {
        unusedEncounterFileList.AddRange(encounterFileListForAcquiredFiles.EncounterFiles);
        unusedEncounterFileList = unusedEncounterFileList.Distinct().ToList(); //중복 제거
    }

    //Fisher Yates algorithm (Knuth Shuffle)
    public void SelectRandomEncounterFile()
    {
        ShuffleList();
        encounterFile = unusedEncounterFileList[0];
    }

    IEnumerator PrintEncounter()
    {
        CopyList();

        foreach (var item in itemList)
        {
            yield return new WaitUntil(() => CallPrintManager(item) == true);
            yield return new WaitForFixedUpdate();
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
            CopyList();
            ConnectEncounter();
        }
        else //인카운터 파일이 null일 경우 
        {
            SelectRandomEncounterFile();
            CopyList();
            SkipEncounter();
        }

        SaveData();
    }

    public void SkipEncounter()
    {
        printManager.ReturnObjects();
        StartCoroutine(PrintEncounter());
    }

    public void ConnectEncounter()
    {
        printManager.ReturnChoiceObjects();
        StartCoroutine(PrintEncounter());
    }

    private void CopyList()
    {
        this.itemList = new List<Item>(encounterFile.itemList);
    }

    private bool CallPrintManager(Item item)
    {
        switch(item.ItemType)
        {
            case ItemType.Text:
                TextItem textItem = item as TextItem;
                string text = textItem.text;

                return printManager.PrintContent(text);

            case ItemType.Sprite:
                SpriteItem spriteItem = item as SpriteItem;
                Sprite sprite = spriteItem.sprite;

                return printManager.PrintContent(sprite);

            case ItemType.Choice:
                ChoiceItem choiceItem = item as ChoiceItem;
                choiceItemList = choiceItem.choiceList;

                return printManager.PrintContent(choiceItemList);
        }


        Debug.Log("Error occurs on CallPrintManager.cs");
        return false;
    }

    private static System.Random random = new System.Random();
    private void ShuffleList()
    {
        //Fisher Yates Shuffle

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
        usedEncounterFileList.Add(unusedEncounterFileList[0]);
        unusedEncounterFileList.RemoveAt(0);
    }

    private void SaveData()
    {
        saveEncounterFileList.unusedEncounterFiles = new List<EncounterFile>(unusedEncounterFileList);
        saveEncounterFileList.usedEncounterFiles = new List<EncounterFile>(usedEncounterFileList);

        EditorUtility.SetDirty(saveEncounterFileList);
        encounterFileListForAcquiredFiles.SaveData();
    }
}
