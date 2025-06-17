using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScenesManager : UnitySingleton<GameScenesManager>
{
    Slider loadingSlider;  // ������ UI Ԫ��
    GameObject loadingPanel;
    GameObject bootGame;
    Text loadingText;  // ��ʾ���ؽ��ȵ��ı� UI Ԫ��
    AsyncOperation asyncLoad = null;
    public CanvasGroup transitionCanvasGroup;  // ���ɳ����� CanvasGroup
    public float transitionDuration = 1f;  // ����ʱ��
    public Dictionary<string, UnityEngine.Object> objDic = new Dictionary<string, UnityEngine.Object>();

    internal void Init(Boot boot)
    {
        bootGame = boot.transform.Find("LoadingRoot").gameObject;
        loadingPanel = boot.transform.Find("LoadingRoot/LoadingPanel").gameObject;
        transitionCanvasGroup = boot.transform.Find("LoadingRoot/Mask").GetComponent<CanvasGroup>();
        loadingText = boot.transform.Find("LoadingRoot/LoadingPanel/Slider/Text (Legacy)").GetComponent<Text>();
        loadingSlider = boot.transform.Find("LoadingRoot/LoadingPanel/Slider").GetComponent<Slider>();
        loadingPanel.SetActive(false);
    }

    /// <summary>
    /// �첽��ת����
    /// </summary>
    /// <param name="sceneName">Ŀ�곡������</param>
    /// <param name="panelName">Ŀ�곡���򿪵ĵ�һ������ һ���Ǳ���</param>
    /// <param name="prePath">��Ҫ�첽���ص���Դ</param>
    public void LoadSceneAsync(string sceneName, string panelName, string[] prePath = null)
    {
        bootGame.SetActive(true);
        StartCoroutine(LoadTransitionScene(sceneName, panelName, prePath));
    }

    // Ԥ������Դ
    private IEnumerator PreloadResourcesAsync(string[] prePath)
    {
        if (prePath == null)
        {
            yield break;
        }
        // ������ҪԤ���ص���Դ�б�
        for (int i = 0; i < prePath.Length; i++)
        {
            yield return new WaitForEndOfFrame();
            string resourcePath = prePath[i];
            ResourceRequest resourceRequest = Resources.LoadAsync(resourcePath);
            while (!resourceRequest.isDone && resourceRequest.asset == null)
            {
                // ��ѡ�����Ը�����Դ���صĽ��ȸ��½�����
                loadingText.text = "����Ԥ������Դ: " + resourcePath;
                loadingSlider.value = Mathf.Lerp(0f, 1f, resourceRequest.progress);
                yield return null;
            }

            // ��Դ�������
            Debug.Log("Ԥ������Դ���: " + resourcePath);
            if (!objDic.ContainsKey(resourcePath))
                objDic.Add(resourcePath, resourceRequest.asset);
        }
    }

    // ���ع��ɳ������������Ч��
    private IEnumerator LoadTransitionScene(string sceneName, string panelName, string[] prePath)
    {
        // ��ʾ���ɳ���������
        yield return FadeInTransition();

        // ����Ŀ�곡��
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Empty");
        asyncLoad.allowSceneActivation = true;

        // �ȴ����ɳ����������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ��ʼ����Ŀ�곡��
        yield return StartCoroutine(LoadGameSceneAsync(sceneName, panelName, prePath));
    }

    // �첽����Ŀ�곡��
    private IEnumerator LoadGameSceneAsync(string sceneName, string panelName, string[] prePath)
    {
        // �������ع���
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // ��ʼ��ʾ���ؽ���
        loadingPanel.gameObject.SetActive(true);

        while (!asyncLoad.isDone)
        {
            // ���½�����

            // ���ؽ��ȴﵽ0.9ʱ����ʾ���״̬
            if (asyncLoad.progress >= 0.9f)
            {
                Resources.UnloadUnusedAssets();
                yield return PreloadResourcesAsync(prePath);
                loadingSlider.value = 1;
                UIManager.Instance.CloseAllWindow();
                UIManager.Instance.OpenWindow(panelName);
                asyncLoad.allowSceneActivation = true;
            }
            else
            {
                loadingSlider.value = asyncLoad.progress / 8f;
                loadingText.text = "���ڼ��س���... " + (asyncLoad.progress * 100 / 8f).ToString("F0") + "%";
            }

            yield return null;
        }

        // ���ؼ��ؽ���
        loadingPanel.gameObject.SetActive(false);

        // ��������Ч��
        yield return FadeOutTransition();
        bootGame.SetActive(false);
    }

    // �������Ч��
    private IEnumerator FadeInTransition()
    {
        float timeElapsed = 0f;
        while (timeElapsed < transitionDuration)
        {
            transitionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transitionCanvasGroup.alpha = 1f;
    }

    // ��������Ч��
    private IEnumerator FadeOutTransition()
    {
        float timeElapsed = 0f;
        while (timeElapsed < transitionDuration)
        {
            transitionCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transitionCanvasGroup.alpha = 0f;
    }
}
