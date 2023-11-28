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

        public async Task<T> GetById(Int32 id)
        {
            string command = $"SELECT * FROM ostatni WHERE ID_OSTATNI = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var otherGoods = new OtherGoods(
                Convert.ToInt32(row["ID_OSTATNI"]),
                row["ZEME_PUVODU"].ToString(),
                Convert.ToDateTime(row["DATUM_TRVANLIVOSTI"])
            );
            
            return (T)Convert.ChangeType(otherGoods, typeof(T));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM ostatni";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var otherGoods = new OtherGoods(
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
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addostatni", parameters);
        }

        public async Task Update(OtherGoods entity)
        {
            var parameters = new Dictionary<string, object>
            {
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
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_OSTATNI", typeof(int));
            dataTable.Columns.Add("ZEME_PUVODU", typeof(string));
            dataTable.Columns.Add("DATUM_TRVANLIVOSTI", typeof(DateTime));
            
            foreach (var otherGoods in OtherGoods)
            {
                dataTable.Rows.Add(
                    otherGoods.IdOtherGoods,
                    otherGoods.CountryOfOrigin,
                    otherGoods.ExpirationDate
                );
            }
            
            return dataTable;
        }
    }
}