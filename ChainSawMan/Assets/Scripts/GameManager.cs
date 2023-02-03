using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public float cost;
    private float limitCost;
    public Text costText;

    public Slider makimaSlider;

    public GameObject[] Enemy;

    public float countEnemy;
    public float coolTimeCount;
    private float maxCoolTimeCount;
    public bool isCoolTime;
    public bool isPlayerDie;
    public bool isBoss;

    public int SpawnSet;
    public Text SpawnTxt;
    public Image[] image;

    public GameObject Merge;
    public Transform LeftTop;
    public Transform RightBottom;
    public GameObject SpawnBtn;
    Animator SpawnAnim;

    public Player player;
    public GameObject gameOver;
    public GameObject cutScene;
    public GameObject clearScene;

    bool isEnd;

    Enemy enemy;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        limitCost = 25;
        cost = 0;
        SpawnSet = 0;
        SpawnAnim = SpawnBtn.GetComponent<Animator>();
        maxCoolTimeCount = coolTimeCount;
        Time.timeScale = 1;
    }

    private void Update()
    {
        EnemyCreation();
        TextUpdate();
        CostSystem();
        MakimaSlider();
        PlayerDie();
        PlayerClear();

        if (Input.GetKeyDown("r"))
        {
            Spawn();
        }

        if (isCoolTime)
        {
            coolTimeCount -= Time.deltaTime;

            for (int i = 0; i < image.Length; i++)
                image[i].fillAmount = (coolTimeCount / maxCoolTimeCount);

            if (coolTimeCount <= 0) isCoolTime = false;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }


    }

    public void Spawn()
    {
        float PosX = Random.Range(LeftTop.position.x, RightBottom.position.x);
        float PosY = Random.Range(LeftTop.position.y, RightBottom.position.y);
        if (SpawnSet < 10 && cost >= 1 && !isPlayerDie)
        {
            SpawnSet++;
            Instantiate(Merge, new Vector3(PosX, PosY, 0), Quaternion.identity);
            SpawnAnim.SetTrigger("Click");
            cost--;
        }   
    }

    public void CostSystem()
    {
        if (countEnemy >= 15) cost += Time.deltaTime;
        else cost += Time.deltaTime * 0.5f;
        

        costText.text = "" + Mathf.Round(cost);
        if (cost >= limitCost)
        {
            cost = limitCost;
        }
    }

    private void EnemyCreation()
    {
        GameObject findEnemy = GameObject.FindWithTag("Enemy");
        if (findEnemy == null)
        {
            if( countEnemy < 21)
            {
                Instantiate(Enemy[Random.Range(0, 2)], new Vector2(6.5f, 1.5f), transform.rotation);
            }
            else if(countEnemy == 21)
            {
                Instantiate(Enemy[2], new Vector2(6.5f, 1.9f), transform.rotation);
                isBoss = true;
            }            
        }
    }

    private void TextUpdate()
    {
        SpawnTxt.text = SpawnSet + "/10";
    }

    public void CoolTIme()
    {
        isCoolTime = true;
        coolTimeCount = maxCoolTimeCount;
    }

    public void MakimaSlider()
    {
        if(!isBoss) makimaSlider.value = (countEnemy / 20);
    }

    void PlayerDie()
    {
        
        if (player.hp <= 0)
        {
            isPlayerDie = true;
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
       
        if (isPlayerDie && Input.GetKeyDown("space"))
            SceneManager.LoadScene("SampleScene");
    }

    void PlayerClear()
    {

        if (countEnemy == 22)
        {
            isEnd = true;
            clearScene.SetActive(true);
        }

        if (isEnd && Input.GetKeyDown("space"))
            SceneManager.LoadScene("Start");
    }

    public IEnumerator MakimaCutScene()
    {
        yield return new WaitForSeconds(2.5f);
        cutScene.SetActive(true);
    }

}