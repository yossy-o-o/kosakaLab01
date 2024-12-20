using UnityEngine;

public class HandleRotation : MonoBehaviour
{
    public Transform handleTransform;
    private float previousAngle = 0f;
    public float rotationEnergyMultiplier = 1f;
    public float generatedEnergy = 0f;

    void Update()
    {
        float currentAngle = handleTransform.localEulerAngles.z; // ハンドルの回転角度を取得
        float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle); // 回転角の変化量
        generatedEnergy += Mathf.Abs(deltaAngle) * rotationEnergyMultiplier; // エネルギーを加算
        previousAngle = currentAngle;
    }
}