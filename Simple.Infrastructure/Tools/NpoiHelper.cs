using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace Simple.Infrastructure.Tools
{
    public class NpoiHelper
    {
        public byte[] OutputExcel<T>(List<T> entitys, string[] title)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i <= entitys.Count; i++)
            {
                if (i == 0)
                {
                    Title = sheet.CreateRow(0);
                    for (int k = 1; k < title.Length + 1; k++)
                    {
                        Title.CreateCell(0).SetCellValue("序号");
                        Title.CreateCell(k).SetCellValue(title[k - 1]);
                    }

                    continue;
                }
                else
                {
                    rows = sheet.CreateRow(i);
                    object entity = entitys[i - 1];
                    for (int j = 1; j <= entityProperties.Length; j++)
                    {
                        object[] entityValues = new object[entityProperties.Length];
                        entityValues[j - 1] = entityProperties[j - 1].GetValue(entity);
                        rows.CreateCell(0).SetCellValue(i);
                        rows.CreateCell(j).SetCellValue(entityValues[j - 1].ToString());
                    }
                }
            }

            byte[] buffer = new byte[1024 * 2];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.ToArray();
                ms.Close();
            }

            return buffer;
        }
        public byte[] OutputExcel(DataTable dataTable, string[] columnTitle)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            //表头
            IRow Title = sheet.CreateRow(0);
            Title.CreateCell(0).SetCellValue("序号");
            for (int k = 1; k < columnTitle.Length + 1; k++)
            {
                Title.CreateCell(k).SetCellValue(columnTitle[k - 1]);
            }
            //内容
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                IRow rows = sheet.CreateRow(i + 1);
                rows.CreateCell(0).SetCellValue(i + 1);
                for (int j = 1; j <= dataTable.Columns.Count; j++)
                {
                    rows.CreateCell(j).SetCellValue(dataTable.Rows[i][j - 1].ToString());
                }
            }
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }
    }
}
