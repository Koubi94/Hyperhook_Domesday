using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class KeySettings : Dictionary<string, Key>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            return;
        }

        KeyCode primary;
        KeyCode secondary;
        Key value;

        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            string key = reader.GetAttribute("Name");
            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);

            value = new Key(primary, secondary);

            Add(key, value);
            reader.Read();
            reader.Read();
        }

        reader.Read();
        if (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.MoveToContent();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (string key in Keys)
        {
            writer.WriteStartElement("Key");
            writer.WriteAttributeString("Name", key.ToString());
            this[key].WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}



public class AxisSettings : Dictionary<string, Axis>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            return;
        }

        KeyCode primary;
        KeyCode secondary;
        Key positive;
        Key negative;
        Axis value;

        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            string key = reader.GetAttribute("Name");
            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            positive = new Key(primary, secondary);

            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            negative = new Key(primary, secondary);

            value = new Axis(positive, negative);

            Add(key, value);
            reader.Read();
            reader.Read();
        }

        reader.Read();
        if (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.MoveToContent();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (string key in Keys)
        {
            writer.WriteStartElement("Axis");
            writer.WriteAttributeString("Name", key.ToString());
            this[key].WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}



public class DPadSettings : Dictionary<string, DPad>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            return;
        }

        KeyCode primary;
        KeyCode secondary;
        Key positive;
        Key negative;
        Axis xAxis;
        Axis yAxis;
        DPad value;

        reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            string key = reader.GetAttribute("Name");
            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            positive = new Key(primary, secondary);

            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            negative = new Key(primary, secondary);

            xAxis = new Axis(positive, negative);

            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            positive = new Key(primary, secondary);

            reader.Read();

            System.Enum.TryParse(reader.GetAttribute("Primary"), out primary);
            System.Enum.TryParse(reader.GetAttribute("Secondary"), out secondary);
            negative = new Key(primary, secondary);

            yAxis = new Axis(positive, negative);

            reader.Read();

            value = new DPad(xAxis, yAxis);
            
            Add(key, value);

            reader.Read();
            reader.Read();
        }

        reader.Read();
        if (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.MoveToContent();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (string key in Keys)
        {
            writer.WriteStartElement("DPad");
            writer.WriteAttributeString("Name", key.ToString());
            this[key].WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}