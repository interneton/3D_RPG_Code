using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

public class GameData : ScriptableObject
{
#if UNITY_EDITOR
    public virtual void parse(System.Object[] objList)
    {

    }

    protected void ParseObject(object obj, object csvObj)
    {
        Dictionary<string, System.Object> dict = csvObj as Dictionary<string, System.Object>;

        FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        string fieldName = "";

        for (int i = 0; i < infos.Length; i++)
        {
            FieldInfo fi = infos[i];

            fieldName = fi.Name;

            if (dict.ContainsKey(fieldName))
            {
                object valueObj = dict[fieldName];

                if (fi.FieldType == typeof(string))
                {
                    fi.SetValue(obj, valueObj.ToString());
                }
                else if (fi.FieldType == typeof(int))
                {
                    fi.SetValue(obj, Convert.ToInt32(valueObj));
                }

                else
                {
                    Debug.LogError("Unsupported Type !! ");
                }

            }

        }
    }

#endif
}
