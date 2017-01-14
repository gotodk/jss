using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Threading;
using System.IO;

namespace 客户端主程序
{
    partial class FormAlertMessage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlertMessage));
            this.glassButton1 = new System.Windows.Forms.Button();
            this.glassButton2 = new System.Windows.Forms.Button();
            this.textControl1 = new Com.Seezt.Skins.TextControl();
            this.glassButton3 = new System.Windows.Forms.Button();
            this.basicPictureBox7 = new Com.Seezt.Skins.BasicPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.basicPictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // glassButton1
            // 
            this.glassButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.glassButton1.BackColor = System.Drawing.Color.Transparent;
            this.glassButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.glassButton1.FlatAppearance.BorderSize = 0;
            this.glassButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.glassButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.glassButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.glassButton1.Location = new System.Drawing.Point(114, 159);
            this.glassButton1.Name = "glassButton1";
            this.glassButton1.Size = new System.Drawing.Size(73, 28);
            this.glassButton1.TabIndex = 2;
            this.glassButton1.TabStop = false;
            this.glassButton1.UseVisualStyleBackColor = false;
            // 
            // glassButton2
            // 
            this.glassButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.glassButton2.BackColor = System.Drawing.Color.Transparent;
            this.glassButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.glassButton2.FlatAppearance.BorderSize = 0;
            this.glassButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.glassButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.glassButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.glassButton2.Location = new System.Drawing.Point(211, 159);
            this.glassButton2.Name = "glassButton2";
            this.glassButton2.Size = new System.Drawing.Size(73, 28);
            this.glassButton2.TabIndex = 1;
            this.glassButton2.TabStop = false;
            this.glassButton2.UseVisualStyleBackColor = false;
            // 
            // textControl1
            // 
            this.textControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textControl1.AutoScroll = true;
            this.textControl1.BackColor = System.Drawing.Color.Transparent;
            this.textControl1.Location = new System.Drawing.Point(56, 34);
            this.textControl1.Name = "textControl1";
            this.textControl1.Size = new System.Drawing.Size(332, 119);
            this.textControl1.TabIndex = 35;
            this.textControl1.Texts = ((System.Collections.ArrayList)(resources.GetObject("textControl1.Texts")));
            // 
            // glassButton3
            // 
            this.glassButton3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.glassButton3.BackColor = System.Drawing.Color.Transparent;
            this.glassButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.glassButton3.FlatAppearance.BorderSize = 0;
            this.glassButton3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.glassButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.glassButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.glassButton3.Location = new System.Drawing.Point(165, 159);
            this.glassButton3.Name = "glassButton3";
            this.glassButton3.Size = new System.Drawing.Size(73, 28);
            this.glassButton3.TabIndex = 0;
            this.glassButton3.TabStop = false;
            this.glassButton3.UseVisualStyleBackColor = false;
            this.glassButton3.Click += new System.EventHandler(this.glassButton3_Click);
            // 
            // basicPictureBox7
            // 
            this.basicPictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.basicPictureBox7.Location = new System.Drawing.Point(10, 46);
            this.basicPictureBox7.Name = "basicPictureBox7";
            this.basicPictureBox7.Size = new System.Drawing.Size(40, 40);
            this.basicPictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.basicPictureBox7.TabIndex = 37;
            this.basicPictureBox7.TabStop = false;
            this.basicPictureBox7.Texts = null;
            // 
            // FormAlertMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.basicPictureBox7);
            this.Controls.Add(this.glassButton3);
            this.Controls.Add(this.textControl1);
            this.Controls.Add(this.glassButton2);
            this.Controls.Add(this.glassButton1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlertMessage";
            this.ShowColorButton = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标题";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormReSendEmail_Load);
            this.Controls.SetChildIndex(this.glassButton1, 0);
            this.Controls.SetChildIndex(this.glassButton2, 0);
            this.Controls.SetChildIndex(this.textControl1, 0);
            this.Controls.SetChildIndex(this.glassButton3, 0);
            this.Controls.SetChildIndex(this.basicPictureBox7, 0);
            ((System.ComponentModel.ISupportInitialize)(this.basicPictureBox7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button glassButton1;
        private Button glassButton2;
        private Com.Seezt.Skins.TextControl textControl1;
        private Button glassButton3;
        private Com.Seezt.Skins.BasicPictureBox basicPictureBox7;

    }
}