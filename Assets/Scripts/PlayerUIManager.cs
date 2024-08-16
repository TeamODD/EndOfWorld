using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public TextMeshProUGUI HPUI;
    public TextMeshProUGUI AttackPointUI;
    public TextMeshProUGUI DefencePointUI;
    public TextMeshProUGUI SpeedPointUI;

    public void HPUpdate(int maxHP, int currentHP)
    {
        HPUI.text="HP "+currentHP+"/"+maxHP;
    }
    public void AttackPointUpdate(int attackPoint)
    {
        AttackPointUI.text="ATK "+ attackPoint;
    }
    public void DefencePointUpdate(int defencePoint)
    {
        DefencePointUI.text = "DEF " + defencePoint;
    }
    public void SpeedPointUpdate(int speedPoint)
    {
        SpeedPointUI.text = "SPD " + speedPoint;
    }
}