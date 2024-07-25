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

    //선택지에서 정답을 골랐을 시
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
        else //인카운터 파일이 null일 경우 
        {
            //랜덤으로 파일 하나 고름
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
