using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static System.Net.Mime.MediaTypeNames;
using System;

public class TextPrintManager : PullingManager
{
    [HideInInspector]
    public TMP_Text textComponent;

    public float fadeSpeed = 23f;
    public int rolloverCharacterSpread = 10;

    private PrintManager printManager;

    private void Start()
    {
        printManager = this.gameObject.GetComponent<PrintManager>();
    }

    public void PrintText(string text)
    {
        PullObject();

        SetTextObject();

        //SetTextContents(text);
        textComponent.SetText(text);

        StartCoroutine(AnimateVertexColors());
    }

    public void ForcePrint()
    {
        StopCoroutine(AnimateVertexColors());
        textComponent.color = new Color32(255, 255, 255, 255);
        EndPrint();
    }

    private void EndPrint()
    {
        printManager.isPrintDone = true;
    }

    /// <summary>
    /// Initialize this.text by component of pulled object
    /// </summary>
    private void SetTextObject()
    {
        textComponent = pulledObjectList[nextPullingIndex - 1].GetComponent<TMP_Text>();
    }

    private void SetTextContents(string text)
    {
        this.textComponent.text = text;
    }

    IEnumerator AnimateVertexColors()
    {
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;
        Color32[] newVertexColors;
        int currentCharacter = 0;
        int startingCharacterRange = currentCharacter;
        bool isRangeMax = false;
        
        while (!isRangeMax)
        {
            int characterCount = textInfo.characterCount;
            // Spread should not exceed the number of characters.
            byte fadeSteps = (byte)Mathf.Max(1, 255 / rolloverCharacterSpread);


            for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
            {
                // Skip characters that are not visible
                if (!textInfo.characterInfo[i].isVisible) continue;
                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                // Get the current character's alpha value.
                byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);


                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

                newVertexColors[vertexIndex + 0] = (Color32)newVertexColors[vertexIndex + 0];
                newVertexColors[vertexIndex + 1] = (Color32)newVertexColors[vertexIndex + 1];
                newVertexColors[vertexIndex + 2] = (Color32)newVertexColors[vertexIndex + 2];
                newVertexColors[vertexIndex + 3] = (Color32)newVertexColors[vertexIndex + 3];

                if (alpha == 255)
                {
                    startingCharacterRange += 1;

                    if (startingCharacterRange == characterCount)
                    {
                        // Update mesh vertex data one last time.
                        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                        // Reset the text object back to original state.
                        //textComponent.ForceMeshUpdate();
                        // Reset our counters.
                        currentCharacter = 0;
                        startingCharacterRange = 0;
                        isRangeMax = true; // Would end the coroutine.
                        Debug.Log("Text Print Animation has done");
                    }

                }
            }

            // Upload the changed vertex colors to the Mesh.
            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            if (currentCharacter + 1 < characterCount) currentCharacter += 1;
            yield return new WaitForSeconds(0.25f - fadeSpeed * 0.01f);
        }

        EndPrint();
        yield return null;
    }
}
