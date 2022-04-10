using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ns
{
    // * Rules
    //      - comment: //
    // * Usage:
    //      CsvLoader csv = new CsvLoader();
    //      if (csv.Open("GameData/test"))
    //      {
    //          while (csv.ReadLine() == true)
    //          {
    //              string str = csv.GetField(0);
    //              int val = csv.GetFieldToInt(1);
    //              ...
    //          }
    //          csv.Close();
    //      }

    public class CsvLoader
    {
        private StringReader m_stringReader = null;
        private List<string> m_fields = null;
        private string m_csvData = "";
        private int m_fieldCount = 0;
        private char m_fieldSeparator = ',';

        public CsvLoader()
        {
            m_fields = new List<string>();
        }

        ~CsvLoader() { Close(); }

        public int FieldCount
        {
            get { return m_fieldCount; }
        }

        public char FieldSeparator
        {
            get { return m_fieldSeparator; }
            set { m_fieldSeparator = value; }
        }

        private bool IsWhiteSpace(char c)
        {
            return (c == ' ' || c == '\t');
        }

        private bool IsField(ref string line)
        {
            if (line == null)
                return false;

            int i = 0;

            while (i < line.Length && IsWhiteSpace(line[i]))
                i++;

            // check if empty line
            if (i >= line.Length)
                return false;

            // check if comment
            if (line[i] == '/')
            {
                int j = i + 1; // index of next character
                if (j < line.Length && line[j] == '/')
                    return false;
            }

            return true;
        }

        private int ParseQuoted(ref string line, ref string field, int index)
        {
            int i = 0;
            StringBuilder bld = new StringBuilder();

            for (i = index; i < line.Length; ++i)
            {
                if (line[i] == '"')
                {
                    ++i;
                    if (i >= line.Length)
                    {
                        break;
                    }
                    else
                    {
                        if (line[i] != '"')
                        {
                            int j = line.IndexOf(FieldSeparator, i);
                            if (j < 0)
                                j = line.Length;

                            // copy remained string
                            for (j -= i; j-- > 0;)
                                bld.Append(line[i++]);

                            break;
                        }
                    }
                }

                bld.Append(line[i]);
            }

            field = bld.ToString();
            return i;
        }

        private int ParsePlane(ref string line, ref string field, int index)
        {
            int i = line.IndexOf(FieldSeparator, index);

            if (i < 0) // can't find next separator
                i = line.Length;

            field = line.Substring(index, i - index);
            return i;
        }

        private int Split(ref string line)
        {
            m_fieldCount = 0; // field count must be zero.

            if (line == null)
                return 0;

            int i = 0;

            while (i < line.Length && IsWhiteSpace(line[i]))
                i++;

            // check if empty line
            if (i >= line.Length)
                return 0;

            int j = 0;
            bool hasNextField = true;
            string field = "";

            do
            {
                if (i < line.Length && line[i] == '"')
                    j = ParseQuoted(ref line, ref field, ++i);
                else
                    j = ParsePlane(ref line, ref field, i);

                if (m_fieldCount >= m_fields.Count)
                    m_fields.Add(field);
                else
                    m_fields[m_fieldCount] = field;

                m_fieldCount++;
                i = j + 1;

                hasNextField = (i < line.Length || (j < line.Length && line[j] == ','));
            }
            while (hasNextField);

            return m_fieldCount;
        }

        public bool Open(string path, bool skipHeader = true, bool readFromResources = true)
        {
            bool isSuccess = true;

            // NOTE : Resources.Load() as TextAsset 을 통해서 파일을 로드하였었으나.
            // TextAsset은 .txt파일 확장자만 지원하여, .csv같은 다른 확장자도 지원하기위하여
            // File.OpenText를 통해서 파일 로드하도록 수정하였음.
            //StreamReader streamReader = null; 
            StringReader streamReader = null;


            //string resourcesPath = "";

            /*
			if( Application.platform == RuntimePlatform.Android )				
				streamReader = File.OpenText(Application.streamingAssetsPath + "/Resources/" + path);									
			else 
			{
				if( readFromResources )
					streamReader = File.OpenText(Application.dataPath + "/Resources/" + path);									
				else
					streamReader = File.OpenText(path);	
			}*/

            path = path.Replace(".csv", "");
            path = path.Replace(".txt", "");
            TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
            streamReader = new StringReader(textAsset.text);

            if (streamReader != null)
            {
                // NOTE: 파일 내용을 멤버에 저장하지 않고,
                // 바로 StringReader로 넘기면 문제가 생길까???
                // 이 부분으로 인해 성능상 부담이 될 것 같은데...
                string text = streamReader.ReadToEnd();
                m_csvData = text;
            }
            else
            {
                m_csvData = "";
                isSuccess = false;
            }

            if (isSuccess == true)
            {
                if (m_stringReader == null)
                {
                    try
                    {
                        StringReader sr = new StringReader(m_csvData);
                        m_stringReader = sr;
                    }
                    catch (Exception e)
                    {
                        isSuccess = false;
                        Debug.Log("Exception: " + e.Message);
                    }
                }
                else
                {
                    isSuccess = false;
                }
            }

            // skip header line if skip flag is setting.
            if (isSuccess == true && skipHeader == true)
            {
                ReadLine();
            }

            return isSuccess;
        }

        public void Close()
        {
            m_fieldCount = 0;

            if (m_stringReader != null)
            {
                m_stringReader.Close();
                m_stringReader = null;
            }
        }

        public bool ReadLine()
        {
            m_fieldCount = 0;

            if (m_stringReader == null)
                return false;

            string line = m_stringReader.ReadLine();

            // filtering empty line or comment
            while (line != null && !IsField(ref line))
                line = m_stringReader.ReadLine();

            if (line == null) // eof
                return false;

            Split(ref line);
            return true;
        }

        public string GetField(int index)
        {
            if (0 <= index && index < m_fieldCount)
                return m_fields[index];
            return "";
        }

        public int GetFieldToInt(int index)
        {
            if (0 <= index && index < m_fieldCount)
            {
                int ret;
                if (Int32.TryParse(m_fields[index], out ret) == true)
                    return ret;
            }
            return 0;
        }

        public float GetFieldToFloat(int index)
        {
            if (0 <= index && index < m_fieldCount)
            {
                float ret;
                if (float.TryParse(m_fields[index], out ret) == true)
                    return ret;
            }
            return 0f;
        }

        public static System.Object[] LoadCsvToObjectList(string path, bool hasFieldName = true, char seprator = ',')
        {
            List<System.Object> dataList = new List<object>();

            CsvLoader csv = new CsvLoader();
            csv.FieldSeparator = seprator;

            if (csv.Open(path, false))
            {
                List<string> fieldNameList = new List<string>();

                if (hasFieldName && csv.ReadLine()) //첫번째 행 - 필드 네임 행
                {
                    for (int i = 0; i < csv.FieldCount; i++)
                        fieldNameList.Add(csv.GetField(i));
                }

                bool bFirstLine = true;

                while (csv.ReadLine() == true)
                {
                    if (bFirstLine)
                    {
                        // 필드 이름이 없는 테이블인 경우 숫자 인덱스를 이용하여 참조할 수 있게 한다.
                        if (!hasFieldName)
                        {
                            for (int i = 0; i < csv.FieldCount; i++)
                                fieldNameList.Add(string.Format("{0}", i));
                        }
                        bFirstLine = false;
                    }

#if UNITY_EDITOR
                    if (fieldNameList.Count != csv.FieldCount)
                    {
                        EditorUtility.DisplayDialog("Error - Invalid field count", csv.GetField(0), "OK");
                    }
#endif

                    Dictionary<string, System.Object> dict = new Dictionary<string, System.Object>();

                    for (int i = 0; i < csv.FieldCount; i++)
                    {
                        string str = csv.GetField(i);
                        dict[fieldNameList[i]] = str;
                    }

                    dataList.Add(dict);
                }
                csv.Close();
            }

            return dataList.ToArray();
        }
    }
}