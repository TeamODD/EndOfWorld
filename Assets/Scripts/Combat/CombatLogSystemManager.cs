using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CombatLogSystemManager : MonoBehaviour
{
    private const float ANIMATIONTIME = 0.2f;
    private readonly Vector2 SCROLLEND = Vector2.zero;
    private readonly string DONOTHING = "���� �ƹ��� �ൿ�� ���� �ʾҴ�.";
    
    [SerializeField]
    private ScrollRect scrollViewObject;
    [SerializeField]
    private GameObject contentObject;
    [SerializeField]
    private TextMeshProUGUI skillTextObject;

    public IEnumerator ScrollPositionSet()
    {
        float currentTime = 0.0f;
        while (currentTime < ANIMATIONTIME)
        {
            currentTime += (Time.deltaTime);
            scrollViewObject.normalizedPosition = Vector2.Lerp(scrollViewObject.normalizedPosition, SCROLLEND, currentTime / ANIMATIONTIME);
            yield return null;
        }

        scrollViewObject.normalizedPosition = SCROLLEND;

        yield return null;
    }

    public void setTextInContents(SkillDB skill, bool isHit)
    {
        skillTextObject.text = null;
        string _skillText = null;

        if (skill != null)
        {
            _skillText += skill.USINGTEXT;

            if (isHit)
            {
                _skillText += " " + skill.HITTEXT;
            }

            else
            {
                _skillText += " " + skill.MISSTEXT;
            }
        }

        else
        {
            _skillText = DONOTHING;
        }

        logInstantiate(_skillText);
    }

    public void SetDamageText(SkillDB skill, int skillDamage)
    {
        skillTextObject.text = null;
        string _skillText = null;
        if (skill.TARGET == SkillSO.Target.Enemy) _skillText += "���� ";
        else _skillText += "�÷��̾�� ";
        
        switch (skill.ATTACKTYPE)
        {
            case SkillSO.SkillAttackType.Attack:
                _skillText += skillDamage.ToString() + " ���ظ� �Ծ���.";
                break;

            case SkillSO.SkillAttackType.Defense:
                _skillText += skillDamage.ToString() + " ���� �����.";
                break;

            case SkillSO.SkillAttackType.Heal:
                _skillText += skillDamage.ToString() + " ��ŭ ȸ���ߴ�.";
                break;
        }

        logInstantiate(_skillText);
    }

    public void startScroll()
    {
        StartCoroutine(ScrollPositionSet());
    }

    private void logInstantiate(string _skillText)
    {
        TextMeshProUGUI text = Instantiate(skillTextObject, contentObject.transform);
        StartCoroutine(TypingEffect(_skillText, text));
    }

    IEnumerator TypingEffect(string _skillText, TextMeshProUGUI textObject)
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < _skillText.Length; i++)
        {
            stringBuilder.Append(_skillText[i]);
            textObject.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
