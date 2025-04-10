using UnityEngine;
using UnityEngine.UI;

public class FloatingUI : MonoBehaviour
{
    private Camera mainCamera;
    private SheepController sheep;
    private Slider hpSlider;
    private Slider mpSlider;

    void Start()
    {
        mainCamera = Camera.main;

        // 親の SheepController を取得
        sheep = GetComponentInParent<SheepController>();

        // HP/MP の Slider を子から探す
        hpSlider = transform.Find("StatusPanel/BarGroup/HPBar")?.GetComponent<Slider>();
        mpSlider = transform.Find("StatusPanel/BarGroup/MPBar")?.GetComponent<Slider>();

        // 最大値は100
        if (hpSlider != null) hpSlider.maxValue = 100;
        if (mpSlider != null) mpSlider.maxValue = 100;
    }

    void LateUpdate()
    {
        // UIをカメラに向け続ける
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        }

        // HP/MP を反映
        if (sheep != null)
        {
            if (hpSlider != null) hpSlider.value = sheep.currentHP;
            if (mpSlider != null) mpSlider.value = sheep.currentMP;
        }
    }
}
