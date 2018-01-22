using System.Windows.Forms;

namespace FindSurfaceRevitPlugin
{
	partial class ImportXYZForm
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
			this.labelFileName = new System.Windows.Forms.Label();
			this.textBoxFileSource = new System.Windows.Forms.TextBox();
			this.buttonBrowser = new System.Windows.Forms.Button();
			this.groupBoxColorEncoding = new System.Windows.Forms.GroupBox();
			this.radioButtonNoColor = new System.Windows.Forms.RadioButton();
			this.radioButtonIntensity = new System.Windows.Forms.RadioButton();
			this.radioButtonRGBBytes = new System.Windows.Forms.RadioButton();
			this.radioButtonRGB = new System.Windows.Forms.RadioButton();
			this.groupBoxNormalization = new System.Windows.Forms.GroupBox();
			this.checkBoxNormalization = new System.Windows.Forms.CheckBox();
			this.labelNormalizationValue = new System.Windows.Forms.Label();
			this.textBoxNormalizationValue = new System.Windows.Forms.TextBox();
			this.groupBoxSubdivision = new System.Windows.Forms.GroupBox();
			this.labelNumPointsApprox = new System.Windows.Forms.Label();
			this.labelNumPointsValue = new System.Windows.Forms.Label();
			this.labelSubdivision = new System.Windows.Forms.Label();
			this.numericUpDownSubdivision = new System.Windows.Forms.NumericUpDown();
			this.labelTotalSubdivision = new System.Windows.Forms.Label();
			this.textBoxTotalSubdivision = new System.Windows.Forms.TextBox();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBoxColorEncoding.SuspendLayout();
			this.groupBoxNormalization.SuspendLayout();
			this.groupBoxSubdivision.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubdivision)).BeginInit();
			this.SuspendLayout();
			// 
			// labelFileName
			// 
			this.labelFileName.AutoSize = true;
			this.labelFileName.Location = new System.Drawing.Point(5, 15);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(72, 12);
			this.labelFileName.TabIndex = 0;
			this.labelFileName.Text = "File source:";
			// 
			// textBoxFileSource
			// 
			this.textBoxFileSource.Location = new System.Drawing.Point(77, 11);
			this.textBoxFileSource.Name = "textBoxFileSource";
			this.textBoxFileSource.ReadOnly = true;
			this.textBoxFileSource.Size = new System.Drawing.Size(351, 21);
			this.textBoxFileSource.TabIndex = 1;
			this.textBoxFileSource.Click += new System.EventHandler(this.buttonBrowser_Click);
			// 
			// buttonBrowser
			// 
			this.buttonBrowser.Location = new System.Drawing.Point(434, 11);
			this.buttonBrowser.Name = "buttonBrowser";
			this.buttonBrowser.Size = new System.Drawing.Size(26, 21);
			this.buttonBrowser.TabIndex = 2;
			this.buttonBrowser.Text = "...";
			this.buttonBrowser.UseVisualStyleBackColor = true;
			this.buttonBrowser.Click += new System.EventHandler(this.buttonBrowser_Click);
			// 
			// groupBoxColorEncoding
			// 
			this.groupBoxColorEncoding.Controls.Add(this.radioButtonNoColor);
			this.groupBoxColorEncoding.Controls.Add(this.radioButtonIntensity);
			this.groupBoxColorEncoding.Controls.Add(this.radioButtonRGBBytes);
			this.groupBoxColorEncoding.Controls.Add(this.radioButtonRGB);
			this.groupBoxColorEncoding.Location = new System.Drawing.Point(10, 40);
			this.groupBoxColorEncoding.Name = "groupBoxColorEncoding";
			this.groupBoxColorEncoding.Size = new System.Drawing.Size(260, 110);
			this.groupBoxColorEncoding.TabIndex = 3;
			this.groupBoxColorEncoding.TabStop = false;
			this.groupBoxColorEncoding.Text = "Color Encoding";
			// 
			// radioButtonNoColor
			// 
			this.radioButtonNoColor.AutoSize = true;
			this.radioButtonNoColor.Location = new System.Drawing.Point(10, 80);
			this.radioButtonNoColor.Name = "radioButtonNoColor";
			this.radioButtonNoColor.Size = new System.Drawing.Size(53, 16);
			this.radioButtonNoColor.TabIndex = 3;
			this.radioButtonNoColor.Text = "None";
			this.radioButtonNoColor.UseVisualStyleBackColor = true;
			this.radioButtonNoColor.CheckedChanged += new System.EventHandler(this.radioButtonNoColor_CheckedChanged);
			// 
			// radioButtonIntensity
			// 
			this.radioButtonIntensity.AutoSize = true;
			this.radioButtonIntensity.Checked = true;
			this.radioButtonIntensity.Location = new System.Drawing.Point(10, 20);
			this.radioButtonIntensity.Name = "radioButtonIntensity";
			this.radioButtonIntensity.Size = new System.Drawing.Size(132, 16);
			this.radioButtonIntensity.TabIndex = 0;
			this.radioButtonIntensity.TabStop = true;
			this.radioButtonIntensity.Text = "Grayscale intensity";
			this.radioButtonIntensity.UseVisualStyleBackColor = true;
			this.radioButtonIntensity.CheckedChanged += new System.EventHandler(this.radioButtonIntensity_CheckedChanged);
			// 
			// radioButtonRGBBytes
			// 
			this.radioButtonRGBBytes.AutoSize = true;
			this.radioButtonRGBBytes.Location = new System.Drawing.Point(10, 60);
			this.radioButtonRGBBytes.Name = "radioButtonRGBBytes";
			this.radioButtonRGBBytes.Size = new System.Drawing.Size(160, 16);
			this.radioButtonRGBBytes.TabIndex = 2;
			this.radioButtonRGBBytes.Text = "RGB bytes (0xRRGGBB)";
			this.radioButtonRGBBytes.UseVisualStyleBackColor = true;
			this.radioButtonRGBBytes.CheckedChanged += new System.EventHandler(this.radioButtonRGBBytes_CheckedChanged);
			// 
			// radioButtonRGB
			// 
			this.radioButtonRGB.AutoSize = true;
			this.radioButtonRGB.Location = new System.Drawing.Point(10, 40);
			this.radioButtonRGB.Name = "radioButtonRGB";
			this.radioButtonRGB.Size = new System.Drawing.Size(48, 16);
			this.radioButtonRGB.TabIndex = 1;
			this.radioButtonRGB.Text = "RGB";
			this.radioButtonRGB.UseVisualStyleBackColor = true;
			this.radioButtonRGB.CheckedChanged += new System.EventHandler(this.radioButtonRGB_CheckedChanged);
			// 
			// groupBoxNormalization
			// 
			this.groupBoxNormalization.Controls.Add(this.checkBoxNormalization);
			this.groupBoxNormalization.Controls.Add(this.labelNormalizationValue);
			this.groupBoxNormalization.Controls.Add(this.textBoxNormalizationValue);
			this.groupBoxNormalization.Location = new System.Drawing.Point(10, 160);
			this.groupBoxNormalization.Name = "groupBoxNormalization";
			this.groupBoxNormalization.Size = new System.Drawing.Size(260, 70);
			this.groupBoxNormalization.TabIndex = 4;
			this.groupBoxNormalization.TabStop = false;
			this.groupBoxNormalization.Text = "Normalization";
			// 
			// checkBoxNormalization
			// 
			this.checkBoxNormalization.AutoSize = true;
			this.checkBoxNormalization.Location = new System.Drawing.Point(12, 20);
			this.checkBoxNormalization.Name = "checkBoxNormalization";
			this.checkBoxNormalization.Size = new System.Drawing.Size(143, 16);
			this.checkBoxNormalization.TabIndex = 1;
			this.checkBoxNormalization.Text = "Enable normalization";
			this.checkBoxNormalization.CheckedChanged += new System.EventHandler(this.checkBoxNormalization_CheckedChanged);
			// 
			// labelNormalizationValue
			// 
			this.labelNormalizationValue.AutoSize = true;
			this.labelNormalizationValue.Location = new System.Drawing.Point(10, 45);
			this.labelNormalizationValue.Name = "labelNormalizationValue";
			this.labelNormalizationValue.Size = new System.Drawing.Size(80, 12);
			this.labelNormalizationValue.TabIndex = 2;
			this.labelNormalizationValue.Text = "Denominator:";
			// 
			// textBoxNormalizationValue
			// 
			this.textBoxNormalizationValue.Enabled = false;
			this.textBoxNormalizationValue.Location = new System.Drawing.Point(96, 42);
			this.textBoxNormalizationValue.Name = "textBoxNormalizationValue";
			this.textBoxNormalizationValue.Size = new System.Drawing.Size(124, 21);
			this.textBoxNormalizationValue.TabIndex = 3;
			this.textBoxNormalizationValue.Text = "32767";
			this.textBoxNormalizationValue.TextChanged += new System.EventHandler(this.textBoxNormalizationValue_TextChanged);
			this.textBoxNormalizationValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNormalizationValue_KeyPress);
			// 
			// groupBoxSubdivision
			// 
			this.groupBoxSubdivision.Controls.Add(this.labelNumPointsApprox);
			this.groupBoxSubdivision.Controls.Add(this.labelNumPointsValue);
			this.groupBoxSubdivision.Controls.Add(this.labelSubdivision);
			this.groupBoxSubdivision.Controls.Add(this.numericUpDownSubdivision);
			this.groupBoxSubdivision.Controls.Add(this.labelTotalSubdivision);
			this.groupBoxSubdivision.Controls.Add(this.textBoxTotalSubdivision);
			this.groupBoxSubdivision.Location = new System.Drawing.Point(280, 40);
			this.groupBoxSubdivision.Name = "groupBoxSubdivision";
			this.groupBoxSubdivision.Size = new System.Drawing.Size(180, 110);
			this.groupBoxSubdivision.TabIndex = 5;
			this.groupBoxSubdivision.TabStop = false;
			this.groupBoxSubdivision.Text = "Subdivision";
			// 
			// labelNumPointsApprox
			// 
			this.labelNumPointsApprox.AutoSize = true;
			this.labelNumPointsApprox.Location = new System.Drawing.Point(75, 25);
			this.labelNumPointsApprox.Name = "labelNumPointsApprox";
			this.labelNumPointsApprox.Size = new System.Drawing.Size(98, 12);
			this.labelNumPointsApprox.TabIndex = 4;
			this.labelNumPointsApprox.Text = "Points (Approx.)";
			// 
			// labelNumPointsValue
			// 
			this.labelNumPointsValue.Location = new System.Drawing.Point(5, 25);
			this.labelNumPointsValue.Name = "labelNumPointsValue";
			this.labelNumPointsValue.Size = new System.Drawing.Size(70, 12);
			this.labelNumPointsValue.TabIndex = 5;
			this.labelNumPointsValue.Text = "N/A";
			this.labelNumPointsValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelSubdivision
			// 
			this.labelSubdivision.Location = new System.Drawing.Point(5, 50);
			this.labelSubdivision.Name = "labelSubdivision";
			this.labelSubdivision.Size = new System.Drawing.Size(90, 12);
			this.labelSubdivision.TabIndex = 0;
			this.labelSubdivision.Text = "Subdivision:";
			this.labelSubdivision.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// numericUpDownSubdivision
			// 
			this.numericUpDownSubdivision.Location = new System.Drawing.Point(100, 48);
			this.numericUpDownSubdivision.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownSubdivision.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownSubdivision.Name = "numericUpDownSubdivision";
			this.numericUpDownSubdivision.Size = new System.Drawing.Size(72, 21);
			this.numericUpDownSubdivision.TabIndex = 1;
			this.numericUpDownSubdivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownSubdivision.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownSubdivision.ValueChanged += new System.EventHandler(this.numericUpDownSubdivision_ValueChanged);
			this.numericUpDownSubdivision.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDownSubdivision_KeyPress);
			// 
			// labelTotalSubdivision
			// 
			this.labelTotalSubdivision.Location = new System.Drawing.Point(5, 75);
			this.labelTotalSubdivision.Name = "labelTotalSubdivision";
			this.labelTotalSubdivision.Size = new System.Drawing.Size(90, 12);
			this.labelTotalSubdivision.TabIndex = 2;
			this.labelTotalSubdivision.Text = "Total cells:";
			this.labelTotalSubdivision.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxTotalSubdivision
			// 
			this.textBoxTotalSubdivision.Location = new System.Drawing.Point(100, 73);
			this.textBoxTotalSubdivision.Name = "textBoxTotalSubdivision";
			this.textBoxTotalSubdivision.ReadOnly = true;
			this.textBoxTotalSubdivision.Size = new System.Drawing.Size(72, 21);
			this.textBoxTotalSubdivision.TabIndex = 3;
			this.textBoxTotalSubdivision.Text = "1000";
			this.textBoxTotalSubdivision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// buttonOpen
			// 
			this.buttonOpen.Location = new System.Drawing.Point(278, 205);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(90, 25);
			this.buttonOpen.TabIndex = 6;
			this.buttonOpen.Text = "&Open";
			this.buttonOpen.UseVisualStyleBackColor = true;
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(370, 205);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(90, 25);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// ImportXYZForm
			// 
			this.AcceptButton = this.buttonOpen;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(469, 245);
			this.Controls.Add(this.labelFileName);
			this.Controls.Add(this.textBoxFileSource);
			this.Controls.Add(this.buttonBrowser);
			this.Controls.Add(this.groupBoxColorEncoding);
			this.Controls.Add(this.groupBoxNormalization);
			this.Controls.Add(this.groupBoxSubdivision);
			this.Controls.Add(this.buttonOpen);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImportXYZForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Import";
			this.groupBoxColorEncoding.ResumeLayout(false);
			this.groupBoxColorEncoding.PerformLayout();
			this.groupBoxNormalization.ResumeLayout(false);
			this.groupBoxNormalization.PerformLayout();
			this.groupBoxSubdivision.ResumeLayout(false);
			this.groupBoxSubdivision.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubdivision)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label labelFileName;
		private TextBox textBoxFileSource;
		private Button buttonBrowser;

		private GroupBox groupBoxColorEncoding;
		private RadioButton radioButtonIntensity;
		private RadioButton radioButtonRGB;
		private RadioButton radioButtonRGBBytes;

		private GroupBox groupBoxNormalization;
		private CheckBox checkBoxNormalization;
		private Label labelNormalizationValue;
		private TextBox textBoxNormalizationValue;

		private GroupBox groupBoxSubdivision;
		private Label labelSubdivision;
		private NumericUpDown numericUpDownSubdivision;
		private Label labelTotalSubdivision;
		private TextBox textBoxTotalSubdivision;

		private Button buttonOpen;
		private Button buttonCancel;
		private Label labelNumPointsApprox;
		private Label labelNumPointsValue;
		private RadioButton radioButtonNoColor;
	}
}