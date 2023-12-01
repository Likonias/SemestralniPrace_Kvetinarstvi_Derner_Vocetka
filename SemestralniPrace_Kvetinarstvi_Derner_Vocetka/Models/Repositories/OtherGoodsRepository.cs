using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class OtherGoodsRepository : IRepository<OtherGoods>
    {
        public ObservableCollection<OtherGoods> OtherGoods { get; set; }
        private OracleDbUtil dbUtil;

        public OtherGoodsRepository()
        {
            OtherGoods = new ObservableCollection<OtherGoods>();
            dbUtil = new OracleDbUtil();
        }

        public async Task<OtherGoods> GetById(Int32 id)
        {
            string command = $"SELECT * FROM ostatni " +
                             $"LEFT JOIN zbozi ON ostatni.id_zbozi = zbozi.id_zbozi" +
                             $"WHERE ID_OSTATNI = {id}";

            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
          
            if (dataTable.Rows.Count == 0)
                return null;
          
            var row = dataTable.Rows[0];
            var otherGoods = new OtherGoods(
                Convert.ToInt32(row["ID_ZBOZI"]),
                row["NAZEV"].ToString(),
                Convert.ToDouble(row["CENA"]),
                Convert.ToChar(row["TYP"]),
                Convert.ToInt32(row["SKLAD"]),
                null,
                Convert.ToInt32(row["ID_OSTATNI"]),
                row["ZEME_PUVODU"].ToString(),
                Convert.ToDateTime(row["DATUM_TRVANLIVOSTI"])
            );

            return otherGoods;
        }

        public async Task GetAll()
        {
            string command = $"SELECT * FROM ostatni " +
                             $"LEFT JOIN zbozi ON ostatni.id_zbozi = zbozi.id_zbozi";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var otherGoods = new OtherGoods(
                    Convert.ToInt32(row["ID_ZBOZI"]),
                    row["NAZEV"].ToString(),
                    Convert.ToDouble(row["CENA"]),
                    Convert.ToChar(row["TYP"]),
                    Convert.ToInt32(row["SKLAD"]),
                    null,
                    Convert.ToInt32(row["ID_OSTATNI"]),
                    row["ZEME_PUVODU"].ToString(),
                    Convert.ToDateTime(row["DATUM_TRVANLIVOSTI"])
                );
                OtherGoods.Add(otherGoods);
            }
        }

        public async Task Add(OtherGoods entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse },
                { "OBRAZEK", null },
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addostatni", parameters);
        }

        public async Task Update(OtherGoods entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", entity.IdGoods },
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse },
                { "OBRAZEK", null },
                {"ID_OSTATNI", entity.IdOtherGoods},
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateostatni", parameters);
        }

        public async Task Delete(OtherGoods entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_OSTATNI", entity.IdOtherGoods}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteostatni", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("NAZEV", typeof(string));
            dataTable.Columns.Add("CENA", typeof(int));
            dataTable.Columns.Add("TYP", typeof(byte));
            dataTable.Columns.Add("SKLAD", typeof(int));
            dataTable.Columns.Add("ZEME_PUVODU", typeof(string));
            dataTable.Columns.Add("DATUM_TRVANLIVOSTI", typeof(DateTime));
          
            foreach (var otherGoods in OtherGoods)
            {   
                DataRow row = dataTable.NewRow();
                row["NAZEV"] = otherGoods.Name;
                row["CENA"] = otherGoods.Price;
                row["TYP"] = otherGoods.Type;
                row["SKLAD"] = otherGoods.Warehouse;
                row["ZEME_PUVODU"] = otherGoods.CountryOfOrigin;
                row["ZEME_PUVODU"] = otherGoods.CountryOfOrigin;
                row["DATUM_TRVANLIVOSTI"] = otherGoods.ExpirationDate;

                dataTable.Rows.Add(row);
            }
          
            return dataTable;
        }
    }
}