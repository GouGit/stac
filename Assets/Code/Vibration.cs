using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {

    public static Vibration Instance = null;
    public float Factor = 1.0f;
    private const string PlayerPrefsKey = "VIBRATION";

    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject vibrator;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaClass vibrationEffectClass;
    public static int defaultAmplitude;

    private UnityEngine.UI.Text m_VibrationText;

    /*
     * "CreateOneShot": One time vibration
     * "CreateWaveForm": Waveform vibration
     * 
     * Vibration Effects class (Android API level 26 or higher)
     * Milliseconds: long: milliseconds to vibrate. Must be positive.
     * Amplitude: int: Strenght of vibration. Between 1-255. (Or default value: -1)
     * Timings: long: If submitting a array of amplitudes, then timings are the duration of each of these amplitudes in millis.
     * Repeat: int: index of where to repeat, -1 for no repeat
     */

    // 진동의 세기를 조절합니다. (Factor는 실행될 모든 진동에 곱해집니다.) ※ 구형 스마트폰은 진동 세기 및 시간 조절이 안됨
    public void SetVibrationFactor(float value)
    {
        Factor = value;
        Factor = Mathf.Clamp(Factor, 0, 1);
        if(m_VibrationText != null)
            m_VibrationText.text = ((int)(value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat(PlayerPrefsKey, Factor);
        PlayerPrefs.Save();
    }

    void OnEnable() {
        Instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        if (getSDKInt() >= 26) {
            vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
        }
#endif
        m_VibrationText = GameObject.Find("VibrationText").GetComponent<UnityEngine.UI.Text>();

        // 데이터가 저장되어있지 않으면 기본값 (1)로 설정
        Factor = PlayerPrefs.GetFloat(PlayerPrefsKey, -1);
        if (Factor == -1)
        {
            SetVibrationFactor(1);
        }

        // 메인 씬일경우 설정의 슬라이드를 저장된 값과 동일하게 맞춘다
        if(SceneLoader.GetNowSceneIndex() == 0)
        {
            GameObject.Find("VibrationController").transform.GetChild(0).GetComponent<UnityEngine.UI.Slider>().value = Factor;
            m_VibrationText.text = ((int)(Factor * 100)).ToString() + "%";
        }
    }

    //Works on API > 25
    public void CreateOneShot(long milliseconds) {
        
        if(isAndroid()) {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26) {
                CreateOneShot(milliseconds, defaultAmplitude);
            }
            else {
                OldVibrate(milliseconds);
            }
        }
        //If not android do simple solution for now
        else {
            Handheld.Vibrate();
        }   
    }

    public void CreateOneShot(long milliseconds, int amplitude) {
        
        if (isAndroid()) {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26) {
                CreateVibrationEffect("createOneShot", new object[] { milliseconds, amplitude * Factor });
            }
            else {
                OldVibrate(milliseconds);
            }
        }
        //If not android do simple solution for now
        else {
            Handheld.Vibrate();
        }
    }

    //Works on API > 25
    public void CreateWaveform(long[] timings, int repeat) {
        //Amplitude array varies between no vibration and default_vibration up to the number of timings
        
        if (isAndroid()) {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26) {
                CreateVibrationEffect("createWaveform", new object[] { timings, repeat });
            }
            else {
                OldVibrate(timings, repeat);
            }
        }
        //If not android do simple solution for now
        else {
            Handheld.Vibrate();
        }
    }

    public void CreateWaveform(long[] timings, int[] amplitudes, int repeat) {
        if (isAndroid()) {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26) {
                CreateVibrationEffect("createWaveform", new object[] { timings, amplitudes, repeat });
            }
            else {
                OldVibrate(timings, repeat);
            }
        }
        //If not android do simple solution for now
        else {
            Handheld.Vibrate();
        }

    }

    //Handels all new vibration effects
    private void CreateVibrationEffect(string function, params object[] args) {

        AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(function, args);
        vibrator.Call("vibrate", vibrationEffect);
    }

    //Handles old vibration effects
    private void OldVibrate(long milliseconds) {
        vibrator.Call("vibrate", milliseconds);
    }
    private void OldVibrate(long[] pattern, int repeat) {
        vibrator.Call("vibrate", pattern, repeat);
    }

    public bool HasVibrator() {
        return vibrator.Call<bool>("hasVibrator");
    }

    public bool HasAmplituideControl() {
        if (getSDKInt() >= 26) {
            return vibrator.Call<bool>("hasAmplitudeControl"); //API 26+ specific
        }
        else {
            return false; //If older than 26 then there is no amplitude control at all
        }

    }

    public void Cancel() {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private int getSDKInt() {
        if(isAndroid()) {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
                return version.GetStatic<int>("SDK_INT");
            }
        }
        else {
            return -1;
        }
        
    }

    private bool isAndroid() {
#if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
#else
        return false;
#endif
    }
}