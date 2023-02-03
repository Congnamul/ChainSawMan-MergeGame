using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSc : MonoBehaviour
{
    GameObject trash;
    Button myButton;
    CutSceneManager _cutSceneManager;
    private Enemy enemy;
    private Player player;

    public GameObject skillEffect;

    private void Start()
    {
        trash = GameObject.Find("Trash");
        myButton = GetComponent<Button>();
    }

    private void Update()
    {
        myButton.interactable = (!GameManager.Instance.isPlayerDie) ? true : false;
        if(!GameManager.Instance.isPlayerDie)
            myButton.interactable = (!GameManager.Instance.isCoolTime) ? true : false;

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _cutSceneManager = GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>();
    }

    public void GoTrash()
    {
        
        TrashRoom();
    }

    public void AkiSkill(int damage)
    {
        if (player.isFight)
        {
            _cutSceneManager.CutPlay(0);
            if ( !_cutSceneManager.isReturn )
            {
                StartCoroutine(AkiKor(damage, 3.8f));
            }
            else
            {
                if( _cutSceneManager.isFirst[0] == 0 ) StartCoroutine(AkiKor(damage, 3.8f));
                else StartCoroutine(AkiKor(damage, 0f));
            }
        }  
    }

    public void DenjiSkill(int damage)
    {
        
        if (player.isFight)
        {
            _cutSceneManager.CutPlay(3);
            if (!_cutSceneManager.isReturn)
            {
                StartCoroutine(DenjiKor(damage, 7.2f));
            }
            else
            {
                if (_cutSceneManager.isFirst[3] == 0) StartCoroutine(DenjiKor(damage, 7.2f));
                else StartCoroutine(DenjiKor(damage, 0f));
            }
        }
    }

    IEnumerator AkiKor(int damage, float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.CoolTIme();
        player.AttackAnime();
        GameObject akiSkill = Instantiate(skillEffect);
        Destroy(akiSkill, 2f);
        yield return new WaitForSeconds(0.4f);
        //damage
        enemy.GetDamage(Random.Range(40, 61));
        StartCoroutine(_cutSceneManager.Shake());
    }


    public void HimenoSkill()
    {
        
        if (player.isFight)
        {
            _cutSceneManager.CutPlay(1);
            if (!_cutSceneManager.isReturn)
            {
                StartCoroutine(HimenoSkill(3.8f));
            }
            else
            {
                if (_cutSceneManager.isFirst[1] == 0) StartCoroutine(HimenoSkill(3.8f));
                else StartCoroutine(HimenoSkill(0f));
            }
        }


    }

    public void PowerSkill(int Heal)
    {

        _cutSceneManager.CutPlay(2);
        if (!_cutSceneManager.isReturn)
        {
            StartCoroutine(PowerKor(3.8f));
        }
        else
        {
            if (_cutSceneManager.isFirst[2] == 0) StartCoroutine(PowerKor(3.8f));
            else StartCoroutine(PowerKor(0f));
        }


        
    }

    public IEnumerator PowerKor(float time)
    {

        yield return new WaitForSeconds(time);
        GameManager.Instance.CoolTIme();
        if( player.hp + 20 <= 100) player.hp += 20;

        GameObject powerSkill = Instantiate(skillEffect);
        Destroy(powerSkill, 1.2f);
    }

    public IEnumerator DenjiKor(int damage, float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.CoolTIme();
        player.hp += 20;
        player.DenjiSkillIdentity();
        GameObject denjiSkill = Instantiate(skillEffect);
        Destroy(denjiSkill, 5f);
        yield return new WaitForSeconds(2.5f);
        //damage
        enemy.GetDamage(Random.Range(200, 301));
        StartCoroutine(_cutSceneManager.Shake());
        yield return new WaitForSeconds(0.5f);
        player.DenjiSkillIdentity();
    }

    private IEnumerator HimenoSkill(float time)
    {
        yield return new WaitForSeconds(time);
        enemy.StopAnime();
        GameObject himenoSkill = Instantiate(skillEffect);
        Destroy(himenoSkill, 5f);
        StartCoroutine(enemy.Stunntxt());
        GameManager.Instance.CoolTIme();
        enemy.attack = 0;
        yield return new WaitForSeconds(5f);
        enemy.attack = enemy.constAttack;
        enemy.StartAnime();
    }

    private void TrashRoom()
    {
        transform.parent = trash.transform;
        transform.position = trash.transform.position;
        Destroy(gameObject, 15f);
    }

}
