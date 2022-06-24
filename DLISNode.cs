using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace DLISEditor
{
	public abstract class DLISNode
	{
		public DLISNode(TreeNode treeNode)
		{
			this.m_treeNode = treeNode;
		}

		public abstract DataTable CreateDataTable(NameOptions nameOptions);

		public abstract string GetSetType();

		public abstract string GetColumnName(int Index);

		public abstract bool GetRow(out string[] rowVal, ref int rowIndex, NameOptions nameOptions);

		public virtual bool NeedsToDisplay(int row)
		{
			return true;
		}

		public virtual int StartRowNumber()
		{
			return 0;
		}

		public abstract void Export(string fileName, NameOptions nameOptions, bool bHeaderInfo);

		public abstract void LoadSettings(XmlDocument doc);

		public TreeNode TreeNode
		{
			get
			{
				return this.m_treeNode;
			}
		}

		public int ColumnCount
		{
			get
			{
				return this.m_columnCount;
			}
		}

		public abstract string HeaderInfo { get; }

		public abstract string XML { get; }

		public abstract uint Size { get; }

		public abstract uint ItemCount { get; }

		public abstract uint SizeOverhead { get; }

		protected TreeNode m_treeNode;

		protected int m_columnCount;
	}
}
