using UnityEngine;

public class HandleRotation : MonoBehaviour
{
    public Transform handleTransform;
    private float previousAngle = 0f;
    public float rotationEnergyMultiplier = 1f;
    public float generatedEnergy = 0f;

    void Update()
    {
        float currentAngle = handleTransform.localEulerAngles.z; // ƒnƒ“ƒhƒ‹‚Ì‰ñ“]Šp“x‚ğæ“¾
        float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle); // ‰ñ“]Šp‚Ì•Ï‰»—Ê
        generatedEnergy += Mathf.Abs(deltaAngle) * rotationEnergyMultiplier; // ƒGƒlƒ‹ƒM[‚ğ‰ÁZ
        previousAngle = currentAngle;
    }
}