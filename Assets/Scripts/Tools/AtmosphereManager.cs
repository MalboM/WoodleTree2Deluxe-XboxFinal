using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour {

    public MeshRenderer cloudRenderer;
    public MeshRenderer skyRenderer;
    public Camera mainCamera;

    [HideInInspector] public enum WeatherEffect { none, rain, snow, wind, sandstorm};

    [System.Serializable] public class AtmoshphereSettings
    {
        public Color cloudColor = Color.white;
        public Color fogColor = Color.white;
        public float farClipPlaneDistance;
        private float fogDensity = 0.006f;
        public float fogStartDistance;
        public float fogDistance;
        public float rotationSpeed;
        public WeatherEffect weatherEffect;
        public Material cloudsMaterial;
    }

    public AtmoshphereSettings defaultSettings;

    public AtmoshphereSettings[] levelSettings;

    AtmoshphereSettings startSettings = new AtmoshphereSettings();
    Material cloudMat;
    Material skyMat;
    float iterator;

    float startRotation;
    float curRotation;

    AtmoshphereSettings curSettings;
    bool transitioning;

    WeatherEffect curWeatherEffect;
    [HideInInspector] public int triggerCount;

    GameObject curWeatherObj;

    public GameObject rainEffect;
    ParticleSystem rainPS;
    ParticleSystem.EmissionModule rainEM;
    ParticleSystem.MainModule rainMM;
    
    public GameObject snowEffect;
    ParticleSystem snowPS;
    ParticleSystem.EmissionModule snowEM;
    ParticleSystem.MainModule snowMM;

    public GameObject windEffect;
    ParticleSystem windPS;
    ParticleSystem.EmissionModule windEM;
    ParticleSystem.MainModule windMM;

    public GameObject sandstormEffect;
    ParticleSystem sandstormPS;
    ParticleSystem.EmissionModule sandstormEM;
    ParticleSystem.MainModule sandstormMM;

    GameObject chara;

    [HideInInspector] public string curLevel;
    [HideInInspector] public int curAtmoID;

    private void Start()
    {
        cloudMat = cloudRenderer.material;
        cloudRenderer.material = cloudMat;

        skyMat = skyRenderer.material;
        skyRenderer.material = skyMat;

        RenderSettings.fogMode = FogMode.Linear;
     //   RenderSettings.fogMode = FogMode.ExponentialSquared;

        RenderSettings.fogColor = defaultSettings.fogColor;
        skyMat.SetColor("_Color", RenderSettings.fogColor);
        cloudMat.SetColor("_Color", defaultSettings.cloudColor);
        mainCamera.farClipPlane = defaultSettings.farClipPlaneDistance + ((float)PlayerPrefs.GetInt("AddedDistance", 2) * 50f);
            if (defaultSettings.fogStartDistance >= defaultSettings.fogDistance)
                defaultSettings.fogStartDistance = defaultSettings.fogDistance - 1f;
            RenderSettings.fogStartDistance = defaultSettings.fogStartDistance;
            RenderSettings.fogEndDistance = defaultSettings.fogDistance;

    //    RenderSettings.fogDensity = defaultSettings.fogDensity;

        curWeatherEffect = WeatherEffect.none;
        curRotation = defaultSettings.rotationSpeed;

        triggerCount = 0;

        rainPS = rainEffect.GetComponentInChildren<ParticleSystem>();
        rainEM = rainPS.emission;
        rainMM = rainPS.main;

        snowPS = snowEffect.GetComponentInChildren<ParticleSystem>();
        snowEM = snowPS.emission;
        snowMM = snowPS.main;

        windPS = windEffect.GetComponentInChildren<ParticleSystem>();
        windEM = windPS.emission;
        windMM = windPS.main;

        sandstormPS = sandstormEffect.GetComponentInChildren<ParticleSystem>();
        sandstormEM = sandstormPS.emission;
        sandstormMM = sandstormPS.main;

        chara = PlayerManager.GetMainPlayer().gameObject;

        curSettings = defaultSettings;
    }

    private void LateUpdate()
    {
        if (curRotation != 0f)
        {
            cloudRenderer.transform.Rotate(new Vector3(0f, curRotation, 0f), Space.Self);
            skyRenderer.transform.rotation = cloudRenderer.transform.rotation;
        }
        if (curWeatherObj != null)
            curWeatherObj.transform.position = chara.transform.position;

        if (!transitioning)
        {
            if (mainCamera.farClipPlane != curSettings.farClipPlaneDistance + ((float)PlayerPrefs.GetInt("AddedDistance", 2) * 50f))
                mainCamera.farClipPlane = curSettings.farClipPlaneDistance + ((float)PlayerPrefs.GetInt("AddedDistance", 2) * 50f);
        }
    }

    public void EnterTrigger(int levelID)
    {
        triggerCount++;
        curAtmoID = levelID;
        StopCoroutine("Transition");
        StartCoroutine("Transition", levelSettings[levelID]);
    }

    public void ExitTrigger()
    {
        triggerCount--;
        if (triggerCount < 0)
            triggerCount = 0;
        if (triggerCount == 0)
        {
            StopCoroutine("Transition");
            StartCoroutine("Transition", defaultSettings);
        }
    }

    void ChangeWeather(WeatherEffect newEffect)
    {
        if(newEffect != curWeatherEffect) {

            //Remove Old
            if(curWeatherEffect != WeatherEffect.none)
            {
                StopCoroutine("DeactivateDelay");
                if(curWeatherEffect == WeatherEffect.rain)
                {
                    curWeatherObj = null;
                    rainEM.enabled = false;
                    rainMM.loop = false;
                    StartCoroutine("DeactivateDelay", rainEffect);
                }
                if (curWeatherEffect == WeatherEffect.snow)
                {
                    curWeatherObj = null;
                    snowEM.enabled = false;
                    snowMM.loop = false;
                    StartCoroutine("DeactivateDelay", snowEffect);
                }
                if (curWeatherEffect == WeatherEffect.wind)
                {
                    curWeatherObj = null;
                    windEM.enabled = false;
                    windMM.loop = false;
                    StartCoroutine("DeactivateDelay", windEffect);
                }
                if (curWeatherEffect == WeatherEffect.sandstorm)
                {
                    curWeatherObj = null;
                    sandstormEM.enabled = false;
                    sandstormMM.loop = false;
                    StartCoroutine("DeactivateDelay", sandstormEffect);
                }
            }

            //Start New
            if(newEffect != WeatherEffect.none)
            {
                if(newEffect == WeatherEffect.rain)
                {
                    curWeatherObj = rainEffect;
                    rainEffect.SetActive(true);
                    rainEM.enabled = true;
                    rainMM.loop = true;
                    rainPS.Play();
                }
                if (newEffect == WeatherEffect.snow)
                {
                    curWeatherObj = snowEffect;
                    snowEffect.SetActive(true);
                    snowEM.enabled = true;
                    snowMM.loop = true;
                    snowPS.Play();
                }
                if (newEffect == WeatherEffect.wind)
                {
                    curWeatherObj = windEffect;
                    windEffect.SetActive(true);
                    windEM.enabled = true;
                    windMM.loop = true;
                    windPS.Play();
                }
                if (newEffect == WeatherEffect.sandstorm)
                {
                    curWeatherObj = sandstormEffect;
                    sandstormEffect.SetActive(true);
                    sandstormEM.enabled = true;
                    sandstormMM.loop = true;
                    sandstormPS.Play();
                }
            }

            curWeatherEffect = newEffect;
        }
    }

    IEnumerator Transition(AtmoshphereSettings endSettings)
    {
        transitioning = true;

        curSettings = endSettings;

        startSettings.cloudColor = cloudMat.GetColor("_Color");
        startSettings.fogColor = RenderSettings.fogColor;
        startSettings.farClipPlaneDistance = mainCamera.farClipPlane;
           startSettings.fogStartDistance = RenderSettings.fogStartDistance;
           startSettings.fogDistance = RenderSettings.fogEndDistance;
     //   startSettings.fogDensity = RenderSettings.fogDensity;
     //   startSettings.rotationSpeed = curRotation;

        if (endSettings.fogStartDistance >= endSettings.fogDistance)
            endSettings.fogStartDistance = endSettings.fogDistance - 1f;

        ChangeWeather(endSettings.weatherEffect);

        for (int t = 0; t <= 120; t++)
        {
            iterator = (t * 1f) / 120f;
            RenderSettings.fogColor = Color.Lerp(startSettings.fogColor, endSettings.fogColor, iterator);
            skyMat.SetColor("_Color", RenderSettings.fogColor);

            if (endSettings.cloudsMaterial != null)
            {
                if (t <= 60)
                {
                    cloudMat.SetColor("_Color", Color.Lerp(startSettings.cloudColor, new Color(startSettings.cloudColor.r, startSettings.cloudColor.g, startSettings.cloudColor.b, 0f), iterator * 2f));
                    if (t == 60)
                    {
                        cloudMat = endSettings.cloudsMaterial;
                        cloudRenderer.material = cloudMat;
                        cloudMat.SetColor("_Color", new Color(endSettings.cloudColor.r, endSettings.cloudColor.g, endSettings.cloudColor.b, 0f));
                    }
                }
                else
                    cloudMat.SetColor("_Color", Color.Lerp(new Color(endSettings.cloudColor.r, endSettings.cloudColor.g, endSettings.cloudColor.b, 0f), endSettings.cloudColor, ((t - 60) * 1f) / 60f));
            }
            else
                cloudMat.SetColor("_Color", Color.Lerp(startSettings.cloudColor, endSettings.cloudColor, iterator));

            mainCamera.farClipPlane = Mathf.Lerp(startSettings.farClipPlaneDistance, endSettings.farClipPlaneDistance + ((float)PlayerPrefs.GetInt("AddedDistance", 2) * 50f), iterator);
            RenderSettings.fogStartDistance = Mathf.Lerp(startSettings.fogStartDistance, endSettings.fogStartDistance, iterator);
            RenderSettings.fogEndDistance = Mathf.Lerp(startSettings.fogDistance, endSettings.fogDistance, iterator);
        //    RenderSettings.fogDensity = Mathf.Lerp(startSettings.fogDensity, endSettings.fogDensity, iterator);
            curRotation = Mathf.Lerp(startSettings.rotationSpeed, endSettings.rotationSpeed, iterator);
            yield return null;
        }

        transitioning = false;
    }

    IEnumerator DeactivateDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        if (obj == rainEffect)
        {
            rainPS.Stop();
            rainEM.enabled = false;
        }
        if (obj == snowEffect)
        {
            snowPS.Stop();
            snowEM.enabled = false;
        }
        if (obj == windEffect)
        {
            windPS.Stop();
            windEM.enabled = false;
        }
        if (obj == sandstormEffect)
        {
            sandstormPS.Stop();
            sandstormEM.enabled = false;
        }
        obj.SetActive(false);
    }
}