using PoprawaMiW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public class FileDataService : IFileDataService
    {
        public List<NumericalFileData> CreateListOfNumericalFileDatas(List<FileData> fileDatas)
        {
            var listOfNumericalFileDatas = new List<NumericalFileData>();
            var numberOfColumns = fileDatas.ElementAt(0).Attributes.Count;
            var indexer = 0;

            foreach(var fileData in fileDatas)
            {
                var numericalFileData = CreateNumericalFileData(fileDatas, indexer);

                if (numericalFileData.NumericalValues.Count == numberOfColumns)
                {
                    listOfNumericalFileDatas.Add(numericalFileData);
                }
                indexer++;
            }

            return listOfNumericalFileDatas;
        }
        private NumericalFileData CreateNumericalFileData(List<FileData> fileDatas, int row)
        {
            var columns = GetAllColumnsToNumerical(fileDatas);
            var decision = fileDatas[row].Decision;
            var numericalValues = new Dictionary<int, double>();

            foreach (var column in columns)
            {
                if (column.Rows.ContainsKey(row))
                {
                    var value = column.Rows[row];
                    var columnId = column.IdOfColumn;

                    numericalValues.Add(columnId, value);
                }
            }

            return new NumericalFileData(decision, numericalValues);
        }
        private List<Column<double>> GetAllColumnsToNumerical(List<FileData> fileDatas)
        {
            var changedSymbolicColumns = ChangeAllSymbolicColumnsToNumerical(fileDatas);
            var numericalColumns = GetAllNumericalColumns(fileDatas);
            var listOfAllColumns = new List<Column<double>>();

            listOfAllColumns.AddRange(numericalColumns);
            listOfAllColumns.AddRange(changedSymbolicColumns);

            listOfAllColumns.OrderBy(x => x.IdOfColumn);

            return listOfAllColumns;
        }
        private List<Column<double>> ChangeAllSymbolicColumnsToNumerical(List<FileData> fileDatas)
        {
            var listOfSymbolicChangedToNumericalColumns = new List<Column<double>>();
            var listOfSymbolicColumns = GetAllSymbolicColumns(fileDatas);

            foreach (var symbolicColumn in listOfSymbolicColumns)
            {
                var assignedSymbols = AssignNumberToSymbol(symbolicColumn);
                var numericalColumn = ChangeSymbolicColumnToNumerical(symbolicColumn, assignedSymbols);

                listOfSymbolicChangedToNumericalColumns.Add(numericalColumn);
            }

            return listOfSymbolicChangedToNumericalColumns;
        }
        private List<Column<string>> GetAllSymbolicColumns(List<FileData> fileDatas)
        {
            var indexesOfSymbolicColumns = fileDatas.ElementAt(0).SymbolicValues.Select(x => x.Key);
            var listOfSymbolicColumns = new List<Column<string>>();

            foreach (var indexOfSymbolicColumn in indexesOfSymbolicColumns)
            {
                var symbolicColumn = GetColumnOfSymbols(fileDatas, indexOfSymbolicColumn);
                listOfSymbolicColumns.Add(symbolicColumn);
            }

            return listOfSymbolicColumns;
        }
        private List<Column<double>> GetAllNumericalColumns(List<FileData> fileDatas)
        {
            var indexOfNumericalColumns = fileDatas.ElementAt(0).NumericalValues.Select(x => x.Key).ToList();
            var listOfNumericalColumns = new List<Column<double>>();

            foreach (var index in indexOfNumericalColumns)
            {
                var numericalColumn = GetColumnOfNumbers(fileDatas, index);
                listOfNumericalColumns.Add(numericalColumn);
            }

            return listOfNumericalColumns;
        }
        private Column<double> ChangeSymbolicColumnToNumerical(Column<string> columnOfSymbols, Dictionary<string, int> assignedSymbols)
        {
            var columnId = columnOfSymbols.IdOfColumn;
            var numberRows = new Dictionary<int, double>();

            foreach (var row in columnOfSymbols.Rows)
            {
                var symbolToNumber = ChangeSymbolToNumber(assignedSymbols, row.Value);
                numberRows.Add(row.Key, symbolToNumber);
            }

            return new Column<double>(columnId, numberRows);
        }
        private Column<string> GetColumnOfSymbols(List<FileData> fileDatas, int indexOfColumn)
        {
            var rows = new Dictionary<int, string>();
            var rowIndex = 0;

            foreach (var fileData in fileDatas)
            {
                var symbolicValueFromColumn = fileData.SymbolicValues[indexOfColumn];
                rows.Add(rowIndex, symbolicValueFromColumn);
                rowIndex++;
            }

            return new Column<string>(indexOfColumn, rows);
        }
        private Column<double> GetColumnOfNumbers(List<FileData> fileDatas, int indexOfColumn)
        {
            var rows = new Dictionary<int, double>();
            var indexer = 0;
            foreach(var fileData in fileDatas)
            {
                if (fileData.NumericalValues.ContainsKey(indexOfColumn))
                {
                    var value = fileData.NumericalValues[indexOfColumn];
                    rows.Add(indexer, value);
                }
                indexer++;
            }
            return new Column<double>(indexOfColumn, rows);
        }
        private Dictionary<string, int> AssignNumberToSymbol(Column<string> column)
        {
            var index = 0;
            var symbols = column.Rows.Select(x => x.Value);
            var frequencies = symbols.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var result = frequencies.OrderBy(x => x.Value);

            var assigned = new Dictionary<string, int>();

            foreach (var x in result)
            {
                assigned.Add(x.Key, index++);
            }

            return assigned;

        }
        private double ChangeSymbolToNumber(Dictionary<string, int> assignedSymbols, string symbol)
        {
            var number = 0;
            var isSymbolAssigned = assignedSymbols.Keys.Contains(symbol);

            if (isSymbolAssigned)
                number = assignedSymbols.First(x => x.Key == symbol).Value;

            return number;
        }
    }
}