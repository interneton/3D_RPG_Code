using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player _player;
    public QuestList _questLists;
    public StatsUI _EquipStats;

    [Header("GameData")]
    public GameData_Item _Data_Item;
    public GameData_Equip _Data_Equip;
    public GameData_Monster _Data_Monster;
    private void Awake()
    {
        Instance = this;

        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1) { DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        Transform equipTrans = UIManger.Instance._EquipSettings.transform.Find("MoveWindow");
        _EquipStats = equipTrans.Find("Stats").GetComponentInChildren<StatsUI>(true);
        _questLists = GetComponent<QuestList>();
    }
}
