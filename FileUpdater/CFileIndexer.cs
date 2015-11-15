using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FileUpdater
{
    sealed class CFileIndexer
    {
        const string strExecuting = "Выполнено ", strReady = "Готово", strOLD = "_OLD";
        readonly string PathFrom = string.Empty; readonly FileAttributes PathFromAttrs = FileAttributes.Normal;
        readonly MainFrm frmMain = null;

        public CFileIndexer(string PathFrom, MainFrm frm)
        {
            if (string.IsNullOrEmpty(PathFrom))
                throw new Exception("Для поиска файлов задан пустой путь");
            if (frm == null)
                throw new Exception("Необходима ссылка на форму");
            this.PathFrom = PathFrom;
            frmMain = frm;
            DirectoryInfo dipf = new DirectoryInfo(PathFrom);
            PathFromAttrs = dipf.Attributes;
        }

        volatile bool Stopped = true;
        Thread MainThread = null;
        object ThisLock = new object();

        public void CopyFilesInOtherThread(string[] PathTo)
        {
            lock (ThisLock)
            {
                try
                {
                    if (!Stopped)
                        throw new Exception("Процесс уже запущен");
                    if (PathTo == null)
                        throw new Exception("Невозможно скопировать по пустому пути");
                    if (MainThread != null)
                        throw new Exception("Поток уже существует");
                    MainThread = new Thread(new ParameterizedThreadStart(ThrCopyFunction));
                    MainThread.Name = "UpdaterThread";
                    MainThread.Priority = ThreadPriority.AboveNormal;
                    Stopped = false;
                    MainThread.Start(PathTo);
                }
                catch
                {
                    Stopped = true;
                    MainThread = null;
                    throw;
                }
            }
        }

        delegate void delPerc(byte Percent);
        void SetPercent(byte Percent)
        {
            if (Percent < 100)
                frmMain.Text = strExecuting + Percent.ToString() + '%';
            else
            {
                frmMain.SetWorkingMode(false);
                switch (Percent)
                {
                    case 100:
                        frmMain.Text = strReady;
                        return;
                    default:
                        frmMain.Text = "Ошибка";
                        return;
                }
            }
        }

        public bool Stop(int Waiting = -1)
        {
            Thread tmpThr;
            lock (ThisLock)
            {
                tmpThr = MainThread;
                if (Stopped || tmpThr == null)
                {
                    Stopped = true;
                    if (MainThread != null)
                    {
                        MainThread.Join();
                        MainThread = null;
                    }
                    return true;
                }
                Stopped = true;
            }
            if (tmpThr.ThreadState != ThreadState.Unstarted)
                if (Waiting < 0)
                    tmpThr.Join();
                else
                    return tmpThr.Join(Waiting);
            return true;
        }

        public bool Running
        {
            get
            {
                lock (ThisLock)
                {
                    if (Stopped || MainThread == null)
                        return false;
                }
                return true;
            }
        }

        bool CopyFolder(string PathTo, FileInfo[] FromFiles, DirectoryInfo DirFrom, decimal SumLen, ref decimal Count)
        {
            string Rename = PathTo + strOLD;
            DirectoryInfo toDir = new DirectoryInfo(PathTo);
            DirectoryInfo di = new DirectoryInfo(Rename);
            if (di.Exists)
            {
                SetDirectoryAttributes(di);
                if (Stopped)
                    return false;
                SetFileAttributes(di);
                if (Stopped)
                    return false;
                di.Delete(true);
            }
            if (toDir.Exists)
                Directory.Move(toDir.FullName, Rename);
            toDir.Create();
            toDir.Attributes = PathFromAttrs;
            DirectoryInfo[] FromDirectories = DirFrom.GetDirectories("*", SearchOption.AllDirectories);
            if (PathTo[PathTo.Length - 1] != '\\') PathTo += '\\';
            foreach (DirectoryInfo dir in FromDirectories)
            {
                string strPathTo = PathTo + dir.FullName.Remove(0, PathFrom.Length);
                DirectoryInfo df = Directory.CreateDirectory(strPathTo);
                df.Attributes = dir.Attributes;
                if (Stopped)
                    return false;
            }
            byte CurPercent = byte.MaxValue; delPerc dp = new delPerc(SetPercent);
            for (int iter = 0; iter < FromFiles.Length; iter++)
            {
                string strPathTo = PathTo + FromFiles[iter].FullName.Remove(0, PathFrom.Length);
                FromFiles[iter].CopyTo(strPathTo, true);
                Count++;
                decimal Percent = Count / SumLen;
                Percent *= 100.0m;
                byte tPerc = Convert.ToByte(Percent);
                if (tPerc != CurPercent)
                {
                    CurPercent = tPerc;
                    frmMain.Invoke(dp, CurPercent);
                }
                if (Stopped)
                    return false;
            }
            return true;
        }

        void ThrCopyFunction(object tmparg)
        {
            try
            {
                string[] PathTo = (string[])tmparg;
                DirectoryInfo dif = new DirectoryInfo(PathFrom);
                FileInfo[] FromFiles = dif.GetFiles("*", SearchOption.AllDirectories);
                decimal SumLen = Convert.ToDecimal(FromFiles.Length) * Convert.ToDecimal(PathTo.Length);
                decimal Count = 0.0m; delPerc dp = new delPerc(SetPercent);
                foreach (string pathto in PathTo)
                {
                    if (!string.IsNullOrEmpty(pathto))
                    {
                        try
                        {
                            if (CopyFolder(pathto, FromFiles, dif, SumLen, ref Count))
                                frmMain.LogWrite("Скопировано: " + pathto);
                            else
                                return;
                        }
                        catch (Exception ex)
                        {
                            frmMain.LogWrite(ex.Message);
                        }
                    }
                    if (Stopped)
                        return;
                }
                frmMain.Invoke(dp, (byte)100);
                frmMain.LogWrite("Операция завершена успешно");
            }
            catch (Exception ex)
            {
                try
                {
                    delPerc dp = new delPerc(SetPercent);
                    frmMain.Invoke(dp, byte.MaxValue);
                    frmMain.LogWrite(ex.Message);
                }
                catch (Exception iex)
                {
                    frmMain.Invoke((Action)delegate()
                    {
                        frmMain.LogWrite(ex.Message);
                        MessageBox.Show(frmMain, iex.Message);
                    });
                }
            }
            lock (ThisLock)
            {
                Stopped = true;
                MainThread = null;
            }
        }

        void SetFileAttributes(DirectoryInfo Target)
        {
            if (Target.Exists)
            {
                FileInfo[] files = Target.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                {
                    if (Stopped)
                        return;
                    File.SetAttributes(file.FullName, FileAttributes.Normal);
                }
            }
        }

        void SetDirectoryAttributes(DirectoryInfo Target)
        {
            if (Target.Exists)
            {
                DirectoryInfo[] Dirs = Target.GetDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo dir in Dirs)
                {
                    if (Stopped)
                        return;
                    dir.Attributes = FileAttributes.Normal;
                }
            }
        }
    }
}