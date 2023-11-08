using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class PuzzleInfo : MonoBehaviour
{
    [SerializeField] GameObject group;
    [SerializeField] GameObject spine;
    public static PuzzleInfo Instance = null;
    //Spine
    private SkeletonAnimation _spineAnimation;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (spine)
        {
            _spineAnimation = spine.GetComponent<SkeletonAnimation>();
        }
    }
    public void Show(Vector2 pos,int puzzleID)
    {
        SoundManager.Instance.PlaySFX("Click");
        group.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
        _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.GetPuzzleName(puzzleID), false);
        //_spineAnimation.AnimationName = "Popup_" + SlotMachine.GetPuzzleName(puzzleID);
        spine.transform.localPosition = pos;

        spine.transform.DOKill();
        spine.transform.localScale = new Vector2(0.5f, 0.5f);
        spine.transform.DOScale(new Vector2(1, 1), 1).SetEase(Ease.OutElastic);
    }
    public void Hide()
    {
        group.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;

    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        Hide();
    }
}
