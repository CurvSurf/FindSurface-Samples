namespace FindSurfaceRevitPlugin
{
	partial class XYZUnitConversionForm
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
			this.groupBoxBoundingBox = new System.Windows.Forms.GroupBox();
			this.labelUnits = new System.Windows.Forms.Label();
			this.labelScale = new System.Windows.Forms.Label();
			this.radioButtonUSUnits = new System.Windows.Forms.RadioButton();
			this.radioButtonSIUnits = new System.Windows.Forms.RadioButton();
			this.comboBoxUnits = new System.Windows.Forms.ComboBox();
			this.textBoxScale = new System.Windows.Forms.TextBox();
			this.textBoxHeight = new System.Windows.Forms.TextBox();
			this.textBoxDepth = new System.Windows.Forms.TextBox();
			this.textBoxWidth = new System.Windows.Forms.TextBox();
			this.labelHeight = new System.Windows.Forms.Label();
			this.labelDepth = new System.Windows.Forms.Label();
			this.labelWidth = new System.Windows.Forms.Label();
			this.groupBoxOrigin = new System.Windows.Forms.GroupBox();
			this.checkBoxMoveToZeros = new System.Windows.Forms.CheckBox();
			this.textBoxOriginZ = new System.Windows.Forms.TextBox();
			this.textBoxOriginY = new System.Windows.Forms.TextBox();
			this.textBoxOriginX = new System.Windows.Forms.TextBox();
			this.labelOriginZ = new System.Windows.Forms.Label();
			this.labelOriginY = new System.Windows.Forms.Label();
			this.labelOriginX = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelMessage = new System.Windows.Forms.Label();
			this.groupBoxBoundingBox.SuspendLayout();
			this.groupBoxOrigin.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBoxBoundingBox
			// 
			this.groupBoxBoundingBox.Controls.Add(this.labelUnits);
			this.groupBoxBoundingBox.Controls.Add(this.labelScale);
			this.groupBoxBoundingBox.Controls.Add(this.radioButtonUSUnits);
			this.groupBoxBoundingBox.Controls.Add(this.radioButtonSIUnits);
			this.groupBoxBoundingBox.Controls.Add(this.comboBoxUnits);
			this.groupBoxBoundingBox.Controls.Add(this.textBoxScale);
			this.groupBoxBoundingBox.Controls.Add(this.textBoxHeight);
			this.groupBoxBoundingBox.Controls.Add(this.textBoxDepth);
			this.groupBoxBoundingBox.Controls.Add(this.textBoxWidth);
			this.groupBoxBoundingBox.Controls.Add(this.labelHeight);
			this.groupBoxBoundingBox.Controls.Add(this.labelDepth);
			this.groupBoxBoundingBox.Controls.Add(this.labelWidth);
			this.groupBoxBoundingBox.Location = new System.Drawing.Point(12, 12);
			this.groupBoxBoundingBox.Name = "groupBoxBoundingBox";
			this.groupBoxBoundingBox.Size = new System.Drawing.Size(186, 176);
			this.groupBoxBoundingBox.TabIndex = 0;
			this.groupBoxBoundingBox.TabStop = false;
			this.groupBoxBoundingBox.Text = "Extent";
			// 
			// labelUnits
			// 
			this.labelUnits.Location = new System.Drawing.Point(8, 145);
			this.labelUnits.Name = "labelUnits";
			this.labelUnits.Size = new System.Drawing.Size(64, 12);
			this.labelUnits.TabIndex = 13;
			this.labelUnits.Text = "Units: ";
			this.labelUnits.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelScale
			// 
			this.labelScale.Location = new System.Drawing.Point(8, 95);
			this.labelScale.Name = "labelScale";
			this.labelScale.Size = new System.Drawing.Size(64, 12);
			this.labelScale.TabIndex = 12;
			this.labelScale.Text = "Scale: ";
			this.labelScale.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// radioButtonUSUnits
			// 
			this.radioButtonUSUnits.AutoSize = true;
			this.radioButtonUSUnits.Location = new System.Drawing.Point(85, 119);
			this.radioButtonUSUnits.Name = "radioButtonUSUnits";
			this.radioButtonUSUnits.Size = new System.Drawing.Size(78, 16);
			this.radioButtonUSUnits.TabIndex = 11;
			this.radioButtonUSUnits.Text = "U.S. units";
			this.radioButtonUSUnits.UseVisualStyleBackColor = true;
			this.radioButtonUSUnits.CheckedChanged += new System.EventHandler(this.radioButtonUnits_CheckedChanged);
			// 
			// radioButtonSIUnits
			// 
			this.radioButtonSIUnits.AutoSize = true;
			this.radioButtonSIUnits.Checked = true;
			this.radioButtonSIUnits.Location = new System.Drawing.Point(14, 119);
			this.radioButtonSIUnits.Name = "radioButtonSIUnits";
			this.radioButtonSIUnits.Size = new System.Drawing.Size(65, 16);
			this.radioButtonSIUnits.TabIndex = 10;
			this.radioButtonSIUnits.TabStop = true;
			this.radioButtonSIUnits.Text = "SI units";
			this.radioButtonSIUnits.UseVisualStyleBackColor = true;
			this.radioButtonSIUnits.CheckedChanged += new System.EventHandler(this.radioButtonUnits_CheckedChanged);
			// 
			// comboBoxUnits
			// 
			this.comboBoxUnits.FormattingEnabled = true;
			this.comboBoxUnits.Location = new System.Drawing.Point(73, 142);
			this.comboBoxUnits.Name = "comboBoxUnits";
			this.comboBoxUnits.Size = new System.Drawing.Size(100, 20);
			this.comboBoxUnits.TabIndex = 8;
			this.comboBoxUnits.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnits_SelectedIndexChanged);
			this.comboBoxUnits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxListUnits_KeyPress);
			// 
			// textBoxScale
			// 
			this.textBoxScale.Location = new System.Drawing.Point(73, 92);
			this.textBoxScale.Name = "textBoxScale";
			this.textBoxScale.Size = new System.Drawing.Size(100, 21);
			this.textBoxScale.TabIndex = 7;
			this.textBoxScale.Text = "1";
			this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScale_KeyPress);
			// 
			// textBoxHeight
			// 
			this.textBoxHeight.Location = new System.Drawing.Point(73, 66);
			this.textBoxHeight.Name = "textBoxHeight";
			this.textBoxHeight.ReadOnly = true;
			this.textBoxHeight.Size = new System.Drawing.Size(100, 21);
			this.textBoxHeight.TabIndex = 5;
			this.textBoxHeight.Text = "000.0000";
			this.textBoxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxDepth
			// 
			this.textBoxDepth.Location = new System.Drawing.Point(73, 42);
			this.textBoxDepth.Name = "textBoxDepth";
			this.textBoxDepth.ReadOnly = true;
			this.textBoxDepth.Size = new System.Drawing.Size(100, 21);
			this.textBoxDepth.TabIndex = 4;
			this.textBoxDepth.Text = "000.0000";
			this.textBoxDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxWidth
			// 
			this.textBoxWidth.Location = new System.Drawing.Point(73, 18);
			this.textBoxWidth.Name = "textBoxWidth";
			this.textBoxWidth.ReadOnly = true;
			this.textBoxWidth.Size = new System.Drawing.Size(100, 21);
			this.textBoxWidth.TabIndex = 3;
			this.textBoxWidth.Text = "000.0000";
			this.textBoxWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelHeight
			// 
			this.labelHeight.Location = new System.Drawing.Point(8, 69);
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Size = new System.Drawing.Size(64, 12);
			this.labelHeight.TabIndex = 2;
			this.labelHeight.Text = "height(z): ";
			this.labelHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelDepth
			// 
			this.labelDepth.Location = new System.Drawing.Point(8, 45);
			this.labelDepth.Name = "labelDepth";
			this.labelDepth.Size = new System.Drawing.Size(64, 12);
			this.labelDepth.TabIndex = 1;
			this.labelDepth.Text = "depth(y): ";
			this.labelDepth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelWidth
			// 
			this.labelWidth.Location = new System.Drawing.Point(8, 21);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(64, 12);
			this.labelWidth.TabIndex = 0;
			this.labelWidth.Text = "width(x): ";
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBoxOrigin
			// 
			this.groupBoxOrigin.Controls.Add(this.checkBoxMoveToZeros);
			this.groupBoxOrigin.Controls.Add(this.textBoxOriginZ);
			this.groupBoxOrigin.Controls.Add(this.textBoxOriginY);
			this.groupBoxOrigin.Controls.Add(this.textBoxOriginX);
			this.groupBoxOrigin.Controls.Add(this.labelOriginZ);
			this.groupBoxOrigin.Controls.Add(this.labelOriginY);
			this.groupBoxOrigin.Controls.Add(this.labelOriginX);
			this.groupBoxOrigin.Location = new System.Drawing.Point(204, 12);
			this.groupBoxOrigin.Name = "groupBoxOrigin";
			this.groupBoxOrigin.Size = new System.Drawing.Size(156, 123);
			this.groupBoxOrigin.TabIndex = 14;
			this.groupBoxOrigin.TabStop = false;
			this.groupBoxOrigin.Text = "Origin";
			// 
			// checkBoxMoveToZeros
			// 
			this.checkBoxMoveToZeros.AutoSize = true;
			this.checkBoxMoveToZeros.Location = new System.Drawing.Point(10, 96);
			this.checkBoxMoveToZeros.Name = "checkBoxMoveToZeros";
			this.checkBoxMoveToZeros.Size = new System.Drawing.Size(117, 16);
			this.checkBoxMoveToZeros.TabIndex = 6;
			this.checkBoxMoveToZeros.Text = "Move to (0, 0, 0)";
			this.checkBoxMoveToZeros.UseVisualStyleBackColor = true;
			this.checkBoxMoveToZeros.CheckedChanged += new System.EventHandler(this.checkBoxMoveToZeros_CheckedChanged);
			// 
			// textBoxOriginZ
			// 
			this.textBoxOriginZ.Location = new System.Drawing.Point(43, 66);
			this.textBoxOriginZ.Name = "textBoxOriginZ";
			this.textBoxOriginZ.Size = new System.Drawing.Size(100, 21);
			this.textBoxOriginZ.TabIndex = 5;
			this.textBoxOriginZ.Text = "000.0000";
			this.textBoxOriginZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxOriginZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOriginZ_KeyPress);
			// 
			// textBoxOriginY
			// 
			this.textBoxOriginY.Location = new System.Drawing.Point(43, 42);
			this.textBoxOriginY.Name = "textBoxOriginY";
			this.textBoxOriginY.Size = new System.Drawing.Size(100, 21);
			this.textBoxOriginY.TabIndex = 4;
			this.textBoxOriginY.Text = "000.0000";
			this.textBoxOriginY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxOriginY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOriginY_KeyPress);
			// 
			// textBoxOriginX
			// 
			this.textBoxOriginX.Location = new System.Drawing.Point(43, 18);
			this.textBoxOriginX.Name = "textBoxOriginX";
			this.textBoxOriginX.Size = new System.Drawing.Size(100, 21);
			this.textBoxOriginX.TabIndex = 3;
			this.textBoxOriginX.Text = "000.0000";
			this.textBoxOriginX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textBoxOriginX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxOriginX_KeyPress);
			// 
			// labelOriginZ
			// 
			this.labelOriginZ.Location = new System.Drawing.Point(8, 69);
			this.labelOriginZ.Name = "labelOriginZ";
			this.labelOriginZ.Size = new System.Drawing.Size(34, 12);
			this.labelOriginZ.TabIndex = 2;
			this.labelOriginZ.Text = "z: ";
			this.labelOriginZ.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelOriginY
			// 
			this.labelOriginY.Location = new System.Drawing.Point(8, 45);
			this.labelOriginY.Name = "labelOriginY";
			this.labelOriginY.Size = new System.Drawing.Size(34, 12);
			this.labelOriginY.TabIndex = 1;
			this.labelOriginY.Text = "y: ";
			this.labelOriginY.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelOriginX
			// 
			this.labelOriginX.Location = new System.Drawing.Point(8, 21);
			this.labelOriginX.Name = "labelOriginX";
			this.labelOriginX.Size = new System.Drawing.Size(34, 12);
			this.labelOriginX.TabIndex = 0;
			this.labelOriginX.Text = "x: ";
			this.labelOriginX.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(204, 141);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 15;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(285, 141);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 16;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelMessage
			// 
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new System.Drawing.Point(13, 196);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(327, 24);
			this.labelMessage.TabIndex = 17;
			this.labelMessage.Text = "* It is recommended to set the origin to the (0, 0, 0) \r\nto make it easy to see t" +
    "he point cloud when you rotate it.";
			// 
			// XYZUnitConversionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(370, 228);
			this.Controls.Add(this.labelMessage);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBoxOrigin);
			this.Controls.Add(this.groupBoxBoundingBox);
			this.Name = "XYZUnitConversionForm";
			this.Text = "XYZUnitConversionForm";
			this.groupBoxBoundingBox.ResumeLayout(false);
			this.groupBoxBoundingBox.PerformLayout();
			this.groupBoxOrigin.ResumeLayout(false);
			this.groupBoxOrigin.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBoxBoundingBox;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelDepth;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.TextBox textBoxHeight;
		private System.Windows.Forms.TextBox textBoxDepth;
		private System.Windows.Forms.TextBox textBoxWidth;
		private System.Windows.Forms.RadioButton radioButtonUSUnits;
		private System.Windows.Forms.RadioButton radioButtonSIUnits;
		private System.Windows.Forms.ComboBox comboBoxUnits;
		private System.Windows.Forms.TextBox textBoxScale;
		private System.Windows.Forms.Label labelUnits;
		private System.Windows.Forms.Label labelScale;
		private System.Windows.Forms.GroupBox groupBoxOrigin;
		private System.Windows.Forms.CheckBox checkBoxMoveToZeros;
		private System.Windows.Forms.TextBox textBoxOriginZ;
		private System.Windows.Forms.TextBox textBoxOriginY;
		private System.Windows.Forms.TextBox textBoxOriginX;
		private System.Windows.Forms.Label labelOriginZ;
		private System.Windows.Forms.Label labelOriginY;
		private System.Windows.Forms.Label labelOriginX;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelMessage;
	}
}