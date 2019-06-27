using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public struct Key : IXmlSerializable
{
    public KeyCode KeyPrimary;
    public KeyCode KeySecondary;

    public Key(KeyCode _primary, KeyCode _secondary)
    {
        KeyPrimary = _primary;
        KeySecondary = _secondary;
    }

    public bool Down()
    {
        return Input.GetKeyDown(KeyPrimary) || Input.GetKeyDown(KeySecondary) &&
            (!Input.GetKey(KeyPrimary) || !Input.GetKey(KeySecondary));
    }

    public bool Up()
    {
        return Input.GetKeyUp(KeyPrimary) || Input.GetKeyUp(KeySecondary) &&
            (!Input.GetKey(KeyPrimary) || !Input.GetKey(KeySecondary));
    }

    public bool Get()
    {
        return Input.GetKey(KeyPrimary) || Input.GetKey(KeySecondary);
    }

    public float GetFloat()
    {
        return Get() || Down() ? 1 : 0;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        return;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("KeyCodes");
        writer.WriteAttributeString("Primary", KeyPrimary.ToString());
        writer.WriteAttributeString("Secondary", KeySecondary.ToString());
        writer.WriteEndElement();
    }
}

[System.Serializable]
public struct Axis : IXmlSerializable
{
    public Key KeyPositive;
    public Key KeyNegative;

    public Axis(Key _positive, Key _negative)
    {
        KeyPositive = _positive;
        KeyNegative = _negative;
    }

    public float Get()
    {
        return KeyPositive.GetFloat() - KeyNegative.GetFloat();
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        return;
    }

    public void WriteXml(XmlWriter writer)
    {
        KeyPositive.WriteXml(writer);
        KeyNegative.WriteXml(writer);
    }
}

[System.Serializable]
public struct DPad : IXmlSerializable
{
    public Axis AxisX;
    public Axis AxisY;

    public DPad(Axis _xAxis, Axis _yAxis)
    {
        AxisX = _xAxis;
        AxisY = _yAxis;
    }

    public Vector2 Get()
    {
        return new Vector2(AxisX.Get(), AxisY.Get());
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        return;
    }

    public void WriteXml(XmlWriter writer)
    {
        AxisX.WriteXml(writer);
        AxisY.WriteXml(writer);
    }
}