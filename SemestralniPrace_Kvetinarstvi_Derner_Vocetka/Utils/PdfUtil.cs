using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils
{
    public class PdfUtil
    {
        CustomerRepository customerRepository;
        public byte[] GeneratePdf(
            string cusName,
            string zpusobPrevzeti,
            List<int> goodsIdInOrder,
            List<int> goodsCountInOrder,
            string selectedOccasion,
            string selectedBilling,
            string selectedDeliveryCompany)
        {
            CustomerRepository customerRepository = new CustomerRepository();

            // Create a simple text-based PDF content
            string pdfContent = $@"
      FAKTURA
---------------------
 Jméno zákazníka: {cusName}
 Způsob Převzetí: {zpusobPrevzeti}
 Zboží: {string.Join(",", goodsIdInOrder)}
 Počet zboží: {string.Join(",", goodsCountInOrder)}
 Selected Occasion: {selectedOccasion}
 Způsob platby: {selectedBilling}
 Dodací společnost: {selectedDeliveryCompany}
";

            // Convert the text content to bytes
            byte[] pdfBytes = Encoding.UTF8.GetBytes(pdfContent);

            return pdfBytes;
        }
    }
}
