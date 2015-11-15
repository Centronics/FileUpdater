using System;
using System.Collections;
using System.Windows.Forms;

namespace FileUpdater
{
    public partial class Scheduler : Form
    {
        public Scheduler()
        {
            InitializeComponent();
        }

        public sealed class CSchedulTimes
        {
            readonly int Hours = -1, Minutes = -1;
            readonly SelectionRange Dates = null;

            public CSchedulTimes(int Hours, int Minutes, SelectionRange Dates)
            {
                if (Hours < 0 || Hours > 23 || Minutes < 0 || Minutes > 59)
                    throw new Exception("Время указано неверно");
                this.Hours = Hours;
                this.Minutes = Minutes;
                this.Dates = Dates;
            }

            public bool TimeEqual(int nHours, int nMins)
            {
                bool res = (nHours == Hours);
                res &= (nMins == Minutes);
                return res;
            }

            private bool DateIn
            {
                get
                {
                    if (Dates.Start == null || Dates.End == null)
                        return true;
                    DateTime dEnd = Dates.End, what = DateTime.Now;
                    if (dEnd.Year == what.Year)
                    {
                        if (dEnd.Month == what.Month)
                        {
                            if (dEnd.Day < what.Day)
                                return false;
                        }
                        else
                            if (dEnd.Month < what.Month)
                                return false;
                    }
                    else
                        if (dEnd.Year < what.Year)
                            return false;
                    DateTime dStart = Dates.Start;
                    if (dStart.Year == what.Year)
                    {
                        if (dStart.Month == what.Month)
                        {
                            if (dStart.Day <= what.Day)
                                return true;
                        }
                        else
                            if (dStart.Month < what.Month)
                                return true;
                    }
                    else
                        if (dStart.Year < what.Year)
                            return true;
                    return false;
                }
            }

            public bool InRange(SelectionRange what)
            {
                DateTime dStart = Dates.Start;
                if (what.End.Year == dStart.Year)
                {
                    if (what.End.Month == dStart.Month)
                    {
                        if (what.End.Day < dStart.Day)
                            return false;
                        else
                            if (what.End.Day == dStart.Day)
                                return true;
                    }
                    else
                        if (what.End.Month < dStart.Month)
                            return false;
                }
                else
                    if (what.End.Year < dStart.Year)
                        return false;
                DateTime dEnd = Dates.End;
                if (what.Start.Year == dEnd.Year)
                {
                    if (what.Start.Month == dEnd.Month)
                    {
                        if (what.Start.Day > dEnd.Day)
                            return false;
                    }
                    else
                        if (what.Start.Month > dEnd.Month)
                            return false;
                }
                else
                    if (what.Start.Year > dEnd.Year)
                        return false;
                return true;
            }

            private bool Now
            {
                get
                {
                    DateTime dt = DateTime.Now;
                    if (DateIn)
                        if (dt.Hour == Hours)
                            if (dt.Minute == Minutes)
                                return true;
                    return false;
                }
            }

            int NowSwitcherHours = -1, NowSwitcherMins = -1;
            public bool IsOnlyNow()
            {
                if (Now)
                {
                    if (NowSwitcherHours == -1)
                    {
                        NowSwitcherHours = Hours;
                        NowSwitcherMins = Minutes;
                        return true;
                    }
                    return false;
                }
                NowSwitcherMins = NowSwitcherHours = -1;
                return false;
            }
        }

        const string strExists = "Такое время уже задано";
        const string strTimesOverlap = "Пересекающиеся диапазоны дат с одинаковым временем";
        const int InsertPosition = 0;
        readonly ArrayList ListOfDT = new ArrayList();

        void AddTime()
        {
            if (string.IsNullOrEmpty(txtHours.Text))
                txtHours.Text = "00";
            if (string.IsNullOrEmpty(txtMins.Text))
                txtMins.Text = "00";
            if (txtMins.Text.Length == 1)
                txtMins.Text = txtMins.Text.Insert(0, "0");
            string sAddTime = txtHours.Text + ":" + txtMins.Text;
            if (!cEveryDay.Checked) sAddTime += " " +
                 calDates.SelectionRange.Start.ToShortDateString();
            if (calDates.SelectionRange.Start != calDates.SelectionRange.End)
                sAddTime += " - " + calDates.SelectionRange.End.ToShortDateString();
            if (!lstTimes.Items.Contains(sAddTime))
            {
                int iHours = Convert.ToInt32(txtHours.Text);
                int iMins = Convert.ToInt32(txtMins.Text);
                foreach (CSchedulTimes ct in ListOfDT)
                    if (ct.InRange(calDates.SelectionRange))
                    {
                        if (ct.TimeEqual(iHours, iMins))
                        {
                            MessageBox.Show(this, strTimesOverlap, "Сообщение",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                CSchedulTimes ctd = new CSchedulTimes(iHours, iMins, cEveryDay.Checked ? new SelectionRange() :
                    calDates.SelectionRange);
                ListOfDT.Insert(InsertPosition, ctd);
                lstTimes.Items.Insert(InsertPosition, sAddTime);
            }
            else
                MessageBox.Show(this, strExists, "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTime();
        }

        bool IsStarted = false;

        public bool Started
        {
            get
            {
                return IsStarted;
            }
        }

        public void Stop()
        {
            IsStarted = false;
        }

        int TimesIter = 0;

        public CSchedulTimes GetNextDate()
        {
            if (TimesIter < ListOfDT.Count)
                return (CSchedulTimes)ListOfDT[TimesIter++];
            TimesIter = 0;
            return null;
        }

        bool RemoveEnd = true;

        private void txtHours_TextChanged(object sender, EventArgs e)
        {
            if (RemoveEnd)
            {
                RemoveSymbols(txtHours);
                if (!string.IsNullOrEmpty(txtHours.Text))
                {
                    try
                    {
                        int Hour = Convert.ToInt32(txtHours.Text);
                        if (Hour > 23)
                            txtHours.Text = string.Empty;
                    }
                    catch
                    {
                        txtHours.Text = string.Empty;
                    }
                }
            }
        }

        private void txtMins_TextChanged(object sender, EventArgs e)
        {
            if (RemoveEnd)
            {
                RemoveSymbols(txtMins);
                if (!string.IsNullOrEmpty(txtMins.Text))
                {
                    try
                    {
                        int Min = Convert.ToInt32(txtMins.Text);
                        if (Min > 59)
                            txtMins.Text = string.Empty;
                    }
                    catch
                    {
                        txtMins.Text = string.Empty;
                    }
                }
            }
        }

        private void RemoveSymbols(TextBox TB)
        {
            if (TB == null) return;
            RemoveEnd = false;
            for (byte k = 0; k < TB.Text.Length; k++)
            {
                switch (TB.Text[k])
                {
                    case '0': continue;
                    case '1': continue;
                    case '2': continue;
                    case '3': continue;
                    case '4': continue;
                    case '5': continue;
                    case '6': continue;
                    case '7': continue;
                    case '8': continue;
                    case '9': continue;
                    default:
                        TB.Text = TB.Text.Remove(k, 1);
                        TB.Select(k--, 0);
                        continue;
                }
            }
            RemoveEnd = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTimes.SelectedIndex != -1)
                MainFrm.RemoveSelectedObjects(lstTimes, ListOfDT);
        }

        bool KeyDowned = false;

        private void Scheduler_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDowned = true;
        }

        private void Scheduler_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyDowned)
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                    case Keys.Enter:
                        AddTime();
                        break;
                    case Keys.F9:
                        btnOK_Click(null, null);
                        break;
                    case Keys.Oemtilde:
                        cEveryDay.Checked = !cEveryDay.Checked;
                        break;
                }
            KeyDowned = false;
        }

        private void lstTimes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDelete_Click(null, null);
        }

        private void Scheduler_Load(object sender, EventArgs e)
        {
            calDates.MinDate = DateTime.Now;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstTimes.Items.Count > 0)
            {
                IsStarted = true;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                MessageBox.Show(this, "Установите время!", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cEveryDay_CheckedChanged(object sender, EventArgs e)
        {
            calDates.Enabled = !cEveryDay.Checked;
        }
    }
}