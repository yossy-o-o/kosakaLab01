using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandleController : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction; // 掴むアクション
    public SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand; // 左手を指定
    public SteamVR_Behaviour_Pose controllerPose; // コントローラーの位置・回転を取得するコンポーネント

    private bool isGrabbing = false; // 掴んでいるか
    private float initialAngle; // 掴んだときの初期角度
    private Vector3 initialControllerDirection; // 掴んだときのコントローラーの初期方向

    void Update()
    {
        // コントローラーが設定されていない場合はエラーを防ぐ
        if (controllerPose == null) return;

        // 掴むボタンが押された
        if (grabAction.GetStateDown(handType))
        {
            StartGrabbing();
        }
        // 掴むボタンが離された
        else if (grabAction.GetStateUp(handType))
        {
            StopGrabbing();
        }

        // 回転処理
        if (isGrabbing)
        {
            RotateValve();
        }
    }

    private void StartGrabbing()
    {
        isGrabbing = true;

        // バルブのZ軸を基準にした初期角度を記録
        initialAngle = transform.eulerAngles.z;

        // 掴んだときのコントローラーからバルブ中心への方向を記録
        initialControllerDirection = controllerPose.transform.position - transform.position;
    }

    private void StopGrabbing()
    {
        isGrabbing = false;
    }

    private void RotateValve()
    {
        // 現在のコントローラーからバルブ中心への方向を取得
        Vector3 currentControllerDirection = controllerPose.transform.position - transform.position;

        // 初期方向と現在方向の角度差を計算（Z軸回転に限定）
        float angleDifference = Vector3.SignedAngle(initialControllerDirection, currentControllerDirection, Vector3.forward);

        // バルブの回転を更新
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            initialAngle + angleDifference
        );
    }

}