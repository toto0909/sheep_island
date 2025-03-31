using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SheepController : MonoBehaviour
{
    public Transform waterSurface;
    private float waterY;

    public float moveSpeed = 2000f;
    public float rotationSpeed = 100f;
    public float changeDirectionInterval = 3f;

    private Animator animator;
    private Vector3 moveDirection;
    private float directionTimer;
    private bool isBacking = false;
    private float backTimer = 0f;
    private float backDuration = 1.0f;

    void Start()
    {
        waterY = waterSurface.position.y;
        animator = GetComponent<Animator>();
        ChooseNewDirection();
    }

    void Update()
    {
        directionTimer -= Time.deltaTime;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if (isBacking)
        {
            // 後退中の処理
            backTimer -= Time.deltaTime;
            transform.Translate(-moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Y座標調整
            Vector3 backPos = transform.position;
            backPos.y = terrainHeight + 1.0f;
            transform.position = backPos;

            // 回転処理
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (animator != null)
            {
                animator.Play("walk_backwards");
            }

            if (backTimer <= 0f)
            {
                isBacking = false;
                ChooseNewDirection(); // バック終了後方向転換
            }
            return;
        }

        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
        {
            ChooseNewDirection();
        }

        if (terrainHeight < waterY)
        {
            // 水面に突っ込んだらバック開始
            isBacking = true;
            backTimer = backDuration;
            return;
        }

        // 通常の前進処理
        // Y座標を調整
        Vector3 pos = transform.position;
        pos.y = terrainHeight + 1.0f;
        transform.position = pos;

        // 移動処理
        Vector3 movement = moveDirection * moveSpeed * 10 * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
        {
            animator.Play("walk_forward");
        }
    }

    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 rawDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        moveDirection = rawDirection.normalized;

        // directionTimer を必ずリセット
        directionTimer = changeDirectionInterval;
    }
}
