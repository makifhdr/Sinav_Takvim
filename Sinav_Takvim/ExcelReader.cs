using System.IO;
using ClosedXML.Excel;

namespace Sinav_Takvim;

public class ExcelReader
{
    DersDAO GetDersDAO = new();
    OgrenciDAO GetOgrenciDAO = new();
    OgrenciDersDAO GetOgrenciDersDAO = new();
    ListeYukleStatusDAO GetListeYukleStatusDAO = new();
    
    public int DersListesiYukle(string filePath, Bolum bolum)
    {
        GetOgrenciDersDAO.OgrenciDerslerSil(bolum);
        GetDersDAO.DerslerSil(bolum);
        GetListeYukleStatusDAO.SetDersListesi(bolum, false);
        
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);
        int sinif = 0;
        bool secmeli = false;

        int kod = 0;
        int ad = 0;
        int ogretmen = 0;
        
        foreach (var row in worksheet.RowsUsed())
        {
            if (row.Cell(1).Value.ToString().Contains("Sınıf"))
            {
                var ilk = row.Cell(1).Value.ToString().First();
                sinif = int.Parse(ilk.ToString());
                secmeli = false;
            }
            else if(row.Cell(1).Value.ToString().StartsWith("SEÇ")) secmeli = true;

            else if (row.Cell(1).Value.ToString().Equals("DERS KODU") ||
                     row.Cell(1).Value.ToString().Equals("DERSİN ADI") ||
                     row.Cell(1).Value.ToString().Equals("DERSİ VEREN ÖĞR. ELEMANI"))
            {
                switch (row.Cell(1).Value.ToString())
                {
                    case "DERS KODU":
                        kod = 1;
                        break;
                    case "DERSİN ADI":
                        ad = 1;
                        break;
                    case "DERSİ VEREN ÖĞR. ELEMANI":
                        ogretmen = 1;
                        break;
                }
                switch (row.Cell(2).Value.ToString())
                {
                    case "DERS KODU":
                        kod = 2;
                        break;
                    case "DERSİN ADI":
                        ad = 2;
                        break;
                    case "DERSİ VEREN ÖĞR. ELEMANI":
                        ogretmen = 2;
                        break;
                }
                switch (row.Cell(3).Value.ToString())
                {
                    case "DERS KODU":
                        kod = 3;
                        break;
                    case "DERSİN ADI":
                        ad = 3;
                        break;
                    case "DERSİ VEREN ÖĞR. ELEMANI":
                        ogretmen = 3;
                        break;
                }
            }
            
            else if(!row.Cell(1).Value.ToString().Equals("DERS KODU"))
            {
                if (string.IsNullOrWhiteSpace(row.Cell(kod).ToString())
                    || string.IsNullOrWhiteSpace(row.Cell(ad).ToString())
                    || string.IsNullOrWhiteSpace(row.Cell(ogretmen).ToString()))
                {
                    return row.RowNumber();
                }
                
                if (!GetDersDAO.DersEkle(new Ders(row.Cell(kod).Value.ToString(),
                        row.Cell(ad).Value.ToString(),
                        row.Cell(ogretmen).Value.ToString(), sinif, secmeli), bolum))
                {
                    return row.RowNumber();
                }
            }
        }
        GetListeYukleStatusDAO.SetDersListesi(bolum, true);
        return -1;
    }

    public int OgrenciListesiYukle(string filePath, Bolum bolum)
    {
        GetOgrenciDersDAO.OgrenciDerslerSil(bolum);
        GetOgrenciDAO.OgrencilerSil(bolum);
        GetListeYukleStatusDAO.SetOgrenciListesi(bolum, false);
        
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);
        
        var ogrenciNo = "";

        int ogrenciNoCol = 0;
        int ogrenciAdCol = 0;
        int ogrenciSinifCol = 0;
        int ogrenciDersCol = 0;

        var row1 = worksheet.Rows().First();

        foreach (var cell in row1.Cells())
        {
            switch(cell.Value.ToString())
            {
                case "Öğrenci No":
                    ogrenciNoCol = cell.WorksheetColumn().ColumnNumber();
                    break;
                case "Ad Soyad":
                    ogrenciAdCol = cell.WorksheetColumn().ColumnNumber();
                    break;
                case "Sınıf":
                    ogrenciSinifCol = cell.WorksheetColumn().ColumnNumber();
                    break;
                case "Ders":
                    ogrenciDersCol = cell.WorksheetColumn().ColumnNumber();
                    break;
            }
        }
        
        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            if (!row.Cell(ogrenciNoCol).Value.ToString().Equals(ogrenciNo))
            {
                if (string.IsNullOrWhiteSpace(row.Cell(ogrenciNoCol).Value.ToString())
                    || string.IsNullOrWhiteSpace(row.Cell(ogrenciAdCol).Value.ToString())
                    || string.IsNullOrWhiteSpace(row.Cell(ogrenciSinifCol).Value.ToString()))
                {
                    return row.RowNumber();
                }
                
                if (!GetOgrenciDAO.OgrenciEkle(new Ogrenci(row.Cell(ogrenciNoCol).Value.ToString(),
                        row.Cell(ogrenciAdCol).Value.ToString(),
                        (int)char.GetNumericValue(row.Cell(ogrenciSinifCol).Value.ToString().First())), bolum))
                {
                    return row.RowNumber();
                }
            }

            if (string.IsNullOrWhiteSpace(row.Cell(ogrenciDersCol).Value.ToString()))
            {
                return row.RowNumber();
            }
            
            if (!GetOgrenciDersDAO.OgrenciDersEkle(row.Cell(ogrenciNoCol).Value.ToString(),
                    row.Cell(ogrenciDersCol).Value.ToString(), bolum))
            {
                return row.RowNumber();
            }

            ogrenciNo = row.Cell(ogrenciNoCol).Value.ToString();
        }
        
        GetListeYukleStatusDAO.SetOgrenciListesi(bolum, true);
        return -1;
    }
}