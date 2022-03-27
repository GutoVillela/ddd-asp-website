using KadoshDomain.Entities;
using KadoshDomain.Enums;

namespace KadoshDomain.Queries.SaleQueries.DTOs
{
    public abstract class SaleBaseDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public int SellerId { get; set; }

        public int StoreId { get; set; }

        public IList<SaleItemDTO> SaleItems { get; set; } = new List<SaleItemDTO>();

        public decimal DownPayment { get; set; } = 0;

        public DateTime? SaleDate { get; set; }

        public ESaleSituation Situation { get; set; }

        public decimal Total { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal TotalToPay { get; set; }

        public static implicit operator SaleBaseDTO(Sale sale)
        {
            SaleBaseDTO saleDTO;

            if (sale is SaleInCash)
                saleDTO = new SaleInCashDTO();
            else if(sale is SaleOnCredit)
                saleDTO = new SaleOnCreditDTO();
            else
            {
                saleDTO = new SaleInInstallmentsDTO();
                (saleDTO as SaleInInstallmentsDTO).NumberOfInstallments = (sale as SaleInInstallments).NumberOfInstallments;
            }

            saleDTO.Id = sale.Id;
            saleDTO.CustomerId = sale.CustomerId;
            saleDTO.CustomerName = sale.Customer?.Name;
            saleDTO.SellerId = sale.SellerId;
            saleDTO.StoreId = sale.StoreId;
            saleDTO.DownPayment = sale.DownPayment;
            saleDTO.SaleDate = sale.SaleDate;
            saleDTO.Situation = sale.Situation;
            saleDTO.Total = sale.Total;
            saleDTO.TotalPaid = sale.TotalPaid;
            saleDTO.TotalToPay = sale.TotalToPay;

            foreach(var item in sale.SaleItems)
            {
                saleDTO.SaleItems.Add(item);
            }

            return saleDTO;
        }
    }
}
