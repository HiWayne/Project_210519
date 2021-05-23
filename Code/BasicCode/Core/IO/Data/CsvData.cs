using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace GameBasic.IO
{

    public class CsvData
    {
        public static string[] SPLIT_LINE = { "\r\n" };

        // header
        public string[] columnName;
        public List<List<string>> rows;

        public int ColCount { get { return columnName.Length; } }
        public int RowCount { get { return rows.Count; } }

        public CsvData(string data, bool hasHead = true)
        {
            // spilt into rows
            string[] strArray = data.Split(SPLIT_LINE, System.StringSplitOptions.RemoveEmptyEntries);
            // first row is column name
            columnName = strArray[0].Split(',');

            // row shifting
            int rowShift = hasHead ? 1 : 0;
            // row count, is last row empty
            int rowCount = strArray[strArray.Length - 1] == "" ? strArray.Length - 1 - rowShift : strArray.Length - rowShift;

            // store row data
            rows = new List<List<string>>(rowCount);
            for (int i = 0; i < rowCount; i++)
                rows.Add(GetColumns(strArray[i + rowShift]));
        }

        /// <summary>
        /// Split row into columns
        /// </summary>
        private List<string> GetColumns(string text)
        {
            // commonSpliter = true: 逗号是分割符
            // commonSpliter = false: 遇到下一个引号前，逗号","表示文本而非分割符。
            bool commonSpliter = true;

            List<string> result = new List<string>(ColCount);
            int startIndex = 0;

            int lastIndex = text.Length - 1;
            for (int i = 0, len = text.Length; i < len; i++)
            {
                // first char is column is null
                if (i == 0 && text[i] == ',')
                {
                    result.Add("");
                    startIndex = i + 1;
                    continue;
                }

                // quot
                if (text[i] == '"')
                    commonSpliter = !commonSpliter;

                // is last char
                bool lastChar = i == lastIndex;

                if (commonSpliter)
                {
                    bool isCommon = text[i] == ',';

                    //last 
                    if (lastChar)
                    {
                        if (isCommon)
                        {
                            result.Add(text.Substring(startIndex, i - startIndex));
                            result.Add("");
                        }
                        else
                        {
                            result.Add(text.Substring(startIndex, i - startIndex + 1));
                        }
                        break;
                    }
                    else
                    {
                        // common
                        if (isCommon)
                        {
                            // add column
                            result.Add(text.Substring(startIndex, i - startIndex));
                            startIndex = i + 1;
                            continue;
                        }
                    }
                }
                // last, the last char is a "
                else if (lastChar)
                {
                    result.Add(text.Substring(startIndex, i - startIndex + 1));
                    Debug.LogError("CSV format error: " + text);
                    break;
                }
            }

            for (int i = 0, len = result.Count; i < len; i++)
            {
                string t = result[i];
                if (t.Length >= 2)
                    result[i] = CheckQuotation(t);
            }

            return result;
        }

        /// <summary>
        /// 处理引号
        /// </summary>
        private string CheckQuotation(string str)
        {
            // remove first and last
            if (str[0] == '"' && str[str.Length - 1] == '"')
                str = str.Substring(1, str.Length - 2);

            // replace double "" with single "
            str = str.Replace("\"\"", "\"");

            return str;
        }

        public T Parse<T>(int row)
        {
            return JsonUtility.FromJson<T>(ToJson(row));
        }

        public JsonArray<T> Parse<T>() where T : new()
        {
            // return new JsonArray<T>() { datas = ParseImpl2<T>() };
            return ParseImpl2<T>();
        }

        List<T> ParseImpl1<T>() where T : new()
        {
            int rowCount = RowCount;
            int colCount = ColCount;

            // Results
            List<T> result = new List<T>(rowCount);

            // Fields
            var fields = typeof(T).GetFields();

            for (int i = 0; i < rowCount; i++)
            {
                List<string> row = rows[i];

                object t = new T();
                for (int j = 0; j < colCount; j++)
                {
                    var fi = fields[j];
                    fi.SetValue(t, row[j]);
                }

                result.Add((T)t);
            }

            return result;
        }

        public JsonArray<T> ParseImpl2<T>()
        {
            return JsonUtility.FromJson<JsonArray<T>>(ToJson());
        }

        /// <summary>
        /// Return a json string at given row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public string ToJson(int row)
        {
            StringBuilder sb = new StringBuilder();
            ToJson(sb, row);
            return sb.ToString();
        }

        /// <summary>
        /// Return a <see cref="JsonArray{T}"/> json string for all rows
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"").Append(JsonArray<object>.ARRAY_NAME).Append("\":[");

            for (int i = 0, len = rows.Count; i < len; i++)
            {
                if (i > 0)
                    sb.Append(",");
                ToJson(sb, i);
            }
            sb.Append("]}");
            return sb.ToString();
        }

        /// <summary>
        /// Parse given row data to json string
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="row"></param>
        void ToJson(StringBuilder sb, int row)
        {
            sb.Append("{");
            for (int i = 0, len = rows[row].Count; i < len; i++)
            {
                string column = rows[row][i];
                if (i > 0)
                    sb.Append(",");
                sb.Append("\"").Append(columnName[i]).Append("\":");

                if (TextUtil.TryParse(column, out float number))
                {
                    sb.Append(TextUtil.ToString(number));
                }
                else
                {
                    column = column.Replace("\"","\\\"");
                    sb.Append("\"").Append(column).Append("\"");
                }

            }
            sb.Append("}");
        }

        public static void CsvBenchMark<T>(CsvData csv) where T : new()
        {
            int rep = 100;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < rep; i++)
            {
                csv.ParseImpl1<T>();
            }
            var t1 = stopwatch.ElapsedMilliseconds;

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < rep; i++)
            {
                csv.ParseImpl2<T>();
            }
            var t2 = stopwatch.ElapsedMilliseconds;

            Debug.Log(string.Format("T1:{0} T2:{1}", t1, t2));
        }
    }
}
