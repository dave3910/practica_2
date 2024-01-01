using practica_2.Utils;
using practica_2.View.PuntoVenta;
using SAPbouiCOM;
using System;
using System.IO;
using System.Xml;

namespace practica_2
{
    internal class Main
    {
        public Main()
        {
            try
            {
                GetSAPBO_Connection();
                Globales.sboApplication.StatusBar.SetText("Conectado con SAP BUSINESS ONE", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                LoadEvents();
                ConfigurarFiltros();
                CrearMenues();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void ConfigurarFiltros()
        {
            try
            {
                SAPbouiCOM.EventFilters oFilters = new SAPbouiCOM.EventFilters();
                SAPbouiCOM.EventFilter oFilter;

                oFilter = oFilters.Add(BoEventTypes.et_FORM_LOAD);
                oFilter.AddEx("FrmPTV");

                oFilter = oFilters.Add(BoEventTypes.et_ITEM_PRESSED);
                oFilter.AddEx("FrmPTV");

                oFilter = oFilters.Add(BoEventTypes.et_MENU_CLICK);
                oFilter.AddEx("FrmPTV");

                Globales.sboApplication.SetFilter(oFilters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadEvents()
        {
            try
            {
                Globales.sboApplication.AppEvent += SboApplication_AppEvent;
                Globales.sboApplication.ItemEvent += SboApplication_ItemEvent;
                Globales.sboApplication.FormDataEvent += SboApplication_FormDataEvent;
                Globales.sboApplication.MenuEvent += SboApplication_MenuEvent;
                //sboApplication.RightClickEvent += SboApplication_RightClickEvent;
            }
            catch { throw; }
        }

        private void SboApplication_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            try
            {
                BubbleEvent = true;

                if (!pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        case "MNUID_PTOVTA":

                            PuntoVenta.CrearFormulario();
                            PuntoVenta.CargarDatosPorDefecto();

                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SboApplication_FormDataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void SboApplication_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                switch (pVal.FormTypeEx)
                {
                    //OBJETOS DE USUARIO
                    case "FrmPTV": PuntoVenta.HandleItemEvent(FormUID, pVal, out BubbleEvent); break;
                }
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                Globales.sboApplication.StatusBar.SetText(ex.Message);
            }
        }

        private void SboApplication_AppEvent(BoAppEventTypes EventType)
        {
            try
            {
                switch (EventType)
                {
                    case BoAppEventTypes.aet_ShutDown:
                    case BoAppEventTypes.aet_CompanyChanged:
                    case BoAppEventTypes.aet_LanguageChanged:
                    case BoAppEventTypes.aet_ServerTerminition:
                        Globales.sboCompany.Disconnect();
                        System.Windows.Forms.Application.Exit();
                        break;
                }
            }
            catch (Exception ex)
            {
                Globales.sboApplication.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        private void CrearMenues()
        {
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                Globales.sboApplication.Forms.GetFormByTypeAndCount(169, 1).Freeze(true);
                string rutaMenuXML = Path.Combine(System.Windows.Forms.Application.StartupPath, "View", "Menues", "Menu.xml");
                xmlDocument.Load(rutaMenuXML);
                Globales.sboApplication.LoadBatchActions(xmlDocument.InnerXml);
            }
            catch (FileNotFoundException)
            {
                Globales.sboApplication.StatusBar.SetText("El recurso menu.xml, no fue encontrado", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
            catch { throw; }
            finally
            {
                Globales.sboApplication.Forms.GetFormByTypeAndCount(169, 1).Freeze(false);
                Globales.sboApplication.Forms.GetFormByTypeAndCount(169, 1).Update();
            }
        }

        private void GetSAPBO_Connection()
        {
            try
            {
                GetUIAPIConnection();
                GetDIAPIConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetUIAPIConnection()
        {
            SboGuiApi sboGuiApi = null;
            string connectionString = string.Empty;

            try
            {
                sboGuiApi = new SboGuiApi();
                connectionString = Environment.GetCommandLineArgs().GetValue(Environment.GetCommandLineArgs().Length > 0 ? 1 : 0).ToString();
                sboGuiApi.Connect(connectionString);
                Globales.sboApplication = sboGuiApi.GetApplication(-1);

                if (Globales.sboApplication is null) throw new NullReferenceException("[UI API - Connection] - Ocurrio un error al obtener la conexión con SAP BUSINESS ONE");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetDIAPIConnection()
        {
            Globales.sboCompany = new SAPbobsCOM.Company();
            string cookie = Globales.sboCompany.GetContextCookie();
            string connStr = Globales.sboApplication.Company.GetConnectionContext(cookie);

            if (Globales.sboCompany.Connected)
                Globales.sboCompany.Disconnect();

            long ret;
            ret = Globales.sboCompany.SetSboLoginContext(connStr);

            if (ret != 0)
                throw new Exception("[DI API]- Ocurrió un error al obtener la conexión DI API");

            ret = Globales.sboCompany.Connect();
        }
    }
}