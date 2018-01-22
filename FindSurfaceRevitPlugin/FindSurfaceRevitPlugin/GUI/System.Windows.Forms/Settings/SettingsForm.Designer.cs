namespace FindSurfaceRevitPlugin
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing&&(components!=null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBoxPointCloudInputData = new System.Windows.Forms.GroupBox();
			this.textBoxMeanDistance = new System.Windows.Forms.TextBox();
			this.labelMeanDistance = new System.Windows.Forms.Label();
			this.textBoxAccuracy = new System.Windows.Forms.TextBox();
			this.labelAccuracy = new System.Windows.Forms.Label();
			this.textBoxTouchRadius = new System.Windows.Forms.TextBox();
			this.labelTouchRadius = new System.Windows.Forms.Label();
			this.textBoxCone2Cyl = new System.Windows.Forms.TextBox();
			this.labelCone2Cyl = new System.Windows.Forms.Label();
			this.groupBoxProcessControl = new System.Windows.Forms.GroupBox();
			this.textBoxLatExtension = new System.Windows.Forms.TextBox();
			this.labelLatExtension = new System.Windows.Forms.Label();
			this.textBoxRadExpansion = new System.Windows.Forms.TextBox();
			this.trackBarRadExpansion = new System.Windows.Forms.TrackBar();
			this.labelRadExpansion = new System.Windows.Forms.Label();
			this.trackBarLatExtension = new System.Windows.Forms.TrackBar();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBoxPointCloudInputData.SuspendLayout();
			this.groupBoxProcessControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRadExpansion)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarLatExtension)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBoxPointCloudInputData
			// 
			this.groupBoxPointCloudInputData.Controls.Add(this.textBoxMeanDistance);
			this.groupBoxPointCloudInputData.Controls.Add(this.labelMeanDistance);
			this.groupBoxPointCloudInputData.Controls.Add(this.textBoxAccuracy);
			this.groupBoxPointCloudInputData.Controls.Add(this.labelAccuracy);
			this.groupBoxPointCloudInputData.Location = new System.Drawing.Point(10, 10);
			this.groupBoxPointCloudInputData.Name = "groupBoxPointCloudInputData";
			this.groupBoxPointCloudInputData.Size = new System.Drawing.Size(180, 75);
			this.groupBoxPointCloudInputData.TabIndex = 0;
			this.groupBoxPointCloudInputData.TabStop = false;
			this.groupBoxPointCloudInputData.Text = "Point Cloud / Input Data";
			// 
			// textBoxMeanDistance
			// 
			this.textBoxMeanDistance.Location = new System.Drawing.Point(121, 42);
			this.textBoxMeanDistance.Name = "textBoxMeanDistance";
			this.textBoxMeanDistance.Size = new System.Drawing.Size(50, 21);
			this.textBoxMeanDistance.TabIndex = 5;
			this.textBoxMeanDistance.Text = "0.005";
			this.textBoxMeanDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxMeanDistance.TextChanged += new System.EventHandler(this.textBoxMeanDistance_TextChanged);
			this.textBoxMeanDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMeanDistance_KeyPress);
			// 
			// labelMeanDistance
			// 
			this.labelMeanDistance.Location = new System.Drawing.Point(5, 45);
			this.labelMeanDistance.Name = "labelMeanDistance";
			this.labelMeanDistance.Size = new System.Drawing.Size(110, 15);
			this.labelMeanDistance.TabIndex = 4;
			this.labelMeanDistance.Text = "Mean Distance:";
			this.labelMeanDistance.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxAccuracy
			// 
			this.textBoxAccuracy.Location = new System.Drawing.Point(121, 17);
			this.textBoxAccuracy.MaxLength = 28;
			this.textBoxAccuracy.Name = "textBoxAccuracy";
			this.textBoxAccuracy.Size = new System.Drawing.Size(50, 21);
			this.textBoxAccuracy.TabIndex = 1;
			this.textBoxAccuracy.Text = "0.005";
			this.textBoxAccuracy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxAccuracy.TextChanged += new System.EventHandler(this.textBoxAccuracy_TextChanged);
			this.textBoxAccuracy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAccuracy_KeyPress);
			// 
			// labelAccuracy
			// 
			this.labelAccuracy.Location = new System.Drawing.Point(5, 20);
			this.labelAccuracy.Name = "labelAccuracy";
			this.labelAccuracy.Size = new System.Drawing.Size(110, 15);
			this.labelAccuracy.TabIndex = 0;
			this.labelAccuracy.Text = "Accuracy:";
			this.labelAccuracy.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxTouchRadius
			// 
			this.textBoxTouchRadius.Location = new System.Drawing.Point(121, 17);
			this.textBoxTouchRadius.Name = "textBoxTouchRadius";
			this.textBoxTouchRadius.Size = new System.Drawing.Size(50, 21);
			this.textBoxTouchRadius.TabIndex = 3;
			this.textBoxTouchRadius.Text = "0.05";
			this.textBoxTouchRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxTouchRadius.TextChanged += new System.EventHandler(this.textBoxTouchRadius_TextChanged);
			this.textBoxTouchRadius.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTouchRadius_KeyPress);
			// 
			// labelTouchRadius
			// 
			this.labelTouchRadius.Location = new System.Drawing.Point(5, 20);
			this.labelTouchRadius.Name = "labelTouchRadius";
			this.labelTouchRadius.Size = new System.Drawing.Size(110, 15);
			this.labelTouchRadius.TabIndex = 2;
			this.labelTouchRadius.Text = "Touch Radius:";
			this.labelTouchRadius.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxCone2Cyl
			// 
			this.textBoxCone2Cyl.Location = new System.Drawing.Point(121, 142);
			this.textBoxCone2Cyl.Name = "textBoxCone2Cyl";
			this.textBoxCone2Cyl.Size = new System.Drawing.Size(50, 21);
			this.textBoxCone2Cyl.TabIndex = 1;
			this.textBoxCone2Cyl.Text = "10.0";
			this.textBoxCone2Cyl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxCone2Cyl.TextChanged += new System.EventHandler(this.textBoxCone2Cyl_TextChanged);
			this.textBoxCone2Cyl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCone2Cyl_KeyPress);
			// 
			// labelCone2Cyl
			// 
			this.labelCone2Cyl.Location = new System.Drawing.Point(5, 145);
			this.labelCone2Cyl.Name = "labelCone2Cyl";
			this.labelCone2Cyl.Size = new System.Drawing.Size(110, 15);
			this.labelCone2Cyl.TabIndex = 0;
			this.labelCone2Cyl.Text = "Cone2Cyl. [deg]:";
			this.labelCone2Cyl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBoxProcessControl
			// 
			this.groupBoxProcessControl.Controls.Add(this.textBoxCone2Cyl);
			this.groupBoxProcessControl.Controls.Add(this.textBoxLatExtension);
			this.groupBoxProcessControl.Controls.Add(this.textBoxTouchRadius);
			this.groupBoxProcessControl.Controls.Add(this.labelCone2Cyl);
			this.groupBoxProcessControl.Controls.Add(this.labelTouchRadius);
			this.groupBoxProcessControl.Controls.Add(this.labelLatExtension);
			this.groupBoxProcessControl.Controls.Add(this.textBoxRadExpansion);
			this.groupBoxProcessControl.Controls.Add(this.trackBarRadExpansion);
			this.groupBoxProcessControl.Controls.Add(this.labelRadExpansion);
			this.groupBoxProcessControl.Controls.Add(this.trackBarLatExtension);
			this.groupBoxProcessControl.Location = new System.Drawing.Point(10, 91);
			this.groupBoxProcessControl.Name = "groupBoxProcessControl";
			this.groupBoxProcessControl.Size = new System.Drawing.Size(180, 173);
			this.groupBoxProcessControl.TabIndex = 2;
			this.groupBoxProcessControl.TabStop = false;
			this.groupBoxProcessControl.Text = "Process Control";
			// 
			// textBoxLatExtension
			// 
			this.textBoxLatExtension.Location = new System.Drawing.Point(121, 92);
			this.textBoxLatExtension.Name = "textBoxLatExtension";
			this.textBoxLatExtension.Size = new System.Drawing.Size(50, 21);
			this.textBoxLatExtension.TabIndex = 4;
			this.textBoxLatExtension.Text = "5";
			this.textBoxLatExtension.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxLatExtension.TextChanged += new System.EventHandler(this.textBoxLatExtension_TextChanged);
			this.textBoxLatExtension.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLatExtension_KeyPress);
			// 
			// labelLatExtension
			// 
			this.labelLatExtension.Location = new System.Drawing.Point(5, 95);
			this.labelLatExtension.Name = "labelLatExtension";
			this.labelLatExtension.Size = new System.Drawing.Size(110, 15);
			this.labelLatExtension.TabIndex = 3;
			this.labelLatExtension.Text = "Lat. Extension:";
			this.labelLatExtension.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxRadExpansion
			// 
			this.textBoxRadExpansion.Location = new System.Drawing.Point(121, 42);
			this.textBoxRadExpansion.Name = "textBoxRadExpansion";
			this.textBoxRadExpansion.Size = new System.Drawing.Size(50, 21);
			this.textBoxRadExpansion.TabIndex = 1;
			this.textBoxRadExpansion.Text = "5";
			this.textBoxRadExpansion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxRadExpansion.TextChanged += new System.EventHandler(this.textBoxRadExpansion_TextChanged);
			this.textBoxRadExpansion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxRadExpansion_KeyPress);
			// 
			// trackBarRadExpansion
			// 
			this.trackBarRadExpansion.AutoSize = false;
			this.trackBarRadExpansion.LargeChange = 1;
			this.trackBarRadExpansion.Location = new System.Drawing.Point(5, 60);
			this.trackBarRadExpansion.Name = "trackBarRadExpansion";
			this.trackBarRadExpansion.Size = new System.Drawing.Size(167, 30);
			this.trackBarRadExpansion.TabIndex = 2;
			this.trackBarRadExpansion.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackBarRadExpansion.Value = 5;
			this.trackBarRadExpansion.ValueChanged += new System.EventHandler(this.trackBarRadExpansion_ValueChanged);
			// 
			// labelRadExpansion
			// 
			this.labelRadExpansion.Location = new System.Drawing.Point(5, 45);
			this.labelRadExpansion.Name = "labelRadExpansion";
			this.labelRadExpansion.Size = new System.Drawing.Size(110, 15);
			this.labelRadExpansion.TabIndex = 0;
			this.labelRadExpansion.Text = "Rad. Expansion:";
			this.labelRadExpansion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// trackBarLatExtension
			// 
			this.trackBarLatExtension.AutoSize = false;
			this.trackBarLatExtension.LargeChange = 1;
			this.trackBarLatExtension.Location = new System.Drawing.Point(5, 110);
			this.trackBarLatExtension.Name = "trackBarLatExtension";
			this.trackBarLatExtension.Size = new System.Drawing.Size(167, 30);
			this.trackBarLatExtension.TabIndex = 5;
			this.trackBarLatExtension.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackBarLatExtension.Value = 5;
			this.trackBarLatExtension.ValueChanged += new System.EventHandler(this.trackBarLatExtension_ValueChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(10, 270);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(85, 23);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(100, 270);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(85, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(199, 301);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBoxProcessControl);
			this.Controls.Add(this.groupBoxPointCloudInputData);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "FindSurface Parameters";
			this.groupBoxPointCloudInputData.ResumeLayout(false);
			this.groupBoxPointCloudInputData.PerformLayout();
			this.groupBoxProcessControl.ResumeLayout(false);
			this.groupBoxProcessControl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRadExpansion)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarLatExtension)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox groupBoxPointCloudInputData;
		private System.Windows.Forms.TextBox textBoxMeanDistance;
		private System.Windows.Forms.Label labelMeanDistance;
		private System.Windows.Forms.TextBox textBoxTouchRadius;
		private System.Windows.Forms.Label labelTouchRadius;
		private System.Windows.Forms.TextBox textBoxAccuracy;
		private System.Windows.Forms.Label labelAccuracy;
		private System.Windows.Forms.TextBox textBoxCone2Cyl;
		private System.Windows.Forms.Label labelCone2Cyl;
		private System.Windows.Forms.GroupBox groupBoxProcessControl;
		private System.Windows.Forms.TextBox textBoxLatExtension;
		private System.Windows.Forms.TrackBar trackBarLatExtension;
		private System.Windows.Forms.Label labelLatExtension;
		private System.Windows.Forms.TextBox textBoxRadExpansion;
		private System.Windows.Forms.TrackBar trackBarRadExpansion;
		private System.Windows.Forms.Label labelRadExpansion;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
	}
}