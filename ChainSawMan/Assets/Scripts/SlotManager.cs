using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotManager : MonoBehaviour
{
    public GameObject[] slotPos;
    public GameObject[] skillButton;
    public GameObject[] skillBtn;

    bool mergeOverlap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SekSkillBut(int slot, int skillnum)
    {
        if (!mergeOverlap)
        {
            GameManager.Instance.SpawnSet -= 1;

            if(slotPos[slot - 1].transform.childCount > 0) 
                Destroy(slotPos[slot - 1].transform.GetChild(0).gameObject);

            Instantiate(skillButton[skillnum - 1], slotPos[slot - 1].transform, false);
            mergeOverlap = true;
        }
        yield return new WaitForSeconds(0.5f);

        mergeOverlap = false;
    }



}
