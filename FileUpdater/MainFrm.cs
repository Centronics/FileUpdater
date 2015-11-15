using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FileUpdater
{
    public partial class MainFrm : Form
    {
        readonly string LogFile = Application.StartupPath;
        readonly string TemplateFile = Application.StartupPath;
        readonly string strBtnStart = string.Empty;

        public MainFrm()
        {
            InitializeComponent();
            LogFile += "\\FileUpdaterLogs.txt";
            TemplateFile += "\\templates.txt";
            strBtnStart = btnGo.Text;
        }

        const string strBtnCancel = "Отмена (F9)";
        const string strVTO = "Пересечение с папкой, которую нужно обновить",
            strVU = "Пересечение с папкой, которая содержит обновления",
            strVR = "Пересечение с рабочей папкой приложения";
        const string strPaths = "Не все пути указаны верно";
        const string strUserLabel = "USER";
        const string strDefTemplateName = "<Имя шаблона>";
        const char cchNewTemplate = '?';

        enum EPaths
        {
            ALL = 0,
            UPDATES,
            FOLDERTOUPDATES
        }

        readonly Scheduler schedul = new Scheduler();
        readonly ArrayList ListOfTemplates = new ArrayList();
        CFileIndexer CFI = null;
        bool FolderUpdateOK = false, FolderToUpdateOK = false, AlreadyPressed = false;
        object ThisLock = new object();

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            if (!btnSaveTemplate.Enabled) return;
            if (string.IsNullOrEmpty(txtTemplateName.Text))
            {
                MessageBox.Show(this, "Имя профиля не может быть пустым", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstUser.Items.Count > 0)
            {
                try
                {
                    if (string.Compare(txtTemplateName.Text, strDefTemplateName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (txtTemplateName.Focused)
                        {
                            MessageBox.Show(this, "Недопустимое имя шаблона", "Имя",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtTemplateName.Text = string.Empty;
                            return;
                        }
                        txtTemplateName.Text = string.Empty;
                        txtTemplateName.Select();
                        return;
                    }
                    foreach (string tName in lstTemplate.Items)
                        if (string.Compare(tName, txtTemplateName.Text, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (txtTemplateName.Focused)
                            {
                                MessageBox.Show(this, "Шаблон с таким именем уже присутствует", "Сообщение",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtTemplateName.Select(0, txtTemplateName.Text.Length);
                                return;
                            }
                            txtTemplateName.Select();
                            txtTemplateName.Select(0, txtTemplateName.Text.Length);
                            return;
                        }
                    ArrayList ArrRecords = new ArrayList();
                    foreach (string str in lstUser.Items)
                        if (!string.IsNullOrEmpty(str))
                            ArrRecords.Add(str);
                    ListOfTemplates.Insert(0, ArrRecords);
                    lstTemplate.Items.Insert(0, txtTemplateName.Text);
                    SaveTemplate();
                    lstTemplate.Select();
                    lstTemplate.SetSelected(0, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Ошибка при сохранении шаблонов",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }
            MessageBox.Show(this, "Список пользователей пуст", "Сообщение",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void SaveTemplate()
        {
            using (StreamWriter sw = new StreamWriter(TemplateFile, false, System.Text.Encoding.Unicode))
            {
                int count = 0;
                foreach (ArrayList lst in ListOfTemplates)
                {
                    if (lst.Count > 0)
                    {
                        sw.Write(cchNewTemplate);
                        sw.WriteLine(lstTemplate.Items[count++]);
                        foreach (string stp in lst)
                            sw.WriteLine(stp);
                    }
                    else
                        continue;
                }
            }
        }

        private void MainFrm_Shown(object sender, EventArgs e)
        {
            try
            {
                string[] strsOfTemplates = null;
                try
                {
                    strsOfTemplates = File.ReadAllLines(TemplateFile);
                }
                catch { return; }
                byte FirstLoaded = 0;
                ArrayList ArrNames = null;
                foreach (string sName in strsOfTemplates)
                {
                    if (!string.IsNullOrEmpty(sName))
                    {
                        if (sName[0] != cchNewTemplate)
                        {
                            if (FirstLoaded < 1) continue;
                            char dch;
                            if (!ValidTemplateName(sName, out dch))
                                throw new Exception("В имени пользователя " + sName +
                                    " в списке " + lstTemplate.Items[lstTemplate.Items.Count - 1] +
                                    " есть недопустимые символы: " + dch);
                            if (FirstLoaded == 1)
                            {
                                foreach (string tName in lstUser.Items)
                                    if (string.Compare(sName, tName, StringComparison.OrdinalIgnoreCase) == 0)
                                        throw new Exception("В списке " + lstTemplate.Items[lstTemplate.Items.Count - 1] +
                                            " присутствуют одинаковые пользователи");
                                ArrNames.Add(sName);
                                lstUser.Items.Add(sName);
                            }
                            else
                                if (!ArrNames.Contains(sName))
                                    ArrNames.Add(sName);
                            continue;
                        }
                        lstTemplate.Items.Add(sName.Remove(0, 1));
                        ArrNames = new ArrayList();
                        ListOfTemplates.Add(ArrNames);
                        if (FirstLoaded < 2)
                            FirstLoaded++;
                    }
                }
                if (lstUser.Items.Count > 0)
                    lstUser.SetSelected(0, true);
                lstUser.Select();
            }
            catch (Exception ex)
            {
                lstTemplate.Items.Clear();
                ListOfTemplates.Clear();
                lstUser.Items.Clear();
                LogWrite(ex.Message);
                MessageBox.Show(this, ex.Message, "Ошибка при загрузке шаблонов",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lstTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTemplate.SelectedIndex >= 0)
            {
                txtTemplateName.Text = (string)lstTemplate.SelectedItem;
                ArrayList al = (ArrayList)ListOfTemplates[lstTemplate.SelectedIndex];
                lstUser.Items.Clear();
                foreach (string sName in al)
                    if (!string.IsNullOrEmpty(sName))
                        lstUser.Items.Add(sName);
            }
        }

        public void LogWrite(string logstr)
        {
            try
            {
                if (string.IsNullOrEmpty(logstr))
                {
                    Invoke((Action)delegate()
                    {
                        MessageBox.Show(this, "Ошибка при записи логов", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    });
                    return;
                }
                lock (ThisLock)
                {
                    logstr = DateTime.Now.ToString() + " " + logstr;
                    Invoke((Action)delegate()
                    {
                        lstLogs.Items.Insert(0, logstr);
                        lstLogs.SetSelected(0, true);
                    });
                    using (StreamWriter sw = new StreamWriter(LogFile, true, System.Text.Encoding.Unicode))
                    {
                        sw.WriteLine(logstr);
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke((Action)delegate()
                {
                    MessageBox.Show(this, ex.Message, "Ошибка при записи логов",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                });
            }
        }

        void ToWorkingMode(bool On)
        {
            lock (ThisLock)
            {
                if (On)
                    btnGo.Text = strBtnCancel;
                else
                    btnGo.Text = strBtnStart;
                foreach (Control ct in Controls)
                {
                    if (ct.GetType() == typeof(Button))
                    {
                        if (ct.Name != "btnGo")
                            ct.Enabled = !On;
                    }
                }
                grpUsers.Enabled = !On;
                lstUser.Enabled = !On;
                lblSelUsers.Enabled = !On;
            }
        }

        public void SetWorkingMode(bool On)
        {
            if (!schedul.Started)
                ToWorkingMode(On);
        }

        static string GetFolderName(string strPath)
        {
            int cou = strPath.Length - 1;
            if (strPath[cou] == '\\')
                strPath = strPath.Remove(cou--);
            for (; cou >= 0; cou--)
            {
                if (strPath[cou] == '\\')
                {
                    cou++;
                    break;
                }
            }
            if (strPath.Length > cou)
            {
                if (cou < 0)
                    cou = 0;
                return strPath.Remove(0, cou);
            }
            return strPath;
        }

        string GetUserName(string strFolderName)
        {
            if (strFolderName.StartsWith(strUserLabel, StringComparison.Ordinal))
                return strFolderName;
            return string.Empty;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            _Go();
        }

        void _Go()
        {
            try
            {
                if (!btnGo.Enabled)
                    return;
                if (schedul.Started)
                {
                    tPlan.Stop();
                    if (CFI != null)
                        CFI.Stop();
                    CFI = null;
                    Text = "Расписание отменено пользователем";
                    LogWrite("Расписание отменено пользователем");
                    schedul.Stop();
                    ToWorkingMode(false);
                    return;
                }
                if (CFI != null)
                {
                    if (CFI.Running)
                    {
                        if (CFI.Stop())
                        {
                            ToWorkingMode(false);
                            Text = "Отменено пользователем";
                            LogWrite("Процесс был остановлен пользователем");
                        }
                        CFI = null;
                        return;
                    }
                    CFI = null;
                }
                if (ExitGuar)
                    return;
                if (!VerUpdatePath() || !VerToUpdatePath())
                    return;
                if (!TestPaths())
                    return;
                ListBox.ObjectCollection allObjs = lstUser.Items;
                if (allObjs.Count > 0)
                {
                    ListBox.SelectedObjectCollection selObjs = lstUser.SelectedItems;
                    System.Windows.Forms.DialogResult dres;
                    if (selObjs.Count <= 0)
                        dres = System.Windows.Forms.DialogResult.No;
                    else
                        if (selObjs.Count < allObjs.Count)
                            dres = MessageBox.Show(this, "Обновить только выделенные?", "Вопрос",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        else
                            dres = System.Windows.Forms.DialogResult.No;
                    string[] strArgs;
                    switch (dres)
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            strArgs = new string[selObjs.Count];
                            selObjs.CopyTo(strArgs, 0);
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            strArgs = new string[allObjs.Count];
                            allObjs.CopyTo(strArgs, 0);
                            break;
                        default: return;
                    }
                    LogWrite("Операция начата пользователем");
                    StartProcedure(strArgs);
                }
            }
            catch (Exception ex)
            {
                CFI = null;
                LogWrite(ex.Message);
            }
        }

        private void tPlan_Tick(object sender, EventArgs e)
        {
            try
            {
                if (schedul.Started)
                {
                    Scheduler.CSchedulTimes time;
                    while ((time = schedul.GetNextDate()) != null)
                    {
                        if (time.IsOnlyNow())
                        {
                            if (CFI != null)
                                if (CFI.Running)
                                {
                                    LogWrite("Процесс по расписанию не был запущен из-за того, что предыдущий процесс не был завершён");
                                    return;
                                }
                            if (lstUser.Items.Count > 0)
                            {
                                string[] strArgs = new string[lstUser.Items.Count];
                                lstUser.Items.CopyTo(strArgs, 0);
                                LogWrite("Операция начата по расписанию");
                                StartProcedure(strArgs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CFI = null;
                LogWrite(ex.Message);
            }
        }

        void GetPathsToCopy(string[] strArgs)
        {
            bool ReadyToContinue = false;
            for (int k = 0; k < strArgs.Length; k++)
            {
                if (txtFolderToUpdate.Text[txtFolderToUpdate.Text.Length - 1] != '\\')
                    txtFolderToUpdate.Text += '\\';
                string verify = txtFolderToUpdate.Text + strArgs[k];
                if (!Directory.Exists(verify))
                {
                    strArgs[k] = string.Empty;
                    LogWrite("Не найдено: " + verify);
                    continue;
                }
                verify = strArgs[k] + "\\" + GetFolderName(txtFolderUpdate.Text);
                verify = txtFolderToUpdate.Text + verify;
                strArgs[k] = verify;
                ReadyToContinue = true;
            }
            if (!ReadyToContinue)
                throw new Exception("Поскольку папки пользователей не существуют, операция не началась");
        }

        private void btnBrMainDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnBrMainDir.Enabled)
                    return;
                if (!string.IsNullOrEmpty(txtFolderUpdate.Text))
                    DOpenFolder.SelectedPath = txtFolderUpdate.Text;
                else
                    DOpenFolder.SelectedPath = txtFolderToUpdate.Text;
                if (DOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string tstr = DOpenFolder.SelectedPath;
                    if (!TestPaths(tstr, EPaths.UPDATES))
                        return;
                    FolderUpdateOK = true;
                    if (tstr[tstr.Length - 1] != '\\') tstr += '\\';
                    txtFolderUpdate.Text = tstr;
                    if (FolderToUpdateOK)
                        EnableButtons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnBrNew_Click(object sender, EventArgs e)
        {
            if (!btnBrNew.Enabled)
                return;
            try
            {
                if (!string.IsNullOrEmpty(txtFolderToUpdate.Text))
                    DOpenFolder.SelectedPath = txtFolderToUpdate.Text;
                else
                    DOpenFolder.SelectedPath = txtFolderUpdate.Text;
                if (DOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string tstr = DOpenFolder.SelectedPath;
                    if (!TestPaths(tstr, EPaths.FOLDERTOUPDATES))
                        return;
                    if (tstr[tstr.Length - 1] != '\\') tstr += '\\';
                    string MainPath = tstr;
                    if (RefreshUserList(tstr))
                        return;
                    if (MessageBox.Show(this, "Пользователи не найдены, но есть возможность добавить пользователей вручную" +
                    Environment.NewLine + "Хотите добавить их вручную?", "Вопрос",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        while (true)
                        {
                            if (DOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                tstr = DOpenFolder.SelectedPath;
                                if (tstr[tstr.Length - 1] != '\\') tstr += '\\';
                                if (!tstr.StartsWith(MainPath, StringComparison.OrdinalIgnoreCase))
                                    if (MessageBox.Show(this, "Путь должен начинаться с " + txtFolderToUpdate.Text +
                                        Environment.NewLine + "Желаете повторить попытку?", "Вопрос",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                                        System.Windows.Forms.DialogResult.Yes)
                                        continue;
                                    else break;
                                lstUser.Items.Add(GetFolderName(tstr));
                                FolderToUpdateOK = true;
                                txtFolderToUpdate.Text = MainPath;
                                Text = "Пользователь добавлен";
                                lstUser.SetSelected(0, true);
                                lstUser.Select();
                                if (FolderUpdateOK)
                                    EnableButtons();
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        bool RefreshUserList(string PathStr)
        {
            lstUser.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(PathStr);
            DirectoryInfo[] dii = di.GetDirectories("*", SearchOption.TopDirectoryOnly);
            if (dii.Length > 0)
            {
                lstUser.BeginUpdate();
                foreach (DirectoryInfo dir in dii)
                {
                    string strName = GetUserName(dir.Name);
                    if (!string.IsNullOrEmpty(strName))
                        lstUser.Items.Add(strName);
                }
                lstUser.EndUpdate();
                if (lstUser.Items.Count > 0)
                {
                    FolderToUpdateOK = true;
                    txtFolderToUpdate.Text = PathStr;
                    Text = "FileUpdater - Найдено: " + lstUser.Items.Count.ToString();
                    lstUser.SetSelected(0, true);
                    lstUser.Select();
                    if (FolderUpdateOK)
                        EnableButtons();
                    return true;
                }
            }
            return false;
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            if (!btnUserAdd.Enabled) return;
            try
            {
                while (true)
                {
                    if (DOpenFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string tstr = DOpenFolder.SelectedPath;
                        if (tstr[tstr.Length - 1] != '\\') tstr += '\\';
                        if (!tstr.StartsWith(txtFolderToUpdate.Text, StringComparison.OrdinalIgnoreCase))
                            if (MessageBox.Show(this, "Путь должен начинаться с " + txtFolderToUpdate.Text +
                                Environment.NewLine + "Желаете повторить попытку?", "Вопрос",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                                System.Windows.Forms.DialogResult.Yes)
                                continue;
                            else return;
                        string UserName = GetFolderName(tstr);
                        foreach (string tName in lstUser.Items)
                            if (string.Compare(tName, UserName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MessageBox.Show(this, "Этот пользователь уже присутствует в списке", "Сообщение",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        lstUser.Items.Add(UserName);
                        lstUser.SetSelected(0, true);
                        lstUser.Select();
                        FolderToUpdateOK = true;
                        Text = "Пользователь добавлен";
                        return;
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainFrm_KeyDown(object sender, KeyEventArgs e)
        {
            AlreadyPressed = true;
        }

        private void MainFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (AlreadyPressed)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        ExitGuar = true;
                        _Go();
                        Application.Exit();
                        break;
                    case Keys.F9:
                        _Go();
                        break;
                    case Keys.F8:
                        btnPlan_Click(null, null);
                        break;
                    case Keys.F3:
                        btnBrMainDir_Click(null, null);
                        break;
                    case Keys.F4:
                        btnBrNew_Click(null, null);
                        break;
                    case Keys.F5:
                        btnRefreshUsersList_Click(null, null);
                        break;
                    case Keys.F11:
                        switch (WindowState)
                        {
                            case FormWindowState.Maximized:
                                WindowState = FormWindowState.Normal;
                                break;
                            case FormWindowState.Normal:
                                WindowState = FormWindowState.Maximized;
                                break;
                        }
                        break;
                    case Keys.S:
                        if (e.Control)
                            btnSaveTemplate_Click(null, null);
                        break;
                    case Keys.Insert:
                        btnUserAdd_Click(null, null);
                        break;
                }
            }
            AlreadyPressed = false;
        }

        private void btnPlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnPlan.Enabled)
                    return;
                if (!TestPaths())
                    return;
                if (schedul.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    if (schedul.Started)
                    {
                        ToWorkingMode(true);
                        Text = "Расписание установлено пользователем";
                        LogWrite(Text);
                        tPlan.Start();
                        return;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool TestPaths(string Verify = "", EPaths paths = EPaths.ALL)
        {
            if (Verify == null)
                return false;
            if (paths == EPaths.ALL)
            {
                if (!FolderUpdateOK || !FolderToUpdateOK)
                {
                    MessageBox.Show(this, strPaths, "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                return true;
            }
            if (string.IsNullOrEmpty(Verify))
                return true;
            switch (paths)
            {
                case EPaths.UPDATES:
                    if (!string.IsNullOrEmpty(txtFolderToUpdate.Text))
                    {
                        if (txtFolderToUpdate.Text.StartsWith(Verify,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(this, strVTO, "Сообщение",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        if (Verify.StartsWith(txtFolderToUpdate.Text,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(this, strVTO, "Сообщение",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                    break;
                case EPaths.FOLDERTOUPDATES:
                    if (!string.IsNullOrEmpty(txtFolderUpdate.Text))
                    {
                        if (txtFolderUpdate.Text.StartsWith(Verify,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(this, strVU, "Сообщение",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        if (Verify.StartsWith(txtFolderUpdate.Text,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(this, strVU, "Сообщение",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                    break;
            }
            if (string.Compare(Verify, Application.StartupPath, StringComparison.OrdinalIgnoreCase) == 0)
            {
                MessageBox.Show(this, strVR, "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.Compare(Application.StartupPath, Verify, StringComparison.OrdinalIgnoreCase) == 0)
            {
                MessageBox.Show(this, strVR, "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        bool ValidTemplateName(string sName, out char chr)
        {
            if (string.IsNullOrEmpty(sName))
            {
                chr = '\0';
                return false;
            }
            char[] dsymbols = Path.GetInvalidPathChars();
            foreach (char ch in dsymbols)
                if (sName.IndexOf(ch) != -1)
                {
                    chr = ch;
                    return false;
                }
            dsymbols = Path.GetInvalidFileNameChars();
            foreach (char ch in dsymbols)
                if (sName.IndexOf(ch) != -1)
                {
                    chr = ch;
                    return false;
                }
            chr = '\0';
            return true;
        }

        private void lstTemplate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDeleteTemplate_Click(null, null);
        }

        private void btnDeleteTemplate_Click(object sender, EventArgs e)
        {
            if (!btnDeleteTemplate.Enabled) return;
            RemoveSelectedObjects(lstTemplate, ListOfTemplates);
            SaveTemplate();
        }

        private void btnUserDel_Click(object sender, EventArgs e)
        {
            if (!btnUserDel.Enabled) return;
            RemoveSelectedObjects(lstUser);
        }

        private void lstUser_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    btnUserDel_Click(null, null);
                    break;
                case Keys.A:
                    if (e.Control)
                        for (int k = 0; k < lstUser.Items.Count; k++)
                            lstUser.SetSelected(k, true);
                    break;
            }
        }

        public static void RemoveSelectedObjects(ListBox lst, ArrayList Arr = null)
        {
            if (lst == null) return;
            int MaxPos = 0;
            for (int k = 0; k < lst.Items.Count; k++)
            {
                if (lst.GetSelected(k))
                {
                    MaxPos = k;
                    lst.Items.RemoveAt(k);
                    if (Arr != null)
                        Arr.RemoveAt(k);
                    k--;
                }
            }
            if (lst.Items.Count > 0)
                if (MaxPos < lst.Items.Count)
                    lst.SetSelected(MaxPos, true);
                else
                    lst.SetSelected(lst.Items.Count - 1, true);
        }

        private void txtTemplateName_Enter(object sender, EventArgs e)
        {
            if (string.Compare(txtTemplateName.Text, strDefTemplateName, StringComparison.OrdinalIgnoreCase) == 0)
                txtTemplateName.Text = string.Empty;
        }

        private void txtTemplateName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTemplateName.Text))
                txtTemplateName.Text = strDefTemplateName;
        }

        bool ExitGuar = false;

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitGuar = true;
            _Go();
        }

        bool txtTempNamePressed = false;

        private void txtTemplateName_KeyDown(object sender, KeyEventArgs e)
        {
            txtTempNamePressed = true;
        }

        private void txtTemplateName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtTempNamePressed)
                btnSaveTemplate_Click(null, null);
            txtTempNamePressed = false;
        }

        void EnableButtons()
        {
            btnUserAdd.Enabled = true;
            btnGo.Enabled = true;
            btnPlan.Enabled = true;
        }

        void StartProcedure(string[] strArgs)
        {
            GetPathsToCopy(strArgs);
            Text = "Очистка старых файлов...";
            LogWrite(Text);
            ToWorkingMode(true);
            CFI = new CFileIndexer(txtFolderUpdate.Text, this);
            CFI.CopyFilesInOtherThread(strArgs);
        }

        private void MainFrm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ni.Visible = true;
                Hide();
                ni.ShowBalloonTip(7000);
            }
        }

        private void ni_MouseUp(object sender, MouseEventArgs e)
        {
            Show();
            if (WindowState != FormWindowState.Normal)
                WindowState = FormWindowState.Normal;
            ni.Visible = false;
        }

        bool VerUpdatePath()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFolderUpdate.Text))
                {
                    if (Directory.Exists(txtFolderUpdate.Text))
                    {
                        if (!TestPaths(txtFolderUpdate.Text, EPaths.UPDATES))
                        {
                            FolderUpdateOK = false;
                            return false;
                        }
                        FolderUpdateOK = true;
                        if (FolderToUpdateOK)
                            EnableButtons();
                    }
                    else
                    {
                        MessageBox.Show(this, "Путь обновлений не существует");
                        FolderUpdateOK = false;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            FolderUpdateOK = false;
            return false;
        }

        bool VerToUpdatePath()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFolderToUpdate.Text))
                {
                    if (Directory.Exists(txtFolderToUpdate.Text))
                    {
                        if (!TestPaths(txtFolderToUpdate.Text, EPaths.FOLDERTOUPDATES))
                        {
                            FolderToUpdateOK = false;
                            return false;
                        }
                        FolderToUpdateOK = true;
                        if (FolderUpdateOK)
                            EnableButtons();
                    }
                    else
                    {
                        MessageBox.Show(this, "Путь для обновлений не существует");
                        FolderToUpdateOK = false;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            FolderToUpdateOK = false;
            return false;
        }

        private void btnRefreshUsersList_Click(object sender, EventArgs e)
        {
            if (!btnRefreshUsersList.Enabled)
                return;
            try
            {
                if (VerToUpdatePath())
                    RefreshUserList(txtFolderToUpdate.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        bool FolderToUpdateKeyDown = false;

        private void txtFolderToUpdate_KeyUp(object sender, KeyEventArgs e)
        {
            if (FolderToUpdateKeyDown)
                if (e.KeyCode == Keys.Enter)
                    btnRefreshUsersList_Click(null, null);
            FolderToUpdateKeyDown = false;
        }

        private void txtFolderToUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            FolderToUpdateKeyDown = true;
        }
    }
}