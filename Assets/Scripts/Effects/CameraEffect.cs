using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    public Material EffectMaterial;

    public Color Tint1;
    public Color Tint2;
    public Color Tint3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EffectMaterial.SetColor("_TintColor", Tint1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EffectMaterial.SetColor("_TintColor", Tint2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EffectMaterial.SetColor("_TintColor", Tint3);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, EffectMaterial);
    }
}
