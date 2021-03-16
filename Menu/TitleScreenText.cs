using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenText : MonoBehaviour
{
    public float TimeBeforeTitleFadesIn;
    public float TimeSpentFadingIn;
    public GameObject StartButton;
    

    protected _2dxFX_NewTeleportation2 _2dfx;
    protected _2dxFX_Shiny_Reflect _shinyFX;
    protected bool sceneStarted;
    protected bool fadingInTitle;

    // Start is called before the first frame update
    void Start()
    {
        _2dfx = GetComponent<_2dxFX_NewTeleportation2>();
        _shinyFX = GetComponent<_2dxFX_Shiny_Reflect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!sceneStarted)
        {
            StartCoroutine(WaitForTitleTime());
        }
    }

    protected IEnumerator WaitForTitleTime()
    {
        sceneStarted = true;
        yield return new WaitForSeconds(TimeBeforeTitleFadesIn);

        if(!fadingInTitle)
        {
            StartCoroutine(FadeInTitle(0, TimeSpentFadingIn));
        }

    }

    protected IEnumerator FadeInTitle(float aValue, float aTime)
    {
        fadingInTitle = true;
        _2dfx._Fade = 1;

        float alpha = _2dfx._Fade;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float newColor = Mathf.Lerp(alpha, aValue, t);
            _2dfx._Fade = newColor;
            yield return null;
        }

        StartButton.SetActive(true);
        _shinyFX.enabled = true;
    }
}
