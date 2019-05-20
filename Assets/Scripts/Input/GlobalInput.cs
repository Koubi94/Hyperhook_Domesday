using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public static class GlobalInput
{
    private static bool s_loaded;
    private static GlobalInputDatabase s_database = new GlobalInputDatabase();

    public static void SetInputs(string[] _keyNames, Key[] _keys, string[] _axesNames, Axis[] _axes, string[] _dPadNames, DPad[] _dPads)
    {
        s_database.SetKeyData(_keyNames, _keys, _axesNames, _axes, _dPadNames, _dPads);
    }

    private static void CheckLoaded()
    {
        if (!s_loaded)
        {
            LoadInput();
        }
    }

    public static bool GetKeyDown(string _name)
    {
        CheckLoaded();

        if (s_database.Keys.ContainsKey(_name))
        {
            return s_database.Keys[_name].Down();
        }

        Debug.LogWarning($"No key with the name '{_name}' found!");
        return false;
    }

    public static bool GetKeyUp(string _name)
    {
        CheckLoaded();

        if (s_database.Keys.ContainsKey(_name))
        {
            return s_database.Keys[_name].Up();
        }

        Debug.LogWarning($"No key with the name '{_name}' found!");
        return false;
    }

    public static bool GetKey(string _name)
    {
        CheckLoaded();

        if (s_database.Keys.ContainsKey(_name))
        {
            return s_database.Keys[_name].Get();
        }

        Debug.LogWarning($"No key with the name '{_name}' found!");
        return false;
    }

    public static float GetAxis(string _name)
    {
        CheckLoaded();

        if (s_database.Axes.ContainsKey(_name))
        {
            return s_database.Axes[_name].Get();
        }

        Debug.LogWarning($"No axis with the name '{_name}' found!");
        return 0;
    }

    public static Vector2 GetDPad(string _name)
    {
        CheckLoaded();

        if (s_database.DPads.ContainsKey(_name))
        {
            return s_database.DPads[_name].Get();
        }

        Debug.LogWarning($"No DPad with the name '{_name}' found!");
        return Vector2.zero;
    }

    public static Vector2 GetMouseVector()
    {
        // TODO: Find another solution for the mouse Input
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public static Vector2 GetMouseWheel()
    {
        return Input.mouseScrollDelta;
    }

    public static void SaveInput()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GlobalInputDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/XML/GlobalInput.xml", FileMode.Create);
        serializer.Serialize(stream, s_database);
        stream.Close();
    }

    public static void LoadInput()
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset textAsset = Resources.Load<TextAsset>("XML/GlobalInput");
        xmlDoc.LoadXml(textAsset.text);

        using (TextReader reader = new StringReader(xmlDoc.InnerXml))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GlobalInputDatabase));
            s_database = (GlobalInputDatabase)serializer.Deserialize(reader);
        }

        s_loaded = true;
    }

    [System.Serializable]
    public class GlobalInputDatabase
    {
        public KeySettings Keys = new KeySettings();
        public AxisSettings Axes = new AxisSettings();
        public DPadSettings DPads = new DPadSettings();

        public void SetKeyData(string[] _keyNames, Key[] _keys, string[] _axisNames, Axis[] _axis, string[] _dPadNames, DPad[] _dPads)
        {
            for (int i = 0; i < _keyNames.Length; i++)
            {
                Keys.Add(_keyNames[i], _keys[i]);
            }

            for (int i = 0; i < _axisNames.Length; i++)
            {
                Axes.Add(_axisNames[i], _axis[i]);
            }

            for (int i = 0; i < _dPadNames.Length; i++)
            {
                DPads.Add(_dPadNames[i], _dPads[i]);
            }
        }
    }
}