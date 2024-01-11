using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Puzzle : MonoBehaviour
{
    enum PuzzleState
    {
        None = 0,
        Random,
    }
    public int columnID = 0;
    public int puzzleID = 0;
    public int dataID = 0;

    public List<GameObject> spines;
    public GameObject spine;
    public GameObject dust;
    public GameObject bgMatch;
    public float randomDuration = 5;
    public float randomSpeed = 1;
    public float randomDelay = 0;

    int _state = 0;
    float _currTime = 0;
    float _currDelayTime = 0;
    Vector2 _offset = new Vector2(0,0);

    //Bounce
    public float bounceSpeed = 3;
    public float bounceAmplitude = 0.1f;
    public float bounceDuration = 0.3f;
    float _currBounceTime = 0;
    bool _isBounce = false;
    public static bool isEnableClick = true;

    //Spine
    private SkeletonAnimation _spineAnimation;
    bool _isInit = true;
    bool _isDelaySpineTimeScale = false;
    bool isDelayShowMatch = false;
    float _matchDuration = 0.3f;
    float _time = 0;
    bool _isMatch = false;
    bool _isMatchCheck = false;
    public GameObject prefabScatterHeart;
    public GameObject sprite;
    bool isAnimateMatch = false;
    public float animateMatchDuration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Spine
        HideAllSpine();
        dataID = Random.Range(0, 10);
        //spine = spines[dataID];
        SetPuzzleData();

        /*if (spine){
            _spineAnimation = spine.GetComponent<SkeletonAnimation>();
            _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(dataID), true);

            //Random.seed(9999);
            SetPuzzleData();
        }*/
    }
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        //Subscribe event
        SlotMachine.OnSlotColumnPreSpin += Spin;
        SlotColumn.OnSpinFinish += RandomFinish;
        SlotMachine.OnActionGetPuzzle += OnActivePuzle;
        SlotMachine.OnCreateSlotMachine += SetPuzzleData;
    }

    private void OnDisable()
    {
        //Unsubscribe event
        SlotMachine.OnSlotColumnPreSpin -= Spin;
        SlotColumn.OnSpinFinish -= RandomFinish;
        SlotMachine.OnActionGetPuzzle -= OnActivePuzle;
        SlotMachine.OnCreateSlotMachine -= SetPuzzleData;

    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case (int)PuzzleState.None:
                {
                    //HandleBounce();
                    break;
                }
            case (int)PuzzleState.Random:
                {
                    //HandleRandom();
                    break;
                }
        }
        if (_isDelaySpineTimeScale)
        {
            _currTime += Time.deltaTime;
            if(_currTime > 0.3f)
            {
                _currTime = 0;
                _isDelaySpineTimeScale = false;
                //_spineAnimation.timeScale = 1;
            }
        }

        if (isAnimateMatch)
        {
            _time += Time.deltaTime;
            if(_time > animateMatchDuration)
            {
                _time = 0;
                isAnimateMatch = false;
                /*if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Cat01)
                    _spineAnimation.AnimationState.SetAnimation(0, "Meow1_Armor_Idle", true);
                else if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Cat02)
                    _spineAnimation.AnimationState.SetAnimation(0, "Meow2_Armor_Idle", true);
                else if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Cat03)
                    _spineAnimation.AnimationState.SetAnimation(0, "Meow3_Armor_Idle", true);
                else if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Cat04)
                    _spineAnimation.AnimationState.SetAnimation(0, "Meow4_Armor_Idle", true);
                else if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Cat05)
                    _spineAnimation.AnimationState.SetAnimation(0, "Meow5_Armor_Idle", true);*/
            }
        }
    }

    void HandleRandom()
    {
        _currDelayTime += Time.deltaTime;
        if (_currDelayTime < randomDelay) return;
        //randomSprite.SetActive(true);
        //SetMaterial();
        _currTime += Time.deltaTime;
        if(_currTime >= randomDuration)
        {
            //Random finish
            //RandomFinish();
        }
        else
        {
            //Random spining
            _offset.y += randomSpeed * Time.deltaTime;
            SetMaterial();
        }
    }

    void Bounce()
    {
        if (!spine) return;

        SetPuzzleData();

        Vector3 p = spine.transform.localPosition;
        p.y = 0.5f;
        spine.transform.localPosition = p;
        spine.transform.DOLocalMoveY(0, bounceDuration).SetEase(Ease.OutElastic).OnComplete(BounceFinish);
        spine.transform.DORestart();
    }
    public void ShowMatch()
    {
        if (_isMatchCheck)
        {
            if (_isMatch)
            {
                SetAlpha(1);
            }
            else
            {
                SetAlpha(0.5f);
            }
        }
        else
        {
            SetAlpha(1);
        }

        if (SlotMachine.Instance.IsComingScatterMode())
        {
            if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Scatter)
            {
                _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationMatch(dataID), true);
                SetAlpha(1);
                JellyEffect();
            }
            else
            {
                if (_isMatch)
                {
                    //bgMatch.SetActive(true);
                    //_spineAnimation.AnimationName = SlotMachine.Instance.GetSpineAnimationMatch(dataID);
                    _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationMatch(dataID), true);
                    spine.transform.localScale = new Vector2(0.8f, 0.8f);
                    spine.transform.DOScale(new Vector2(1.0f, 1.0f), 1).SetEase(Ease.OutElastic);
                    isAnimateMatch = true;
                    _time = 0;
                }
                else
                {
                    SetAlpha(0.5f);
                    //bgMatch.SetActive(false);
                    //_spineAnimation.AnimationName = SlotMachine.Instance.GetSpineAnimationIdle(dataID);
                    _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(dataID), true);

                }
            }
        }
        else
        {
            if (_isMatch)
            {
                //bgMatch.SetActive(true);
                //_spineAnimation.AnimationName = SlotMachine.Instance.GetSpineAnimationMatch(dataID);
                _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationMatch(dataID), true);
                spine.transform.localScale = new Vector2(0.8f, 0.8f);
                spine.transform.DOScale(new Vector2(1.0f, 1.0f), 1).SetEase(Ease.OutElastic);
                isAnimateMatch = true;
                _time = 0;
            }
            else
            {
                //bgMatch.SetActive(false);
                //_spineAnimation.AnimationName = SlotMachine.Instance.GetSpineAnimationIdle(dataID);
                _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(dataID), true);

            }
        }
    }
    public void JellyEffect()
    {
        spine.transform.localScale = new Vector2(0.8f, 0.8f);
        spine.transform.DOScale(new Vector2(1.0f, 1.0f), 1).SetEase(Ease.OutElastic);
    }
    public void WildSpawnFX()
    {
        //_spineAnimation.AnimationState.SetAnimation(0, "Wild_Spawn", false);
    }
    public void SetPuzzleData()
    {
        //return;
        HideAllSpine();


        if (_isInit)
        {
            spine = spines[dataID];
            spine.SetActive(true);
            if (spine)
            {
                _spineAnimation = spine.GetComponent<SkeletonAnimation>();
                _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(dataID), true);
            }

            _isInit = false;
            return;
        }

        if (_spineAnimation)
        {
            SlotMachine slot = SlotMachine.Instance;
            dataID = slot.slotData.data[puzzleID];
            spine = spines[dataID];
            spine.SetActive(true);
            _spineAnimation = spine.GetComponent<SkeletonAnimation>();

            //Find match
            bool isMatch = false;
            if (slot.slotData.matches != null)
            {
                foreach (var match in slot.slotData.matches)
                {
                    foreach (int d in match)
                    {
                        if (d == puzzleID)
                        {
                            isMatch = true;
                            break;
                        }
                    }
                    if (isMatch) break;
                }
            }
            _isDelaySpineTimeScale = true;
            //_spineAnimation.timeScale = 12;
            isAnimateMatch = false;
            _time = 0;
            isDelayShowMatch = true;
            _isMatch = isMatch;
            //bgMatch.SetActive(false);
            SetAlpha(1);
            //_spineAnimation.AnimationName = slot.GetSpineAnimationIdle(dataID);
            _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(dataID), true);

            if (slot.slotData.matches.Count > 0)
            {
                _isMatchCheck = true;
            }
            else
            {
                _isMatchCheck = false;
            }
        }
    }
    public void CreateCollectableHeart()
    {
        if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Collectable)
        {
            GameObject heart = Instantiate(prefabScatterHeart, Vector3.zero, Quaternion.identity);
            heart.transform.position = transform.position;
            heart.transform.localScale = Vector3.zero;

            _spineAnimation.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationMatch(dataID), true);
            SetAlpha(1);
            JellyEffect();
        }
    }
    public void SetAlpha(float alpha)
    {
        {
            Color c = _spineAnimation.skeleton.GetColor();
            c.a = alpha;
            _spineAnimation.skeleton.SetColor(c);
        }

/*        {
            Color c = bgMatch.GetComponent<SpriteRenderer>().color;
            c.a = alpha;
            bgMatch.GetComponent<SpriteRenderer>().color = c;
        }*/
    }
    void HandleBounce()
    {
        if (!_isBounce) return;

        _currBounceTime += Time.deltaTime;
        if(_currBounceTime > bounceDuration)
        {
            //SetBounceMaterial(0, 0);
            _isBounce = false;
            _currBounceTime = 0;
        }
    }
    void BounceFinish()
    {
        //Debug.Log("Bounce Finish : " + puzzleID);
    }
    void SetBounceMaterial(float speed,float amplitude)
    {
        /*
        SpriteRenderer rend = sprite.GetComponent<SpriteRenderer>();
        Material _mat = rend.material;
        _mat.SetFloat("_Speed", speed);
        _mat.SetFloat("_Amplitude", amplitude);
        */
    }

    void SetMaterial()
    {
        //SpriteRenderer rend = randomSprite.GetComponent<SpriteRenderer>();
        //Material _mat = rend.material;
        //_mat.SetVector("_Offset", _offset);
    }

    public void Spin()
    {
        /*_state = (int)PuzzleState.Random;
        _currTime = 0;
        _currDelayTime = 0;
        _offset = Vector2.zero;*/
        spine.SetActive(false);
    }

    void RandomFinish(int column)
    {
        if(column == columnID)
            Bounce();
    }
    void OnActivePuzle(int index)
    {
        if(index == puzzleID)
        {
            SlotMachine.activePuzzle = this;
        }
    }
    private void OnMouseOver()
    {
        if (!isEnableClick) return;
        if (SlotMachine.isSpinning) return;
        if (UIRoundRewardPopup.Instance.isAppear) return;

        if (Input.GetMouseButtonDown(0) ){
            if (dataID == (int)SlotMachine.SlotMachineID.Puzzle_Scatter || dataID == (int)SlotMachine.SlotMachineID.Puzzle_Wild || dataID == (int)SlotMachine.SlotMachineID.Puzzle_Collectable) return;

            //Check wild tall
            GameObject[] wilds = GameObject.FindGameObjectsWithTag("WildTall");
            foreach(GameObject w in wilds)
            {
                if (w.GetComponent<WildTall>().columnID == columnID) return;
            }

            Debug.Log("puzlleID : " + dataID);
            Vector2 p = transform.position;
            p.x += 1.0f;
            p.y += 1.3f;
            if (puzzleID == 9 || puzzleID == 10 || puzzleID == 11)
                p.x -= 0.3f;
            if (puzzleID == 12 || puzzleID == 13 || puzzleID == 14)
                p.x -= 2.0f;
            if (puzzleID == 2 || puzzleID == 5 || puzzleID == 8 || puzzleID == 11 || puzzleID == 14)
                p.y += 0.6f;

            //Row 1
            if (puzzleID == 0 || puzzleID == 3 || puzzleID == 6 || puzzleID == 9 || puzzleID == 12)
                p.y = 2.85f;
            //Row 2
            if (puzzleID == 1 || puzzleID == 4 || puzzleID == 7 || puzzleID == 10 || puzzleID == 13)
                p.y = -0.1f;
            //Row 3
            if (puzzleID == 2 || puzzleID == 5 || puzzleID == 8 || puzzleID == 11 || puzzleID == 14)
                p.y = -2.75f;
            
            //middle
            if (puzzleID == 7)
                p.x = 0;
            PuzzleInfo.Instance.Show(p,dataID);
        }
    }
    void HideAllSpine()
    {
        foreach(GameObject sp in spines)
        {
            sp.SetActive(false);
        }
    }
}
