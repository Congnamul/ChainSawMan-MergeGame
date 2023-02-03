using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Creature
{
    public GameObject rayPos;
    public Slider hpBar;
    public bool isAttack;
    bool isDie;
    Animator anim;
    float maxhp;
    int[] costInt = new int[] { 1, 3, 5, 7, 7 };

    CutSceneManager cutSceneManager;
    public GameObject DamageTxt;
    public GameObject stunTxt;


    public int[] upHp = new int[4];
    public int[] upAttack = new int[4];


    private void Start()
    {
        isAttack = true;
        isDie = false;
        anim = GetComponent<Animator>();

        cutSceneManager = GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>();

        if ((int)GameManager.Instance.countEnemy / 5 >= 4)
        {

            this.hp = upHp[3];
            this.attack = upAttack[3];
        }
        else
        {
            this.hp = upHp[(int)GameManager.Instance.countEnemy / 5];
            this.attack = upAttack[(int)GameManager.Instance.countEnemy / 5];
        }

        maxhp = hp;
        constAttack = attack;

    }

    private void Update()
    {
        if (cutSceneManager.isCut) return;
        Move();
        EnemyDetection();
        Die();

        hpBar.value = Mathf.Lerp(hpBar.value, hp / maxhp, Time.deltaTime * 2);
    }
    public void Move()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    public void EnemyDetection()
    {
        float rayMaxDistance = 0.8f;
        RaycastHit2D hit = Physics2D.Raycast(rayPos.transform.position, Vector2.left, rayMaxDistance);
        Debug.DrawRay(rayPos.transform.position, rayPos.transform.right * -rayMaxDistance, Color.red);
        if (hit.collider != null)
        {
            if (isAttack == true && hit.collider.tag != gameObject.tag)
            {
                StartCoroutine(Attack(hit.collider.gameObject.GetComponent<Player>()));
                speed = 0;

                
            }
        }
    }

    public void GetDamage(int dam)
    {
        hp -= dam;
        if (cutSceneManager.isCut)
            StartCoroutine(spawntxt(dam));
        else
        {
            GameObject hudText = Instantiate(DamageTxt);
            hudText.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
            hudText.GetComponent<DamageTxt>().damage = dam;
        }
    }
    public IEnumerator spawntxt(int dam)
    {
        yield return new WaitForSeconds(3.5f);
        GameObject hudText = Instantiate(DamageTxt);
        hudText.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
        hudText.GetComponent<DamageTxt>().damage = dam;

    }

    public IEnumerator Stunntxt()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject hudText = Instantiate(stunTxt);
        hudText.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
    }


    public IEnumerator Attack(Player player)
    {
        isAttack = false;
        yield return new WaitForSeconds(AttackCool);
        anim.SetTrigger("isAttack");
        player.GetDamage(attack);
        isAttack = true;
    }
    
    public void Die()
    {
        if (hp <= 0 && !isDie)
        {
            isDie = true;

            GameManager.Instance.cost += costInt[(int)GameManager.Instance.countEnemy / 5];
            GameManager.Instance.countEnemy++;
            Destroy(gameObject, 1f);
        }
    }

    public void StopAnime()
    {
        anim.GetComponent<Animator>().speed = 0;
    }

    public void StartAnime()
    {
        anim.GetComponent<Animator>().speed = 1;
    }
}
