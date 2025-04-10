using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class SheepController : MonoBehaviour
{
    public Transform waterSurface;
    public Gender gender;
    private float waterY;

    public float moveSpeed = 2000f;
    public float rotationSpeed = 100f;
    public float changeDirectionInterval = 3f;

    public int maxHP = 100;
    public int maxMP = 100;
    public float currentHP { get; private set; }
    public float currentMP { get; private set; }

    private Animator animator;
    private Vector3 moveDirection;
    private float directionTimer;
    private bool isBacking = false;
    private float backTimer = 0f;
    private float backDuration = 1.0f;

    //検証用
    private float hpDecreaseRate = 5f; // 1秒あたり減るHP

    void Start()
    {
        // HP・MP 初期化（最大100、初期70）
        currentHP = 70;
        currentMP = 70;

        // 性別ランダム割り当て
        gender = (Random.value > 0.5f) ? Gender.Male : Gender.Female;

        // 性別アイコン取得・表示
        var genderText = GetComponentInChildren<TMP_Text>();
        if (genderText != null)
        {
            if (gender == Gender.Male)
            {
                genderText.text = "♂";
                genderText.color = Color.cyan;
            }
            else
            {
                genderText.text = "♀";
                genderText.color = new Color(1f, 0.5f, 0.7f); // ピンク
            }
        }

        waterY = waterSurface.position.y;
        animator = GetComponent<Animator>();
        ChooseNewDirection();
    }

    void Update()
    {
        directionTimer -= Time.deltaTime;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        //検証用
        // HP減少処理
        currentHP -= hpDecreaseRate * Time.deltaTime;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP); // 範囲制限

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
                ChooseNewDirection();
            }
            return;
        }

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

        // 前進処理と回転
        Vector3 pos = transform.position;
        pos.y = terrainHeight + 1.0f;
        transform.position = pos;

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

        directionTimer = changeDirectionInterval;
    }
}
