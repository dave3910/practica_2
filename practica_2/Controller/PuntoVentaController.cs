using practica_2.Model;
using practica_2.Utils;
using SAPbobsCOM;
using System;

namespace practica_2.Controller
{
    internal class PuntoVentaController
    {
        internal SocioNegocio BuscarSocio(string dni)
        {
            try
            {
                Recordset oRecordset = Globales.sboCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT \"CardCode\", \"CardName\" FROM OCRD WHERE \"LicTradNum\" = '{dni}' ";
                oRecordset.DoQuery(query);

                if (oRecordset.RecordCount == 0)
                    return null;

                SocioNegocio socio = new SocioNegocio()
                {
                    CodigoSAP = oRecordset.Fields.Item(0).Value,
                    DNI = dni,
                    Nombre = oRecordset.Fields.Item(1).Value
                };

                return socio;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}