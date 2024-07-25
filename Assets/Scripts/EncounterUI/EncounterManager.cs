using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndOfWorld.EncounterSystem;
using System.IO;
using System.Linq;

public class EncounterManager : MonoBehaviour
{
    [SerializeField]
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
        StartCoroutine(PrintEncounter());
    }

    IEnumerator PrintEncounter()
    {
        copyList();

        foreach (var item in itemList)
        {
            yield return new WaitUntil(() => CallPrintManager(item) == true);
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    //���������� ������ ����� ��
    public void TakeAChoice(int index)
    {

        void selectEncounterFile()
        {
            encounterFile = choiceItemList[index].encounterFile;
        }

        if (choiceItemList[index].encounterFile != null)
        {
            selectEncounterFile();
            copyList();
            ConnectEncounter();
        }
        else //��ī���� ������ null�� ��� 
        {
            //�������� ���� �ϳ� ��
            copyList();
            SkipEncounter();
        }
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

    private void copyList()
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
}
