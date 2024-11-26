using UnityEngine;
using System.Collections.Generic;
using Valve.VR;
using System;

public class LaserReflector : MonoBehaviour
{
    public GameObject laserPrefab;  // レーザーのPrefab
    public int maxReflections = 5;  // 最大反射回数
    public float maxLaserDistance = 100f;  // 最大レーザー距離
    public LayerMask reflectLayerMask;  // 反射可能なレイヤー
    public Material[] laserMaterials;  // 反射ごとに使用するマテリアル
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    private Boolean interacrtui;

    private List<GameObject> laserSegments = new List<GameObject>();  // 生成したレーザーオブジェクト
    
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        interacrtui = Iui.GetState(SteamVR_Input_Sources.RightHand);
        if (interacrtui)
        {
            GenerateLaserPath();
        }
        else
        {
            ClearLaserSegments();
        }
    }

    void GenerateLaserPath()
{
    ClearLaserSegments();

    Vector3 startPosition = transform.position;
    Vector3 direction = transform.forward;
    int reflections = 0;
    Material currentMaterial = laserMaterials[0];  // 最初のレーザーの色（デフォルト）

    while (reflections < maxReflections)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPosition, direction, out hit, maxLaserDistance, reflectLayerMask))
        {
            // レーザーセグメントを生成
            List<Vector3> positions = new List<Vector3> { startPosition, hit.point };
            CreateLaserSegment(startPosition, hit.point, reflections, currentMaterial, positions);

            // コライダーに応じてマテリアルを変更
            currentMaterial = ChooseMaterialBasedOnCollider(hit.collider, currentMaterial);

            // 反射処理
            direction = Vector3.Reflect(direction, hit.normal);
            startPosition = hit.point;

            reflections++;
        }
        else
        {
            // 何にも当たらなかった場合
            List<Vector3> positions = new List<Vector3> { startPosition, startPosition + direction * maxLaserDistance };
            CreateLaserSegment(startPosition, startPosition + direction * maxLaserDistance, reflections, currentMaterial, positions);
            break;
        }
    }
}



    void CreateLaserSegment(Vector3 start, Vector3 end, int reflectionIndex, Material currentMaterial, List<Vector3> posList)
{
    GameObject laser = Instantiate(laserPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    laser.tag = "laser";  // タグ設定
    //laser.layer = LayerMask.NameToLayer("laserLayer");  // レイヤー設定

    // LineRendererを取得
    LineRenderer line = laser.GetComponent<LineRenderer>();
    line.positionCount = posList.Count;

    // LineRendererに座標を設定
    for (int iLoop = 0; iLoop < posList.Count; iLoop++)
    {
        line.SetPosition(iLoop, posList[iLoop]);
    }

    // BoxColliderを追加して調整
    BoxCollider boxCollider = laser.AddComponent<BoxCollider>();
    //boxCollider.isTrigger = true;

    // レーザーの方向ベクトルを計算
    Vector3 direction = end - start;
    float length = direction.magnitude; // レーザーの長さ

    // レーザーの回転を設定
    laser.transform.position = start;
    laser.transform.LookAt(end);

    // コライダーの中心を設定 (ローカル空間での中心)
    boxCollider.center = new Vector3(0, 0, length / 2.0f);

    // コライダーのサイズを設定 (長さに沿ったスケール)
    boxCollider.size = new Vector3(0.1f, 0.1f, length);

    // LineRendererの設定
    line.SetPosition(0, start);
    line.SetPosition(1, end);
    line.material = currentMaterial;

    // 作成したレーザーをリストに追加
    laserSegments.Add(laser);
}



    Material ChooseMaterialBasedOnCollider(Collider collider, Material currentMaterial)
    {
        if (collider.CompareTag("Wall"))
        {
            if (currentMaterial == laserMaterials[2])
            {
                return laserMaterials[4];  // シアン
            }
            else if (currentMaterial == laserMaterials[3])
            {
                return laserMaterials[5];  // マゼンタ
            }
            return laserMaterials[1]; // 青
        }
        else if (collider.CompareTag("Wall2"))
        {
            if (currentMaterial == laserMaterials[1])
            {
                return laserMaterials[4];  // シアン
            }
            else if (currentMaterial == laserMaterials[3])
            {
                return laserMaterials[6];  // イエロー
            }
            return laserMaterials[2]; // 緑
        }
        else if (collider.CompareTag("Wall3"))
        {
            if (currentMaterial == laserMaterials[2])
            {
                return laserMaterials[6];  // イエロー
            }
            else if (currentMaterial == laserMaterials[1])
            {
                return laserMaterials[5];  // マゼンタ
            }
            return laserMaterials[3]; // 赤
        }
        return currentMaterial;
    }

    void ClearLaserSegments()
    {
        foreach (GameObject laser in laserSegments)
        {
            Destroy(laser);
        }
        laserSegments.Clear();
    }

}