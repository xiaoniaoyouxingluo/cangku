using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LoadingPanel : BasePanel
{
    private Animator fadeAnimator;
    private bool isFadeInComplete = false;
    private bool isHiding = false;
    private Coroutine fadeOutCoroutine;

    protected override void Awake()
    {

        //���������ʱ����Ը���

        base.Awake();
        fadeAnimator = transform.GetChild(0).GetComponent<Animator>();

        // �������ؽ����¼�
        EventCenter.Instance.AddEventListener<float>("SceneLoadchange", UpdateProgress);
    }

    private void OnEnable()
    {
        // ����״̬
        isFadeInComplete = false;
        isHiding = false;

        // ���Ž��붯��
        if (fadeAnimator != null)
        {
            fadeAnimator.Play("UI_Scene_FadeIn");
            StartCoroutine(WaitForFadeInComplete());
        }
    }

    private IEnumerator WaitForFadeInComplete()
    {
        // �ȴ����붯�����
        float fadeInDuration = GetAnimationLength(fadeAnimator, "UI_Scene_FadeIn");
        yield return new WaitForSeconds(fadeInDuration);

        isFadeInComplete = true;

        // ����Ѿ��������أ�������ʼ����
        if (isHiding)
        {
            StartFadeOut();
        }
    }

    private void UpdateProgress(float progress)
    {
        float displayProgress = Mathf.Clamp01(progress / 0.9f);
        // ������Ը��½�������ʾ
        transform.SetAsLastSibling();
    }

    public override void HideMe(UnityAction callBack)
    {
        // ��ֹ�ظ�����
        if (isHiding) return;

        isHiding = true;

        // ������붯������ɣ�������ʼ����
        if (isFadeInComplete)
        {
            StartFadeOut(callBack);
        }
        else
        {
            // ������붯����û��ɣ�����ص��ȴ���ɺ�ִ��
            fadeOutCoroutine = StartCoroutine(WaitForFadeInThenFadeOut(callBack));
        }
    }

    private void StartFadeOut(UnityAction callBack = null)
    {
        // �Ƴ�����
        EventCenter.Instance.RemoveEventListener<float>("SceneLoadchange", UpdateProgress);

        // ���Ž�������
        if (fadeAnimator != null)
        {
            fadeAnimator.Play("UI_Scene_FadeOut");
        }

        // �ȴ�����������ɺ�ִ�лص�
        float fadeOutDuration = GetAnimationLength(fadeAnimator, "UI_Scene_FadeOut");
        StartCoroutine(CompleteFadeOutAfterDelay(fadeOutDuration, callBack));
    }

    private IEnumerator WaitForFadeInThenFadeOut(UnityAction callBack)
    {
        // �ȴ����붯�����
        yield return new WaitUntil(() => isFadeInComplete);

        // ��ʼ����
        StartFadeOut(callBack);
    }

    private IEnumerator CompleteFadeOutAfterDelay(float delay, UnityAction callBack)
    {
        yield return new WaitForSeconds(delay);

        // ������ɺ�ִ�����غͻص�
        base.HideMe(callBack);
        isHiding = false;
    }

    // ��ȡ��������
    private float GetAnimationLength(Animator animator, string animationName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            //û��animtorʲô��
            return 0.5f;
        }

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        //�Ҳ������name
        return 1; // Ĭ��1��
    }
}