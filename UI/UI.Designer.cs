namespace WindowsFormsApplication2
{
    partial class UI
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lbAction = new System.Windows.Forms.ListBox();
            this.lbInventory = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbLocInv = new System.Windows.Forms.ListBox();
            this.lbDirections = new System.Windows.Forms.ListBox();
            this.lblInventory = new System.Windows.Forms.Label();
            this.lblObjects = new System.Windows.Forms.Label();
            this.rtbLocation = new System.Windows.Forms.RichTextBox();
            this.rtbCommandLine = new System.Windows.Forms.RichTextBox();
            this.lblDirections = new System.Windows.Forms.Label();
            this.lbConversation = new System.Windows.Forms.ListBox();
            this.rtbDiagnosis = new System.Windows.Forms.RichTextBox();
            this.pnlConversation = new System.Windows.Forms.Panel();
            this.pnlNormalMode = new System.Windows.Forms.Panel();
            this.NormalModeTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.MainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TopLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TopRightLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TopLeftLayout = new System.Windows.Forms.TableLayoutPanel();
            this.JustCmdLIne = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoesThisWorkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlConversation.SuspendLayout();
            this.pnlNormalMode.SuspendLayout();
            this.NormalModeTableLayout.SuspendLayout();
            this.MainTableLayout.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            this.TopLayout.SuspendLayout();
            this.TopRightLayout.SuspendLayout();
            this.TopLeftLayout.SuspendLayout();
            this.JustCmdLIne.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(635, 349);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // lbAction
            // 
            this.lbAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAction.FormattingEnabled = true;
            this.lbAction.ItemHeight = 20;
            this.lbAction.Location = new System.Drawing.Point(3, 3);
            this.lbAction.Name = "lbAction";
            this.NormalModeTableLayout.SetRowSpan(this.lbAction, 2);
            this.lbAction.Size = new System.Drawing.Size(272, 240);
            this.lbAction.TabIndex = 1;
            this.lbAction.SelectedIndexChanged += new System.EventHandler(this.lbAction_SelectedIndexChanged);
            // 
            // lbInventory
            // 
            this.lbInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInventory.FormattingEnabled = true;
            this.lbInventory.ItemHeight = 20;
            this.lbInventory.Location = new System.Drawing.Point(281, 23);
            this.lbInventory.Name = "lbInventory";
            this.lbInventory.Size = new System.Drawing.Size(272, 220);
            this.lbInventory.TabIndex = 2;
            this.lbInventory.SelectedIndexChanged += new System.EventHandler(this.lbInventory_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(498, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 48);
            this.button1.TabIndex = 3;
            this.button1.Text = "Do Action";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbLocInv
            // 
            this.lbLocInv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLocInv.FormattingEnabled = true;
            this.lbLocInv.ItemHeight = 20;
            this.lbLocInv.Location = new System.Drawing.Point(559, 23);
            this.lbLocInv.Name = "lbLocInv";
            this.lbLocInv.Size = new System.Drawing.Size(272, 220);
            this.lbLocInv.TabIndex = 4;
            this.lbLocInv.SelectedIndexChanged += new System.EventHandler(this.lbLocInv_SelectedIndexChanged);
            // 
            // lbDirections
            // 
            this.lbDirections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDirections.FormattingEnabled = true;
            this.lbDirections.ItemHeight = 20;
            this.lbDirections.Location = new System.Drawing.Point(837, 23);
            this.lbDirections.Name = "lbDirections";
            this.lbDirections.Size = new System.Drawing.Size(156, 220);
            this.lbDirections.TabIndex = 5;
            this.lbDirections.SelectedIndexChanged += new System.EventHandler(this.lbDirections_SelectedIndexChanged);
            // 
            // lblInventory
            // 
            this.lblInventory.AutoSize = true;
            this.lblInventory.Location = new System.Drawing.Point(281, 0);
            this.lblInventory.Name = "lblInventory";
            this.lblInventory.Size = new System.Drawing.Size(90, 20);
            this.lblInventory.TabIndex = 7;
            this.lblInventory.Text = "Your things";
            // 
            // lblObjects
            // 
            this.lblObjects.AutoSize = true;
            this.lblObjects.Location = new System.Drawing.Point(559, 0);
            this.lblObjects.Name = "lblObjects";
            this.lblObjects.Size = new System.Drawing.Size(98, 20);
            this.lblObjects.TabIndex = 8;
            this.lblObjects.Text = "You can see";
            // 
            // rtbLocation
            // 
            this.rtbLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLocation.Location = new System.Drawing.Point(3, 3);
            this.rtbLocation.Name = "rtbLocation";
            this.rtbLocation.ReadOnly = true;
            this.rtbLocation.Size = new System.Drawing.Size(337, 201);
            this.rtbLocation.TabIndex = 10;
            this.rtbLocation.Text = "";
            // 
            // rtbCommandLine
            // 
            this.rtbCommandLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCommandLine.Location = new System.Drawing.Point(3, 3);
            this.rtbCommandLine.Name = "rtbCommandLine";
            this.rtbCommandLine.ReadOnly = true;
            this.rtbCommandLine.Size = new System.Drawing.Size(489, 48);
            this.rtbCommandLine.TabIndex = 11;
            this.rtbCommandLine.Text = "";
            // 
            // lblDirections
            // 
            this.lblDirections.AutoSize = true;
            this.lblDirections.Location = new System.Drawing.Point(837, 0);
            this.lblDirections.Name = "lblDirections";
            this.lblDirections.Size = new System.Drawing.Size(80, 20);
            this.lblDirections.TabIndex = 24;
            this.lblDirections.Text = "Directions";
            // 
            // lbConversation
            // 
            this.lbConversation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbConversation.FormattingEnabled = true;
            this.lbConversation.ItemHeight = 20;
            this.lbConversation.Location = new System.Drawing.Point(0, 0);
            this.lbConversation.Name = "lbConversation";
            this.lbConversation.Size = new System.Drawing.Size(996, 246);
            this.lbConversation.TabIndex = 25;
            this.lbConversation.SelectedIndexChanged += new System.EventHandler(this.lbConversation_SelectedIndexChanged);
            // 
            // rtbDiagnosis
            // 
            this.rtbDiagnosis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDiagnosis.Location = new System.Drawing.Point(3, 210);
            this.rtbDiagnosis.Name = "rtbDiagnosis";
            this.rtbDiagnosis.ReadOnly = true;
            this.rtbDiagnosis.Size = new System.Drawing.Size(337, 202);
            this.rtbDiagnosis.TabIndex = 9;
            this.rtbDiagnosis.Text = "";
            // 
            // pnlConversation
            // 
            this.pnlConversation.Controls.Add(this.lbConversation);
            this.pnlConversation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConversation.Location = new System.Drawing.Point(0, 0);
            this.pnlConversation.Name = "pnlConversation";
            this.pnlConversation.Size = new System.Drawing.Size(996, 246);
            this.pnlConversation.TabIndex = 26;
            // 
            // pnlNormalMode
            // 
            this.pnlNormalMode.Controls.Add(this.NormalModeTableLayout);
            this.pnlNormalMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNormalMode.Location = new System.Drawing.Point(0, 0);
            this.pnlNormalMode.Name = "pnlNormalMode";
            this.pnlNormalMode.Size = new System.Drawing.Size(996, 246);
            this.pnlNormalMode.TabIndex = 27;
            // 
            // NormalModeTableLayout
            // 
            this.NormalModeTableLayout.ColumnCount = 4;
            this.NormalModeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.NormalModeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.NormalModeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.NormalModeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.NormalModeTableLayout.Controls.Add(this.lbAction, 0, 0);
            this.NormalModeTableLayout.Controls.Add(this.lblInventory, 1, 0);
            this.NormalModeTableLayout.Controls.Add(this.lblObjects, 2, 0);
            this.NormalModeTableLayout.Controls.Add(this.lbInventory, 1, 1);
            this.NormalModeTableLayout.Controls.Add(this.lblDirections, 3, 0);
            this.NormalModeTableLayout.Controls.Add(this.lbDirections, 3, 1);
            this.NormalModeTableLayout.Controls.Add(this.lbLocInv, 2, 1);
            this.NormalModeTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NormalModeTableLayout.Location = new System.Drawing.Point(0, 0);
            this.NormalModeTableLayout.Name = "NormalModeTableLayout";
            this.NormalModeTableLayout.RowCount = 2;
            this.NormalModeTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.NormalModeTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.NormalModeTableLayout.Size = new System.Drawing.Size(996, 246);
            this.NormalModeTableLayout.TabIndex = 0;
            // 
            // MainTableLayout
            // 
            this.MainTableLayout.ColumnCount = 1;
            this.MainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayout.Controls.Add(this.BottomPanel, 0, 1);
            this.MainTableLayout.Controls.Add(this.TopLayout, 0, 0);
            this.MainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayout.Location = new System.Drawing.Point(0, 33);
            this.MainTableLayout.Name = "MainTableLayout";
            this.MainTableLayout.RowCount = 2;
            this.MainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.MainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.MainTableLayout.Size = new System.Drawing.Size(1002, 679);
            this.MainTableLayout.TabIndex = 28;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.pnlNormalMode);
            this.BottomPanel.Controls.Add(this.pnlConversation);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(3, 430);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(996, 246);
            this.BottomPanel.TabIndex = 0;
            // 
            // TopLayout
            // 
            this.TopLayout.ColumnCount = 2;
            this.TopLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.TopLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.TopLayout.Controls.Add(this.TopRightLayout, 1, 0);
            this.TopLayout.Controls.Add(this.TopLeftLayout, 0, 0);
            this.TopLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopLayout.Location = new System.Drawing.Point(3, 3);
            this.TopLayout.Name = "TopLayout";
            this.TopLayout.RowCount = 1;
            this.TopLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopLayout.Size = new System.Drawing.Size(996, 421);
            this.TopLayout.TabIndex = 1;
            // 
            // TopRightLayout
            // 
            this.TopRightLayout.ColumnCount = 1;
            this.TopRightLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopRightLayout.Controls.Add(this.rtbDiagnosis, 0, 1);
            this.TopRightLayout.Controls.Add(this.rtbLocation, 0, 0);
            this.TopRightLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopRightLayout.Location = new System.Drawing.Point(650, 3);
            this.TopRightLayout.Name = "TopRightLayout";
            this.TopRightLayout.RowCount = 2;
            this.TopRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TopRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TopRightLayout.Size = new System.Drawing.Size(343, 415);
            this.TopRightLayout.TabIndex = 0;
            // 
            // TopLeftLayout
            // 
            this.TopLeftLayout.ColumnCount = 1;
            this.TopLeftLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopLeftLayout.Controls.Add(this.JustCmdLIne, 0, 1);
            this.TopLeftLayout.Controls.Add(this.richTextBox1, 0, 0);
            this.TopLeftLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopLeftLayout.Location = new System.Drawing.Point(3, 3);
            this.TopLeftLayout.Name = "TopLeftLayout";
            this.TopLeftLayout.RowCount = 2;
            this.TopLeftLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopLeftLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.TopLeftLayout.Size = new System.Drawing.Size(641, 415);
            this.TopLeftLayout.TabIndex = 1;
            // 
            // JustCmdLIne
            // 
            this.JustCmdLIne.ColumnCount = 2;
            this.JustCmdLIne.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.JustCmdLIne.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.JustCmdLIne.Controls.Add(this.button1, 1, 0);
            this.JustCmdLIne.Controls.Add(this.rtbCommandLine, 0, 0);
            this.JustCmdLIne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JustCmdLIne.Location = new System.Drawing.Point(3, 358);
            this.JustCmdLIne.Name = "JustCmdLIne";
            this.JustCmdLIne.RowCount = 1;
            this.JustCmdLIne.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.JustCmdLIne.Size = new System.Drawing.Size(635, 54);
            this.JustCmdLIne.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1002, 33);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howDoesThisWorkToolStripMenuItem,
            this.hintToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howDoesThisWorkToolStripMenuItem
            // 
            this.howDoesThisWorkToolStripMenuItem.Name = "howDoesThisWorkToolStripMenuItem";
            this.howDoesThisWorkToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.howDoesThisWorkToolStripMenuItem.Text = "How does this work?";
            this.howDoesThisWorkToolStripMenuItem.Click += new System.EventHandler(this.howDoesThisWorkToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // hintToolStripMenuItem
            // 
            this.hintToolStripMenuItem.Name = "hintToolStripMenuItem";
            this.hintToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.hintToolStripMenuItem.Text = "Hint";
            this.hintToolStripMenuItem.Click += new System.EventHandler(this.hintToolStripMenuItem_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 712);
            this.Controls.Add(this.MainTableLayout);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "UI";
            this.Text = "Tiny Cave Adventure";
            this.pnlConversation.ResumeLayout(false);
            this.pnlNormalMode.ResumeLayout(false);
            this.NormalModeTableLayout.ResumeLayout(false);
            this.NormalModeTableLayout.PerformLayout();
            this.MainTableLayout.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.TopLayout.ResumeLayout(false);
            this.TopRightLayout.ResumeLayout(false);
            this.TopLeftLayout.ResumeLayout(false);
            this.JustCmdLIne.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListBox lbAction;
        private System.Windows.Forms.ListBox lbInventory;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbLocInv;
        private System.Windows.Forms.ListBox lbDirections;
        private System.Windows.Forms.Label lblInventory;
        private System.Windows.Forms.Label lblObjects;
        private System.Windows.Forms.RichTextBox rtbLocation;
        private System.Windows.Forms.RichTextBox rtbCommandLine;
        private System.Windows.Forms.Label lblDirections;
        private System.Windows.Forms.ListBox lbConversation;
        private System.Windows.Forms.RichTextBox rtbDiagnosis;
        private System.Windows.Forms.Panel pnlConversation;
        private System.Windows.Forms.Panel pnlNormalMode;
        private System.Windows.Forms.TableLayoutPanel NormalModeTableLayout;
        private System.Windows.Forms.TableLayoutPanel MainTableLayout;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.TableLayoutPanel TopLayout;
        private System.Windows.Forms.TableLayoutPanel TopRightLayout;
        private System.Windows.Forms.TableLayoutPanel TopLeftLayout;
        private System.Windows.Forms.TableLayoutPanel JustCmdLIne;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howDoesThisWorkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hintToolStripMenuItem;
    }
}

