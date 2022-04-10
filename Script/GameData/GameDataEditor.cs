using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ns;



public class GameDataEditor : EditorWindow
{
    GameData gameData;


    [MenuItem("Window/GameData Editor")]
    static public void OPENGameData()
    {
        EditorWindow.GetWindow<GameDataEditor>(false, "GameData Editor", true);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("== GAME DATA EDITOR ==");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();

        gameData = EditorGUILayout.ObjectField(gameData, typeof(GameData), false) as GameData;

        if (GUILayout.Button("import from csv"))
        {
            string filePath = "GameData/csv/" + gameData.name + ".csv";

            bool hasFieldName = true;
            char seperator = ',';
            System.Object[] objList = CsvLoader.LoadCsvToObjectList(filePath, hasFieldName, seperator);

            gameData.parse(objList);
            EditorUtility.SetDirty(gameData);

            Debug.Log(gameData.name + "적용이 완료되었습니다");
        }



    }
}

