using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	public enum ShapeTypes { Plane, Sphere, Cylinder, Cone, Torus };

	public class DirectShapeBase
	{
		#region Variables
		protected string m_name=string.Empty;
		protected ElementId m_element_id;
		protected Document m_document;
		protected ShapeTypes m_shape_type;
		#endregion

		#region Properties
		public string Name { get { return m_name; } }
		public ElementId ElementId { get { return m_element_id; } }
		public Document Document { get { return m_document; } }
		public ShapeTypes ShapeType { get { return m_shape_type; } }
		#endregion

		#region Methods
		public void Hide() => SetVisible( false );
		public void Unhide() => SetVisible( true );
		public void Delete()
		{
			if( GetShape()==null ) return;

			using( Transaction t = new Transaction( Document, m_name ) )
			{
				t.Start();
				Document.Delete( ElementId );
				t.Commit();
			}
		}
		#endregion

		#region Implementation
		protected DirectShapeBase( Document doc, string name )
		{
			m_document=doc;
			m_name=name;
		}

		private void SetVisible( bool visible )
		{
			if( GetShape()==null ) return;

			using( Transaction t = new Transaction( Document, m_name ) )
			{
				t.Start();
				if( visible )	Document.ActiveView.UnhideElements( new ElementId[] { ElementId } );
				else			Document.ActiveView.HideElements( new ElementId[] { ElementId } );
				t.Commit();
			}
		}

		private DirectShape GetShape()
		{
			FilteredElementCollector coll=new FilteredElementCollector( Document );
			List<DirectShape> shapes=coll.OfClass( typeof( DirectShape ) )
				.ToElements().Cast<DirectShape>().ToList();
			foreach( DirectShape shape in shapes )
			{
				if( shape.Name==Name ) return shape;
			}

			return null;
		}
		#endregion
	}
}
