using practica_2.Controller;
using practica_2.Utils;
using SAPbouiCOM;
using System;

namespace practica_2.View.PuntoVenta
{
    internal static partial class PuntoVenta
    {
        private const string PATH = "View/PuntoVenta/FormPTV.srf";
        private const string TYPE = "FrmPTV";

        private static readonly PuntoVentaController controller;
        private static Form oForm;

        static PuntoVenta()
        {
            controller = new PuntoVentaController();
        }

        internal static void HandleItemEvent(string formUID, ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            oForm = Globales.sboApplication.Forms.Item(formUID);

            try
            {
                switch (pVal.EventType)
                {
                    case BoEventTypes.et_FORM_LOAD:
                        Form_Load(pVal, out bubbleEvent);
                        break;

                    case BoEventTypes.et_ITEM_PRESSED:
                        Item_Pressed(pVal, out bubbleEvent);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Form_Load(ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                bubbleEvent = true;

                if (!pVal.BeforeAction)
                {
                    oForm.Visible = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Item_Pressed(ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                bubbleEvent = true;

                switch (pVal.ItemUID)
                {
                    case "Item_5": BuscarSocioNegocio(pVal, out bubbleEvent); break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void CargarDatosPorDefecto()
        {
            try
            {
                oForm.DataSources.UserDataSources.Item("UD_VEN").Value = Globales.sboCompany.UserName;
                oForm.DataSources.UserDataSources.Item("UD_FECH").Value = Globales.sboCompany.GetDBServerDate().ToString("yyyyMMdd");
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void CrearFormulario()
        {
            try
            {
                var formUID = string.Concat(TYPE, new Random().Next(0, 1000));
                oForm = Globales.CrearFormulario(PATH, TYPE, formUID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}