using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";

    CustomActions input;

    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;
    
    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Stopping distanceを適切な値に設定
        agent.stoppingDistance = 0.5f;

        input = new CustomActions();
        AssignInputs();
    }

    void AssignInputs()
    {
        // タップまたはクリック時にClickToMoveメソッドが呼ばれるように設定
        input.Main.Move.performed += ctx => ClickToMove(ctx);
    }

    void ClickToMove(InputAction.CallbackContext context)
    {
        // UIボタンがクリックされた場合は処理を行わない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // タッチまたはクリックの位置を取得
        Vector2 position = Mouse.current.position.ReadValue();
        if (Touchscreen.current != null && Touchscreen.current.press.isPressed)
        {
            position = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        // スクリーンポイントからレイを生成
        Ray ray = Camera.main.ScreenPointToRay(position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, clickableLayers)) 
        {
            agent.destination = hit.point;
            if(clickEffect != null)
            {
                Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
        }
    }

    void OnEnable() 
    {
        // Input Systemを有効に
        input.Enable();
    }

    void OnDisable() 
    {
        // Input Systemを無効に
        input.Disable();
    }

    void FaceTarget()
    {
        // 移動している（エージェントの速度が0ではない）時のみ回転するようにします。
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            // Y軸の回転は無視するために、Y成分を0に設定します。
            direction.y = 0;

            // directionが非常に小さいベクトル（ほぼゼロ）の場合は回転をスキップします。
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
            }
        }
    }


    void Update() 
    {
        // エージェントが目的地に到達したかどうかの判定を追加
        bool hasReachedDestination = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

        // 移動しているかどうかで回転を制御
        if (!hasReachedDestination)
        {
            FaceTarget();
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }


}