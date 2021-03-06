﻿using FSLib.App.SimpleUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConfig
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isAppRunning = false;
            Mutex mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out isAppRunning);
            if (!isAppRunning)
            {
                MessageBox.Show("程序已在运行！");
                Environment.Exit(1);
            }
            var updater = Updater.CreateUpdaterInstance();
            updater.Error += (s, e) =>
            {
                MessageBox.Show("更新发生了错误：" + updater.Context.Exception.Message, "自动更新");
            };
            updater.NoUpdatesFound += (s, e) =>
            {
                //MessageBox.Show("当前版本已为最新！", "自动更新");
            };
            updater.MinmumVersionRequired += (s, e) =>
            {
                MessageBox.Show("当前版本过低无法使用自动更新！", "自动更新");
            };
            Updater.CheckUpdateSimple();

            if (!updater.Context.HasUpdate)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }
    }
}
