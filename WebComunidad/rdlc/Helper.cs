using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;

namespace WebComunidad.rdlc
{
    public class Helper
    {
        public static byte[] GetBytesPdfReport(string rdlcEmbeddedResourceName, ReportParameter rdlcParameter, ReportDataSource rdlcDataSource)
        {
            return GetBytesPdfReport(rdlcEmbeddedResourceName, rdlcParameter, new ReportDataSource[] { rdlcDataSource });
        }
        public static byte[] GetBytesPdfReport(string rdlcEmbeddedResourceName, ReportParameter rdlcParameter, IEnumerable<ReportDataSource> rdlcDataSources)
        {
            return GetBytesPdfReport(rdlcEmbeddedResourceName, new ReportParameter[] { rdlcParameter } , rdlcDataSources);
        }
        public static byte[] GetBytesPdfReport(string rdlcEmbeddedResourceName, IEnumerable<ReportParameter> rdlcParameters, IEnumerable<ReportDataSource> rdlcDataSources)
        {
            LocalReport localReport = new LocalReport();
            localReport.DataSources.Clear();
            localReport.ReportEmbeddedResource = rdlcEmbeddedResourceName;
            localReport.SetParameters(rdlcParameters);
            rdlcDataSources.ToList().ForEach(x => localReport.DataSources.Add(x));

            string rdlcType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            // render
            renderedBytes = localReport.Render(rdlcType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return renderedBytes;
        }
    }
}