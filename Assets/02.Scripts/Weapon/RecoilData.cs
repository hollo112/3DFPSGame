using UnityEngine;

public class RecoilData : MonoBehaviour
{
    public AnimationCurve PitchCurve;
    public AnimationCurve YawCurve;

    public float RecoilDuration = 0.1f;
    public float RecoilPower = 3f;

    public float AccumulationStrength = 0.2f;
}
