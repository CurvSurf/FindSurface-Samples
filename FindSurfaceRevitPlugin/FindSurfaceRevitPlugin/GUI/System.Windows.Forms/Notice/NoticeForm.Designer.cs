namespace FindSurfaceRevitPlugin
{
	partial class NoticeForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoticeForm));
			this.labelNotice1 = new System.Windows.Forms.Label();
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// labelNotice1
			// 
			this.labelNotice1.Location = new System.Drawing.Point(55, 12);
			this.labelNotice1.Name = "labelNotice1";
			this.labelNotice1.Size = new System.Drawing.Size(443, 110);
			this.labelNotice1.TabIndex = 1;
			this.labelNotice1.Text = resources.GetString("labelNotice1.Text");
			// 
			// richTextBox
			// 
			this.richTextBox.Location = new System.Drawing.Point(13, 109);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(485, 154);
			this.richTextBox.TabIndex = 3;
			this.richTextBox.Text = resources.GetString("richTextBox.Text");
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 269);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(485, 35);
			this.button1.TabIndex = 4;
			this.button1.Text = "OK, I understand.";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Image = global::FindSurfaceRevitPlugin.Properties.Resources.ic_info_outline_black_32;
			this.pictureBox.InitialImage = global::FindSurfaceRevitPlugin.Properties.Resources.ic_info_outline_black_48dp;
			this.pictureBox.Location = new System.Drawing.Point(12, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(37, 39);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// NoticeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(510, 316);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.richTextBox);
			this.Controls.Add(this.labelNotice1);
			this.Controls.Add(this.pictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NoticeForm";
			this.Text = "FindSurfaceRevitPlugin: Notice";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Label labelNotice1;
		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.Button button1;
	}
}