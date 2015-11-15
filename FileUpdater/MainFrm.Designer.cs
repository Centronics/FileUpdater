namespace FileUpdater
{
    partial class MainFrm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.btnBrMainDir = new System.Windows.Forms.Button();
            this.lstUser = new System.Windows.Forms.ListBox();
            this.lblSelUsers = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtFolderUpdate = new System.Windows.Forms.TextBox();
            this.txtFolderToUpdate = new System.Windows.Forms.TextBox();
            this.btnBrNew = new System.Windows.Forms.Button();
            this.btnPlan = new System.Windows.Forms.Button();
            this.DOpenFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnUserDel = new System.Windows.Forms.Button();
            this.tPlan = new System.Windows.Forms.Timer(this.components);
            this.grpUsers = new System.Windows.Forms.GroupBox();
            this.btnDeleteTemplate = new System.Windows.Forms.Button();
            this.txtTemplateName = new System.Windows.Forms.TextBox();
            this.lstTemplate = new System.Windows.Forms.ListBox();
            this.btnUserAdd = new System.Windows.Forms.Button();
            this.lstLogs = new System.Windows.Forms.ListBox();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnRefreshUsersList = new System.Windows.Forms.Button();
            this.grpUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveTemplate.Location = new System.Drawing.Point(308, 79);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(184, 23);
            this.btnSaveTemplate.TabIndex = 8;
            this.btnSaveTemplate.Text = "Сохранить шаблон (Ctrl+S)";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // btnBrMainDir
            // 
            this.btnBrMainDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrMainDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrMainDir.Location = new System.Drawing.Point(272, 6);
            this.btnBrMainDir.Name = "btnBrMainDir";
            this.btnBrMainDir.Size = new System.Drawing.Size(237, 23);
            this.btnBrMainDir.TabIndex = 2;
            this.btnBrMainDir.Text = "Путь к дистрибутиву (F3)";
            this.btnBrMainDir.UseVisualStyleBackColor = true;
            this.btnBrMainDir.Click += new System.EventHandler(this.btnBrMainDir_Click);
            // 
            // lstUser
            // 
            this.lstUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstUser.FormattingEnabled = true;
            this.lstUser.ItemHeight = 16;
            this.lstUser.Location = new System.Drawing.Point(12, 252);
            this.lstUser.Name = "lstUser";
            this.lstUser.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstUser.Size = new System.Drawing.Size(497, 116);
            this.lstUser.TabIndex = 5;
            this.lstUser.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstUser_KeyUp);
            // 
            // lblSelUsers
            // 
            this.lblSelUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelUsers.AutoSize = true;
            this.lblSelUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelUsers.Location = new System.Drawing.Point(9, 233);
            this.lblSelUsers.Name = "lblSelUsers";
            this.lblSelUsers.Size = new System.Drawing.Size(206, 16);
            this.lblSelUsers.TabIndex = 8;
            this.lblSelUsers.Text = "Выбранные пользователи:";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnGo.Location = new System.Drawing.Point(12, 432);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(497, 23);
            this.btnGo.TabIndex = 13;
            this.btnGo.Text = "Запуск (F9)";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtFolderUpdate
            // 
            this.txtFolderUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFolderUpdate.Location = new System.Drawing.Point(12, 6);
            this.txtFolderUpdate.MaxLength = 260;
            this.txtFolderUpdate.Name = "txtFolderUpdate";
            this.txtFolderUpdate.Size = new System.Drawing.Size(254, 22);
            this.txtFolderUpdate.TabIndex = 1;
            // 
            // txtFolderToUpdate
            // 
            this.txtFolderToUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderToUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFolderToUpdate.Location = new System.Drawing.Point(12, 32);
            this.txtFolderToUpdate.MaxLength = 260;
            this.txtFolderToUpdate.Name = "txtFolderToUpdate";
            this.txtFolderToUpdate.Size = new System.Drawing.Size(254, 22);
            this.txtFolderToUpdate.TabIndex = 3;
            this.txtFolderToUpdate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFolderToUpdate_KeyDown);
            this.txtFolderToUpdate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFolderToUpdate_KeyUp);
            // 
            // btnBrNew
            // 
            this.btnBrNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrNew.Location = new System.Drawing.Point(272, 32);
            this.btnBrNew.Name = "btnBrNew";
            this.btnBrNew.Size = new System.Drawing.Size(237, 22);
            this.btnBrNew.TabIndex = 4;
            this.btnBrNew.Text = "Путь к каталогам пользователей (F4)";
            this.btnBrNew.UseVisualStyleBackColor = true;
            this.btnBrNew.Click += new System.EventHandler(this.btnBrNew_Click);
            // 
            // btnPlan
            // 
            this.btnPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPlan.Location = new System.Drawing.Point(308, 21);
            this.btnPlan.Name = "btnPlan";
            this.btnPlan.Size = new System.Drawing.Size(184, 23);
            this.btnPlan.TabIndex = 6;
            this.btnPlan.Text = "Планировщик (F8)";
            this.btnPlan.UseVisualStyleBackColor = true;
            this.btnPlan.Click += new System.EventHandler(this.btnPlan_Click);
            // 
            // DOpenFolder
            // 
            this.DOpenFolder.ShowNewFolderButton = false;
            // 
            // btnUserDel
            // 
            this.btnUserDel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserDel.Location = new System.Drawing.Point(12, 403);
            this.btnUserDel.Name = "btnUserDel";
            this.btnUserDel.Size = new System.Drawing.Size(497, 23);
            this.btnUserDel.TabIndex = 12;
            this.btnUserDel.Text = "Удалить пользователя (Del)";
            this.btnUserDel.UseVisualStyleBackColor = true;
            this.btnUserDel.Click += new System.EventHandler(this.btnUserDel_Click);
            // 
            // tPlan
            // 
            this.tPlan.Interval = 5000;
            this.tPlan.Tick += new System.EventHandler(this.tPlan_Tick);
            // 
            // grpUsers
            // 
            this.grpUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpUsers.Controls.Add(this.btnDeleteTemplate);
            this.grpUsers.Controls.Add(this.txtTemplateName);
            this.grpUsers.Controls.Add(this.lstTemplate);
            this.grpUsers.Controls.Add(this.btnPlan);
            this.grpUsers.Controls.Add(this.btnSaveTemplate);
            this.grpUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grpUsers.Location = new System.Drawing.Point(12, 88);
            this.grpUsers.Name = "grpUsers";
            this.grpUsers.Size = new System.Drawing.Size(498, 142);
            this.grpUsers.TabIndex = 15;
            this.grpUsers.TabStop = false;
            this.grpUsers.Text = "Список шаблонов";
            // 
            // btnDeleteTemplate
            // 
            this.btnDeleteTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeleteTemplate.Location = new System.Drawing.Point(308, 111);
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.Size = new System.Drawing.Size(184, 23);
            this.btnDeleteTemplate.TabIndex = 9;
            this.btnDeleteTemplate.Text = "Удалить шаблон (Del)";
            this.btnDeleteTemplate.UseVisualStyleBackColor = true;
            this.btnDeleteTemplate.Click += new System.EventHandler(this.btnDeleteTemplate_Click);
            // 
            // txtTemplateName
            // 
            this.txtTemplateName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTemplateName.Location = new System.Drawing.Point(308, 50);
            this.txtTemplateName.MaxLength = 260;
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.Size = new System.Drawing.Size(184, 22);
            this.txtTemplateName.TabIndex = 7;
            this.txtTemplateName.Text = "<Имя шаблона>";
            this.txtTemplateName.Enter += new System.EventHandler(this.txtTemplateName_Enter);
            this.txtTemplateName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTemplateName_KeyDown);
            this.txtTemplateName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTemplateName_KeyUp);
            this.txtTemplateName.Leave += new System.EventHandler(this.txtTemplateName_Leave);
            // 
            // lstTemplate
            // 
            this.lstTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstTemplate.FormattingEnabled = true;
            this.lstTemplate.ItemHeight = 16;
            this.lstTemplate.Location = new System.Drawing.Point(6, 18);
            this.lstTemplate.Name = "lstTemplate";
            this.lstTemplate.Size = new System.Drawing.Size(296, 116);
            this.lstTemplate.TabIndex = 10;
            this.lstTemplate.SelectedIndexChanged += new System.EventHandler(this.lstTemplate_SelectedIndexChanged);
            this.lstTemplate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstTemplate_KeyUp);
            // 
            // btnUserAdd
            // 
            this.btnUserAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUserAdd.Location = new System.Drawing.Point(12, 374);
            this.btnUserAdd.Name = "btnUserAdd";
            this.btnUserAdd.Size = new System.Drawing.Size(497, 23);
            this.btnUserAdd.TabIndex = 11;
            this.btnUserAdd.Text = "Добавить пользователя (Insert)";
            this.btnUserAdd.UseVisualStyleBackColor = true;
            this.btnUserAdd.Click += new System.EventHandler(this.btnUserAdd_Click);
            // 
            // lstLogs
            // 
            this.lstLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstLogs.FormattingEnabled = true;
            this.lstLogs.HorizontalScrollbar = true;
            this.lstLogs.ItemHeight = 16;
            this.lstLogs.Location = new System.Drawing.Point(12, 461);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(497, 68);
            this.lstLogs.TabIndex = 14;
            // 
            // ni
            // 
            this.ni.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ni.BalloonTipText = "FileUpdater";
            this.ni.BalloonTipTitle = "FileUpdater здесь!";
            this.ni.Icon = ((System.Drawing.Icon)(resources.GetObject("ni.Icon")));
            this.ni.Text = "FileUpdater";
            this.ni.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ni_MouseUp);
            // 
            // btnRefreshUsersList
            // 
            this.btnRefreshUsersList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshUsersList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRefreshUsersList.Location = new System.Drawing.Point(12, 60);
            this.btnRefreshUsersList.Name = "btnRefreshUsersList";
            this.btnRefreshUsersList.Size = new System.Drawing.Size(254, 22);
            this.btnRefreshUsersList.TabIndex = 17;
            this.btnRefreshUsersList.Text = "Обновить список пользователей (F5)";
            this.btnRefreshUsersList.UseVisualStyleBackColor = true;
            this.btnRefreshUsersList.Click += new System.EventHandler(this.btnRefreshUsersList_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 541);
            this.Controls.Add(this.btnRefreshUsersList);
            this.Controls.Add(this.lstLogs);
            this.Controls.Add(this.btnUserAdd);
            this.Controls.Add(this.grpUsers);
            this.Controls.Add(this.btnUserDel);
            this.Controls.Add(this.btnBrNew);
            this.Controls.Add(this.txtFolderToUpdate);
            this.Controls.Add(this.txtFolderUpdate);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblSelUsers);
            this.Controls.Add(this.lstUser);
            this.Controls.Add(this.btnBrMainDir);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileUpdater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Shown += new System.EventHandler(this.MainFrm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFrm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFrm_KeyUp);
            this.Resize += new System.EventHandler(this.MainFrm_Resize);
            this.grpUsers.ResumeLayout(false);
            this.grpUsers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Button btnBrMainDir;
        private System.Windows.Forms.ListBox lstUser;
        private System.Windows.Forms.Label lblSelUsers;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtFolderUpdate;
        private System.Windows.Forms.TextBox txtFolderToUpdate;
        private System.Windows.Forms.Button btnBrNew;
        private System.Windows.Forms.Button btnPlan;
        private System.Windows.Forms.FolderBrowserDialog DOpenFolder;
        private System.Windows.Forms.Button btnUserDel;
        private System.Windows.Forms.Timer tPlan;
        private System.Windows.Forms.GroupBox grpUsers;
        private System.Windows.Forms.ListBox lstTemplate;
        private System.Windows.Forms.TextBox txtTemplateName;
        private System.Windows.Forms.Button btnDeleteTemplate;
        private System.Windows.Forms.Button btnUserAdd;
        private System.Windows.Forms.ListBox lstLogs;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.Button btnRefreshUsersList;
    }
}

