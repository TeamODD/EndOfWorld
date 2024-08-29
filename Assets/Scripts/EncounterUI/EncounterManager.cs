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
    /// Ư���� ���ǿ� ���� �߰��� EncounterFile�� �������� �Լ�
    /// </summary>
    private void BringAcquiredFiles()
    {
        if (encounterFileListForAcquiredFiles.acquiredEncounterFileList == null) return;
        int listLength = encounterFileListForAcquiredFiles.acquiredEncounterFileList.Count;

        for (short i = 0; i < listLength; i++)
        {
            //�ѹ��� �߰� �� �� ���ٸ� && EncounterFile�� chaperIndex�� EncounterManager�� chaperIndex�� ���ٸ� 
            if (!encounterFileListForAcquiredFiles.IsItAdded(i) && encounterFileListForAcquiredFiles.GetChaperIndex(i) == this.chaperIndex) 
            {   //����Ʈ�� �߰�
                unusedEncounterFileList.Add(encounterFileListForAcquiredFiles.acquiredEncounterFileList[i].GetEncounterFile());
            }
        }

        //unusedEncounterFileList = unusedEncounterFileList.Distinct().ToList(); //�ߺ� ����
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
        //��ī���� ���� �� ����Ʈ�� ��Ұ� ���� ���(special encounter�� �����ϰ� ���� ���)
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

    //���������� ����� ��
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
        else //��ī���� ������ null�� ��� 
        {
            thisProgressLevel += 1;

            SelectRandomEncounterFile();
            CopyItems();
            SkipEncounter();
        }

        SaveData();
    }

    /// <summary>
    /// SpecialEncounter�� �ִ��� Ȯ���ϰ� ����
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
        //����Ʈ ���̸� �ϳ��� �ٿ����� �� �ȿ��� ������ ��ҿ� ����Ʈ�� ���� �ִ� ��Ҹ� �ٲ۴�.

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
