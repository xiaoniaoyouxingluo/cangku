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

        //这个动画到时候可以改下

        base.Awake();
        fadeAnimator = transform.GetChild(0).GetComponent<Animator>();

        // 场景加载进度事件
        EventCenter.Instance.AddEventListener<float>("SceneLoadchange", UpdateProgress);
    }

    private void OnEnable()
    {
        // 重置状态
        isFadeInComplete = false;
        isHiding = false;

        // 播放渐入动画
        if (fadeAnimator != null)
        {
            fadeAnimator.Play("UI_Scene_FadeIn");
            StartCoroutine(WaitForFadeInComplete());
        }
    }

    private IEnumerator WaitForFadeInComplete()
    {
        // 等待渐入动画完成
        float fadeInDuration = GetAnimationLength(fadeAnimator, "UI_Scene_FadeIn");
        yield return new WaitForSeconds(fadeInDuration);

        isFadeInComplete = true;

        // 如果已经请求隐藏，立即开始渐出
        if (isHiding)
        {
            StartFadeOut();
        }
    }

    private void UpdateProgress(float progress)
    {
        float displayProgress = Mathf.Clamp01(progress / 0.9f);
        // 这里可以更新进度条显示
        transform.SetAsLastSibling();
    }

    public override void HideMe(UnityAction callBack)
    {
        // 防止重复调用
        if (isHiding) return;

        isHiding = true;

        // 如果渐入动画已完成，立即开始渐出
        if (isFadeInComplete)
        {
            StartFadeOut(callBack);
        }
        else
        {
            // 如果渐入动画还没完成，保存回调等待完成后执行
            fadeOutCoroutine = StartCoroutine(WaitForFadeInThenFadeOut(callBack));
        }
    }

    private void StartFadeOut(UnityAction callBack = null)
    {
        // 移除监听
        EventCenter.Instance.RemoveEventListener<float>("SceneLoadchange", UpdateProgress);

        // 播放渐出动画
        if (fadeAnimator != null)
        {
            fadeAnimator.Play("UI_Scene_FadeOut");
        }

        // 等待渐出动画完成后执行回调
        float fadeOutDuration = GetAnimationLength(fadeAnimator, "UI_Scene_FadeOut");
        StartCoroutine(CompleteFadeOutAfterDelay(fadeOutDuration, callBack));
    }

    private IEnumerator WaitForFadeInThenFadeOut(UnityAction callBack)
    {
        // 等待渐入动画完成
        yield return new WaitUntil(() => isFadeInComplete);

        // 开始渐出
        StartFadeOut(callBack);
    }

    private IEnumerator CompleteFadeOutAfterDelay(float delay, UnityAction callBack)
    {
        yield return new WaitForSeconds(delay);

        // 动画完成后执行隐藏和回调
        base.HideMe(callBack);
        isHiding = false;
    }

    // 获取动画长度
    private float GetAnimationLength(Animator animator, string animationName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            //没有animtor什么的
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
        //找不到这个name
        return 1; // 默认1秒
    }
}