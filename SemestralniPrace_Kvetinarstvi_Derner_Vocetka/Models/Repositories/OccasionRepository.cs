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
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories{
    public class OccasionRepository : IRepository<Occasion>
    {
        public async Task<T> GetById(Int32 id)
        {
            string command = $"SELECT * FROM prilezitosti WHERE ID_PRILEZITOST = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var occasion = new Occasion(
                Convert.ToInt32(row["ID_PRILEZITOST"]),
                (OccasionType)Enum.Parse(typeof(OccasionType), row["DRUH_PRILEZITOSTI"].ToString())
            );

            return (T)Convert.ChangeType(occasion, typeof(T));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM prilezitosti";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);
            
            foreach (DataRow row in dataTable.Rows)
            {
                var occasion = new Occasion(
                    Convert.ToInt32(row["ID_PRILEZITOST"]),
                    (OccasionType)Enum.Parse(typeof(OccasionType), row["DRUH_PRILEZITOSTI"].ToString())
                );
                Occasions.Add(occasion);
            }
        }

        public async Task Add(Occasion entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DRUH_PRILEZITOSTI", entity.OccasionType.ToString()}
            };
            await dbUtil.ExecuteStoredProcedureAsync("addprilezitosti", parameters);
        }

        public async Task Update(Occasion entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_PRILEZITOST", entity.Id},
                {"DRUH_PRILEZITOSTI", entity.OccasionType.ToString()}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateprilezitosti", parameters);
        }

        public async Task Delete(Occasion entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_PRILEZITOST", entity.Id}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteprilezitosti", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_PRILEZITOST", typeof(int));
            dataTable.Columns.Add("DRUH_PRILEZITOSTI", typeof(string));
            
            foreach (var occasion in Occasions)
            {
                dataTable.Rows.Add(occasion.Id, occasion.OccasionType.ToString());
            }
            
            return dataTable;
        }
    }
}