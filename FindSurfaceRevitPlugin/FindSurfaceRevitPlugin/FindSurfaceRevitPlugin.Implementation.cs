using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

using CurvSurf;

namespace FindSurfaceRevitPlugin
{
	public static partial class FindSurfaceRevitPlugin
	{
		#region Variables
		private static FindSurface s_find_surface=null;
		private static UniformGridPointCloudEngine s_outlier_point_cloud_engine=null;
		private static OctreePointCloudEngine s_inlier_point_cloud_engine=null;
		private static DirectShapeEngine s_direct_shape_manager=null;

		private static string s_file_full_name=string.Empty;
		private static float[] s_xyz=null;
		private static int[] s_color=null;
		private static int s_subdivision=0;
		public static Units s_measurementUnit=Units.None;

		private static bool s_temporary_outlier_deletion=false;
		#endregion

		#region Constant
		private static string S_OUTLIER_PC_ENGINE_IDENTIFIER="xyz-outlier";
		private static string S_INLIER_PC_ENGINE_IDENTIFIER="xyz-inlier";
		private static readonly int S_PLANE_INLIER_COLOR = 0x00e6194b;
		private static readonly int S_SPHERE_INLIER_COLOR = 0x003cb44b;
		private static readonly int S_CYLINDER_INLIER_COLOR = 0x000082c8;
		private static readonly int S_CONE_INLIER_COLOR = 0x00f58231;
		private static readonly int S_TORUS_INLIER_COLOR = 0x00f032e6;
		#endregion

		#region Implementation
		private static void PopupInitialMessage()
		{
			System.Windows.Forms.MessageBox.Show( 
				"This plugin is designed as an example demonstrating what FindSurface can do."+
				"It is not recommended to reuse this plugin for practical uses.",
				"FindSurfaceRevitPlugin"
				);
		}

		/// <summary>
		/// Shows a warning message.
		/// This method is designed to be called when users explicitly deleted the point cloud
		/// that corresponds to the one that is used by FindSurface. 
		/// (e.g., pressing the [delete] button after clicking the point cloud)
		/// </summary>
		private static void PromptOutlierDeletedWarning()
		{
			if( s_temporary_outlier_deletion==false )
			{
				TaskDialog.Show( "Outlier deleted!",
					"User deleted the outlier point cloud."+Environment.NewLine+
					"To use the features in Find shapes tab:"+Environment.NewLine+
					"- Click [Reset] button to restore the initial state."+Environment.NewLine+
					"- Click [Open] button to import another point cloud file." );

				FindSurfaceRevitPluginUI.GetRibbonPanel( FindSurfaceRevitPluginUI.RibbonPanelFindSurfaceName ).Enabled=false;
			}
			s_temporary_outlier_deletion=false;
		}

		private static void GenerateRay( UIDocument uiDoc, out XYZ ray_origin, out XYZ ray_direction )
		{
			ray_origin=XYZ.Zero;
			ray_direction=XYZ.Zero;

			XYZ eye = uiDoc.ActiveView.Origin;
			ray_direction=-uiDoc.ActiveView.ViewDirection;

			Plane view_plane = Plane.CreateByNormalAndOrigin(ray_direction, eye);
			using( Transaction t = new Transaction( uiDoc.Document ) )
			{
				t.Start( "Temporarily set work plane to pick point in 3D." );

				SketchPlane view_sketchplane = SketchPlane.Create(uiDoc.Document, view_plane);
				uiDoc.ActiveView.SketchPlane=view_sketchplane;
				uiDoc.ActiveView.ShowActiveWorkPlane();
				try
				{
					ray_origin=uiDoc.Selection.PickPoint( "Please pick a point." );
				}
				catch( Autodesk.Revit.Exceptions.OperationCanceledException e ) { throw e; }
				finally { t.RollBack(); }
			}
		}

		private static Result OnFindCommand( UIDocument ui_doc,
			FS_FEATURE_TYPE feature_type, string command_name, string shape_name,
			Action<Document, FS_FEATURE_RESULT?> post_action )
		{
			uint index;

			XYZ ray_origin=XYZ.Zero, ray_direction=XYZ.Zero;
			try
			{
				GenerateRay( ui_doc, out ray_origin, out ray_direction );
			}
			catch( Autodesk.Revit.Exceptions.OperationCanceledException ) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if( OutlierPointCloudEngine.Cast( ray_origin, ray_direction, 0.01f, out index )==false )
			{
				TaskDialog.Show( command_name, "There is no points picked." );
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindSurface( feature_type, index );
			if( result==null )
			{
				TaskDialog.Show( command_name, $"FindSurface failed to find a {shape_name}." );
				return Result.Failed;
			}

			post_action( ui_doc.Document, result );

			return Result.Succeeded;
		}

		private static void DrawPlaneAndInliers( Document doc, FS_FEATURE_RESULT? result )
		{
			// fetch parameters of the plane found by FindSurface.
			XYZ top_left, top_right, bottom_left, bottom_right;
			FetchParametersOfPlane( result, out bottom_left, out bottom_right, out top_left, out top_right );

			// create the inlier point cloud corresponding to the plane.
			string name="Plane"+DirectShapeEngine.DirectPlaneInstanceCount;
			ExtractInlierPointCloud( doc, name, S_PLANE_INLIER_COLOR );

			// draw the plane on the Revit document.
			DirectShapeEngine.DrawPlane( doc, top_left, top_right, bottom_left, bottom_right );
		}

		private static void DrawSphereAndInliers( Document doc, FS_FEATURE_RESULT? result )
		{
			// fetch parameters of the sphere found by FindSurface.
			XYZ center;
			double radius;
			FetchParametersOfSphere( result, out center, out radius );

			// create the inlier point cloud corresponding to the sphere.
			string name = "Sphere"+DirectShapeEngine.DirectSphereInstanceCount;
			ExtractInlierPointCloud( doc, name, S_SPHERE_INLIER_COLOR );

			// draw the sphere on the Revit document.
			DirectShapeEngine.DrawSphere( doc, center, radius );
		}

		private static void DrawCylinderAndInliers( Document doc, FS_FEATURE_RESULT? result )
		{
			// fetch parameters of the cylinder found by FindSurface.
			XYZ top, bottom;
			double radius;
			FetchParametersOfCylinder( result, out top, out bottom, out radius );

			// create the inlier point cloud corresponding to the cylinder.
			string name = "Cylinder"+DirectShapeEngine.DirectCylinderInstanceCount;
			ExtractInlierPointCloud( doc, name, S_CYLINDER_INLIER_COLOR );

			// draw the cylinder on the Revit document.
			DirectShapeEngine.DrawCylinder( doc, top, bottom, radius );
		}

		private static void DrawConeAndInliers( Document doc, FS_FEATURE_RESULT? result )
		{
			// fetch parameters of the cone found by FindSurface.
			XYZ top, bottom;
			double top_radius, bottom_radius;
			FetchParametersOfCone( result, out top, out bottom, out top_radius, out bottom_radius );

			// create the inlier point cloud corresponding to the cone.
			string name = "Cone"+DirectShapeEngine.DirectConeInstanceCount;
			ExtractInlierPointCloud( doc, name, S_CONE_INLIER_COLOR );

			// draw the cone on the Revit document.
			DirectShapeEngine.DrawCone( doc, top, bottom, top_radius, bottom_radius );
		}
		
		private static void DrawTorusAndInliers( Document doc, FS_FEATURE_RESULT? result )
		{
			// fetch parameters of the torus found by FindSurface.
			XYZ center, axis;
			double mean_radius, tube_radius;
			FetchParametersOfTorus( result, out center, out axis, out mean_radius, out tube_radius );

			// create the inlier point cloud corresponding to the torus.
			string name = "Torus"+DirectShapeEngine.DirectTorusInstanceCount;
			ExtractInlierPointCloud( doc, name, S_TORUS_INLIER_COLOR );

			// find two edges of the torus part.
			XYZ tube_begin, tube_end;

			string last_pointcloud_name = InlierPointCloudEngine.GetPointCloudNames().Last();
			XYZ[] inlier_points = InlierPointCloudEngine.GetCloudPoints( last_pointcloud_name ).ToList().ConvertAll(x=>(XYZ)x).ToArray();

			CalculateTubeEdges( inlier_points, center, axis, out tube_begin, out tube_end );
			double positive_angle=FindSurfaceRevitPluginUtils.GetPositiveAngleBetween(tube_begin, tube_end, tube_begin.CrossProduct(tube_end) );

			// draw the torus on the Revit document.
			DirectShapeEngine.DrawTorus( doc, center, axis, mean_radius, tube_radius, tube_begin, positive_angle );
		}

		private static void CalculateTubeEdges( 
			XYZ[] points_on_tube_surface, 
			XYZ center_of_torus, 
			XYZ axis_of_torus, 
			out XYZ tube_begin, 
			out XYZ tube_end )
		{
			// make vectors pointing from the center to the points on the surface.
			List<XYZ> vectors = points_on_tube_surface.ToList().ConvertAll(x=>x-center_of_torus);

			// project all points on the tube surface to the plane perpendicular to the axis of the torus.
			List<XYZ> projected_vectors=
				vectors.ConvertAll(
					x=>axis_of_torus.CrossProduct(x).CrossProduct(axis_of_torus)
				);

			// calculate barycentric coordinates of the projected points.
			XYZ barycentric=projected_vectors.Aggregate( (p0,p1)=>p0+p1 ) /projected_vectors.Count;
			
			// tube_middle is a vector pointing from the center of the torus to the center of mass.
			XYZ tube_middle=barycentric.Normalize();

			// find the elements to which a vector pointing from the center of torus has min/max angles to the tube_middle.
			FindSurfaceRevitPluginUtils.MinMaxElement( projected_vectors, x => FindSurfaceRevitPluginUtils.GetAngleBetween( tube_middle, x.Normalize(), axis_of_torus ), out tube_begin, out tube_end );
		}

		private static void FetchParametersOfPlane( FS_FEATURE_RESULT? result, out XYZ bottom_left, out XYZ bottom_right, out XYZ top_left, out XYZ top_right )
		{
			// get transform of the point clouds.
			Transform t=OutlierPointCloudEngine.GetPointCloudTransform();

			// fetch parameters.
			FS_PLANE_PARAMS p = result.Value.GetParamAsPlane();
			System.Numerics.Vector3 lower_left = p.LowerLeft;
			System.Numerics.Vector3 lower_right = p.LowerRight;
			System.Numerics.Vector3 upper_left = p.UpperLeft;
			System.Numerics.Vector3 upper_right = p.UpperRight;

			// convert the parameters to the type Revit uses.
			bottom_left=t.OfPoint( new XYZ( lower_left.X, lower_left.Y, lower_left.Z ) );
			bottom_right=t.OfPoint( new XYZ( lower_right.X, lower_right.Y, lower_right.Z ) );
			top_left=t.OfPoint( new XYZ( upper_left.X, upper_left.Y, upper_left.Z ) );
			top_right=t.OfPoint( new XYZ( upper_right.X, upper_right.Y, upper_right.Z ) );
		}

		private static void FetchParametersOfSphere( FS_FEATURE_RESULT? result, out XYZ center, out double radius )
		{
			// get transform of the point clouds.
			Transform t=OutlierPointCloudEngine.GetPointCloudTransform();

			// fetch parameters.
			FS_SPHERE_PARAMS p = result.Value.GetParamAsSphere();
			System.Numerics.Vector3 c = p.Center;
			float r=p.Radius;

			// convert the parameters to the type Revit uses.
			center=t.OfPoint( new XYZ( c.X, c.Y, c.Z ) );
			radius=r;
		}

		private static void FetchParametersOfCylinder( FS_FEATURE_RESULT? result, out XYZ top, out XYZ bottom, out double radius )
		{
			// get transform of the point clouds.
			Transform t=OutlierPointCloudEngine.GetPointCloudTransform();

			// fetch parameters.
			FS_CYLINDER_PARAMS p = result.Value.GetParamAsCylinder();
			System.Numerics.Vector3 tc = p.Top;
			System.Numerics.Vector3 bc = p.Bottom;
			float r = p.Radius;

			// convert the parameters to the type Revit uses.
			top=t.OfPoint( new XYZ( tc.X, tc.Y, tc.Z ) );
			bottom=t.OfPoint( new XYZ( bc.X, bc.Y, bc.Z ) );
			radius=r;
		}

		private static void FetchParametersOfCone( FS_FEATURE_RESULT? result, out XYZ top, out XYZ bottom, out double top_radius, out double bottom_radius )
		{
			// get transform of the point clouds.
			Transform t=OutlierPointCloudEngine.GetPointCloudTransform();

			// fetch parameters.
			FS_CONE_PARAMS p = result.Value.GetParamAsCone();
			System.Numerics.Vector3 tc = p.Top;
			System.Numerics.Vector3 bc = p.Bottom;
			float tr = p.TopRadius;
			float br = p.BottomRadius;

			// convert the parameters to the type Revit uses.
			top=t.OfPoint( new XYZ( tc.X, tc.Y, tc.Z ) );
			bottom=t.OfPoint( new XYZ( bc.X, bc.Y, bc.Z ) );
			top_radius=tr;
			bottom_radius=br;
		}

		private static void FetchParametersOfTorus( FS_FEATURE_RESULT? result, out XYZ center, out XYZ axis, out double mean_radius, out double tube_radius )
		{
			// get transform of the point clouds.
			Transform t=OutlierPointCloudEngine.GetPointCloudTransform();

			// fetch parameters.
			FS_TORUS_PARAMS p = result.Value.GetParamAsTorus();
			System.Numerics.Vector3 c = p.Center;
			System.Numerics.Vector3 a = p.Normal;
			float mr = p.MeanRadius;
			float tr = p.TubeRadius;

			// convert the parameters to the type Revit uses.
			center=t.OfPoint( new XYZ( c.X, c.Y, c.Z ) );
			axis=t.OfVector( new XYZ( a.X, a.Y, a.Z ) ).Normalize();
			mean_radius=mr;
			tube_radius=tr;
		}

		private static void ExtractInlierPointCloud( Document doc, string inlier_identifier, int highlight_color )
		{
			CloudPoint[] original_points=OutlierPointCloudEngine.GetCloudPoint();

			List<CloudPoint> inlier_point_list=new List<CloudPoint>();
			List<CloudPoint> outlier_point_list=new List<CloudPoint>();
			List<float> outlier_xyz=new List<float>();

			// this is a flag array of which each element is set when the point of its corresponding location is in outlier.
			bool[] outlier_flags = FindSurface.GetInOutlierFlags();
			for( int k = 0;k<outlier_flags.Length;k++ )
			{
				float x=original_points[k].X;
				float y=original_points[k].Y;
				float z=original_points[k].Z;
				int color=original_points[k].Color;

				if( outlier_flags[k] ) // outlier
				{
					outlier_point_list.Add( new CloudPoint( x, y, z, color ) );
					outlier_xyz.AddRange( new float[] { x, y, z } );
				}
				else // inlier
				{
					color=FindSurfaceRevitPluginUtils.MixARGB( color, highlight_color );
					inlier_point_list.Add( new CloudPoint( x, y, z, color ) );
				}
			}

			Transform outlier_transform=OutlierPointCloudEngine.GetPointCloudTransform();

			// replace outlier (or original one) to new outlier points.
			s_temporary_outlier_deletion=true;
			OutlierPointCloudEngine.CreatePointCloud( doc, "Outlier", outlier_point_list.ToArray(), outlier_transform, s_subdivision );

			// give new outlier points to FindSurface.
			FindSurface.SetPointCloud( outlier_xyz.ToArray() );

			// create inlier point cloud.
			InlierPointCloudEngine.CreatePointCloud( doc, inlier_identifier, inlier_point_list.ToArray(), outlier_transform );
		}
		#endregion
	}
}
