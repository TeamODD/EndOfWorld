using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogSystemManager : MonoBehaviour
{
    private const float ANIMATIONTIME = 0.2f;
    private readonly Vector2 SCROLLEND = Vector2.zero;
    private readonly string DONOTHING = "적은 아무런 행동도 하지 않았다.";
    [SerializeField]
    private ScrollRect scrollViewObject;
    [SerializeField]
    private GameObject contentObject;
    [SerializeField]
    private TextMeshProUGUI skillText;

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
        skillText.text = null;

        if(skill != null)
        {
            skillText.text += skill.USINGTEXT;

            if (isHit)
            {
                skillText.text += " " + skill.HITTEXT;
            }

            else
            {
                skillText.text += " " + skill.MISSTEXT;
            }
        }

        else
        {
            skillText.text = DONOTHING;
        }

        Instantiate(skillText, contentObject.transform);
    }

    public void SetDamageText(SkillDB skill, int skillDamage)
    {
        skillText.text = null;

        if (skill.TARGET == SkillSO.Target.Enemy) skillText.text += "상대는 ";
        else skillText.text += "플레이어는 ";
        
        switch (skill.ATTACKTYPE)
        {
            case SkillSO.SkillAttackType.Attack:
                skillText.text += skillDamage.ToString() + " 피해를 입었다.";
                break;

            case SkillSO.SkillAttackType.Defense:
                skillText.text += skillDamage.ToString() + " 방어도를 얻었다.";
                break;

            case SkillSO.SkillAttackType.Heal:
                skillText.text += skillDamage.ToString() + " 만큼 회복했다.";
                break;
        }


        Instantiate(skillText, contentObject.transform);
        StartCoroutine (ScrollPositionSet());
    }
}
