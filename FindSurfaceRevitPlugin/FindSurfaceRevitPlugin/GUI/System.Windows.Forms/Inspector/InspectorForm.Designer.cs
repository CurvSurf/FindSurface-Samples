namespace FindSurfaceRevitPlugin
{
	partial class InspectorForm
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
			this.richTextBoxInspectResult = new System.Windows.Forms.RichTextBox();
			this.buttonInspect = new System.Windows.Forms.Button();
			this.buttonClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// richTextBoxInspectResult
			// 
			this.richTextBoxInspectResult.Location = new System.Drawing.Point(12, 43);
			this.richTextBoxInspectResult.Name = "richTextBoxInspectResult";
			this.richTextBoxInspectResult.ReadOnly = true;
			this.richTextBoxInspectResult.Size = new System.Drawing.Size(303, 480);
			this.richTextBoxInspectResult.TabIndex = 11;
			this.richTextBoxInspectResult.Text = "Click [Inspect] after selecting a shape or point cloud.";
			// 
			// buttonInspect
			// 
			this.buttonInspect.Location = new System.Drawing.Point(12, 13);
			this.buttonInspect.Name = "buttonInspect";
			this.buttonInspect.Size = new System.Drawing.Size(75, 23);
			this.buttonInspect.TabIndex = 12;
			this.buttonInspect.Text = "Inspect";
			this.buttonInspect.UseVisualStyleBackColor = true;
			this.buttonInspect.Click += new System.EventHandler(this.Inspect);
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(93, 13);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(75, 23);
			this.buttonClear.TabIndex = 13;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.Clear);
			// 
			// InspectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(327, 535);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.buttonInspect);
			this.Controls.Add(this.richTextBoxInspectResult);
			this.Name = "InspectorForm";
			this.Text = "Inspector";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBoxInspectResult;
		private System.Windows.Forms.Button buttonInspect;
		private System.Windows.Forms.Button buttonClear;
	}
}