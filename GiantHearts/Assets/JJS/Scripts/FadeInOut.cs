using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image image;
    float alpha = 225;
    public delegate void myDelegate();
    public delegate void myDelegateINT(int a);

    void Start()
    {
        //image.color = new Color(0, 0, 0, alpha);
    }

    //----------------------------------------------
    //�ܺο��� �Լ��� �ҷ������ŵ� �� �Լ����� ����

    private void FadeIn(int playtime)    // ������� ��
    {
        //StartCoroutine(FadeInStart(playtime));
    }

    public void FadeOut(myDelegate action,int playtime)   // ��ο����� ��
    {
        StartCoroutine(FadeOutStart(action,playtime));
    }

    //----------------------------------------------

    IEnumerator FadeInStart(myDelegate action, int playtime)
    {
        action();

        float elapsedTime = 0f;

        while (elapsedTime < playtime)
        {
            alpha = elapsedTime / playtime;

            Mathf.Lerp(255, 0, alpha);
            Color c = image.color;
            c.a = 1-alpha;
            image.color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator FadeOutStart(myDelegate action, int playtime)
    {
        Color temp = image.color;
        temp.a = 0.0f;
        image.color = temp;
        image.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < playtime)
        {
            alpha = elapsedTime / playtime;

            Mathf.Lerp(0, 255, alpha);
            Color c = image.color;
            c.a = alpha;
            image.color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeInStart(action,playtime));
    }


    /// <summary>
    /// 
    /// </summary>
    // ================================================================= int�� ����
    public void FadeOutINT(myDelegateINT action,int param ,int playtime)   // ��ο����� ��
    {
        StartCoroutine(FadeOutStartINT(action, param, playtime));
    }

    IEnumerator FadeOutStartINT(myDelegateINT action,int param ,int playtime)
    {
        Color temp = image.color;
        temp.a = 0.0f;
        image.color = temp;
        image.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < playtime)
        {
            alpha = elapsedTime / playtime;

            Mathf.Lerp(0, 255, alpha);
            Color c = image.color;
            c.a = alpha;
            image.color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeInStartINT(action, param,playtime));
    }

    IEnumerator FadeInStartINT(myDelegateINT action,int param ,int playtime)
    {
        action(param);

        float elapsedTime = 0f;

        while (elapsedTime < playtime)
        {
            alpha = elapsedTime / playtime;

            Mathf.Lerp(255, 0, alpha);
            Color c = image.color;
            c.a = 1 - alpha;
            image.color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.gameObject.SetActive(false);
        yield break;
    }


    void Update()
    {
    }
}
