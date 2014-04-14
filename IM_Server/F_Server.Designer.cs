namespace IM_Server
{
    partial class F_Server
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Server));
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.控制台ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.推出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_SysUser = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.udpSocket1 = new MyIM.UDPSocket(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = "D:\\学习测试程序\\IM\\IM_Server\\bin\\Debug\\MP10.ssk";
            this.skinEngine1.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("skinEngine1.SkinStreamMain")));
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.控制台ToolStripMenuItem,
            this.系统ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(593, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 控制台ToolStripMenuItem
            // 
            this.控制台ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始服务ToolStripMenuItem});
            this.控制台ToolStripMenuItem.Name = "控制台ToolStripMenuItem";
            this.控制台ToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.控制台ToolStripMenuItem.Text = "控制台";
            // 
            // 开始服务ToolStripMenuItem
            // 
            this.开始服务ToolStripMenuItem.Name = "开始服务ToolStripMenuItem";
            this.开始服务ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.开始服务ToolStripMenuItem.Text = "开始服务";
            this.开始服务ToolStripMenuItem.Click += new System.EventHandler(this.Tool_Open);
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.推出ToolStripMenuItem});
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.系统ToolStripMenuItem.Text = "系统";
            // 
            // 推出ToolStripMenuItem
            // 
            this.推出ToolStripMenuItem.Name = "推出ToolStripMenuItem";
            this.推出ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.推出ToolStripMenuItem.Text = "退出";
            this.推出ToolStripMenuItem.Click += new System.EventHandler(this.推出ToolStripMenuItem_Click);
            // 
            // LV_SysUser
            // 
            this.LV_SysUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.LV_SysUser.Location = new System.Drawing.Point(0, 31);
            this.LV_SysUser.Name = "LV_SysUser";
            this.LV_SysUser.Size = new System.Drawing.Size(593, 272);
            this.LV_SysUser.TabIndex = 1;
            this.LV_SysUser.UseCompatibleStateImageBehavior = false;
            this.LV_SysUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 73;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IP地址";
            this.columnHeader2.Width = 116;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "端口";
            this.columnHeader3.Width = 76;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "姓名";
            this.columnHeader4.Width = 109;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "状态";
            this.columnHeader5.Width = 197;
            // 
            // udpSocket1
            // 
            this.udpSocket1.Active = false;
            this.udpSocket1.LocalHost = "127.0.0.1";
            this.udpSocket1.LocalPort = 11000;
            // 
            // F_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 299);
            this.Controls.Add(this.LV_SysUser);
            this.Controls.Add(this.menuStrip1);
            this.Name = "F_Server";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.F_Server_FormClosed);
            this.Load += new System.EventHandler(this.F_Server_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 控制台ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推出ToolStripMenuItem;
        private System.Windows.Forms.ListView LV_SysUser;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private MyIM.UDPSocket udpSocket1;
    }
}

