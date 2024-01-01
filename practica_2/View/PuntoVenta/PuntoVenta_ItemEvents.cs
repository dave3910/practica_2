using practica_2.Model;
using System;

namespace practica_2.View.PuntoVenta
{
    internal partial class PuntoVenta
    {
        private static SocioNegocio model;

        private static void BuscarSocioNegocio(SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                bubbleEvent = true;

                if (!pVal.BeforeAction)
                {
                    string dni = oForm.DataSources.UserDataSources.Item("UD_DNI").Value;
                    if (string.IsNullOrEmpty(dni))
                        throw new ArgumentNullException("Debe ingresar el número de DNI del cliente");

                    model = controller.BuscarSocio(dni);

                    if (model != null)
                        UpdateUserData();
                    else
                        OpenUserRegisterForm();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void UpdateUserData()
        {
            try
            {
                oForm.DataSources.UserDataSources.Item("UD_CNOM").Value = model.Nombre;
                oForm.DataSources.UserDataSources.Item("UD_CCOD").Value = model.CodigoSAP;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void OpenUserRegisterForm()
        {
            throw new NotImplementedException();
        }
    }
}