using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnEffect : MonoBehaviour
{
    public Renderer CharacterRenderer;

    [Range(0, 1)]
    public float BurnAmount;

    public List<ParticleSystem> Particles;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShader();
    }

    public void Reset()
    {
        if (DieRoutine != null)
        {
            StopCoroutine(DieRoutine);
        }
        BurnAmount = 0;
        foreach (var particleSystem in Particles)
        {
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }

    Coroutine DieRoutine;
    public void StartDie()
    {
        if (DieRoutine != null)
        {
            StopCoroutine(DieRoutine);
        }
        DieRoutine = StartCoroutine(StartDieRoutine());
    }

    private IEnumerator StartDieRoutine()
    {
        foreach (var particleSystem in Particles)
        {
            particleSystem.Play();
        }
        while (BurnAmount < 1)
        {
            BurnAmount += 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
        foreach (var particleSystem in Particles)
        {
            particleSystem.Stop();
        }
    }

    private void UpdateShader()
    {
        CharacterRenderer.material.SetFloat("_Amount", BurnAmount);
    }
}
