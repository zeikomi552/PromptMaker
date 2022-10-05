using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Common;
using PromptMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.ViewModels
{
    internal class SettingVM : ViewModelBase
    {
        #region 共通変数
        /// <summary>
        /// 共通変数
        /// </summary>
        public GBLValues CommonValues
        {
            get
            {
                return GBLValues.GetInstance();
            }
        }
        #endregion

        #region 設定ファイルオブジェクト[SettingConf]プロパティ
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ用変数
        /// </summary>
        ConfigManager<SettingConfM> _SettingConf = new ConfigManager<SettingConfM>("Config", "Setting.conf", new SettingConfM());
        /// <summary>
        /// 設定ファイルオブジェクト[SettingConf]プロパティ
        /// </summary>
        public ConfigManager<SettingConfM> SettingConf
        {
            get
            {
                return _SettingConf;
            }
            set
            {
                if (_SettingConf == null || !_SettingConf.Equals(value))
                {
                    _SettingConf = value;
                    NotifyPropertyChanged("SettingConf");
                }
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Init(object sender, EventArgs e)
        {
            try
            {
                this.SettingConf.LoadXML();
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            try
            {
                if (ShowMessage.ShowQuestionYesNo("保存した後に画面を閉じます。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                {
                    this.SettingConf.SaveXML();
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region キャンセル
        /// <summary>
        /// キャンセル
        /// </summary>
        public void Cancel()
        {
            try
            {
                if (ShowMessage.ShowQuestionYesNo("保存せず画面を閉じます。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
                {
                    this.DialogResult = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region クローズ
        /// <summary>
        /// クローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Close(object sender, EventArgs e)
        {
            if (ShowMessage.ShowQuestionYesNo("保存せず画面を閉じます。よろしいですか？", "確認") == System.Windows.MessageBoxResult.Yes)
            {
                this.DialogResult = false;
            }
        }
        #endregion
    }
}
