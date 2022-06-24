using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Rp66COMLib;

namespace DLISEditor
{
	public class DLISFrameNode : DLISNode
	{
		public DLISFrameNode(ISlbDLISFrame paramFrame, TreeNode treeNode, uint fromRange, uint toRange, double firstIndex, double lastIndex) : base(treeNode)
		{
			this.m_frame = paramFrame;
			this.iCountChannels = this.m_frame.Channels.Count;
			this.m_Channels = new ISlbDLISChannel[this.iCountChannels];
			for (int i = 0; i < this.iCountChannels; i++)
			{
				this.m_Channels[i] = this.m_frame.Channels[i];
			}
			this.m_columnCount = this.iCountChannels + 1;
			this.m_recNoStart = fromRange;
			this.m_recNoEnd = toRange;
			this.m_indexStart = firstIndex;
			this.m_indexEnd = lastIndex;
			this.m_bConsiderIndex = false;
			this.m_noOfFrames = 0U;
			this.m_frameliveDataSize = 0U;
			this.m_aChannelSize = new uint[this.m_columnCount - 1];
		}

		public string GetMaxIndex(out string units)
		{
			double d;
			string text;
			this.m_frame.GetIndexMax(out d, out text);
			units = text;
			if (this.m_frame.IndexType == "TIME")
			{
				try
				{
					return DateTime.FromOADate(d).ToString();
				}
				catch (ArgumentException)
				{
				}
			}
			return d.ToString();
		}

		public string GetMinIndex(out string units)
		{
			double d;
			string text;
			this.m_frame.GetIndexMin(out d, out text);
			units = text;
			if (this.m_frame.IndexType == "TIME")
			{
				try
				{
					return DateTime.FromOADate(d).ToString();
				}
				catch (ArgumentException)
				{
				}
			}
			return d.ToString();
		}

		public override string HeaderInfo
		{
			get
			{
				string text = string.Concat(new object[]
				{
					"Name :",
					this.m_frame.OriginNo,
					",",
					this.m_frame.CopyNo,
					",",
					this.m_frame.Name,
					", "
				});
				string text2;
				string minIndex = this.GetMinIndex(out text2);
				string maxIndex = this.GetMaxIndex(out text2);
				text = string.Concat(new string[]
				{
					text,
					" Index Range :",
					minIndex,
					"...",
					maxIndex,
					", (Units ",
					text2,
					")"
				});
				text = text + " , No of frames :" + this.ItemCount.ToString();
				text = text + ", Current Selection :" + (this.m_bAllRecords ? "All Records " : (this.m_bConsiderIndex ? "Index " : "Record# "));
				string text3 = null;
				string text4 = null;
				if (!this.m_bAllRecords)
				{
					if (this.m_bConsiderIndex)
					{
						text3 = this.m_indexStart.ToString();
						if (this.m_frame.IndexType == "TIME")
						{
							try
							{
								text3 = DateTime.FromOADate(this.m_indexStart).ToString();
							}
							catch (ArgumentException)
							{
							}
						}
						text4 = this.m_indexEnd.ToString();
						if (!(this.m_frame.IndexType == "TIME"))
						{
							goto IL_1CF;
						}
						try
						{
							text4 = DateTime.FromOADate(this.m_indexStart).ToString();
							goto IL_1CF;
						}
						catch (ArgumentException)
						{
							goto IL_1CF;
						}
					}
					text3 = this.m_recNoStart.ToString();
					text4 = this.m_recNoEnd.ToString();
					IL_1CF:
					text += text3.ToString();
					text = text + "..." + text4.ToString();
				}
				return text;
			}
		}

		public bool AllRecords
		{
			get
			{
				return this.m_bAllRecords;
			}
			set
			{
				this.m_bAllRecords = value;
			}
		}

		public override string GetSetType()
		{
			return this.m_frame.Name;
		}

		public override string GetColumnName(int Index)
		{
			if (Index < 0 || Index > this.m_columnCount)
			{
				return null;
			}
			if (Index == 0)
			{
				return "Record#";
			}
			string str = this.m_frame.Channels[Index - 1].OriginNo.ToString() + ',' + this.m_frame.Channels[Index - 1].CopyNo.ToString();
			return str + this.m_frame.Channels[Index - 1].Name;
		}

		public override int StartRowNumber()
		{
			return (int)this.m_recNoStart;
		}

		public override bool NeedsToDisplay(int rowNo)
		{
			double num;
			double num2;
			if (this.m_indexStart > this.m_indexEnd)
			{
				num = this.m_indexStart;
				num2 = this.m_indexEnd;
			}
			else
			{
				num = this.m_indexEnd;
				num2 = this.m_indexStart;
			}
			bool result;
			if (this.m_bConsiderIndex)
			{
				double index = this.m_frame.Index;
				result = (index >= num2 && index <= num);
			}
			else
			{
				result = (this.m_bAllRecords || ((long)rowNo >= (long)((ulong)this.m_recNoStart) && (long)rowNo <= (long)((ulong)this.m_recNoEnd)));
			}
			return result;
		}

		private void Reset()
		{
			if (!(this.m_treeNode.Parent.Parent.Tag is CSlbDLISFileClass))
			{
				return;
			}
			CSlbDLISFileClass cslbDLISFileClass = (CSlbDLISFileClass)this.m_treeNode.Parent.Parent.Tag;
			cslbDLISFileClass.GoToBookMark(1);
		}

		public override DataTable CreateDataTable(NameOptions nameOptions)
		{
			NameOptions options = new NameOptions(true, true);
			this.Reset();
			DataTable dataTable = new DataTable(this.m_frame.Name);
			dataTable.Columns.Add("Record#");
			TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
			for (int i = 0; i < this.iCountChannels; i++)
			{
				if (triStateTreeView.GetChecked(this.m_treeNode.Nodes[i]) == TriStateTreeView.CheckState.Checked)
				{
					string text = Util.GetChannelName(options, this.m_Channels[i]);
					text = text + " (" + this.m_Channels[i].Units + ")";
					dataTable.Columns.Add(text);
				}
			}
			double num;
			double num2;
			if (this.m_indexStart > this.m_indexEnd)
			{
				num = this.m_indexStart;
				num2 = this.m_indexEnd;
			}
			else
			{
				num = this.m_indexEnd;
				num2 = this.m_indexStart;
			}
			int j = 0;
			int num3 = j;
			this.m_frame.ReadNextFrame(ref j);
			while (j > num3)
			{
				bool flag = this.m_bAllRecords;
				if (!this.m_bAllRecords)
				{
					if (this.m_bConsiderIndex)
					{
						double index = this.m_frame.Index;
						flag = (index >= num2 && index <= num);
					}
					else
					{
						flag = ((long)j >= (long)((ulong)this.m_recNoStart) && (long)j <= (long)((ulong)this.m_recNoEnd));
					}
				}
				if (flag)
				{
					DataRow dataRow = dataTable.NewRow();
					int num4 = 1;
					dataRow[0] = j;
					for (int k = 0; k < this.iCountChannels; k++)
					{
						if (triStateTreeView.GetChecked(this.m_treeNode.Nodes[k]) == TriStateTreeView.CheckState.Checked)
						{
							string text2 = this.m_Channels[k].StringValue;
							if (text2.Length > 1024)
							{
								text2 = text2.Substring(0, 1024);
							}
							dataRow[num4++] = ((text2 == null) ? "" : text2);
						}
					}
					dataTable.Rows.Add(dataRow);
				}
				num3 = j;
				this.m_frame.ReadNextFrame(ref j);
			}
			this.m_noOfFrames = (uint)num3;
			return dataTable;
		}

		public override bool GetRow(out string[] row, ref int rowIndex, NameOptions nameOptions)
		{
			row = new string[this.m_columnCount];
			int num = rowIndex;
			this.m_frame.ReadNextFrame(ref rowIndex);
			if (rowIndex == num)
			{
				return false;
			}
			if (!this.NeedsToDisplay(rowIndex))
			{
				return true;
			}
			row[0] = rowIndex.ToString();
			for (int i = 0; i < this.iCountChannels; i++)
			{
				TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
				if (triStateTreeView.GetChecked(this.m_treeNode.Nodes[i]) == TriStateTreeView.CheckState.Checked)
				{
					string stringValue = this.m_Channels[i].StringValue;
					row[i + 1] = ((stringValue == null) ? "" : stringValue);
				}
			}
			return true;
		}

		public override void Export(string fileName, NameOptions nameOptions, bool FrameheaderInfo)
		{
			this.Reset();
			bool[] array = new bool[this.m_treeNode.Nodes.Count];
			TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
			int num = 0;
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
			CSlbDLISFrameExportOptions cslbDLISFrameExportOptions = new CSlbDLISFrameExportOptionsClass();
			cslbDLISFrameExportOptions.AllRecords = this.m_bAllRecords;
			cslbDLISFrameExportOptions.IndexBasedFilter = this.m_bConsiderIndex;
			cslbDLISFrameExportOptions.RecNoStart = this.m_recNoStart;
			cslbDLISFrameExportOptions.RecNoEnd = this.m_recNoEnd;
			cslbDLISFrameExportOptions.IndexStart = this.m_indexStart;
			cslbDLISFrameExportOptions.IndexEnd = this.m_indexEnd;
			cslbDLISFrameExportOptions.ChannelsToExport = array;
			cslbDLISFrameExportOptions.NeedOriginNo = nameOptions.NeedOrigin;
			cslbDLISFrameExportOptions.NeedCopyNo = nameOptions.NeedCopy;
			cslbDLISFrameExportOptions.NeedRepCode = nameOptions.NeedRepCode;
			cslbDLISFrameExportOptions.NeedUnit = nameOptions.NeedUnit;
			cslbDLISFrameExportOptions.ExistingOrigins = array2;
			cslbDLISFrameExportOptions.NewOrigins = array3;
			this.m_frame.Export(fileName, cslbDLISFrameExportOptions);
		}

		public bool IsIndexBasedQuery
		{
			get
			{
				return this.m_bConsiderIndex;
			}
			set
			{
				this.m_bConsiderIndex = value;
			}
		}

		public uint RecordNoStart
		{
			get
			{
				return this.m_recNoStart;
			}
			set
			{
				this.m_recNoStart = value;
			}
		}

		public uint RecordNoEnd
		{
			get
			{
				return this.m_recNoEnd;
			}
			set
			{
				this.m_recNoEnd = value;
			}
		}

		public double IndexStart
		{
			get
			{
				return this.m_indexStart;
			}
			set
			{
				this.m_indexStart = value;
			}
		}

		public double IndexEnd
		{
			get
			{
				return this.m_indexEnd;
			}
			set
			{
				this.m_indexEnd = value;
			}
		}

		public ISlbDLISFrame Frame
		{
			get
			{
				return this.m_frame;
			}
		}

		public override uint ItemCount
		{
			get
			{
				if (this.m_noOfFrames == 0U)
				{
					this.Reset();
					int i = 0;
					int num = i;
					this.m_frame.ReadNextFrame(ref i);
					while (i > num)
					{
						num = i;
						this.m_frame.ReadNextFrame(ref i);
					}
					this.m_noOfFrames = (uint)num;
				}
				return this.m_noOfFrames;
			}
		}

		public override uint Size
		{
			get
			{
				bool flag = false;
				uint num = 0U;
				if (this.m_frameliveDataSize == 0U)
				{
					ISlbDLISChannels channels = this.m_frame.Channels;
					int count = channels.Count;
					for (int i = 0; i < count; i++)
					{
						ISlbDLISChannel slbDLISChannel = channels[i];
						RepCode_t repCode = slbDLISChannel.RepCode;
						if (Util.IsVarData(repCode))
						{
							flag = true;
						}
						else
						{
							num += Util.GetDataSize(slbDLISChannel);
						}
					}
					if (this.m_noOfFrames == 0U || flag)
					{
						this.Reset();
						int j = 0;
						int num2 = j;
						this.m_frame.ReadNextFrame(ref j);
						while (j > num2)
						{
							channels = this.m_frame.Channels;
							count = channels.Count;
							for (int k = 0; k < count; k++)
							{
								ISlbDLISChannel slbDLISChannel2 = channels[k];
								if (flag && Util.IsVarData(slbDLISChannel2.RepCode))
								{
									string stringValue = slbDLISChannel2.StringValue;
									uint dataSize = Util.GetDataSize(slbDLISChannel2);
									this.m_frameliveDataSize += dataSize;
									this.m_aChannelSize[k] += dataSize;
								}
							}
							num2 = j;
							this.m_frame.ReadNextFrame(ref j);
						}
						this.m_noOfFrames = (uint)num2;
					}
				}
				this.m_frameliveDataSize += this.m_noOfFrames * num;
				return this.m_frameliveDataSize;
			}
		}

		public override uint SizeOverhead
		{
			get
			{
				return this.ItemCount + this.ItemCount;
			}
		}

		public uint GetChannelSize(int i)
		{
			uint size = this.Size;
			ISlbDLISChannel slbDLISChannel = this.m_frame.Channels[i];
			RepCode_t repCode = slbDLISChannel.RepCode;
			if (Util.IsVarData(repCode))
			{
				return this.m_aChannelSize[i];
			}
			return this.ItemCount * Util.GetDataSize(slbDLISChannel);
		}

		public override string XML
		{
			get
			{
				string text = "<Frame>";
				text = text + "<Name>" + this.m_treeNode.Text + "</Name>";
				text = text + "<IsIndexbasedQuery>" + this.m_bConsiderIndex.ToString() + "</IsIndexbasedQuery>";
				if (this.m_bAllRecords)
				{
					text += "<AllRecords>true</AllRecords>";
				}
				else if (this.m_bConsiderIndex)
				{
					text += "<IndexStart>";
					string str = this.m_indexStart.ToString();
					if (this.m_frame.IndexType == "TIME")
					{
						try
						{
							str = DateTime.FromOADate(this.m_indexStart).ToString("MMM dd, yyyy hh:mm:ss tt");
						}
						catch (ArgumentException)
						{
						}
					}
					text += str;
					text += "</IndexStart>";
					text += "<IndexEnd>";
					str = this.m_indexEnd.ToString();
					if (this.m_frame.IndexType == "TIME")
					{
						try
						{
							str = DateTime.FromOADate(this.m_indexEnd).ToString("MMM dd, yyyy hh:mm:ss tt");
						}
						catch (ArgumentException)
						{
						}
					}
					text += str;
					text += "</IndexEnd>";
				}
				else
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"<RecordNoStart>",
						this.m_recNoStart,
						"</RecordNoStart>"
					});
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						"<RecordNoEnd>",
						this.m_recNoEnd,
						"</RecordNoEnd>"
					});
				}
				TriStateTreeView triStateTreeView = (TriStateTreeView)this.m_treeNode.TreeView;
				foreach (object obj3 in this.m_treeNode.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj3;
					text += "<Channel>";
					text = text + "<Name>" + treeNode.Text + "</Name>";
					text = text + "<Selected>" + (triStateTreeView.GetChecked(treeNode) == TriStateTreeView.CheckState.Checked).ToString() + "</Selected>";
					text += "</Channel>";
				}
				text += "</Frame>";
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
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("./Frames/Frame");
			foreach (object obj2 in xmlNodeList2)
			{
				XmlNode xmlNode3 = (XmlNode)obj2;
				XmlNode xmlNode4 = xmlNode3.SelectSingleNode("./Name");
				if (xmlNode4.InnerText == this.m_treeNode.Text)
				{
					foreach (object obj3 in this.m_treeNode.Nodes)
					{
						TreeNode treeNode = (TreeNode)obj3;
						XmlNodeList xmlNodeList3 = xmlNode3.SelectNodes("./Channel");
						foreach (object obj4 in xmlNodeList3)
						{
							XmlNode xmlNode5 = (XmlNode)obj4;
							if (xmlNode5.SelectSingleNode("./Name").InnerText == treeNode.Text && xmlNode5.SelectSingleNode("./Selected").InnerXml == "True")
							{
								TriStateTreeView triStateTreeView = (TriStateTreeView)treeNode.TreeView;
								triStateTreeView.SetChecked(treeNode, TriStateTreeView.CheckState.Checked);
								break;
							}
						}
					}
					XmlNode xmlNode6 = xmlNode3.SelectSingleNode("./RecordNoStart");
					if (xmlNode6 != null)
					{
						Util.ValidateUINT(xmlNode6.InnerText, ref this.m_recNoStart);
					}
					xmlNode6 = xmlNode3.SelectSingleNode("./RecordNoEnd");
					if (xmlNode6 != null)
					{
						Util.ValidateUINT(xmlNode6.InnerText, ref this.m_recNoEnd);
					}
					xmlNode6 = xmlNode3.SelectSingleNode("./IndexStart");
					if (xmlNode6 != null)
					{
						if (this.m_frame.IndexType != "TIME")
						{
							this.m_bConsiderIndex = Util.ValidateDOUBLE(xmlNode6.InnerText, ref this.m_indexStart);
						}
						else
						{
							try
							{
								this.m_indexStart = DateTime.Parse(xmlNode6.InnerText).ToOADate();
								this.m_bConsiderIndex = true;
							}
							catch (FormatException)
							{
								this.m_bConsiderIndex = false;
							}
						}
					}
					xmlNode6 = xmlNode3.SelectSingleNode("./IndexEnd");
					if (xmlNode6 != null)
					{
						if (this.m_frame.IndexType != "TIME")
						{
							this.m_bConsiderIndex = (this.m_bConsiderIndex && Util.ValidateDOUBLE(xmlNode6.InnerText, ref this.m_indexEnd));
						}
						else
						{
							try
							{
								this.m_indexEnd = DateTime.Parse(xmlNode6.InnerText).ToOADate();
							}
							catch (FormatException)
							{
								this.m_bConsiderIndex = false;
							}
						}
					}
					xmlNode6 = xmlNode3.SelectSingleNode("./AllRecords");
					if (xmlNode6 != null && xmlNode6.InnerText == "true")
					{
						this.m_bAllRecords = true;
					}
				}
			}
		}

		private ISlbDLISFrame m_frame;

		private bool m_bAllRecords;

		private bool m_bConsiderIndex;

		private uint m_recNoStart;

		private uint m_recNoEnd;

		private double m_indexStart;

		private double m_indexEnd;

		private uint m_noOfFrames;

		private uint m_frameliveDataSize;

		private uint[] m_aChannelSize;

		private ISlbDLISChannel[] m_Channels;

		private int iCountChannels;
	}
}
