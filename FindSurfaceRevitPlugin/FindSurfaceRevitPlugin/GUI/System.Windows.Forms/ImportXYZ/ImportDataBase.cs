using System;
using System.IO;
using System.Reflection;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	public class ImportDataBase
	{
		#region Class Member Variables
		protected Document m_active_doc;
		protected string m_import_folder;
		protected string m_import_file_name;
		protected string m_import_file_full_name;
		protected string m_filter;
		protected string m_title;
		#endregion

		#region Class Properties
		public Document ActiveDocument { get { return m_active_doc; } }
		public string ImportFileFullName { get { return m_import_file_full_name; } set { m_import_file_full_name=value; } }
		public string ImportFileName { get { return m_import_file_name; } set { m_import_file_name=value; } }
		public string Filter { get { return m_filter; } }
		public string ImportFolder { get { return m_import_folder; } set { m_import_folder=value; } }
		public string Title { get { return m_title; } }
		#endregion

		#region Class Member Methods
		public ImportDataBase( Document document )
		{
			m_active_doc=document;
			m_filter=String.Empty;
			Initialize();
		}

		private void Initialize()
		{
			String dll_file_path = Assembly.GetExecutingAssembly().Location;
			m_import_folder=Path.GetDirectoryName( dll_file_path );
			m_import_file_full_name=String.Empty;
		}

		public virtual void Import()
		{
			if( m_import_file_full_name==null ) throw new NullReferenceException();
		}
		#endregion
	}
}
