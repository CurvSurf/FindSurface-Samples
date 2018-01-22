# FindSurface-Samples / FindSurfaceRevitPlugin / Readme.md
**Curv*Surf* FindSurface™ SDK** Sample - FindSurfaceRevitPlugin

Overview
--------

This sample code is Revit plugin source code developed by using our FindSurface SDK.

This program is designed to demonstrate a possibility of showing a new method, more effective and much simpler than an old fashioned way, to get the structures primitives just by using its point cloud data.

This sample plug-in performs only basic functions as an example using FindSurface SDK and is not suitable for practical use.

**The sample only runs with our FindSurface SDK library files (FindSurface.dll, etc.).**

**You must either [request a free trial](http://developers.curvsurf.com/licenses.jsp) or [purchase a license](https://developers.curvsurf.com/licenses.jsp) to activate the library files.**


Quick Start
-----------

Start to build the sample source code, then there exists plug-in files at "bin" directory in the project.

**This sample plugin has not been published in the Revit app store, so you will see a warning message that confirms that you intend to load the plugin when you run Revit.**

Download the addin manifest file at [here]() and put the file in your Revit addin folder (ex: C:\ProgramData\Autodesk\Revit\Addins\2017\) to indicate Revit the location of the plugin. Or you may write an addin manifest file following the instruction in [here](https://knowledge.autodesk.com/search-result/caas/CloudHelp/cloudhelp/2017/ENU/Revit-API/files/GUID-7577712B-B09F-4585-BE0C-FF16A5078D29-htm.html).

Revit program will start to load the plug-in automatically, as you run the program. When the plug-in loads completely, the FindSurface tab will be added to the Revit ribbon menu. FindSurface tab buttons allow a user not only to load point clouds but also to detect primitives such as plane, sphere, cylinder, cone, and torus. When the user clicks on the blue buttons in the FindSurface tab and then clicks on the surface of the point cloud, our plug-in will detect the parametric geometry around the point and display it on the Revit workspace.


The following instructions briefly describes the functionalities provided by our sample plug-in.

- Point cloud

	- Open: loading a point cloud file.
	- Clean up: removing every components in a Revit workspace.
	- Reset: only removing recent parametric geometry data you obtained.

In order to perform the following functions below, click on a surface of a point cloud after pressing a button.

- Find surface

	- Plane: finding a plane around the given surface of the point cloud.
	- Sphere: finding a sphere around the given surface of the point cloud.
	- Cylinder: finding a cylinder around the given surface of the point cloud. 
	- Cone: finding either a cone or a conical frustum from the given surface of the point cloud.
	- Any shape: finding a most appropriate primitive from the given surface of the point cloud.

In order to perform the following functions below, click two surfaces of point cloud consequently after pressing a button.

- Find surface 2pts

	- Strip plane: finding a strip plane including the two given areas of the point cloud.
	- Rod cylinder: finding a rod cylinder including the two given area of the point cloud.

In order to perform the following functions below, click three surfaces of point cloud consequently after pressing a button.

- Find surface 3pts

	- Disk cylinder: finding a disk cylinder including the three given areas of the point cloud.
	- Disk cone: finding a disk conical frustum including the three given areas of the point cloud.
	- Thin ring torus: finding a ring torus including the three given areas of the point cloud.

- Configuration

	- Repeat: excuting the most recent task.
	- View list: adjusting a visibility of outlier/inlier point cloud and the detected primitives.
	- Inspector: showing a window displaying the size and the position of the detected primitives.
	- Settings: adjusting the parameters of FindSurface library.

- FindSurface parameter

	- See [documentation](https://developers.curvsurf.com/documentation.jsp) > Concepts for more details.


Contact
-------

Send an email to support@curvsurf.com to contact our support team, if you have any question to ask.
