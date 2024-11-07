using System.Collections;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 Transform을 할당
    public float followDelay = 0.5f;  // 플레이어 이동 시작 후 따라가기 지연 시간
    public float boundaryRadius = 1.5f; // 플레이어로부터 벗어나지 않도록 할 범위 반경
    public float followSpeed = 5f;    // 카메라가 따라가는 속도

    private bool startFollowing = false;
    private bool isCheckingMovement = false; // 이동 상태 검사 여부

    private void Start()
    {
        followSpeed = CharacterManager.Instance.player.moveSpeed;
    }

    private void Update()
    {
        followSpeed = CharacterManager.Instance.player.moveSpeed;

        // 플레이어의 이동이 시작되었는지 확인
        if ((Vector2)CharacterManager.Instance.transform.position != CharacterManager.Instance.player.Target)
        {
            if (!isCheckingMovement)
            {
                isCheckingMovement = true;
                StartCoroutine(StartFollowingAfterDelay()); // 이동 시작 후 딜레이 적용
            }
        }

        if (startFollowing)
        {
            FollowPlayer();
        }
    }

    private IEnumerator StartFollowingAfterDelay()
    {
        startFollowing = false; // 일단 따라가기 중지
        yield return new WaitForSeconds(followDelay); // 딜레이 적용

        startFollowing = true; // 딜레이 후 따라가기 시작
        isCheckingMovement = false; // 이동 상태 재검사 허용
    }

    private void FollowPlayer()
    {
        Vector3 offset = playerTransform.position - transform.position;

        if (offset.magnitude > boundaryRadius)
        {
            Vector3 direction = offset.normalized;
            float distanceToMove = (offset.magnitude - boundaryRadius) * Time.deltaTime * followSpeed;

            transform.Translate(direction * distanceToMove, Space.World);

            // Z 축 고정 (카메라 깊이 설정)
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }
    }
}
