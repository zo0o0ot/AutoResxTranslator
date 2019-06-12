using System.Collections.Generic;
using System.Xml;

/* 
 * AutoResxTranslator
 * by Salar Khalilzadeh
 * 
 * https://autoresxtranslator.codeplex.com/
 * Mozilla Public License v2
 */
namespace AutoResxTranslator
{
	public static class ResxTranslator
	{
		public static List<XmlNode> ReadResxData(XmlDocument doc)
		{
			var root = doc.SelectSingleNode("root");
			var dataList = new List<XmlNode>();
			foreach (XmlNode node in root.ChildNodes)
			{
				if (node.NodeType != XmlNodeType.Element)
					continue;
				if (node.Name != "data")
					continue;
				dataList.Add(node);
			}
			// node.Attributes["name"].Value
			return dataList;
		}
		public static void AddLanguageNode(XmlDocument doc, string key, string value)
		{
			// xml:space="preserve" is essential for some software to read

			var root = doc.SelectSingleNode("root");

			var node = doc.CreateElement("data");
			
			var nameAtt = doc.CreateAttribute("name");
			nameAtt.Value = key;
			node.Attributes.Append(nameAtt);

			var xmlspaceAtt = doc.CreateAttribute("xml:space");
			xmlspaceAtt.Value = "preserve";
			node.Attributes.Append(xmlspaceAtt);

			var valNode = doc.CreateElement("value");
			valNode.InnerText = value;
			node.AppendChild(valNode);

			root.AppendChild(node);
		}

		public static XmlNode GetDataValueNode(XmlNode dataNode)
		{
			for (int i = 0; i < dataNode.ChildNodes.Count; i++)
			{
				var node = dataNode.ChildNodes[i];
				if (node.NodeType != XmlNodeType.Element)
					continue;
				if (node.Name == "value")
					return node;
			}
			return null;
		}

		public static void SetDataValue(XmlDocument doc,XmlNode dataNode, string value)
		{
			var valueNode = GetDataValueNode(dataNode);
			if (valueNode == null)
			{
				var valNode = doc.CreateElement("value");
				valNode.InnerText = value;
				dataNode.AppendChild(valNode);
			}
			else
			{
				valueNode.InnerText = value;
			}
		}

		public static string GetDataKeyName(XmlNode dataNode)
		{
			if (dataNode == null)
				return string.Empty;
			return dataNode.Attributes["name"].Value;
		}
		public static string GetDataValueNodeContent(XmlNode dataNode)
		{
			for (int i = 0; i < dataNode.ChildNodes.Count; i++)
			{
				var node = dataNode.ChildNodes[i];
				if (node.NodeType != XmlNodeType.Element)
					continue;
				if (node.Name == "value")
					return node.InnerText;
			}
			return null;
		}

        public static bool HasDisplayText(XmlNode dataNode)
        {
            //If the data has a type attribute, then the value isn't a string.
            string type = dataNode?.Attributes?.GetNamedItem("type")?.Value;
            if (!string.IsNullOrEmpty(type))
                return false;

            //Sanity check. All data nodes should have a name.
            string name = GetDataKeyName(dataNode);
            if (string.IsNullOrEmpty(name))
                return false;

            //Determine whether the name includes a period.
            int dotIndex = name.LastIndexOf('.');

            //If the name doesn't include a period, then it is likely a string resource and ought to be translated.
            //WinForms control resource names should all have a period separating the control name from the property name.
            //String resources aren't allowed to have a period.
            if (dotIndex < 0)
                return true;

            //Use the property's name to determine whether it has display text.
            //For now we'll assume every property named 'Text' is display text and all other properties are not.
            //The ones I have encountered are Text, HeaderText, and ToolTipText.
            //The TextAlign property shouldn't be translated, but that has a type attribute and thus doesn't reach this code path.
            string propertyName = name.Substring(dotIndex + 1);
            if (propertyName.IndexOf("Text", System.StringComparison.InvariantCultureIgnoreCase) >= 0)
                return true;

            //Assume it doesn't contain display text.
            return false;
        }
	}
}
