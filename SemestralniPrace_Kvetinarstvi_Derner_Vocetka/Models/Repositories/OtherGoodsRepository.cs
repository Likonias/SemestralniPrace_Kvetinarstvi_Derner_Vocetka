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

        public async Task<OtherGoods> GetById(int id)
        {
            string command = $"SELECT zbozi.ID_ZBOZI, zbozi.NAZEV, zbozi.CENA, zbozi.TYP, zbozi.SKLAD, zbozi.OBRAZEK, ostatni.id_ostatni, ostatni.zeme_puvodu, ostatni.datum_trvanlivosti FROM ostatni " +
                             $"LEFT JOIN zbozi ON ostatni.id_zbozi = zbozi.id_zbozi " +
                             $"WHERE ID_OSTATNI = {id}";

            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
          
            if (dataTable.Rows.Count == 0)
                return null;
          
            var row = dataTable.Rows[0];

            //TODO fix otherGoods add
            byte[] imageBytes = row["OBRAZEK"] as byte[];
            DateTime dateString = (DateTime)row["DATUM_TRVANLIVOSTI"]; // Assuming this is a string representation of the date
            DateOnly dateOnly = DateOnly.FromDateTime(dateString);

            var otherGoods = new OtherGoods(
                Convert.ToInt32(row["ID_ZBOZI"]),
                row["NAZEV"].ToString(),
                Convert.ToDouble(row["CENA"]),
                Convert.ToChar(row["TYP"]),
                Convert.ToInt32(row["SKLAD"]),
                imageBytes,
                Convert.ToInt32(row["ID_OSTATNI"]),
                row["ZEME_PUVODU"].ToString(),
                dateOnly
            );

            return otherGoods;
        }

        public async Task GetAll()
        {
            OtherGoods.Clear();
            string command = $"SELECT zbozi.ID_ZBOZI, zbozi.NAZEV, zbozi.CENA, zbozi.TYP, zbozi.SKLAD, zbozi.OBRAZEK, ostatni.id_ostatni, ostatni.zeme_puvodu, ostatni.datum_trvanlivosti FROM ostatni " +
                             $"LEFT JOIN zbozi ON ostatni.id_zbozi = zbozi.id_zbozi";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                //TODO fix otherGoods add
                byte[] imageBytes = row["OBRAZEK"] as byte[];
                DateTime dateString = (DateTime)row["DATUM_TRVANLIVOSTI"]; // Assuming this is a string representation of the date
                DateOnly dateOnly = DateOnly.FromDateTime(dateString);
                
                var otherGoods = new OtherGoods(
                    Convert.ToInt32(row["ID_ZBOZI"]),
                    row["NAZEV"].ToString(),
                    Convert.ToDouble(row["CENA"]),
                    Convert.ToChar(row["TYP"]),
                    Convert.ToInt32(row["SKLAD"]),
                    imageBytes,
                    Convert.ToInt32(row["ID_OSTATNI"]),
                    row["ZEME_PUVODU"].ToString(),
                    dateOnly
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
                { "SKLAD", entity.Warehouse },
                { "OBRAZEK", entity.Image },
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate.Value.Day + "." + entity.ExpirationDate.Value.Month + "." + entity.ExpirationDate.Value.Year}
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
                { "OBRAZEK", entity.Image },
                {"ID_OSTATNI", entity.IdOtherGoods},
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate.Value.Day + "." + entity.ExpirationDate.Value.Month + "." + entity.ExpirationDate.Value.Year}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateostatni", parameters);
        }

        public async Task Delete(OtherGoods otherGoods)
        {

            var parameters = new Dictionary<string, object>
            {
                {"ID_ZBOZI", otherGoods.IdGoods},
                {"ID_OSTATNI", otherGoods.IdOtherGoods }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteostatni", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("IdOtherGoods");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Warehouse");
            dataTable.Columns.Add("CountryOfOrigin");
            dataTable.Columns.Add("ExpirationDate");

            foreach (var otherGoods in OtherGoods)
            {
                dataTable.Rows.Add(
                    otherGoods.IdOtherGoods,
                    otherGoods.Name,
                    otherGoods.Price,
                    otherGoods.Warehouse,
                    otherGoods.CountryOfOrigin,
                    otherGoods.ExpirationDate.Value.Day + "." + otherGoods.ExpirationDate.Value.Month + "." + otherGoods.ExpirationDate.Value.Year
                );
            }

            return dataTable;
        }

        public List<OtherGoods> GetOtherGoods()
        {
            Task.Run(async () => await GetAll()).Wait();
            var otherGoods = OtherGoods.ToList();

            return otherGoods;
        }

        public Task Delete(int id)
        {
            //TODO kazis to ludku
            throw new NotImplementedException();
        }
    }
}