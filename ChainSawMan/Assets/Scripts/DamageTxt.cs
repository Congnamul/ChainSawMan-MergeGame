using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTxt : MonoBehaviour
{
    public float moveSpeed;
    public float alphaSpeed;
    public float destroySpeed;
    public int damage;
    TextMesh text;
    Color alpha;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();
        if (damage != 0)
            text.text = damage + "";

        alpha = text.color;
        Destroy(gameObject, destroySpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }
}
