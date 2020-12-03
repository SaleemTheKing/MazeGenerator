using System;
using UnityEngine;

public class AudioFrequencies : MonoBehaviour
{
    private AudioSource audioSource;
    public static float[] smallSamples = new float[64];
    public static float[] mediumSamples = new float[128];
    public static float[] largeSamples = new float[256];
    public static float[] superSamples = new float[512];

    private void Awake()
    {
        audioSource = GameObject.FindWithTag("AudioManager").GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        GetSpectrumData();
    }

    private void GetSpectrumData()
    {
        audioSource.GetSpectrumData(smallSamples, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(mediumSamples, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(largeSamples, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(superSamples, 0, FFTWindow.Blackman);
    }
}
