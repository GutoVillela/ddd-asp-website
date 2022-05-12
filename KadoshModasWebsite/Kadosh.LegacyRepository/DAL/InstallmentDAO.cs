using KadoshDomain.Enums;
using KadoshDomain.LegacyEntities;
using System.Data;
using System.Data.SqlClient;

namespace Kadosh.LegacyRepository.DAL
{
    internal class InstallmentDAO
    {
        private const string TABLE_NAME = "TB_PARCELAS_DA_VENDA";
        private readonly Connection _connection;

        public InstallmentDAO(Connection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<InstallmentLegacy>> ReadAllFromSaleLegacyAsync(int saleLegacyId)
        {
            using var connection = await _connection.GetConnectionAsync();
            SqlCommand cmd = new(@"SELECT * FROM " + TABLE_NAME + " WHERE VENDA = @VENDA ORDER BY PARCELA DESC", connection);
            cmd.Parameters.AddWithValue("@VENDA", saleLegacyId).SqlDbType = SqlDbType.Int;

            SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

            List<InstallmentLegacy> installments = new();

            while (await dataReader.ReadAsync())
            {
                InstallmentLegacy installment = new()
                {
                    Number = int.Parse(dataReader["PARCELA"].ToString()),
                    Value = decimal.Parse(dataReader["VALOR_PARCELA"].ToString()),
                    Discount = string.IsNullOrEmpty(dataReader["DESCONTO"].ToString()) ? 0 : decimal.Parse(dataReader["DESCONTO"].ToString()),
                    Situation = (EInstallmentLegacySituation)int.Parse(dataReader["SITUACAO"].ToString()),
                    MaturityDate = DateTime.Parse(dataReader["VENCIMENTO"].ToString()),
                    SaleId = int.Parse(dataReader["VENDA"].ToString())
                };

                if (!string.IsNullOrEmpty(dataReader["DT_PAGAMENTO"].ToString()))
                    installment.SettlementDate = DateTime.Parse(dataReader["DT_PAGAMENTO"].ToString());

                installments.Add(installment);
            }

            dataReader.Close();

            return installments;
        }
    }
}