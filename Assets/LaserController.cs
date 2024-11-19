using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float laserSpeed = 5f; // レーザーの拡大速度
    public GameObject ancar;

    private float maxScaleY = 10f; // 最大スケールY（レーザーがどれだけ長くなるか）
    private bool isColliding = false;

    void Update()
    {
        if (!isColliding)
        {
            // レーザーを拡大する
            float newScaleY = ancar.transform.localScale.y + laserSpeed * Time.deltaTime;

            // 最大スケールを超えないように調整
            if (newScaleY > maxScaleY)
                newScaleY = maxScaleY;

            ancar.transform.localScale = new Vector3(ancar.transform.localScale.x, newScaleY, ancar.transform.localScale.z);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
    }
}
