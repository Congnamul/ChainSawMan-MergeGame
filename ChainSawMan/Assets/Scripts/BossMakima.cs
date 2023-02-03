using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMakima : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator anim, sliderAnim;

    public GameObject rayPos;
    Enemy enemy;

    bool isMeet;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        spriteRenderer.enabled = false;
        anim.enabled = false;

        enemy.hpBar = GameObject.Find("MakimaSlider").GetComponent<Slider>();
        sliderAnim = GameObject.Find("MakimaSlider").GetComponent<Animator>();
    }

    private void Update()
    {
        MakimaRay();
    }

    private void MakimaRay()
    {
        float rayMaxDistance = 0.8f;
        RaycastHit2D hit = Physics2D.Raycast(rayPos.transform.position, Vector2.left, rayMaxDistance);
        Debug.DrawRay(rayPos.transform.position, rayPos.transform.right * -rayMaxDistance, Color.red);
        if (hit.collider != null)
        {
            spriteRenderer.enabled = true;
            anim.enabled = true;
            StartCoroutine(GameManager.Instance.MakimaCutScene());
            if (!isMeet) StartCoroutine(MakimaAttack());

        }
    }

    IEnumerator MakimaAttack()
    {
        sliderAnim.SetBool("isBoss", true);

        isMeet = true;
        enemy.attack = 0;
        yield return new WaitForSeconds(14f);
        enemy.attack = enemy.constAttack;
        anim.SetBool("isMeet", false);
    }
}
