using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
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
    public class FlowerRepository
    {
        public ObservableCollection<Flower> Flowers { get; set; }
        private OracleDbUtil dbUtil;
        
        public FlowerRepository()
        {
            Flowers = new ObservableCollection<Flower>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Flower> GetById(Int32 id)
        { 
            string command = $"SELECT zbozi.ID_ZBOZI, zbozi.NAZEV, zbozi.CENA, zbozi.TYP, zbozi.SKLAD, zbozi.OBRAZEK, kvetiny.ID_KVETINA, kvetiny.STAV, kvetiny.STARI FROM kvetiny " +
                             $"LEFT JOIN zbozi ON kvetiny.id_zbozi = zbozi.id_zbozi " +
                             $"WHERE ID_KVETINA = {id}";
            
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var flower = new Flower(
                Convert.ToInt32(row["ID_ZBOZI"]),
                row["NAZEV"].ToString(),
                Convert.ToDouble(row["CENA"]),
                Convert.ToChar(row["TYP"]),
                Convert.ToInt32(row["SKLAD"]),
                null,
                Convert.ToInt32(row["ID_KVETINA"]),
                MapDatabaseValueToEnum(row["STAV"].ToString()),
                Convert.ToInt32(row["STARI"])
            );
            
            return (Flower)Convert.ChangeType(flower, typeof(Flower));
        }

        public async Task GetAll()
        {
            Flowers.Clear();
            string command = $"SELECT zbozi.ID_ZBOZI, zbozi.NAZEV, zbozi.CENA, zbozi.TYP, zbozi.SKLAD, zbozi.OBRAZEK, kvetiny.ID_KVETINA, kvetiny.STAV, kvetiny.STARI FROM kvetiny " +
                             $"LEFT JOIN zbozi ON kvetiny.id_zbozi = zbozi.id_zbozi ";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            foreach (DataRow row in dataTable.Rows)
            {
                var flower = new Flower(
                    Convert.ToInt32(row["ID_ZBOZI"]),
                    row["NAZEV"].ToString(),
                    Convert.ToDouble(row["CENA"]),
                    Convert.ToChar(row["TYP"]),
                    Convert.ToInt32(row["SKLAD"]),
                    null,
                    Convert.ToInt32(row["ID_KVETINA"]),
                    MapDatabaseValueToEnum(row["STAV"].ToString()),
                    Convert.ToInt32(row["STARI"])
                );
                Flowers.Add(flower);
            }
        }

        public async Task Add(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse },
                { "OBRAZEK", null },
                {"STAV", entity.State.ToString()},
                {"STARI", entity.Age}
            };
            await dbUtil.ExecuteStoredProcedureAsync("addkvetiny", parameters);
        }

        public async Task Update(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", entity.IdGoods },
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse },
                { "OBRAZEK", null },
                {"ID_KVETINA", entity.IdFlower},
                {"STAV", entity.State.ToString()},
                {"STARI", entity.Age}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatekvetiny", parameters);
        }

        public async Task Delete(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_ZBOZI", entity.IdGoods},
                {"ID_KVETINA", entity.IdFlower}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletekvetiny", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("NAZEV", typeof(string));
            dataTable.Columns.Add("CENA", typeof(int));
            dataTable.Columns.Add("TYP", typeof(byte));
            dataTable.Columns.Add("SKLAD", typeof(int));
            dataTable.Columns.Add("Stav", typeof(string));
            dataTable.Columns.Add("Vek", typeof(int));

            foreach (var flowers in Flowers)
            {
                DataRow row = dataTable.NewRow();
                row["ID"] = flowers.IdFlower;
                row["NAZEV"] = flowers.Name;
                row["CENA"] = flowers.Price;
                row["TYP"] = flowers.Type;
                row["SKLAD"] = flowers.Warehouse;
                row["Stav"] = flowers.State.ToString();
                row["Vek"] = flowers.Age;
                
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        
        private FlowerStateEnum MapDatabaseValueToEnum(string databaseValue)
        {
            if (Enum.TryParse<FlowerStateEnum>(databaseValue, out var flowerStateEnum))
            {
                return flowerStateEnum;
            }
            throw new ArgumentException($"Invalid FlowerStateEnum value: {databaseValue}");
        }

        public List<Flower> GetFlowers()
        {
            Task.Run(async () => await GetAll()).Wait();
            var flowers = Flowers.ToList();

            return flowers;
        }
    }
}