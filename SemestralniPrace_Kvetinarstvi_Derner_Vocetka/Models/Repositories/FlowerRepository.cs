using Oracle.ManagedDataAccess.Client;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            string command = "GET_KVETINA_BY_ID";

            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            byte[] imageBytes = row["OBRAZEK"] as byte[] ?? new byte[0]; // Get the image bytes or empty byte array if null

            var flower = new Flower(
                Convert.ToInt32(row["ID_ZBOZI"]),
                row["NAZEV"].ToString(),
                Convert.ToDouble(row["CENA"]),
                Convert.ToChar(row["TYP"]),
                Convert.ToInt32(row["SKLAD"]),
                imageBytes,
                Convert.ToInt32(row["ID_KVETINA"]),
                MapDatabaseValueToEnum(row["STAV"].ToString()),
                Convert.ToInt32(row["STARI"])
            );
            
            return (Flower)Convert.ChangeType(flower, typeof(Flower));
        }

        public async Task GetAll()
        {
            Flowers.Clear();
            string command = "GET_ALL_KVETINY";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);
            
            foreach (DataRow row in dataTable.Rows)
            {
                byte[] imageBytes = row["OBRAZEK"] as byte[] ?? new byte[0]; // Get the image bytes or empty byte array if null

                var flower = new Flower(
                    Convert.ToInt32(row["ID_ZBOZI"]),
                    row["NAZEV"].ToString(),
                    Convert.ToDouble(row["CENA"]),
                    Convert.ToChar(row["TYP"]),
                    Convert.ToInt32(row["SKLAD"]),
                    imageBytes,
                    Convert.ToInt32(row["ID_KVETINA"]),
                    MapDatabaseValueToEnum(row["STAV"].ToString()),
                    Convert.ToInt32(row["STARI"])
                );
                Flowers.Add(flower);
            }
        }

        public async Task Add(Flower entity, string fileName, string fileExtension)
        {
            var parameters = new Dictionary<string, object>
            {
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "SKLAD", entity.Warehouse },
                // Convert the byte[] to OracleParameter with OracleDbType.Blob
                //{ "OBRAZEK", new OracleParameter("OBRAZEK", OracleDbType.Blob) { Value = entity.Image } },
                { "STAV", entity.State.ToString() },
                { "STARI", entity.Age },
                { "FILE_NAME", fileName },
                { "FILE_EXTENSION", fileExtension }
            };

            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "p_OBRAZEK";
            blobParameter.OracleDbType = OracleDbType.Blob;
            blobParameter.Value = entity.Image;

            await dbUtil.ExecuteStoredProcedureAsyncWithBlob("AddData.addkvetiny", blobParameter, parameters);
        }

        public async Task Update(Flower entity, string fileName, string fileExtension)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", entity.IdGoods },
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "SKLAD", entity.Warehouse },
                {"ID_KVETINA", entity.IdFlower},
                {"STAV", entity.State.ToString()},
                {"STARI", entity.Age},
                { "FILE_NAME", fileName },
                { "FILE_EXTENSION", fileExtension }
            };

            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "OBRAZEK";
            blobParameter.OracleDbType = OracleDbType.Blob;
            blobParameter.Value = entity.Image;

            await dbUtil.ExecuteStoredProcedureAsyncWithBlob("updatekvetiny", blobParameter, parameters);
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

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(int));
            dataTable.Columns.Add("Warehouse", typeof(int));
            dataTable.Columns.Add("FlowerState", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Columns.Add("Image", typeof(byte[]));
            dataTable.Columns.Add("Image_name", typeof(string));

            foreach (var flower in Flowers)
            {
                Image image = null;

                if (flower.Image != null && flower.Image.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(flower.Image))
                    {
                        image = Image.FromStream(ms);
                    }
                }

                dataTable.Rows.Add(
                    flower.IdFlower,
                    flower.Name,
                    flower.Price,
                    flower.Warehouse,
                    flower.State.ToString(),
                    flower.Age,
                    flower.Image,
                    await dbUtil.GetFileNameFromBlobInfo(flower.IdGoods, "ZBOZI")
                    );
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