using UnityEngine;

public class reflect : MonoBehaviour
{
    public ParticleSystem particleSystem;  // 対象のパーティクルシステム
    public GameObject reflectionPrefab;    // 反射時に出す新しいパーティクルのプレハブ

    private void Start()
    {
        // パーティクルシステムの衝突を有効にする
        var collisionModule = particleSystem.collision;
        collisionModule.enabled = true;
        collisionModule.collidesWith = LayerMask.GetMask("Default"); // 衝突対象のレイヤーを指定
        collisionModule.type = ParticleSystemCollisionType.World;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突した位置と法線を使って反射方向を計算
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitNormal = collision.contacts[0].normal;

        // 反射方向を計算
        Vector3 reflectionDirection = Vector3.Reflect(particleSystem.transform.forward, hitNormal);

        // 新しいパーティクルを反射方向に発生させる
        SpawnReflectedParticle(hitPoint, reflectionDirection);
    }

    private void SpawnReflectedParticle(Vector3 position, Vector3 direction)
    {
        // 反射方向にパーティクルを生成
        var reflectedParticle = Instantiate(reflectionPrefab, position, Quaternion.identity);
        var particleSystem = reflectedParticle.GetComponent<ParticleSystem>();

        // パーティクルの進行方向を設定
        var mainModule = particleSystem.main;
        mainModule.startRotation = Quaternion.LookRotation(direction).eulerAngles.y;
        particleSystem.Play();
    }
}
