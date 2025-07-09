using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_EyeEffect : MonoBehaviour
{
    [Header("Eye Settings")]
    public RectTransform eyePupil;     // 眼球（瞳孔）
    public float maxMovement = 15f;    // 眼球最大移动距离
    public float followSpeed = 10f;    // 跟随速度
    public float returnSpeed = 8f;     // 返回中心的速度
    public Vector3 offset = new Vector3(-100, -80);

    [Header("Effects")]
    public bool enableSquashStretch = true; // 启用挤压拉伸效果
    public float squashFactor = 0.3f;       // 挤压程度

    private RectTransform parentCanvas;
    private Vector2 eyeCenter;
    private Vector2 targetPosition;
    private Vector2 originalScale;

    void Start()
    {
        // 获取父级Canvas
        parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // 记录眼睛中心位置和原始比例
        eyeCenter = transform.position + offset;
        originalScale = eyePupil.localScale;
    }

    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;

        // 转换为Canvas空间的位置
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas, mousePosition, null, out localPoint);

        // 计算眼睛中心在Canvas空间的位置
        Vector2 eyeCenterLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas, eyeCenter, null, out eyeCenterLocal);

        // 计算方向向量
        Vector2 direction = (localPoint - eyeCenterLocal).normalized;

        // 计算目标位置（限制在最大移动范围内）
        float distance = Mathf.Min(Vector2.Distance(localPoint, eyeCenterLocal), maxMovement);
        targetPosition = eyeCenterLocal + direction * distance;

        // 平滑移动眼球
        Vector2 currentPos = eyePupil.localPosition;
        Vector2 newPosition = Vector2.Lerp(currentPos, targetPosition,
            (targetPosition - currentPos).magnitude > 0.1f ? followSpeed * Time.deltaTime : returnSpeed * Time.deltaTime);

        eyePupil.localPosition = newPosition;

        // 添加挤压拉伸效果
        if (enableSquashStretch)
        {
            ApplySquashStretchEffect(direction);
        }
    }

    private void ApplySquashStretchEffect(Vector2 direction)
    {
        // 计算移动距离比例
        float moveAmount = Vector2.Distance(eyePupil.localPosition, Vector2.zero) / maxMovement;

        // 计算挤压拉伸比例
        Vector2 scaleEffect = new Vector2(
            1f + moveAmount * squashFactor * Mathf.Abs(direction.x),
            1f - moveAmount * squashFactor * Mathf.Abs(direction.x)
        );

        // 应用缩放
        eyePupil.localScale = Vector2.Lerp(
            eyePupil.localScale,
            new Vector2(originalScale.x * scaleEffect.x, originalScale.y * scaleEffect.y),
            followSpeed * Time.deltaTime
        );
    }
}
