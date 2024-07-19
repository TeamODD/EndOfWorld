using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatHUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ATK_Text;
    [SerializeField] private TMP_Text DEF_Text;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button skillButton;
    [SerializeField] private GameObject skillButtonParent;

    public void SetHUD(StatSystem unit)
    {
        ATK_Text.text = "" + unit.currentAttackPoint;
        DEF_Text.text = "" + unit.currentDefensePoint;
        hpSlider.maxValue = unit.maxHitPoint;
        hpSlider.value = unit.currentHitPoint;
    }

    public void SetHPSlider(int currentHP)
    {
        hpSlider.value = currentHP;
    }

    public void SetSkillButton(List<SkillDB> skills)
    {
        int index = 0;

        //당장은 테스트를 위해 위치상관없이 생성만
        foreach (SkillDB skill in skills)
        {
            Button button = Instantiate(skillButton);
            button.transform.SetParent(skillButtonParent.transform);
            button.onClick.AddListener(CombatSystemManager.Instance.OnSkillButton);

            SkillButtonInfo info = button.GetComponent<SkillButtonInfo>();
            info.InfoInit(index, skill.NAME, skill.USES);


            index++;
        }
    }

}
