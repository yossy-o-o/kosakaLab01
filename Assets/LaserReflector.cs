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
    private List<GameObject> laserSegments = new List<GameObject>();  // 生成したレーザーオブジェクト
    private Material firstHitMaterial = null;  // 最初に当たった壁のマテリアルを保存
    private Boolean interacrtui;

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
            // 最初に当たった壁の色を設定
            /*if (reflections == 0)
            {
                currentMaterial = ChooseMaterialBasedOnCollider(hit.collider);
            }*/

            // 反射後のレーザーを生成する際、壁の種類に応じて色を変える
            CreateLaserSegment(startPosition, hit.point, reflections, hit.collider, currentMaterial);
            direction = Vector3.Reflect(direction, hit.normal);
            startPosition = hit.point;
            reflections++;

            // 次の反射に向けて、新たに当たった壁の色を設定
            if (hit.collider != null)
            {
                currentMaterial = ChooseMaterialBasedOnCollider(hit.collider, currentMaterial);

            }
        }
        else
        {
            // 反射がない場合の最終レーザーを生成
            CreateLaserSegment(startPosition, startPosition + direction * maxLaserDistance, reflections, null, currentMaterial);
            break;
        }
    }
}

void CreateLaserSegment(Vector3 start, Vector3 end, int reflectionIndex, Collider hitCollider, Material currentMaterial)
{
    GameObject laser = Instantiate(laserPrefab);
    LineRenderer lineRenderer = laser.GetComponent<LineRenderer>();
    lineRenderer.SetPosition(0, start);
    lineRenderer.SetPosition(1, end);

    // 各レーザーの色を設定（反射回数ごとに色を設定）
    lineRenderer.material = currentMaterial;

    laserSegments.Add(laser);
}

Material ChooseMaterialBasedOnCollider(Collider collider, Material currentMaterial)
{
    // 衝突対象と現在のマテリアルに基づいて次の色を決定
    if (collider.CompareTag("Wall")) //青
    {
        if (currentMaterial == laserMaterials[2])//緑
        {
            return laserMaterials[4];  //シアン
        }
        else if (currentMaterial == laserMaterials[3])//赤
        {
            return laserMaterials[5];  //マゼンタ
        }
        return laserMaterials[1];  
    }
    else if (collider.CompareTag("Wall2"))//緑
    {
        if (currentMaterial == laserMaterials[1])//青
        {
            return laserMaterials[4];  //シアン
        }
        else if (currentMaterial == laserMaterials[3])
        {
            return laserMaterials[6];  //イエロー
        }
        return laserMaterials[2];  
    }
    else if (collider.CompareTag("Wall3"))//赤
    {
        if (currentMaterial == laserMaterials[2])//緑
        {
            return laserMaterials[6];  
        }
        if (currentMaterial == laserMaterials[1])
        {
            return laserMaterials[5];  //マゼンタ
        }
        return laserMaterials[3]; 
    }
    else
    {
        return currentMaterial;
    
    }
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
