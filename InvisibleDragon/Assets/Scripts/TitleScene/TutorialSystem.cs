using System;
using System.Collections;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private Sprite[] m_images;
    [SerializeField] private Single[] m_sceneTimes;
    [SerializeField] private String[] m_sceneTexts;
    [SerializeField] private SceneTextPresenter textUI;
    [SerializeField] private TitleUIPresenter titleUI;
    [SerializeField] private SkipInfoMessagePresenter SkipInfoUI;
    [SerializeField] private SpriteRenderer imageRenderer;

    private Single m_skipButtonTime = 0.0f;
    private Boolean m_isSkipButtonPress = false;
    private Boolean m_isTutorialDone = false;

    private void Start()
    {
        SoundManager.Ins.PrintBGM("tutorial");
        StartCoroutine(TutorialCoroutine());
    }

    private void Update()
    {
        if (m_isTutorialDone) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_isSkipButtonPress = true;
            m_skipButtonTime = Time.time;
            SkipInfoUI.Bind();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            if (m_isSkipButtonPress && Time.time - m_skipButtonTime >= 3)
            {
                StopAllCoroutines();
                imageRenderer.sprite = m_images[m_images.Length - 1];
                textUI.Bind(m_sceneTexts[m_sceneTexts.Length - 1], 2f);
                titleUI.Bind();
                m_isSkipButtonPress = true;
                m_isTutorialDone = true;
                SkipInfoUI.Hide();
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SkipInfoUI.Hide();
            m_isSkipButtonPress = false;
        }
    }

    IEnumerator TutorialCoroutine()
    {
        textUI.Bind("", 0f);
        imageRenderer.sprite = null;
        for(Int32 i = 0; i < m_images.Length; i++)
        {
            imageRenderer.sprite = m_images[i];
            textUI.Bind(m_sceneTexts[i], m_sceneTimes[i] / 2);
            yield return new WaitForSeconds(m_sceneTimes[i]);
        }
        titleUI.Bind();
        yield return null;
    }
}