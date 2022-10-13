using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace PromptMaker.Models
{
    public class GfpGanM
    {
        #region GFPGANの実行
        /// <summary>
        /// GFPGANの実行
        /// </summary>
        /// <param name="inference_gfpgan_path">スクリプトファイル</param>
        /// <param name="inputfile">入力ファイル</param>
        /// <param name="outfilename">出力先ファイル名</param>
        /// <returns>true:成功 false:失敗</returns>
        public static bool Execute(string inference_gfpgan_path, string inputfile, string outfilename)
        {
            try
            {
                // 一時フォルダの取得
                string tempDir = Path.GetTempPath();

                // 新しいGUIDを生成する
                var guid = Guid.NewGuid();

                // 一時フォルダの作成
                tempDir = Path.Combine(tempDir, "PromptMaker-" + guid.ToString());
                PathManager.CreateDirectory(tempDir);

                // ファイルの存在確認
                if (File.Exists(inputfile))
                {
                    // ファイル名の取得
                    var fileName = System.IO.Path.GetFileName(inputfile);

                    // ターゲットファイルパス
                    string tgtFilePath = Path.Combine(tempDir, fileName);

                    // ファイルのコピー（同じ名前のファイルがある場合は上書き）
                    File.Copy(inputfile, tgtFilePath, true);
                }
                else
                {
                    return false;
                }

                var myProcess = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        WorkingDirectory = tempDir
                    }
                };

                myProcess.Start();
                using (var sw = myProcess.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        // Vital to activate Anaconda
                        sw.WriteLine(@"C:\ProgramData\Anaconda3\Scripts\activate.bat");
                        sw.WriteLine($"python {inference_gfpgan_path} -i \"{tempDir}\" -o \"{tempDir}\" -v 1.3 -s 2");
                    }
                }

                StreamReader myStreamReader = myProcess.StandardOutput;

                string? myString = myStreamReader.ReadLine();
                myProcess.WaitForExit();
                string output = myProcess.StandardOutput.ReadToEnd(); // 標準出力の読み取り
                myProcess.Close();

                // ディレクトリ直下のすべてのファイル一覧を取得する
                var file = Directory.GetFiles(Path.Combine(tempDir, "restored_imgs")).LastOrDefault();

                if (!string.IsNullOrEmpty(file))
                {
                    // ファイルのコピー（同じ名前のファイルがある場合は上書き）
                    File.Move(file, outfilename, true);

                    // DirectoryInfoのインスタンスを生成する
                    DirectoryInfo di = new DirectoryInfo(tempDir);

                    // 元のディレクトリを削除する
                    di.Delete(true);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
    }
}
