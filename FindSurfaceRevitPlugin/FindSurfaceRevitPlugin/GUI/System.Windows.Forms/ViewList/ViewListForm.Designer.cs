namespace FindSurfaceRevitPlugin
{
	partial class ViewListForm
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
			this.groupBoxPointClouds = new System.Windows.Forms.GroupBox();
			this.buttonPointCloudsHideAll = new System.Windows.Forms.Button();
			this.buttonPointCloudsSetAllVisible = new System.Windows.Forms.Button();
			this.checkedListBoxPointClouds = new System.Windows.Forms.CheckedListBox();
			this.groupBoxShapes = new System.Windows.Forms.GroupBox();
			this.buttonShapesHideAll = new System.Windows.Forms.Button();
			this.buttonShapesSetAllVisible = new System.Windows.Forms.Button();
			this.checkedListBoxDirectShapes = new System.Windows.Forms.CheckedListBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.groupBoxPointClouds.SuspendLayout();
			this.groupBoxShapes.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBoxPointClouds
			// 
			this.groupBoxPointClouds.Controls.Add(this.buttonPointCloudsHideAll);
			this.groupBoxPointClouds.Controls.Add(this.buttonPointCloudsSetAllVisible);
			this.groupBoxPointClouds.Controls.Add(this.checkedListBoxPointClouds);
			this.groupBoxPointClouds.Location = new System.Drawing.Point(12, 12);
			this.groupBoxPointClouds.Name = "groupBoxPointClouds";
			this.groupBoxPointClouds.Size = new System.Drawing.Size(220, 160);
			this.groupBoxPointClouds.TabIndex = 0;
			this.groupBoxPointClouds.TabStop = false;
			this.groupBoxPointClouds.Text = "Point Clouds";
			// 
			// buttonPointCloudsHideAll
			// 
			this.buttonPointCloudsHideAll.Location = new System.Drawing.Point(110, 125);
			this.buttonPointCloudsHideAll.Name = "buttonPointCloudsHideAll";
			this.buttonPointCloudsHideAll.Size = new System.Drawing.Size(100, 25);
			this.buttonPointCloudsHideAll.TabIndex = 4;
			this.buttonPointCloudsHideAll.Text = "Hide All";
			this.buttonPointCloudsHideAll.UseVisualStyleBackColor = true;
			this.buttonPointCloudsHideAll.Click += new System.EventHandler(this.buttonPointCloudsHideAll_Click);
			// 
			// buttonPointCloudsSetAllVisible
			// 
			this.buttonPointCloudsSetAllVisible.Location = new System.Drawing.Point(5, 125);
			this.buttonPointCloudsSetAllVisible.Name = "buttonPointCloudsSetAllVisible";
			this.buttonPointCloudsSetAllVisible.Size = new System.Drawing.Size(100, 25);
			this.buttonPointCloudsSetAllVisible.TabIndex = 3;
			this.buttonPointCloudsSetAllVisible.Text = "Set All Visible";
			this.buttonPointCloudsSetAllVisible.UseVisualStyleBackColor = true;
			this.buttonPointCloudsSetAllVisible.Click += new System.EventHandler(this.buttonPointCloudsSetAllVisible_Click);
			// 
			// checkedListBoxPointClouds
			// 
			this.checkedListBoxPointClouds.FormattingEnabled = true;
			this.checkedListBoxPointClouds.Location = new System.Drawing.Point(10, 20);
			this.checkedListBoxPointClouds.Name = "checkedListBoxPointClouds";
			this.checkedListBoxPointClouds.Size = new System.Drawing.Size(200, 100);
			this.checkedListBoxPointClouds.TabIndex = 2;
			this.checkedListBoxPointClouds.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxPointClouds_ItemCheck);
			// 
			// groupBoxShapes
			// 
			this.groupBoxShapes.Controls.Add(this.buttonShapesHideAll);
			this.groupBoxShapes.Controls.Add(this.buttonShapesSetAllVisible);
			this.groupBoxShapes.Controls.Add(this.checkedListBoxDirectShapes);
			this.groupBoxShapes.Location = new System.Drawing.Point(12, 182);
			this.groupBoxShapes.Name = "groupBoxShapes";
			this.groupBoxShapes.Size = new System.Drawing.Size(220, 160);
			this.groupBoxShapes.TabIndex = 1;
			this.groupBoxShapes.TabStop = false;
			this.groupBoxShapes.Text = "Shapes";
			// 
			// buttonShapesHideAll
			// 
			this.buttonShapesHideAll.Location = new System.Drawing.Point(110, 125);
			this.buttonShapesHideAll.Name = "buttonShapesHideAll";
			this.buttonShapesHideAll.Size = new System.Drawing.Size(100, 25);
			this.buttonShapesHideAll.TabIndex = 3;
			this.buttonShapesHideAll.Text = "Hide All";
			this.buttonShapesHideAll.UseVisualStyleBackColor = true;
			this.buttonShapesHideAll.Click += new System.EventHandler(this.buttonShapesHideAll_Click);
			// 
			// buttonShapesSetAllVisible
			// 
			this.buttonShapesSetAllVisible.Location = new System.Drawing.Point(5, 125);
			this.buttonShapesSetAllVisible.Name = "buttonShapesSetAllVisible";
			this.buttonShapesSetAllVisible.Size = new System.Drawing.Size(100, 25);
			this.buttonShapesSetAllVisible.TabIndex = 2;
			this.buttonShapesSetAllVisible.Text = "Set All Visible";
			this.buttonShapesSetAllVisible.UseVisualStyleBackColor = true;
			this.buttonShapesSetAllVisible.Click += new System.EventHandler(this.buttonShapesSetAllVisible_Click);
			// 
			// checkedListBoxDirectShapes
			// 
			this.checkedListBoxDirectShapes.FormattingEnabled = true;
			this.checkedListBoxDirectShapes.Location = new System.Drawing.Point(10, 20);
			this.checkedListBoxDirectShapes.Name = "checkedListBoxDirectShapes";
			this.checkedListBoxDirectShapes.Size = new System.Drawing.Size(200, 100);
			this.checkedListBoxDirectShapes.TabIndex = 1;
			this.checkedListBoxDirectShapes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxShapes_ItemCheck);
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(12, 347);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(220, 25);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// ViewListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(244, 381);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.groupBoxShapes);
			this.Controls.Add(this.groupBoxPointClouds);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ViewListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "View List";
			this.groupBoxPointClouds.ResumeLayout(false);
			this.groupBoxShapes.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBoxPointClouds;
		private System.Windows.Forms.CheckedListBox checkedListBoxPointClouds;
		private System.Windows.Forms.Button buttonPointCloudsSetAllVisible;
		private System.Windows.Forms.Button buttonPointCloudsHideAll;
		private System.Windows.Forms.GroupBox groupBoxShapes;
		private System.Windows.Forms.CheckedListBox checkedListBoxDirectShapes;
		private System.Windows.Forms.Button buttonShapesSetAllVisible;
		private System.Windows.Forms.Button buttonShapesHideAll;
		private System.Windows.Forms.Button buttonClose;
	}
}