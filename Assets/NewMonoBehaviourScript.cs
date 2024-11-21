using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //InteractUIボタンが押されてるのかを判定するためのIuiという関数にSteamVR_Actions.default_InteractUIを固定
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    //結果の格納用Boolean型関数interacrtui
    private Boolean interacrtui;

    public int maxReflections = 5;                   // 最大反射回数
    public float maxLaserDistance = 100f;            // レーザーの最大距離
    public LayerMask reflectLayerMask;               // 反射対象のレイヤー
    public Material[] laserMaterials;                // 用意したレーザー用のマテリアル配列

    private LineRenderer lineRenderer;

    void Start()
    {
        Application.targetFrameRate = 60;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;               // レーザーを非表示に設定

        if (laserMaterials.Length > 0)
        {
            lineRenderer.material = laserMaterials[0];  // 初期マテリアルを設定
        }
        else
        {
            Debug.LogWarning("レーザーマテリアルが設定されていません。");
        }
    }

    void Update()
    {
        interacrtui = Iui.GetState(SteamVR_Input_Sources.RightHand);
        // ボタンが押されている間、毎フレーム経路を更新
        if (interacrtui)
        {
            GenerateLaserPath();                    // レーザーの経路を生成
            lineRenderer.enabled = true;            // レーザーを表示
        }
        else
        {
            lineRenderer.enabled = false;           // ボタンを離したらレーザーを非表示
        }
    }

    void GenerateLaserPath()
    {
        // レーザーの起点と方向
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.forward;

        List<Vector3> laserPoints = new List<Vector3>();
        laserPoints.Add(startPosition);

        int reflections = 0;
        while (reflections < maxReflections)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPosition, direction, out hit, maxLaserDistance, reflectLayerMask))
            {
                laserPoints.Add(hit.point);

                // 壁の法線に基づいて反射方向を計算
                Vector3 surfaceNormal = hit.normal;
                direction = Vector3.Reflect(direction, surfaceNormal);
                startPosition = hit.point;

                // 反射回数に応じてマテリアルを変更（例: 反射するたびに異なる色に変更）
                if (laserMaterials.Length > reflections)
                {
                    lineRenderer.material = laserMaterials[reflections]; // 反射回数に応じたマテリアルを設定
                }

                reflections++;
            }
            else
            {
                // 反射回数の上限に達したら、最大距離まで伸ばす
                laserPoints.Add(startPosition + direction * maxLaserDistance);
                break;
            }
        }

        // LineRendererを設定
        lineRenderer.positionCount = laserPoints.Count;
        lineRenderer.SetPositions(laserPoints.ToArray());
    }
}
