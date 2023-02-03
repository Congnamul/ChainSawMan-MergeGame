using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int slotNum;
    public int slotSkill = 0;
    //public bool isSkillSet = false;

    public void OnSkillUpdate()
    {
        SlotManager slotManager = GameObject.Find("SlotManager").GetComponent<SlotManager>();
        StartCoroutine(slotManager.SekSkillBut(slotNum, slotSkill));
    }
}
