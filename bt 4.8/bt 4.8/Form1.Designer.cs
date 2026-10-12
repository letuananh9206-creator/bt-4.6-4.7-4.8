namespace bt_4._8
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;
        private MenuStrip menuStrip;
        private ToolStripMenuItem studentsMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel progressStatusLabel;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            studentsMenuItem = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            progressStatusLabel = new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();

            menuStrip.Items.AddRange(new ToolStripItem[] { studentsMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1000, 24);
            menuStrip.TabIndex = 0;

            studentsMenuItem.Name = "studentsMenuItem";
            studentsMenuItem.Size = new Size(111, 20);
            studentsMenuItem.Text = "Quản lý sinh viên";

            statusStrip.Items.AddRange(new ToolStripItem[] { progressStatusLabel });
            statusStrip.Location = new Point(0, 628);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1000, 22);
            statusStrip.TabIndex = 1;

            progressStatusLabel.Name = "progressStatusLabel";
            progressStatusLabel.Size = new Size(72, 17);
            progressStatusLabel.Text = "Sẵn sàng";

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "Form1";
            Text = "Quản lý dữ liệu - MDI";

            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
