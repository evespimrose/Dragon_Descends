using System.Collections;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform�� �Ҵ�
    public float followDelay = 0.5f;  // �÷��̾� �̵� ���� �� ���󰡱� ���� �ð�
    public float boundaryRadius = 1.5f; // �÷��̾�κ��� ����� �ʵ��� �� ���� �ݰ�
    public float followSpeed = 5f;    // ī�޶� ���󰡴� �ӵ�

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