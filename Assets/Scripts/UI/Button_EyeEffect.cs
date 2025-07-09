using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_EyeEffect : MonoBehaviour
{
    [Header("Eye Settings")]
    public RectTransform eyePupil;     // ����ͫ�ף�
    public float maxMovement = 15f;    // ��������ƶ�����
    public float followSpeed = 10f;    // �����ٶ�
    public float returnSpeed = 8f;     // �������ĵ��ٶ�
    public Vector3 offset = new Vector3(-100, -80);

    [Header("Effects")]
    public bool enableSquashStretch = true; // ���ü�ѹ����Ч��
    public float squashFactor = 0.3f;       // ��ѹ�̶�

    private RectTransform parentCanvas;
    private Vector2 eyeCenter;
    private Vector2 targetPosition;
    private Vector2 originalScale;

    void Start()
    {
        // ��ȡ����Canvas
        parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // ��¼�۾�����λ�ú�ԭʼ����
        eyeCenter = transform.position + offset;
        originalScale = eyePupil.localScale;
    }

    void Update()
    {
        // ��ȡ�������Ļ�ϵ�λ��
        Vector2 mousePosition = Input.mousePosition;

        // ת��ΪCanvas�ռ��λ��
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas, mousePosition, null, out localPoint);

        // �����۾�������Canvas�ռ��λ��
        Vector2 eyeCenterLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas, eyeCenter, null, out eyeCenterLocal);

        // ���㷽������
        Vector2 direction = (localPoint - eyeCenterLocal).normalized;

        // ����Ŀ��λ�ã�����������ƶ���Χ�ڣ�
        float distance = Mathf.Min(Vector2.Distance(localPoint, eyeCenterLocal), maxMovement);
        targetPosition = eyeCenterLocal + direction * distance;

        // ƽ���ƶ�����
        Vector2 currentPos = eyePupil.localPosition;
        Vector2 newPosition = Vector2.Lerp(currentPos, targetPosition,
            (targetPosition - currentPos).magnitude > 0.1f ? followSpeed * Time.deltaTime : returnSpeed * Time.deltaTime);

        eyePupil.localPosition = newPosition;

        // ��Ӽ�ѹ����Ч��
        if (enableSquashStretch)
        {
            ApplySquashStretchEffect(direction);
        }
    }

    private void ApplySquashStretchEffect(Vector2 direction)
    {
        // �����ƶ��������
        float moveAmount = Vector2.Distance(eyePupil.localPosition, Vector2.zero) / maxMovement;

        // ���㼷ѹ�������
        Vector2 scaleEffect = new Vector2(
            1f + moveAmount * squashFactor * Mathf.Abs(direction.x),
            1f - moveAmount * squashFactor * Mathf.Abs(direction.x)
        );

        // Ӧ������
        eyePupil.localScale = Vector2.Lerp(
            eyePupil.localScale,
            new Vector2(originalScale.x * scaleEffect.x, originalScale.y * scaleEffect.y),
            followSpeed * Time.deltaTime
        );
    }
}
