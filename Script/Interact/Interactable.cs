using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public string _Name;

    [SerializeField] protected Image _MyImage;
    [SerializeField] protected Text _NamePopUp;
    [SerializeField] protected float _Interact_radius = 3f;
    [SerializeField] protected float _UI_CanvasSight = 10f;

    protected Transform _player;
    protected bool _isSelect = false;
    [SerializeField] protected CamPlayer _camPlayer;
    [SerializeField] protected Quaternion _myRotation; // 기울기 저장해서 원래대로 돌아오기

    protected bool _AbleToInteracted = false;

    protected virtual void Start()
    {
        Transform CanvasTrans = transform.Find("UI Canvas");
        if (CanvasTrans != null)
        {
            _NamePopUp = CanvasTrans.Find("Name").GetComponent<Text>();
            _MyImage = CanvasTrans.Find("Status_Image").GetComponent<Image>();
            _NamePopUp.text = _Name;
        }

        _myRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        _camPlayer = Camera.main.transform.parent.parent.GetComponent<CamPlayer>();
    }

    public virtual void Interact()
    {
        Debug.Log("interact " + transform.name);

    }

    private void Update()
    {
        InteractDistance();
    }

    protected virtual void InteractDistance()    //인터랙트 가능한 범위 체크
    {
        if (_isSelect && !_AbleToInteracted)
        {
            float distance = Vector3.Distance(_player.position, transform.position);

            if (distance <= _Interact_radius)
            {
                _AbleToInteracted = true;
                Interact();
            }
        }
    }

    public void SelectTrans(Transform playerTransform)
    {
        _isSelect = true;
        _player = playerTransform;
        _AbleToInteracted = false;
        StopCoroutine("ForwardToMyPos");
    }

    public void NullSelectTrans()
    {
        _player = null;
        _isSelect = false;
        _AbleToInteracted = false;
        _camPlayer.maxDistance = 4f;
        StartCoroutine("ForwardToMyPos");
    }

    IEnumerator ForwardToMyPos()
    {
        while (transform.rotation != _myRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _myRotation, Time.deltaTime * 5f);
            yield return null;
        }
    }
}
