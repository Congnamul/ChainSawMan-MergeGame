using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : Creature
{
    public GameObject rayPos;
    public GameObject backGround;
    public Slider hpBar;
    CutSceneManager cutSceneManager;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public bool isFight;

    public float shakeTime;
    public float shakeAmount;

    public GameObject makimaEffect;
    private void Start()
    {
        anim = GetComponent<Animator>();
        cutSceneManager = GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (cutSceneManager.isCut) return;
        EnemyDetection();
        hpBar.value = Mathf.Lerp(hpBar.value, hp / 100, Time.deltaTime * 2);
    }

    private void EnemyDetection()
    {
        float rayMaxDistance = 0.8f;
        RaycastHit2D hit = Physics2D.Raycast(rayPos.transform.position, Vector2.right, rayMaxDistance);
        Debug.DrawRay(rayPos.transform.position, rayPos.transform.right * rayMaxDistance, Color.red);
        if (hit.collider!= null)
        {
            MapMove[] mapComponents = backGround.GetComponentsInChildren<MapMove>();
            for (int i = 0; i < mapComponents.Length; i++)
            {
                mapComponents[i].enabled = false;
            }

            isFight = true;

            anim.SetBool("isRun", false);
        }
        else
        {
            MapMove[] mapComponents = backGround.GetComponentsInChildren<MapMove>();
            for (int i = 0; i < mapComponents.Length; i++)
            {
                mapComponents[i].enabled = true;
            }

            isFight = false;

            anim.SetBool("isRun", true);
        }
    }
    public void GetDamage(float dam)
    {
        hp -= dam;
        if(dam > 0)
        {
            StartCoroutine(DamageRed());
            StartCoroutine(PlayerShake());
        }
            
    }


    public void AttackAnime()
    {
        anim.SetTrigger("attackEnemy");
    }

    public void DenjiSkillIdentity()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }


    public void isDamage()
    {

    }

    IEnumerator DamageRed()
    {
        if (GameManager.Instance.isBoss) Instantiate(makimaEffect);
        spriteRenderer.color = new Color(0.8f, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator PlayerShake()
    {
        Vector2 originPosition = new Vector3(-1.79f, 1.18f, 0);
        float elapsedTime = 0.0f;

        while (elapsedTime < shakeTime)
        {
            Vector2 randomPoint = originPosition + Random.insideUnitCircle * shakeAmount;
            gameObject.transform.localPosition = new Vector3(randomPoint.x, randomPoint.y, 0);

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        gameObject.transform.localPosition = new Vector3(-1.79f, 1.18f, 0);
    }
}
