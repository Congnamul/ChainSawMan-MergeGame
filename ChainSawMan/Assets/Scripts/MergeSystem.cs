using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeSystem : MonoBehaviour
{
    public int level;
    float pick_time;

    public bool isDrag;
    public bool isMerge;
    bool isSpawn;

    public GameObject mergeEffect;

    public Text leveltxt;

    CircleCollider2D Circle;

    public GameObject[] image;

    GameObject DragTop;
    GameObject DragBottom;

    // Start is called before the first frame update
    void Start()
    {
        Circle = GetComponent<CircleCollider2D>();
        level = 1;
        isDrag = false;
        DragTop = GameObject.Find("DragTop");
        DragBottom = GameObject.Find("DragBottom");
    }

    // Update is called once per frame
    void Update()
    {
        
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;
        leveltxt.text = level + "";
        for(int i = 0; i < image.Length; i++)
        {
            image[i].SetActive(i+1 == level);
        }
    }

    void OnMouseDrag()
    {
        isDrag = true;
        isSpawn = true;

        pick_time += Time.deltaTime;

        if (pick_time < 0.1f) return;

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.z + 10f));

        point.x = Mathf.Clamp(point.x, DragTop.transform.position.x, DragBottom.transform.position.x);
        point.y = Mathf.Clamp(point.y, DragBottom.transform.position.y, DragTop.transform.position.y);

        transform.position = point;

        mergeEffect.SetActive(true);
        GameObject[] mergeAll = GameObject.FindGameObjectsWithTag("Merge");
        for (int i = 0; i < mergeAll.Length; i++)
        {
            if (level == mergeAll[i].GetComponent<MergeSystem>().level)
            {
                mergeAll[i].GetComponent<MergeSystem>().mergeEffect.SetActive(true);
            }
        }
    }

    void OnMouseUp()
    {
        isDrag = false;
        pick_time = 0;

        mergeEffect.SetActive(false);
        GameObject[] mergeAll = GameObject.FindGameObjectsWithTag("Merge");
        for (int i = 0; i < mergeAll.Length; i++)
        {
            if (level == mergeAll[i].GetComponent<MergeSystem>().level)
            {
                mergeAll[i].GetComponent<MergeSystem>().mergeEffect.SetActive(false);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Merge")
        {
            MergeSystem other = collision.gameObject.GetComponent<MergeSystem>();
            if (level == other.level && !isMerge && !other.isMerge && level < 4 && !other.isDrag && !isDrag && isSpawn)
            {
                other.Hide(transform.position);
                LevelUPV2();

                GameManager.Instance.SpawnSet--;
            }
        }

        if (collision.gameObject.tag == "Slot" && isDrag == false)
        {
            Slot slot = collision.GetComponent<Slot>();
            slot.slotSkill = level;
            slot.OnSkillUpdate();
            Destroy(gameObject);
        }
    }


    public void Hide(Vector3 targetPos)
    {
        isMerge = true;
        Circle.enabled = false;
        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int count = 0;
        while (count < 20)
        {
            count++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }

        isMerge = false;

        Destroy(gameObject);
    }




    void LevelUPV2()
    {
        isMerge = true;
        StartCoroutine(LevelUPRoutine());
    }

    IEnumerator LevelUPRoutine()
    {
        level = level + 1;
        yield return new WaitForSeconds(0.1f);

        isMerge = false;
    }
}