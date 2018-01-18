# FindSurface-Samples / SimpleGUI / Readme.md
**Curv*Surf* FindSurface™ SDK** Samples - SimpleGUI

Overview
--------

This sample provides developers an example of application with graphic user interfaces that allows users to interact with the application.

The sample is implemented for Windows and Linux environments.

The sample program allows users to click a surface on a rendered point cloud to begin detecting the parametric model of the surface.

**The sample will not work without SDK library files (FindSurface.dll, etc.).
You must [purchase a license](https://developers.curvsurf.com/licenses.jsp) (or [request a free trial](http://developers.curvsurf.com/licenses.jsp)) to download and activate the library file.**


Quick Start
------------

### How to run the demo program

The program receives the following command line arguments:
	
  - accuracy
  - mean-distance
  - touch-radius-step
  - point-cloud-filename

The "accuracy" and "mean-distance" are the parameters of FindSurface. See "C APIs" > Enumerations > Parameter at [here](https://developers.curvsurf.com/documentation.jsp) for more details.

The "touch-radius-step" is a step value of when you change touch radius. 

The "point-cloud-filename" is the file to load in the program.

The usage to set this arguments is as follows:

	usage: SimpleGUI.exe [options <option-value>] [point-cloud-filename]
		options:
			-a 		accuracy (the default value is 0.003, which represents 3 mm)
			-d 		mean distance (the default value is 0.01, which represents 10 mm)
			-s 		touch radius step (the default value is 0.01, which represents 10 mm)

		point-cloud-filename:
			<filename>.xyz 		it will try to find "sample.xyz", if omitted.

	example: SimpleGUI.exe -a 0.002 -s 0.02 -d 0.1 

The example will results that the program sets accuracy to 2 mm, mean distance to 100 mm, touch radius step to 20 mm respectively, and tries to find the default file "sample.xyz" since no filename is given. It will fail if the file cannot be found.


### Getting Started to develop your own application

Our [developer website](https://developers.curvsurf.com/documentation.jsp) provides detailed instruction on how to develop using FindSurface SDK.


CONTACT
-------

Send an email to support@curvsurf.com, to contact our support team.
