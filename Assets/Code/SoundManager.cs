using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return instance;
        }
    }
    private const string PlayerPrefsKey = "VOLUME";

    private AudioSource m_BGMSource;
    public AudioSource BGMSource
    {
        get
        {
            return m_BGMSource;
        }
    }
    private AudioSource[] m_SFXSource = new AudioSource[5];
    public AudioSource[] SFXSource
    {
        get
        {
            return m_SFXSource;
        }
    }
    private int m_iSESourceIndex;
    [Range(0, 1)]
    public float m_BGMVoslume;
    [Tooltip("클립을 배열에 넣은 후 SoundManager 스크립트의 BGMList를 수정해주세요.")]
    public AudioClip[] m_BGMClips;
    [Tooltip("클립을 배열에 넣은 후 SoundManager 스크립트의 SFXList를 수정해주세요.")]
    public AudioClip[] m_SFXClips;

    private UnityEngine.UI.Text m_VolumeText;

    public enum BGMList
    {
        BGM_MAIN,
    }

    public enum SFXList
    {
        KNIFE_1,
        MONSTER_DAMAGE,
        MONSTER_DEATH,
        UI_TOUCH,
    }

    #region STATIC_FUNCTION
    public static IEnumerator FadeIn(AudioSource audioSource, float time, float volume = 1.0f)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / time;
            yield return null;
        }
        audioSource.volume = volume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float time)
    {
        float startVolume = audioSource.volume;
        while(audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / time;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
        { Destroy(this); return; }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        
        if (SceneLoader.GetNowSceneIndex() == 0)
        {
            m_VolumeText = GameObject.Find("VolumeText").GetComponent<UnityEngine.UI.Text>();
        }

        // 데이터가 저장되어있지 않으면 기본값 (1)로 설정
        float volume = PlayerPrefs.GetFloat(PlayerPrefsKey, -1);
        if (volume == -1)
            SetVolume(1);
        else
            AudioListener.volume = volume;

        if (SceneLoader.GetNowSceneIndex() == 0)
        {
            UnityEngine.UI.Slider VolumeController = GameObject.Find("VolumeController").transform.GetChild(0).GetComponent<UnityEngine.UI.Slider>();
            VolumeController.value = volume;
            VolumeController.onValueChanged.AddListener((value) =>
            {
                SetVolume(value);
            });
            m_VolumeText.text = ((int)(volume * 100)).ToString() + "%";
        }

        m_iSESourceIndex = 0;

        m_BGMSource = gameObject.AddComponent<AudioSource>();
        m_BGMSource.loop = true;
        m_BGMSource.volume = m_BGMVoslume;
        PlayBGM(0, 5, 0.2f);
        for (int i = 0; i < 5; i++)
        {
            m_SFXSource[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public bool PlayBGM(BGMList soundIndex, float time = 0, float volume = 1.0f)
    {
        if (m_BGMClips.Length - 1 < (int)soundIndex)
            return false;
        m_BGMSource.clip = m_BGMClips[(int)soundIndex];
        StartCoroutine(FadeIn(m_BGMSource, time, volume));
        //m_BGMSource.Play();
        return true;
    }

    public void StopBGM()
    {
        if (m_BGMSource.isPlaying)
            m_BGMSource.Stop();
    }

    public bool PlaySFX(SFXList soundIndex, float pitch = 1.0f, float volume = 1.0f)
    {
        if (m_SFXClips.Length - 1 < (int)soundIndex)
            return false;
        m_SFXSource[m_iSESourceIndex].clip = m_SFXClips[(int)soundIndex];
        m_SFXSource[m_iSESourceIndex].volume = volume;
        m_SFXSource[m_iSESourceIndex].pitch = pitch;
        m_SFXSource[m_iSESourceIndex].Play();
        m_iSESourceIndex++;
        m_iSESourceIndex = m_iSESourceIndex % 5;
        return true;
    }

    public void SetVolume(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        AudioListener.volume = value;
        if (m_VolumeText != null)
            m_VolumeText.text = ((int)(value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat(PlayerPrefsKey, value);
        PlayerPrefs.Save();
    }

    // 테스트용
#if UNITY_EDITOR
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    PlayBGM(BGMList.BGM_MAIN, 5, 0.2f);
    }
#endif
}