using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using PromptMaker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.ViewModels
{
    public class PromptMakerVM : ViewModelBase
    {
        #region プロンプト[Prompt]プロパティ
        /// <summary>
        /// プロンプト[Prompt]プロパティ用変数
        /// </summary>
        string _Prompt = string.Empty;
        /// <summary>
        /// プロンプト[Prompt]プロパティ
        /// </summary>
        public string Prompt
        {
            get
            {
                return _Prompt;
            }
            set
            {
                if (_Prompt == null || !_Prompt.Equals(value))
                {
                    _Prompt = value;
                    NotifyPropertyChanged("Prompt");
                }
            }
        }
        #endregion

        #region プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ
        /// <summary>
        /// プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ用変数
        /// </summary>
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        ConfigManager<ModelList<PromptConsistM>> _PromptComposerConf;
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        /// <summary>
        /// プロンプト構成要素コンフィグ[PromptComposerConf]プロパティ
        /// </summary>
        public ConfigManager<ModelList<PromptConsistM>> PromptComposerConf
        {
            get
            {
                return _PromptComposerConf;
            }
            set
            {
                if (_PromptComposerConf == null || !_PromptComposerConf.Equals(value))
                {
                    _PromptComposerConf = value;
                    NotifyPropertyChanged("PromptComposerConf");
                }
            }
        }
        #endregion

        #region フレーズリスト[PhreseList]プロパティ
        /// <summary>
        /// フレーズリスト[PhreseList]プロパティ用変数
        /// </summary>
        ModelList<PhraseM> _PhreseList = new ModelList<PhraseM>();
        /// <summary>
        /// フレーズリスト[PhreseList]プロパティ
        /// </summary>
        public ModelList<PhraseM> PhreseList
        {
            get
            {
                return _PhreseList;
            }
            set
            {
                if (_PhreseList == null || !_PhreseList.Equals(value))
                {
                    _PhreseList = value;
                    NotifyPropertyChanged("PhreseList");
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
                List<PhraseM> list = new List<PhraseM>();
                foreach (var prompt in this.PromptComposerConf.Item.Items)
                {
                    string[] phrase_list = prompt.Prompt.Split(",");

                    foreach (var phrase in phrase_list)
                    {
                        // 不要な文字列を削除
                        var phrase_tmp = phrase.Trim().Replace("\t"," ").ToLower();

                        // 空白のため次へ
                        if (string.IsNullOrWhiteSpace(phrase_tmp))
                        {
                            continue;
                        }

                        // 既に登録されているものを取り出し
                        var elem_list = (from x in list
                                       where x.Phrase.ToLower().Equals(phrase_tmp)
                                       select x);

                        // 一致するものがないので追加
                        if (!elem_list.Any())
                        {
                            PhraseM tmp = new PhraseM();        // オブジェクト作成
                            tmp.Phrase = phrase_tmp;            // フレーズ
                            tmp.Frequency = 1;                  // 頻度
                            list.Add(tmp);                      // 要素に追加
                        }
                        else
                        {
                            var elem = elem_list.First();       // 最初の要素取り出し
                            elem.Frequency ++;                  // 頻度のカウントアップ
                        }
                    }
                }

                // フレーズリストの更新
                this.PhreseList.Items = new System.Collections.ObjectModel.ObservableCollection<PhraseM>(list.OrderByDescending(x=>x.Frequency));
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region フレーズの翻訳
        /// <summary>
        /// フレーズの翻訳
        /// </summary>
        public void GoogleTranslate()
        {
            try
            {
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    string url = $"https://translate.google.co.jp/?sl=en&tl=ja&text={this.PhreseList.SelectedItem.Phrase}&op=translate";
                    MoveURL(url);
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region Google検索
        /// <summary>
        /// Google検索
        /// </summary>
        public void GoogleSearch()
        {
            try
            {
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    string url = $"https://www.google.co.jp/search?q={this.PhreseList.SelectedItem.Phrase}";
                    MoveURL(url);
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region Lexica検索
        /// <summary>
        /// Lexica検索
        /// </summary>
        public void LexicaSearch()
        {
            try
            {
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    string url = $"https://lexica.art/?q={this.PhreseList.SelectedItem.Phrase}";
                    MoveURL(url);
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region PromptからLexicaを検索する
        /// <summary>
        /// PromptからLexicaを検索する
        /// </summary>
        public void LexicaSearchFromPrompt()
        {
            try
            {
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    string url = $"https://lexica.art/?q={this.Prompt}";
                    MoveURL(url);
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region URLの移動処理
        /// <summary>
        /// URLの移動処理
        /// </summary>
        /// <param name="url"></param>
        private void MoveURL(string url)
        {
            try
            {
                ProcessStartInfo pi = new ProcessStartInfo()
                {
                    FileName = url,
                    UseShellExecute = true,
                };

                Process.Start(pi);
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region プロンプトに追加処理
        /// <summary>
        /// プロンプトに追加処理
        /// </summary>
        public void PromptAdd()
        {
            try
            {
                // nullチェック
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    // まだ追加されていない場合
                    if (!this.Prompt.Contains(this.PhreseList.SelectedItem.Phrase))
                    {
                        // まだ何も追加されていない場合
                        if (string.IsNullOrEmpty(this.Prompt))
                        {
                            this.Prompt += this.PhreseList.SelectedItem.Phrase;
                        }
                        // 既に何かが追加されている場合
                        else
                        {
                            this.Prompt += ", " + this.PhreseList.SelectedItem.Phrase;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region プロンプトの除去
        /// <summary>
        /// プロンプトの除去
        /// </summary>
        public void PromptDel()
        {
            try
            {
                // nullチェック
                if (this.PhreseList != null && this.PhreseList.SelectedItem != null)
                {
                    // 追加されている場合
                    if (this.Prompt.Contains(this.PhreseList.SelectedItem.Phrase))
                    {
                        this.Prompt = this.Prompt.Replace(", " + this.PhreseList.SelectedItem.Phrase, "");    // 消す
                        this.Prompt = this.Prompt.Replace(this.PhreseList.SelectedItem.Phrase, "");           // 消す
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region プロンプトのクリア処理
        /// <summary>
        /// プロンプトのクリア処理
        /// </summary>
        public void PromptClear()
        {
            try
            {
                this.Prompt = String.Empty;
            }
            catch (Exception e)
            {
                ShowMessage.ShowErrorOK(e.Message, "Error");
            }
        }
        #endregion

        #region 閉じる処理
        /// <summary>
        /// 閉じる処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Close(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}
