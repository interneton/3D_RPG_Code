using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPC_Status_Sprite
{
    public string _Name;
    public Sprite _MyImage;
}
public class UIManger : MonoBehaviour
{
    [Header("게임 UI 오브젝트")]
    public GameObject _inventoryUI;
    public GameObject _SettingsUI;
    public GameObject _EquipSettings;
    public GameObject _DialogUI;
    public GameObject _QuestUI;
    public GameObject _SkillUI; // 스킬 아이콘
    public GameObject _SkillBookUI; // 스킬 창아이콘
    public EquipPopUP _EquipPopup;

    [Header("NPC 상태등 표시 이미지")]
    public List<NPC_Status_Sprite> _NpcStatusSprite;
   
    [Header("Equip 팝업 슬롯 관련")]
    public bool _IsPoint = false;
    [SerializeField] float _Delay = 1.0f;
    [SerializeField] float _IsClickTime = 0.0f;


    #region Singleton
    public static UIManger Instance;
    private void Awake()
    {
        Instance = this;

        var obj = FindObjectsOfType<UIManger>();
        if (obj.Length == 1) { DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
    #endregion

    private void Start()
    {
        _EquipPopup = transform.GetComponentInChildren<EquipPopUP>(true);

    }

    private void Update()
    {
        Inputkey_OpenUI();

        #region 인벤토리가 켜져있을때 활성화
        // 아이템 슬롯을 가진 UI 가 켜져있을때만 팝업 켜기
        if (_inventoryUI.gameObject.activeSelf || _QuestUI.gameObject.activeSelf
            || _EquipSettings.gameObject.activeSelf)
        {
            if (_IsPoint == true)
            {
                if (_IsClickTime <= _Delay)
                    _IsClickTime += Time.fixedDeltaTime;
            }

            if (_IsPoint == false)
                _IsClickTime = 0;

            if (_IsClickTime >= _Delay)
               _EquipPopup.gameObject.SetActive(true);
            else if (_IsClickTime < _Delay)
              _EquipPopup.gameObject.SetActive(false);
        }
        #endregion
    }

    // UI 오브젝트 Input 모음
    private void Inputkey_OpenUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.Find("SecondUI").childCount != 0)
            {
                Transform trans = transform.Find("SecondUI");

                SequenceUI(trans.GetChild(trans.childCount - 1).gameObject);
                return;
            }
            SequenceUI(_SettingsUI);
        }

        if(Input.GetKeyDown(KeyCode.E))
            SequenceUI(_EquipSettings);

        if(Input.GetKeyDown(KeyCode.Q))
            SequenceUI(_QuestUI);

        if (Input.GetKeyDown(KeyCode.I))
            SequenceUI(_inventoryUI);

        if (Input.GetKeyDown(KeyCode.K))
            SequenceUI(_SkillBookUI);

    }



    // UI 오브젝트 켜기, 끄기
    public void SequenceUI(GameObject UI)
    {
        if (UI != null)
        {
            UI.SetActive(!UI.activeSelf);
            if (!UI.activeSelf)  // 꺼진다면
            {
                UI.transform.parent = transform.Find("UI_PopUp");
                _IsPoint = false;
                _IsClickTime = 0.0f;
                _EquipPopup.gameObject.SetActive(false);
            }
            else if (UI.activeSelf)  //켜진다면
            {
                UI.transform.parent = transform.Find("SecondUI");
                if (UI != _SettingsUI && _SettingsUI.activeSelf)
                {
                    _SettingsUI.SetActive(!_SettingsUI.activeSelf);
                }
            }
        }
    }



}
