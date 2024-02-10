using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    public Camera targetCamera; // ズーム管理するカメラ
    public Button plusButton; // ズームインボタン
    public Button minusButton; // ズームアウトボタン

    private int zoomLevel = 3; // 初期ズームレベル
    private int maxZoomLevel = 5; // 最大ズームレベル
    private int minZoomLevel = 1; // 最小ズームレベル
    private float[] zoomStages = {15f, 13f, 11f, 9f, 7f}; // 5段階のorthographicSizeの値

    public Transform targetCharacter; // カメラのターゲットとなるキャラクター
    public Vector3 offset = new Vector3(0, 3, 0); // キャラクターからのオフセット
    public float followSpeed = 5f; // キャラクターに追従する速度
    public Vector3 defaultCameraPosition; // カメラの初期位置

    void Start()
    {
        defaultCameraPosition = targetCamera.transform.position;
        plusButton.onClick.AddListener(ZoomIn);
        minusButton.onClick.AddListener(ZoomOut);
        UpdateCameraZoom();
    }

    void ZoomIn()
    {
        if (zoomLevel < maxZoomLevel)
        {
            zoomLevel++;
            UpdateCameraZoom();
        }
    }

    void ZoomOut()
    {
        if (zoomLevel > minZoomLevel)
        {
           zoomLevel--;
            UpdateCameraZoom();
        }
    }

    void UpdateCameraZoom()
    {
        if (targetCamera != null)
        {
            targetCamera.orthographicSize = zoomStages[zoomLevel - 1];
        }
    }

    void LateUpdate()
    {
        // ズームレベルが最小の場合、カメラの位置をデフォルトの位置に設定
        if (zoomLevel == minZoomLevel)
        {
            targetCamera.transform.position = defaultCameraPosition;
        }
        // それ以外の場合は、キャラクターを追従
        else if (targetCharacter != null)
        {
            FollowTarget();
        }
    }

    void FollowTarget()
    {
        // ターゲットの位置にオフセットを加えた位置
        Vector3 targetPosition = targetCharacter.position + offset;

        // カメラの位置をターゲットの位置に滑らかに移動させる
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
