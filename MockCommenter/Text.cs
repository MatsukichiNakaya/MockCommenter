using System;
using System.IO;
using System.Text;

namespace Project.FileIO
{
    /// <summary>
    /// テキストファイルの読込み・書き込み
    /// </summary>
    /// <remarks>
    /// エンコード引数が無の場合は
    /// ファイルのエンコード情報から
    /// それも無い場合はOSのデフォルトを設定
    /// </remarks>
    public class TextFile
    {
        /// <summary>ファイル末尾への追加指定</summary>
        public const Boolean APPEND = true;
        /// <summary>ファイルの上書き指定</summary>
        public const Boolean OVER_WRITE = false;
        /// <summary>Shift_JIS(文字コード)</summary>
        public const String SHIFT_JIS = "Shift_JIS";

        /// <summary>判定できるバイト数</summary>
        private const Int32 JUDGMENT_NUM = 2;

        /// <summary>
        /// テキストファイルのエンコード情報を判定する
        /// </summary>
        /// <param name="bytes">判定を行う為のバイト情報</param>
        /// <remarks>
        /// UTFのどれに相当するか判定するものなので
        /// それ以外は別途確認すること
        /// </remarks>
        public static Encoding? DetectEncodingFromBOM(Byte[] bytes)
        {   // 判定不可
            if (bytes.Length < JUDGMENT_NUM) { return null; }

            if ((bytes[0] == 0xfe) && (bytes[1] == 0xff)) {   //UTF-16 BE
                return new UnicodeEncoding(true, true);
            }

            if ((bytes[0] == 0xff) && (bytes[1] == 0xfe)) {
                if ((4 <= bytes.Length) &&
                    (bytes[2] == 0x00) && (bytes[3] == 0x00)) {   //UTF-32 LE
                    return new UTF32Encoding(false, true);
                }
                //UTF-16 LE
                return new UnicodeEncoding(false, true);
            }
            // 判定不可
            if (bytes.Length < 3) { return null; }

            if ((bytes[0] == 0xef) && (bytes[1] == 0xbb) && (bytes[2] == 0xbf)) {   //UTF-8
                return new UTF8Encoding(true, true);
            }
            // 判定不可
            if (bytes.Length < 4) { return null; }
            if ((bytes[0] == 0x00) && (bytes[1] == 0x00) &&
                        (bytes[2] == 0xfe) && (bytes[3] == 0xff)) {   //UTF-32 BE
                return new UTF32Encoding(true, true);
            }
            return null;// 判定不可
        }

        /// <summary>
        /// 文字列としての読み込み
        /// </summary>
        /// <param name="filePath">読込みファイルのパス</param>
        public static String? Read(String filePath)
        {
            // ファイル有無の確認
            if (!File.Exists(filePath)) { return null; }
            // バイト配列で取得
            var rawData = File.ReadAllBytes(filePath);
            // エンコード情報取得(UTF 関連の情報のみ)
            var enc = DetectEncodingFromBOM(rawData);
            // null の場合　オペレーティングシステムのデフォルトを使用
            enc ??= Encoding.Default;

            return Read(filePath, enc);
        }

        /// <summary>
        /// 他のプロセスがファイルを開いていても読込可能
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="enc"></param>
        public static String? Read(String filePath, Encoding enc)
        {
            if (!File.Exists(filePath)) { return null; }
            var result = new StringBuilder();
            using (var fs = new FileStream(filePath, FileMode.Open,
                                           FileAccess.Read, FileShare.ReadWrite)) {
                using TextReader sr = new StreamReader(fs, enc);
                String? line;
                while ((line = sr?.ReadLine()) != null) {
                    result.Append(line).Append("\r\n");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String[]? ReadLines(String filePath)
        {
            return ReadLines(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public static String[]? ReadLines(String filePath, Encoding enc)
        {
            return Read(filePath, enc)?.Replace("\r\n", "\n").Split('\n');
        }

        /// <summary>
        /// テキストの出力(UTF-8)
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="text">テキストの内容</param>
        /// <param name="append">ファイルへの追加(true)または上書き(false)</param>
        public static void Write(String path, String text, Boolean append)
        {
            using var writer = new StreamWriter(path, append, Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
        }

        /// <summary>
        /// テキストの出力
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="text">テキストの内容</param>
        /// <param name="append">ファイルへの追加(true)または上書き(false)</param>
        /// <param name="enc">エンコード情報</param>
        public static void Write(String path, String text, Boolean append, Encoding enc)
        {
            using var writer = new StreamWriter(path, append, enc);
            writer.Write(text);
            writer.Flush();
        }
    }
}
