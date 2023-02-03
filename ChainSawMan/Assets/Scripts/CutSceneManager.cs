using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public GameObject[] cutObject;
    public VideoPlayer cutVideo;
    public VideoClip[] cutSource;
    public Image fade;

    public float shakeTime;
    public float shakeAmount;
    public Transform cam;

    public int[] isFirst = new int[4];
    public bool isReturn;
    public bool isCut;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < isFirst.Length; i++)
        {
            isFirst[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void CutPlay(int num)
    {
        if (isFirst[num] == 1 && isReturn) return;

        isCut = true;
        StartCoroutine(CameraOn());
        //cutObject[0].SetActive(true);
        cutObject[1].SetActive(true);
        cutVideo.clip = cutSource[num];
        StartCoroutine(SetFalse(num));
    }

    IEnumerator SetFalse(int num)
    {
        yield return new WaitForSeconds(3.8f);
        isFirst[num] = 1;
        isCut = false;
        if (num != 2)
        {
            //StartCoroutine(Shake());
        }
        if(num == 3) yield return new WaitForSeconds(3.4f);
        cutObject[0].SetActive(false);
        cutObject[1].SetActive(false);

    }

    IEnumerator CameraOn()
    {
        yield return new WaitForSeconds(0.1f);

        cutObject[0].SetActive(true);
    }

    public IEnumerator Shake()
    {
        Vector2 originPosition = new Vector3(0, 2.5f, -10);
        float elapsedTime = 0.0f;

        while (elapsedTime < shakeTime)
        {
            Vector2 randomPoint = originPosition + Random.insideUnitCircle * shakeAmount;
            cam.localPosition = new Vector3(randomPoint.x, randomPoint.y, -10);

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        cam.localPosition = new Vector3(0, 2.5f, -10);
    }
}
