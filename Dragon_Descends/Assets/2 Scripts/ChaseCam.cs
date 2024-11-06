using System.Collections;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 Transform을 할당
    public float followDelay = 0.5f;  // 플레이어 이동 시작 후 따라가기 지연 시간
    public float boundaryRadius = 1.5f; // 플레이어로부터 벗어나지 않도록 할 범위 반경
    public float followSpeed = 5f;    // 카메라가 따라가는 속도

    private bool startFollowing = false;

    private void Start()
    {
        followSpeed = CharacterManager.Instance.player.moveSpeed;
        StartCoroutine(StartFollowingAfterDelay());
    }

    private void Update()
    {
        followSpeed = CharacterManager.Instance.player.moveSpeed;
        if (startFollowing)
        {
            FollowPlayer();
        }
    }

    private IEnumerator StartFollowingAfterDelay()
    {
        yield return new WaitForSeconds(followDelay);
        startFollowing = true;
    }

    private void FollowPlayer()
    {
        Vector3 offset = playerTransform.position - transform.position;

        if (offset.magnitude > boundaryRadius)
        {
            Vector3 direction = offset.normalized;
            float distanceToMove = (offset.magnitude - boundaryRadius) * Time.deltaTime * followSpeed;

            transform.Translate(direction * distanceToMove, Space.World);

            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }
    }
}
