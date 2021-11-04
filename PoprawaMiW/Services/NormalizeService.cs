using PoprawaMiW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public class NormalizeService : INormalizeService
    {
        public List<NumericalFileData> NormalizeData(List<NumericalFileData> fileDatas, double maxRange, double minRange)
        {
            var numericalFileDatas = new List<NumericalFileData>();
            var decisions = fileDatas.Select(x => x.Decision).ToList();
            var normalizedColumns = NormalizeColumns(fileDatas, maxRange, minRange);
            var numberOfRows = normalizedColumns.ElementAt(0).Rows.Count;
            
            for(int i = 0; i < numberOfRows; i++)
            {
                var fileData = CreateNumericalFileDataByRow(normalizedColumns, i, decisions[i]);
                numericalFileDatas.Add(fileData);
            }

            return numericalFileDatas;
        }
        private List<Column<double>> NormalizeColumns(List<NumericalFileData> fileDatas, double maxRange, double minRange)
        {
            var columns = GetAllColumns(fileDatas);
            var normalizedColumns = new List<Column<double>>();
            foreach (var column in columns)
            {
                var normalizedColumn = NormalizeColumn(column, maxRange, minRange);
                normalizedColumns.Add(normalizedColumn);
            }

            return normalizedColumns;
        }
        private NumericalFileData CreateNumericalFileDataByRow(List<Column<double>> columns, int row, string decision)
        {
            var numericalValues = new Dictionary<int, double>();
            var indexer = 0;

            foreach (var column in columns)
            {
                if (column.Rows.ContainsKey(row))
                {
                    var value = column.Rows[row];
                    numericalValues.Add(indexer, value);
                }
                indexer++;
            }

            return new NumericalFileData(decision, numericalValues);
        }
        private List<Column<double>> GetAllColumns(List<NumericalFileData> fileDatas)
        {
            var allColumns = new List<Column<double>>();
            var numberOfColumns = fileDatas.ElementAt(0).NumericalValues.Select(x => x.Key).ToList();

            foreach (var idOfColumn in numberOfColumns)
            {
                var column = GetSingleColumn(fileDatas, idOfColumn);
                allColumns.Add(column);
            }

            return allColumns;
        }
        private Column<double> GetSingleColumn(List<NumericalFileData> fileDatas, int indexOfColumn)
        {
            var numericalValues = new Dictionary<int, double>();
            var indexer = 0;
            foreach (var fileData in fileDatas)
            {
                if (fileData.NumericalValues.ContainsKey(indexOfColumn))
                {
                    var value = fileData.NumericalValues[indexOfColumn];
                    numericalValues.Add(indexer, value);
                }
                indexer++;
            }

            return new Column<double>(indexOfColumn, numericalValues);
        }
        private Column<double> NormalizeColumn(Column<double> columnInput, double maxRange, double minRange)
        {
            var normalizedValues = new Dictionary<int, double>();
            var max = FindMaxInColumn(columnInput);
            var min = FindMinInColumn(columnInput);
            var indexer = 0;

            foreach (var row in columnInput.Rows)
            {
                var normalizedData = NormalizeData(row.Value, min, max, maxRange, minRange);
                normalizedValues.Add(indexer, normalizedData);
                indexer++;
            }

            var normalizedColumn = new Column<double>(columnInput.IdOfColumn, normalizedValues);

            return normalizedColumn;
        }
        private double NormalizeData(double value, double valueMin, double valueMax, double maxRange, double minRange)
        {
            var normalizePattern = (((value - valueMin) / (valueMax - valueMin)) * (maxRange - minRange)) + minRange;
            return Math.Round(normalizePattern, 3);
        }
        private double FindMaxInColumn(Column<double> column)
        {
            return column.Rows.Select(x => x.Value).Max();
        }
        private double FindMinInColumn(Column<double> column)
        {
            return column.Rows.Select(x => x.Value).Min();
        }
    }
}
