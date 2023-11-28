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
    public class FlowerRepository : IRepository<Flower>
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
            string command = $"SELECT * FROM kvetiny WHERE ID_KVETINA = {id}";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var flower = new Flower(
                Convert.ToInt32(row["ID_KVETINA"]),
                row["NAZEV"].ToString(),
                Convert.ToInt32(row["CENA"]),
                Convert.ToInt32(row["POCET"])
            );
            
            return (Flower)Convert.ChangeType(flower, typeof(Flower));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM kvetiny";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            foreach (DataRow row in dataTable.Rows)
            {
                var flower = new Flower(
                    Convert.ToInt32(row["ID_KVETINA"]),
                    row["NAZEV"].ToString(),
                    Convert.ToInt32(row["CENA"]),
                    Convert.ToInt32(row["POCET"])
                );
                Flowers.Add(flower);
            }
        }

        public async Task Add(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"STAV", entity.State.ToString()},
                {"STARI", entity.Age}
            };
            await dbUtil.ExecuteStoredProcedureAsync("addkvetina", parameters);
        }

        public async Task Update(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_KVETINA", entity.IdFlower},
                {"STAV", entity.State.ToString()},
                {"STARI", entity.Age}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatekvetina", parameters);
        }

        public async Task Delete(Flower entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_KVETINA", entity.IdFlower}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletekvetina", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("State", typeof(FlowerStateEnum));
            dataTable.Columns.Add("Age", typeof(int));
            
            foreach (var flower in Flowers)
            {
                dataTable.Rows.Add(flower.State, flower.Age);
            }
            
            return dataTable;
        }
    }
}