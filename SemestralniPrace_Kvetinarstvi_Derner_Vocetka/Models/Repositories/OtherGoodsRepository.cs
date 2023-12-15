using Oracle.ManagedDataAccess.Client;
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
using System.Windows.Documents;

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
            string command = "GET_OSTATNI_BY_ID";

            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);

            if (dataTable.Rows.Count == 0)
                return null;
          
            var row = dataTable.Rows[0];

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
            string command = "GET_ALL_OSTATNI";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                byte[] imageBytes = row["OBRAZEK"] as byte[] ?? new byte[0];
                
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

        public async Task Add(OtherGoods entity, string fileName, string fileExtension)
        {
            
            var parameters = new Dictionary<string, object>
            {
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "SKLAD", entity.Warehouse },
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate.Value.Day + "." + entity.ExpirationDate.Value.Month + "." + entity.ExpirationDate.Value.Year},
                { "FILE_NAME", fileName },
                { "FILE_EXTENSION", fileExtension }
            };

            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "OBRAZEK";
            blobParameter.OracleDbType = OracleDbType.Blob;
            blobParameter.Value = entity.Image;

            await dbUtil.ExecuteStoredProcedureAsyncWithBlob("addostatni", blobParameter, parameters);

        }

        public async Task Update(OtherGoods entity, string fileName, string fileExtension)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", entity.IdGoods },
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse },
                {"ID_OSTATNI", entity.IdOtherGoods},
                { "ZEME_PUVODU", entity.CountryOfOrigin },
                { "DATUM_TRVANLIVOSTI", entity.ExpirationDate.Value.Day + "." + entity.ExpirationDate.Value.Month + "." + entity.ExpirationDate.Value.Year},
                { "FILE_NAME", fileName },
                { "FILE_EXTENSION", fileExtension }
            };

            OracleParameter blobParameter = new OracleParameter();
            blobParameter.ParameterName = "OBRAZEK";
            blobParameter.OracleDbType = OracleDbType.Blob;
            blobParameter.Value = entity.Image;

            await dbUtil.ExecuteStoredProcedureAsyncWithBlob("updateostatni", blobParameter, parameters);
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
            dataTable.Columns.Add("Image", typeof(byte[]));
            dataTable.Columns.Add("Image_name", typeof(string));


            foreach (var otherGoods in OtherGoods)
            {
                Image image = null;
                if (otherGoods.Image != null && otherGoods.Image.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(otherGoods.Image))
                    {
                        image = Image.FromStream(ms);
                    }
                }

                dataTable.Rows.Add(
                    otherGoods.IdOtherGoods,
                    otherGoods.Name,
                    otherGoods.Price,
                    otherGoods.Warehouse,
                    otherGoods.CountryOfOrigin,
                    otherGoods.ExpirationDate.Value.Day + "." + otherGoods.ExpirationDate.Value.Month + "." + otherGoods.ExpirationDate.Value.Year,
                    otherGoods.Image,
                    await dbUtil.GetFileNameFromBlobInfo(otherGoods.IdGoods, "ZBOZI")

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
            throw new NotImplementedException();
        }

        public Task Add(OtherGoods entity)
        {
            //not using rn
            throw new NotImplementedException();
        }
        public Task Update(OtherGoods entity)
        {
            throw new NotImplementedException();
        }

    }
}