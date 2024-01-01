using SAPbouiCOM;
using System;
using System.Xml;

namespace practica_2.Utils
{
    public static class Globales
    {
        public static SAPbobsCOM.Company sboCompany = null;
        public static SAPbouiCOM.Application sboApplication = null;

        internal static Form CrearFormulario(string path, string tipo, string id)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                FormCreationParams CreationPackage = sboApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
                xmlDocument.Load(path);
                CreationPackage.XmlData = xmlDocument.InnerXml;
                CreationPackage.FormType = tipo;
                CreationPackage.UniqueID = id;
                return sboApplication.Forms.AddEx(CreationPackage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}