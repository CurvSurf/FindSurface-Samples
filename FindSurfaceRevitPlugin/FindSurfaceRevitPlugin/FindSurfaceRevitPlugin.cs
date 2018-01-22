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
		/// <summary>
		/// The instance of FindSurface
		/// </summary>
		public static FindSurface FindSurface { get { return s_find_surface; } }

		/// <summary>
		/// The instance of UniformGridPointCloudEngine
		/// </summary>
		public static UniformGridPointCloudEngine OutlierPointCloudEngine { get { return s_outlier_point_cloud_engine; } }

		/// <summary>
		/// The instance of InlierPointCloudEngine
		/// </summary>
		public static OctreePointCloudEngine InlierPointCloudEngine { get { return s_inlier_point_cloud_engine; } }

		/// <summary>
		/// The instance of CustomDirectShape
		/// </summary>
		public static DirectShapeEngine DirectShapeEngine { get { return s_direct_shape_manager; } }

		/// <summary>
		/// The measurement basic unit of when displaying information (e.g., length, height)
		/// </summary>
		public static Units MeasurementUnit { get { return s_measurementUnit; } set { s_measurementUnit=value; } }

		/// <summary>
		/// Initializes FindSurface, custom point cloud engines, DirectShapeManager instances when Revit loaded this plugin.
		/// </summary>
		public static void OnPluginStartUp()
		{
			// initialize FindSurface
			s_find_surface=FindSurface.GetInstance();

			// initialize parameters temporarily
			FindSurface.Accuracy=0.003f;      // Set Sensor Measurement Accuracy 
			FindSurface.MeanDistance=0.01f;  // Set Mean Distance of Neighboring Points
			FindSurface.TouchRadius=0.025f;   // Touch Size of the Seed Region

			// initialize Custom Point Cloud Engines
			s_outlier_point_cloud_engine=new UniformGridPointCloudEngine( S_OUTLIER_PC_ENGINE_IDENTIFIER );
			PointCloudEngineRegistry.RegisterPointCloudEngine( S_OUTLIER_PC_ENGINE_IDENTIFIER, s_outlier_point_cloud_engine, false );
			s_inlier_point_cloud_engine=new OctreePointCloudEngine( S_INLIER_PC_ENGINE_IDENTIFIER );
			PointCloudEngineRegistry.RegisterPointCloudEngine( S_INLIER_PC_ENGINE_IDENTIFIER, s_inlier_point_cloud_engine, false );

			// initialize DirectShape Engines
			s_direct_shape_manager=new DirectShapeEngine();
		}

		/// <summary>
		/// Cleans up FindSurface, custom point cloud engines, DirectShapeManager instances before Revit is terminated.
		/// </summary>
		public static void OnPluginShutdown()
		{
			// clean up FindSurface
			FindSurface.CleanUp();

			// clean up XYZPointCloudEngine
			List<string> engine_names = PointCloudEngineRegistry.GetSupportedEngines() as List<string>;
			foreach( string engine_name in engine_names )
			{
				if( engine_name==S_OUTLIER_PC_ENGINE_IDENTIFIER||engine_name==S_INLIER_PC_ENGINE_IDENTIFIER )
					PointCloudEngineRegistry.UnregisterPointCloudEngine( engine_name );
			}
		}
		
		/// <summary>
		/// Hides or unhides objects according to the check states of checked list boxes in View List tool window.
		/// </summary>
		public static void OnPluginIdling()
		{
			// check if there is a visibility change of point clouds in ViewListForm.
			foreach( KeyValuePair<string, ObjectVisibility> pair in ViewListForm.GetPointCloudsVisibilityChanged() )
			{
				string point_cloud_identifier = pair.Key;
				ObjectVisibility point_cloud_visibility = pair.Value;

				if( OutlierPointCloudEngine.ContainPointCloud( point_cloud_identifier ) )
				{
					if( point_cloud_visibility.Visible ) OutlierPointCloudEngine.UnhidePointCloud();
					else OutlierPointCloudEngine.HidePointCloud();
				}
				else if( InlierPointCloudEngine.ContainPointCloud( point_cloud_identifier ) )
				{
					if( point_cloud_visibility.Visible ) InlierPointCloudEngine.UnhidePointCloud( point_cloud_identifier );
					else InlierPointCloudEngine.HidePointCloud( point_cloud_identifier );
				}
				else continue;

				point_cloud_visibility.Expire();
			}

			// check if there are visibility changes of DirectShape instances in ViewListForm.
			foreach( KeyValuePair<string, ObjectVisibility> shape in ViewListForm.GetShapesVisibilityChanged() )
			{
				if( shape.Value.Visible ) DirectShapeEngine.UnhideDirectShape( shape.Key );
				else DirectShapeEngine.HideDirectShape( shape.Key );

				shape.Value.Expire();
			}
		}

		/// <summary>
		/// Removes all the stuffs this plugin made when Revit closed the document.
		/// </summary>
		/// <param name="e">The arguments of the event</param>
		public static void OnDocumentClosing( Autodesk.Revit.DB.Events.DocumentClosingEventArgs e )
		{
			OutlierPointCloudEngine.RemovePointCloud();
			InlierPointCloudEngine.RemoveAllPointClouds();
			DirectShapeEngine.RemoveAllDirectShapes();
		}

		/// <summary>
		/// Deletes the instance wrapper of point clouds and direct shapes when they were deleted by users in Revit document.
		/// </summary>
		/// <param name="e">The arguments of the event</param>
		public static void OnContentChanged( Autodesk.Revit.DB.Events.DocumentChangedEventArgs e )
		{
			// when there are deleted objects
			foreach( ElementId eid in e.GetDeletedElementIds() )
			{
				// if the deleted object is PointCloud type
				if( OutlierPointCloudEngine.ContainPointCloud( eid ) )
				{
					PromptOutlierDeletedWarning();
					OutlierPointCloudEngine.RemovePointCloud();
				}
				else if( InlierPointCloudEngine.ContainPointCloud( eid ) )
				{
					InlierPointCloudEngine.RemovePointCloud( eid );
				}
				else if( DirectShapeEngine.ContainDirectShape( eid ) )
				{
					DirectShapeEngine.RemoveDirectShape( eid );
				}
			}
		}

		/// <summary>
		/// Creates a point cloud and send the data to FindSurface when users imported a point cloud file using Open button in FindSurface tab.
		/// </summary>
		/// <param name="doc">The active document that Revit uses</param>
		/// <param name="file_full_name">The name of imported file</param>
		/// <param name="xyz_array">Imported xyz data</param>
		/// <param name="color_array">Imported color data</param>
		/// <param name="subdivision">Subdivision number</param>
		/// <param name="unit">The base unit of when the inspector displays information</param>
		public static void OnOpenCommand( Document doc, string file_full_name, float[] xyz_array, int[] color_array, int subdivision, Units unit )
		{
			s_file_full_name=file_full_name;
			s_xyz=xyz_array;
			s_color=color_array;
			s_subdivision=subdivision;
			s_measurementUnit=unit;

			CloudPoint[] points;
			FindSurfaceRevitPluginUtils.Pack( xyz_array, color_array, out points );
			s_temporary_outlier_deletion=true;
			OutlierPointCloudEngine.CreatePointCloud( doc, file_full_name, points, Transform.Identity, subdivision );

			FindSurface.SetPointCloud( xyz_array );
		}

		/// <summary>
		/// Cleans up the stuff so that the plugin set to the initial state 
		/// as if it is loaded at first.
		/// </summary>
		public static void OnCleanUpCommand()
		{
			// remove all point cloud instances
			s_temporary_outlier_deletion=true;
			OutlierPointCloudEngine.RemovePointCloud();
			InlierPointCloudEngine.RemoveAllPointClouds();

			DirectShapeEngine.RemoveAllDirectShapes();

			OnPluginShutdown();
			OnPluginStartUp();
		}

		/// <summary>
		/// Resets the plugin to the state of when users imported a point cloud file.
		/// </summary>
		/// <returns>false if any point cloud file has been opened or true otherwise</returns>
		public static bool OnResetCommand( Document document )
		{
			OnCleanUpCommand();

			CloudPoint[] points;
			if( s_xyz==null||s_xyz.Length==0 ) return false;
			FindSurfaceRevitPluginUtils.Pack( s_xyz, s_color, out points );
			OutlierPointCloudEngine.CreatePointCloud( document, s_file_full_name, points, Transform.Identity, s_subdivision );

			FindSurface.SetPointCloud( s_xyz );
			return true;
		}

		/// <summary>
		/// Runs the command "Find Plane".
		/// It is the action of the button "Plane" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindPlaneCommand( UIDocument ui_doc )
		{ 
			return OnFindCommand( ui_doc, FS_FEATURE_TYPE.FS_TYPE_PLANE, "FindPlane", "plane", DrawPlaneAndInliers );
		}

		/// <summary>
		/// Runs the command "Find Sphere".
		/// It is the action of the button "Sphere" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindSphereCommand( UIDocument ui_doc )
		{ 
			return OnFindCommand( ui_doc, FS_FEATURE_TYPE.FS_TYPE_SPHERE, "FindSphere", "sphere", DrawSphereAndInliers );
		}

		/// <summary>
		/// Runs the command "Find Cylinder".
		/// It is the action of the button "Cylinder" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindCylinderCommand( UIDocument ui_doc )
		{
			return OnFindCommand( ui_doc, FS_FEATURE_TYPE.FS_TYPE_CYLINDER, "FindCylinder", "cylinder", DrawCylinderAndInliers );
		}

		/// <summary>
		/// Runs the command "Find Cone".
		/// It is the action of the button "Cone" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindConeCommand( UIDocument ui_doc )
		{ 
			return OnFindCommand( ui_doc, 
				FS_FEATURE_TYPE.FS_TYPE_CONE, "FindCone", "cone", 
				( document, result ) =>
			{
				switch( result.Value.type )
				{
					case FS_FEATURE_TYPE.FS_TYPE_CONE: DrawConeAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_CYLINDER: DrawCylinderAndInliers( document, result ); break;
				}
			} );
		}

		/// <summary>
		/// Runs the command "Find Torus".
		/// It is the action of the button "Torus" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindTorusCommand( UIDocument ui_doc )
		{ 
			return OnFindCommand( ui_doc, 
				FS_FEATURE_TYPE.FS_TYPE_TORUS, "FindTorus", "torus", 
				( document, result ) =>
			{
				switch( result.Value.type )
				{
					case FS_FEATURE_TYPE.FS_TYPE_TORUS: DrawTorusAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_CYLINDER: DrawCylinderAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_SPHERE: DrawSphereAndInliers( document, result ); break;
				}
			} );
		}

		/// <summary>
		/// Runs the command "Find Any Shapes".
		/// It is the action of the button "Any Shape" in the "Find Shapes" panel.
		/// </summary>
		/// <param name="ui_doc">The UI document</param>
		/// <returns>Revit command result</returns>
		public static Result OnFindAnyShapeCommand( UIDocument ui_doc )
		{ 
			return OnFindCommand( ui_doc, 
				FS_FEATURE_TYPE.FS_TYPE_ANY, "FindAnyShape", "any shape", 
				( document, result ) =>
			{
				switch( result.Value.type )
				{
					case FS_FEATURE_TYPE.FS_TYPE_PLANE: DrawPlaneAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_SPHERE: DrawSphereAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_CYLINDER: DrawCylinderAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_CONE: DrawConeAndInliers( document, result ); break;
					case FS_FEATURE_TYPE.FS_TYPE_TORUS: DrawTorusAndInliers( document, result ); break;
				}
			} );
		}

		public static Result OnFindStripPlaneCommand( UIDocument ui_doc )
		{
			uint index0, index1;

			XYZ ray0_origin=XYZ.Zero, ray0_direction=XYZ.Zero;
			XYZ ray1_origin=XYZ.Zero, ray1_direction=XYZ.Zero;
			try
			{
				GenerateRay(ui_doc, out ray0_origin, out ray0_direction);
				GenerateRay(ui_doc, out ray1_origin, out ray1_direction);
			}
			catch(Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if(OutlierPointCloudEngine.Cast(ray0_origin, ray0_direction, 0.01f, out index0)==false)
			{
				TaskDialog.Show("FindStripPlane", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray1_origin, ray1_direction, 0.01f, out index1)==false)
			{
				TaskDialog.Show("FindStripPlane", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindStripPlane( index0, index1 );
			if(result==null)
			{
				TaskDialog.Show("FindStripPlane", $"FindSurface failed to find a strip plane.");
				return Result.Failed;
			}

			DrawPlaneAndInliers(ui_doc.Document, result);

			return Result.Succeeded;
		}

		public static Result OnFindRodCylinderCommand( UIDocument ui_doc )
		{
			uint index0, index1;

			XYZ ray0_origin=XYZ.Zero, ray0_direction=XYZ.Zero;
			XYZ ray1_origin=XYZ.Zero, ray1_direction=XYZ.Zero;
			try
			{
				GenerateRay(ui_doc, out ray0_origin, out ray0_direction);
				GenerateRay(ui_doc, out ray1_origin, out ray1_direction);
			}
			catch(Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if(OutlierPointCloudEngine.Cast(ray0_origin, ray0_direction, 0.01f, out index0)==false)
			{
				TaskDialog.Show("FindRodCylinder", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray1_origin, ray1_direction, 0.01f, out index1)==false)
			{
				TaskDialog.Show("FindRodCylinder", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindRodCylinder( index0, index1 );
			if(result==null)
			{
				TaskDialog.Show("FindRodCylinder", $"FindSurface failed to find a rod cylinder.");
				return Result.Failed;
			}

			DrawCylinderAndInliers(ui_doc.Document, result);

			return Result.Succeeded;
		}

		public static Result OnFindDiskCylinderCommand( UIDocument ui_doc )
		{
			uint index0, index1, index2;

			XYZ ray0_origin=XYZ.Zero, ray0_direction=XYZ.Zero;
			XYZ ray1_origin=XYZ.Zero, ray1_direction=XYZ.Zero;
			XYZ ray2_origin=XYZ.Zero, ray2_direction=XYZ.Zero;
			try
			{
				GenerateRay(ui_doc, out ray0_origin, out ray0_direction);
				GenerateRay(ui_doc, out ray1_origin, out ray1_direction);
				GenerateRay(ui_doc, out ray2_origin, out ray2_direction);
			}
			catch(Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if(OutlierPointCloudEngine.Cast(ray0_origin, ray0_direction, 0.01f, out index0)==false)
			{
				TaskDialog.Show("FindDiskCylinder", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray1_origin, ray1_direction, 0.01f, out index1)==false)
			{
				TaskDialog.Show("FindDiskCylinder", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray2_origin, ray2_direction, 0.01f, out index2)==false)
			{
				TaskDialog.Show("FindDiskCylinder", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindDiskCylinder( index0, index1, index2 );
			if(result==null)
			{
				TaskDialog.Show("FindDiskCylinder", $"FindSurface failed to find a disk cylinder.");
				return Result.Failed;
			}

			DrawCylinderAndInliers(ui_doc.Document, result);

			return Result.Succeeded;
		}

		public static Result OnFindDiskConeCommand(UIDocument ui_doc)
		{
			uint index0, index1, index2;

			XYZ ray0_origin=XYZ.Zero, ray0_direction=XYZ.Zero;
			XYZ ray1_origin=XYZ.Zero, ray1_direction=XYZ.Zero;
			XYZ ray2_origin=XYZ.Zero, ray2_direction=XYZ.Zero;
			try
			{
				GenerateRay(ui_doc, out ray0_origin, out ray0_direction);
				GenerateRay(ui_doc, out ray1_origin, out ray1_direction);
				GenerateRay(ui_doc, out ray2_origin, out ray2_direction);
			}
			catch(Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if(OutlierPointCloudEngine.Cast(ray0_origin, ray0_direction, 0.01f, out index0)==false)
			{
				TaskDialog.Show("FindDiskCone", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray1_origin, ray1_direction, 0.01f, out index1)==false)
			{
				TaskDialog.Show("FindDiskCone", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray2_origin, ray2_direction, 0.01f, out index2)==false)
			{
				TaskDialog.Show("FindDiskCone", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindDiskCone( index0, index1, index2 );
			if(result==null)
			{
				TaskDialog.Show("FindDiskCone", $"FindSurface failed to find a disk cone.");
				return Result.Failed;
			}

			switch(result.Value.type)
			{
				case FS_FEATURE_TYPE.FS_TYPE_CYLINDER:	DrawCylinderAndInliers(ui_doc.Document, result); break;
				case FS_FEATURE_TYPE.FS_TYPE_CONE:		DrawConeAndInliers(ui_doc.Document, result); break;
			}
			
			return Result.Succeeded;
		}

		public static Result OnFindThinRingTorusCommand(UIDocument ui_doc)
		{
			uint index0, index1, index2;

			XYZ ray0_origin=XYZ.Zero, ray0_direction=XYZ.Zero;
			XYZ ray1_origin=XYZ.Zero, ray1_direction=XYZ.Zero;
			XYZ ray2_origin=XYZ.Zero, ray2_direction=XYZ.Zero;
			try
			{
				GenerateRay(ui_doc, out ray0_origin, out ray0_direction);
				GenerateRay(ui_doc, out ray1_origin, out ray1_direction);
				GenerateRay(ui_doc, out ray2_origin, out ray2_direction);
			}
			catch(Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

			// cast ray to point cloud and get an index of the point hit.
			if(OutlierPointCloudEngine.Cast(ray0_origin, ray0_direction, 0.01f, out index0)==false)
			{
				TaskDialog.Show("FindThinRingTorus", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray1_origin, ray1_direction, 0.01f, out index1)==false)
			{
				TaskDialog.Show("FindThinRingTorus", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}
			if(OutlierPointCloudEngine.Cast(ray2_origin, ray2_direction, 0.01f, out index2)==false)
			{
				TaskDialog.Show("FindThinRingTorus", "There is no points picked."+Environment.NewLine+"Command cancelled.");
				return Result.Failed;
			}

			// run FindSurface to find shape
			FS_FEATURE_RESULT? result = FindSurface.RunFindThinRingTorus( index0, index1, index2 );
			if(result==null)
			{
				TaskDialog.Show("FindThinRingTorus", $"FindSurface failed to find a thin ring torus.");
				return Result.Failed;
			}

			switch(result.Value.type)
			{
				case FS_FEATURE_TYPE.FS_TYPE_CYLINDER:	DrawCylinderAndInliers(ui_doc.Document, result); break;
				case FS_FEATURE_TYPE.FS_TYPE_SPHERE:	DrawSphereAndInliers(ui_doc.Document, result); break;
				case FS_FEATURE_TYPE.FS_TYPE_TORUS:		DrawTorusAndInliers(ui_doc.Document, result); break;
			}

			return Result.Succeeded;
		}
	}
}
