using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualize : MonoBehaviour
{
    AudioSource _audioSource;
    float[] _samples = new float[512];
    float[] _freqBand = new float[8];
    float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float _amplitude, _amplitudeBuffer;
    private float _amplitudeHighest;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
        BandBuffer();
        CreateAudioband();
        GetAmplitude();
    }


    public void GetAmplitude()
    {
        float tempAmplitude = 0;
        float tempAmplitudeBuffer = 0;
        for(int i = 0; i < 8; i++)
        {
            tempAmplitude += _audioBand[i];
            tempAmplitudeBuffer += _audioBandBuffer[i];
        }
        if(tempAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = tempAmplitude;
        }
        _amplitude = tempAmplitude / _amplitudeHighest;
        _amplitudeBuffer = tempAmplitudeBuffer / _amplitudeHighest;

    }

    public void CreateAudioband()
    {
        for(int i = 0; i < 8; i++)
        {
            if(_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    public void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    public void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if(_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }

            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    public void MakeFrequencyBand()
    {
        int count = 0;

        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for(int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }
    }
}
