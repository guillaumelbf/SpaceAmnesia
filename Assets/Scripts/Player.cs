using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EAnim
{
    IDLE,
    UP,
    DOWN,
    SIDE
};

public class Player : MonoBehaviour
{
    private ChatBox _chatBox;
    
    Rigidbody2D mRigidbody;
    Animator animator = null;

    [SerializeField] float speed = 0;
    [SerializeField] GameObject skin = null;

    [Header("Sounds")]
    [SerializeField] AudioSource fxSource = null;

    private Vector2 curVeclocity = Vector2.zero;
    private Vector3 baseSkinScale = Vector3.zero;
    private Vector3 reverseSideSkinScale = Vector3.zero;

    
    public GameObject canvasTutorial;
    private bool _inTutorial;

    public GameObject canvasOptions;
    private bool _inPause;
    
    // Start is called before the first frame update
    void Start()
    {
        _chatBox = GetComponent<ChatBox>();
        mRigidbody = GetComponent<Rigidbody2D>();
        animator = skin.GetComponent<Animator>();

        baseSkinScale = skin.transform.localScale;
        reverseSideSkinScale = new Vector3(baseSkinScale.x * -1,baseSkinScale.y,baseSkinScale.z);
        
        canvasTutorial.SetActive(true);
        _inTutorial = true;
        _inPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inTutorial && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            _inTutorial = false;
            canvasTutorial.SetActive(false);
        }

        if (_inPause && Input.GetKeyDown(KeyCode.Escape))
        {
            _inPause = false;
            canvasOptions.SetActive(false);
        }
        else if (!_inPause && !_chatBox.isWriting && Input.GetKeyDown(KeyCode.Escape))
        {
            _inPause = true;
            canvasOptions.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        mRigidbody.velocity = Vector2.zero;
        switchAnim(EAnim.IDLE);
        skin.transform.localScale = baseSkinScale;
        verticalMove(Input.GetAxisRaw("Vertical"));
        horizontalMove(Input.GetAxisRaw("Horizontal"));
        mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
    }

    void verticalMove(float _direction)
    {
        if (_chatBox.isWriting || Dialog.isPrinting || Dialog.waitingAction || _inTutorial || _inPause)
            return;
        if(_direction.Equals(1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.up, ref curVeclocity,0.05f);
            switchAnim(EAnim.UP);
        }
        else if(_direction.Equals(-1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.down, ref curVeclocity, 0.05f);
            switchAnim(EAnim.DOWN);
        }
    }

    void horizontalMove(float _direction)
    {
        if (_chatBox.isWriting || Dialog.isPrinting || Dialog.waitingAction || _inTutorial || _inPause)
            return;
        if (_direction.Equals(1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.right, ref curVeclocity, 0.05f);
            switchAnim(EAnim.SIDE);
            skin.transform.localScale = reverseSideSkinScale;
        }
        else if (_direction.Equals(-1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.left, ref curVeclocity, 0.05f);
            switchAnim(EAnim.SIDE);
        }
    }

    void switchAnim(EAnim _anim)
    {
        animator.SetBool("MoveUp", false);
        animator.SetBool("MoveDown", false);
        animator.SetBool("MoveSide", false);

        switch (_anim)
        {
            case EAnim.IDLE:
                fxSource.mute = true;
                break;
            case EAnim.UP:
                animator.SetBool("MoveUp", true);
                fxSource.mute = false;
                break;
            case EAnim.DOWN:
                animator.SetBool("MoveDown", true);
                fxSource.mute = false;
                break;
            case EAnim.SIDE:
                animator.SetBool("MoveSide", true);
                fxSource.mute = false;
                break;
            default:break;
        }
    }
}
