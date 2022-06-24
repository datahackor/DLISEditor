using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Rp66COMLib;

namespace DLISEditor
{
	public class DLISSetNode : DLISNode
	{
		public DLISSetNode(ISlbDLISSet paramSet, TreeNode treeNode) : base(treeNode)
		{
			this.m_set = paramSet;
			this.m_Objects = this.m_set.Objects;
			this.m_ObjectCount = this.m_Objects.Count;
			this.m_Attributes = this.m_set.Attributes;
			this.m_AttributeCount = this.m_Attributes.Count;
			this.m_columnCount = this.m_AttributeCount + 1;
			this.m_liveDataSize = 0U;
			this.m_ObjNameMaxCopyDic = null;
		}

		public override string GetSetType()
		{
			return this.m_set.Type;
		}

		public override string GetColumnName(int Index)
		{
			if (Index < 0 || Index > this.m_columnCount)
			{
				return null;
			}
			if (Index == 0)
			{
				return "Object Name";
			}
			return this.m_Attributes[Index - 1].Name;
		}

		public override bool GetRow(out string[] row, ref int rowIndex, NameOptions nameOptions)
		{
			row = new string[this.m_columnCount];
			if (rowIndex >= this.m_ObjectCount)
			{
				return false;
			}
			ISlbDLISObject slbDLISObject = this.m_Objects[rowIndex];
			string key = string.Concat(new object[]
			{
				slbDLISObject.OriginNo,
				" ",
				slbDLISObject.CopyNo,
				" ",
				slbDLISObject.Name
			});
			if (this.m_ChangedObjNamesDic.ContainsKey(key))
			{
				string text = this.m_ChangedObjNamesDic[key];
				if (nameOptions.NeedCopy && nameOptions.NeedOrigin)
				{
					row[0] = text.Replace(" ", ",");
				}
				else
				{
					int num = text.IndexOf(" ");
					int num2 = text.LastIndexOf(" ");
					string text2 = text.Substring(num2 + 1);
					string str = text.Substring(num + 1, num2 - num);
					if (nameOptions.NeedOrigin)
					{
						row[0] = str + "," + text2;
					}
					else if (nameOptions.NeedCopy)
					{
						row[0] = slbDLISObject.OriginNo + "," + text2;
					}
					else
					{
						row[0] = text2;
					}
				}
			}
			else
			{
				row[0] = Util.GetObjName(nameOptions, slbDLISObject);
			}
			ISlbDLISAttributes attributes = slbDLISObject.Attributes;
			for (int i = 0; i < this.m_AttributeCount; i++)
			{
				if (((TriStateTreeView)this.m_treeNode.Nodes[i].TreeView).GetChecked(this.m_treeNode.Nodes[i]) == TriStateTreeView.CheckState.Checked)
				{
					ISlbDLISAttribute slbDLISAttribute = attributes[i];
					string text3 = null;
					if (nameOptions.NeedRepCode)
					{
						text3 = text3 + " [" + slbDLISAttribute.RepCode.ToString().Substring(8) + "] ";
					}
					if (slbDLISAttribute.DLISLibRepCode == DLISLibRepCode_t.DLISLib_Obname)
					{
						object[] array = (object[])slbDLISAttribute.Value;
						if (array == null)
						{
							goto IL_2EA;
						}
						if (array.Length > 1)
						{
							text3 += "[";
						}
						for (int j = 0; j < array.Length; j++)
						{
							ISlbDLISObjectName slbDLISObjectName = (ISlbDLISObjectName)array[j];
							if (slbDLISObjectName.Name.Length != 0)
							{
								text3 += Util.GetObjName(nameOptions, slbDLISObjectName);
							}
							if (array.Length > 1)
							{
								text3 += ((j + 1 == array.Length) ? ']' : ',');
							}
						}
					}
					else
					{
						text3 += slbDLISAttribute.StringValue;
						string units = slbDLISAttribute.Units;
						if (nameOptions.NeedUnit && units.Length > 0 && units.Length < 278)
						{
							text3 = text3 + " (" + slbDLISAttribute.Units + ")";
						}
					}
					row[i + 1] = ((text3 == null) ? "" : ((text3.Length > 32768) ? text3.Substring(0, 32768) : text3));
				}
				IL_2EA:;
			}
			rowIndex++;
			return true;
		}

		public bool GetRow(out string[] row, ref int rowIndex)
		{
			row = new string[this.m_columnCount];
			if (rowIndex >= this.m_ObjectCount)
			{
				return false;
			}
			ISlbDLISObject slbDLISObject = this.m_Objects[rowIndex];
			row[0] = Util.GetObjName(slbDLISObject);
			ISlbDLISAttributes attributes = slbDLISObject.Attributes;
			int i = 0;
			while (i < this.m_AttributeCount)
			{
				ISlbDLISAttribute slbDLISAttribute = attributes[i];
				string text = null;
				if (slbDLISAttribute.DLISLibRepCode != DLISLibRepCode_t.DLISLib_Obname)
				{
					text += slbDLISAttribute.StringValue;
					goto IL_F3;
				}
				object[] array = (object[])slbDLISAttribute.Value;
				if (array != null)
				{
					if (array.Length > 1)
					{
						text += "[";
					}
					for (int j = 0; j < array.Length; j++)
					{
						ISlbDLISObjectName slbDLISObjectName = (ISlbDLISObjectName)array[j];
						if (slbDLISObjectName.Name.Length != 0)
						{
							text += Util.GetObjName(slbDLISObjectName);
						}
						if (array.Length > 1)
						{
							text += ((j + 1 == array.Length) ? ']' : ',');
						}
					}
					goto IL_F3;
				}
				IL_106:
				i++;
				continue;
				IL_F3:
				row[i + 1] = ((text == null) ? "" : text);
				goto IL_106;
			}
			return true;
		}

		public DataTable CreateDataTable(NameOptions nameOptions, List<int> colIndexArray, Dictionary<string, string> changedObjNamesDic, int selectedColIndex, string searchContext)
		{
			this.m_colIndexArray = colIndexArray;
			this.m_ChangedObjNamesDic = changedObjNamesDic;
			this.m_selectedColIndex = selectedColIndex;
			this.m_searchContext = searchContext;
			return this.CreateDataTable(nameOptions);
		}

		public override DataTable CreateDataTable(NameOptions nameOptions)
		{
			string type = this.m_set.Type;
			DataTable dataTable = new DataTable(type);
			dataTable.Columns.Add("NO");
			dataTable.Columns.Add("");
			for (int i = 0; i < this.m_columnCount; i++)
			{
				TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
				if (i == 0 || triStateTreeView.GetChecked(this.m_treeNode.Nodes[i - 1]) == TriStateTreeView.CheckState.Checked)
				{
					string text = this.GetColumnName(i);
					if (dataTable.Columns.Contains(text))
					{
						text = text + "_" + i.ToString();
					}
					dataTable.Columns.Add(text);
					this.m_colIndexArray.Add(i);
				}
			}
			int num = 0;
			string[] array;
			while (this.GetRow(out array, ref num, nameOptions))
			{
				if (this.m_selectedColIndex == -1 || array[this.m_selectedColIndex] == null || array[this.m_selectedColIndex].ToUpper().Contains(this.m_searchContext))
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow[0] = (num - 1).ToString();
					int num2 = 2;
					for (int j = 0; j < array.Length; j++)
					{
						TriStateTreeView triStateTreeView2 = (TriStateTreeView)this.m_treeNode.TreeView;
						if (j == 0 || triStateTreeView2.GetChecked(this.m_treeNode.Nodes[j - 1]) == TriStateTreeView.CheckState.Checked)
						{
							dataRow[num2++] = array[j];
						}
					}
					dataTable.Rows.Add(dataRow);
				}
			}
			return dataTable;
		}

		public bool CanBeEditable(int colIndex)
		{
			return colIndex >= 0 && colIndex <= this.m_columnCount && !this.m_set.Type.EndsWith("440-EDIT-INFO") && (colIndex <= 1 || ((!this.m_set.Type.Equals("CHANNEL") || (!this.m_Attributes[colIndex - 1].Name.Equals("DIMENSION") && !this.m_Attributes[colIndex - 1].Name.Equals("REPRESENTATION-CODE"))) && this.m_Attributes[colIndex - 1].DLISLibRepCode != DLISLibRepCode_t.DLISLib_Obname && this.m_Attributes[colIndex - 1].DLISLibRepCode != DLISLibRepCode_t.DLISLib_Objref && this.m_Attributes[colIndex - 1].DLISLibRepCode != DLISLibRepCode_t.DLISLib_Date));
		}

		public bool CanBeDeleted()
		{
			return !this.m_set.Type.Contains("ORIGIN") && !this.m_set.Type.Equals("440-EDIT-INFO");
		}

		public string GetCellStringValue(int rowIndex, int colIndex, NameOptions nameOptions)
		{
			string[] array;
			this.GetRow(out array, ref rowIndex, nameOptions);
			return array[colIndex];
		}

		public string GetCellObName(int rowIndex)
		{
			if (this.m_Objects == null || this.m_Objects.Count <= rowIndex)
			{
				return string.Empty;
			}
			CSlbDLISObject cslbDLISObject = this.m_Objects[rowIndex];
			return string.Concat(new object[]
			{
				cslbDLISObject.OriginNo,
				" ",
				cslbDLISObject.CopyNo,
				" ",
				cslbDLISObject.Name
			});
		}

		public void GetCellInfo(int rowIndex, int colIndex, out string objName, out string attrName, out string value, out string type, out string unit)
		{
			objName = this.GetCellObName(rowIndex);
			if (colIndex == 0)
			{
				value = objName;
				type = string.Empty;
				unit = string.Empty;
				attrName = string.Empty;
				return;
			}
			CSlbDLISAttribute cslbDLISAttribute = this.m_Objects[rowIndex].Attributes[colIndex - 1];
			attrName = cslbDLISAttribute.Name;
			value = cslbDLISAttribute.StringValue;
			type = cslbDLISAttribute.DLISLibRepCode.ToString();
			unit = cslbDLISAttribute.Units;
		}

		public string GetNewObName(int rowIndex, string newObjName)
		{
			if (this.m_ObjNameMaxCopyDic == null)
			{
				this.m_ObjNameMaxCopyDic = new Dictionary<string, uint>();
				for (int i = 0; i < this.m_ObjectCount; i++)
				{
					CSlbDLISObject cslbDLISObject = this.m_Objects[i];
					if (this.m_ObjNameMaxCopyDic.ContainsKey(cslbDLISObject.Name))
					{
						if (this.m_ObjNameMaxCopyDic[cslbDLISObject.Name] < cslbDLISObject.CopyNo)
						{
							this.m_ObjNameMaxCopyDic[cslbDLISObject.Name] = cslbDLISObject.CopyNo;
						}
					}
					else
					{
						this.m_ObjNameMaxCopyDic.Add(cslbDLISObject.Name, cslbDLISObject.CopyNo);
					}
				}
			}
			if (this.m_ObjNameMaxCopyDic.ContainsKey(newObjName))
			{
				Dictionary<string, uint> objNameMaxCopyDic;
				(objNameMaxCopyDic = this.m_ObjNameMaxCopyDic)[newObjName] = objNameMaxCopyDic[newObjName] + 1U;
			}
			else
			{
				this.m_ObjNameMaxCopyDic.Add(newObjName, 0U);
			}
			return string.Concat(new object[]
			{
				this.m_Objects[rowIndex].OriginNo,
				" ",
				this.m_ObjNameMaxCopyDic[newObjName],
				" ",
				newObjName
			});
		}

		public EditedInfo UpdateAttributeValue(int rowIndex, int colIndex, string newValue, string newType, string newUnit)
		{
			EditedInfo editedInfo = new EditedInfo();
			CSlbDLISAttribute cslbDLISAttribute = this.m_Objects[rowIndex].Attributes[colIndex - 1];
			if (cslbDLISAttribute.DLISLibRepCode.ToString() != newType)
			{
				cslbDLISAttribute.DLISLibRepCode = (DLISLibRepCode_t)Enum.Parse(typeof(DLISLibRepCode_t), newType, true);
				cslbDLISAttribute.StringValue = newValue;
				cslbDLISAttribute.Units = newUnit;
				editedInfo.DataType = newType;
				editedInfo.StringValue = newValue;
				editedInfo.Unit = newUnit;
			}
			else
			{
				if (cslbDLISAttribute.StringValue != newValue)
				{
					cslbDLISAttribute.StringValue = newValue;
					editedInfo.StringValue = newValue;
				}
				if (cslbDLISAttribute.Units != newUnit)
				{
					cslbDLISAttribute.Units = newUnit;
					editedInfo.Unit = newUnit;
				}
			}
			editedInfo.AttrIndex = colIndex;
			return editedInfo;
		}

		public void GetSetAttributeIndex(Dictionary<string, int> dicAttributeIndex)
		{
			for (int i = 0; i < this.m_AttributeCount; i++)
			{
				if (((TriStateTreeView)this.m_treeNode.Nodes[i].TreeView).GetChecked(this.m_treeNode.Nodes[i]) == TriStateTreeView.CheckState.Checked)
				{
					string name = this.m_Attributes[i].Name;
					if (!dicAttributeIndex.ContainsKey(name))
					{
						dicAttributeIndex.Add(name, i);
					}
				}
			}
		}

		public override void Export(string fileName, NameOptions nameOptions, bool bExportHeaderInfo)
		{
			bool[] array = new bool[this.m_treeNode.Nodes.Count];
			int num = 0;
			TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
			foreach (object obj in this.m_treeNode.Nodes)
			{
				TreeNode node = (TreeNode)obj;
				array[num++] = (triStateTreeView.GetChecked(node) == TriStateTreeView.CheckState.Checked);
			}
			uint[] array2 = new uint[nameOptions.OriginMap.Count];
			uint[] array3 = new uint[nameOptions.OriginMap.Count];
			IEnumerator enumerator2 = nameOptions.OriginMap.Keys.GetEnumerator();
			IEnumerator enumerator3 = nameOptions.OriginMap.Values.GetEnumerator();
			num = 0;
			while (enumerator2.MoveNext() && enumerator3.MoveNext())
			{
				array2[num] = (uint)enumerator2.Current;
				array3[num] = (uint)enumerator3.Current;
				num++;
			}
			CSlbDLISSetExportOptions cslbDLISSetExportOptions = new CSlbDLISSetExportOptionsClass();
			cslbDLISSetExportOptions.AttrsToExport = array;
			cslbDLISSetExportOptions.NeedOriginNo = nameOptions.NeedOrigin;
			cslbDLISSetExportOptions.NeedCopyNo = nameOptions.NeedCopy;
			cslbDLISSetExportOptions.NeedRepCode = nameOptions.NeedRepCode;
			cslbDLISSetExportOptions.NeedUnit = nameOptions.NeedUnit;
			cslbDLISSetExportOptions.HeaderInfo = bExportHeaderInfo;
			cslbDLISSetExportOptions.ExistingOrigins = array2;
			cslbDLISSetExportOptions.NewOrigins = array3;
			this.m_set.Export(fileName, cslbDLISSetExportOptions);
		}

		public ISlbDLISSet Set
		{
			get
			{
				return this.m_set;
			}
		}

		public override uint ItemCount
		{
			get
			{
				return (uint)this.m_ObjectCount;
			}
		}

		public override uint Size
		{
			get
			{
				if (this.m_liveDataSize == 0U)
				{
					this.m_liveDataSize += (uint)(this.m_set.Type.Length + this.m_set.Name.Length + 2);
					for (int i = 0; i < this.m_AttributeCount; i++)
					{
						this.m_liveDataSize += (uint)(1 + this.m_Attributes[i].Name.Length + 1);
					}
					ISlbDLISObjects objects = this.m_set.Objects;
					int count = objects.Count;
					for (int j = 0; j < count; j++)
					{
						ISlbDLISObject slbDLISObject = objects[j];
						this.m_liveDataSize += Util.GetDataSize(slbDLISObject.Name, RepCode_t.REPCODE_OBNAME) + 1U;
						ISlbDLISAttributes attributes = slbDLISObject.Attributes;
						for (int k = 0; k < this.m_AttributeCount; k++)
						{
							ISlbDLISAttribute attr = attributes[k];
							this.m_liveDataSize += Util.GetDataSize(attr);
						}
					}
				}
				return this.m_liveDataSize;
			}
		}

		public override uint SizeOverhead
		{
			get
			{
				return 0U;
			}
		}

		public override string HeaderInfo
		{
			get
			{
				string str = "Name:" + this.m_set.Name;
				str = str + ", Type:" + this.m_set.Type;
				return str + ", " + (this.m_set.Encrypted ? "Encrypted," : "Not Encrypted,");
			}
		}

		public override string XML
		{
			get
			{
				TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
				string text = "<Set>";
				text = text + "<Name>" + this.m_treeNode.Text + "</Name>";
				foreach (object obj in this.m_treeNode.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					text += "<Attribute>";
					text = text + "<Name>" + treeNode.Text + "</Name>";
					text = text + "<Selected>" + (triStateTreeView.GetChecked(treeNode) == TriStateTreeView.CheckState.Checked).ToString() + "</Selected>";
					text += "</Attribute>";
				}
				text += "</Set>";
				return text;
			}
		}

		public override void LoadSettings(XmlDocument doc)
		{
			string text = this.m_treeNode.Parent.Parent.Text;
			bool flag = false;
			XmlNodeList xmlNodeList = doc.SelectNodes("/SelectionTree/LogicalFile");
			XmlNode xmlNode = null;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.SelectSingleNode("./Name").InnerText == text)
				{
					flag = true;
					xmlNode = xmlNode2;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("./Sets/Set");
			foreach (object obj2 in xmlNodeList2)
			{
				XmlNode xmlNode3 = (XmlNode)obj2;
				if (xmlNode3.SelectSingleNode("./Name").InnerText == this.m_treeNode.Text)
				{
					foreach (object obj3 in this.m_treeNode.Nodes)
					{
						TreeNode treeNode = (TreeNode)obj3;
						XmlNodeList xmlNodeList3 = xmlNode3.SelectNodes("./Attribute");
						foreach (object obj4 in xmlNodeList3)
						{
							XmlNode xmlNode4 = (XmlNode)obj4;
							if (xmlNode4.SelectSingleNode("./Name").InnerText == treeNode.Text && xmlNode4.SelectSingleNode("./Selected").InnerXml == "True")
							{
								triStateTreeView.SetChecked(treeNode, TriStateTreeView.CheckState.Checked);
								break;
							}
						}
					}
				}
			}
		}

		private ISlbDLISSet m_set;

		private ISlbDLISObjects m_Objects;

		private ISlbDLISAttributes m_Attributes;

		private uint m_liveDataSize;

		private int m_selectedColIndex;

		private readonly int m_ObjectCount;

		private readonly int m_AttributeCount;

		private string m_searchContext;

		private List<int> m_colIndexArray;

		private Dictionary<string, uint> m_ObjNameMaxCopyDic;

		private Dictionary<string, string> m_ChangedObjNamesDic;
	}
}
