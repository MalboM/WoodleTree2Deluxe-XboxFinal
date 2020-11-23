using UnityEngine;
using System.Collections;

public class LowPass : MonoBehaviour
{
    void Start()
    {
        sr = AudioSettings.outputSampleRate;
    }

    private float sr = 44000;

    [Range(100,20000)]
    public float cutoff = 20000;
    private float cutoffwas = 1;
    
	[Range(0.0f,0.95f)]
    public float resonance = 0;
    private float resonancewas = 1;
    
	[Range(0,1)]
    public float dryWet = 1;
    
	[Range(0,1)]
    public float volume = 1;
    
	private float ic0 = 0, ic1 = 0, ic2 = 0, oc1 = 0, oc2 = 0;
    private float ldi1 = 0, ldi2 = 0, ldo1 = 0, ldo2 = 0;
    private float rdi1 = 0, rdi2 = 0, rdo1 = 0, rdo2 = 0;
    private const float dc = 1e-18f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (cutoffwas != cutoff || resonancewas != resonance)
        {
            float w0 = ((2 * Mathf.PI) * cutoff) / sr; 
            float alpha = Mathf.Sin(w0) / 2 * (1 - resonance);
            float cos = Mathf.Cos(w0);
            float b0, b1, b2, a0, a1, a2;
            b0 = (1 - cos) * 0.5f;
            b1 = 1 - cos;
            b2 = b0;
            a0 = 1 + alpha;
            a1 = -2 * cos;
            a2 = 1 - alpha;
            ic0 = b0 / a0;
            ic1 = b1 / a0;
            ic2 = b2 / a0;
            oc1 = -(a1 / a0);
            oc2 = -(a2 / a0);
        }
        cutoffwas = cutoff;
        resonancewas = resonance;
        if (channels == 2)
        {
            for (int i = 0; i <  data.Length; i+=2)
            {
                float ly = ic0 * data [i] + ic1 * ldi1 + ic2 * ldi2 + oc1 * ldo1 + oc2 * ldo2 + dc;
                float ry = ic0 * data [i + 1] + ic1 * rdi1 + ic2 * rdi2 + oc1 * rdo1 + oc2 * rdo2 + dc;
                ldi2 = ldi1;
                ldi1 = data [i];
                ldo2 = ldo1;
                ldo1 = ly;

                rdi2 = rdi1;
                rdi1 = data [i + 1];
                rdo2 = rdo1;
                rdo1 = ry;

                data [i] = volume * (data [i] * (1 - dryWet) + (dryWet) * ly);
                data [i + 1] = volume * (data [i + 1] * (1 - dryWet) + (dryWet) * ry);
            }
        } else
        {
            for (int i = 0; i <  data.Length; i++)
            {
                float ly = ic0 * data [i] + ic1 * ldi1 + ic2 * ldi2 + oc1 * ldo1 + oc2 * ldo2 + dc;
                ldi2 = ldi1;
                ldi1 = data [i];
                ldo2 = ldo1;
                ldo1 = ly;           
                data [i] = volume * (data [i] * (1 - dryWet) + (dryWet) * ly);
            }
        }
    }
}
