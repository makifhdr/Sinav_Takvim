using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClosedXML.Excel;

namespace Sinav_Takvim;

public partial class SinavProgramiWindow
{
    private List<(Ders, int)> dersVeSureler;
    private DateTime baslangicTarihi;
    private DateTime bitisTarihi;
    private SinavTuru sinavTuru;
    private int beklemeSuresi;
    private bool ayniZaman;
    private Bolum bolum;
    private (int baslangic, int bitis) saatAraligi;
    
    private DerslikDAO derslikDAO = new();
    private OgrenciDersDAO ogrenciDersDAO = new();
    private DersDAO dersDAO = new();
    
    private List<Sinav> sinavList = [];
    
    public SinavProgramiWindow(List<(Ders, int)> dersVeSureler, DateTime baslangicTarihi, DateTime bitisTarihi,
        SinavTuru sinavTuru, int beklemeSuresi, bool ayniZaman, (int baslangic, int bitis) saatAraligi, 
        Bolum bolum)
    {
        this.dersVeSureler = dersVeSureler;
        this.baslangicTarihi = baslangicTarihi;
        this.bitisTarihi = bitisTarihi;
        this.sinavTuru = sinavTuru;
        this.beklemeSuresi = beklemeSuresi;
        this.ayniZaman = ayniZaman;
        this.bolum = bolum;
        this.saatAraligi = saatAraligi;
        InitializeComponent();
        TitleTextBlock.Text = $"{BolumHelper.GetIsim(bolum)} Bölümü {sinavTuru} Sınav Programı";

        if (ayniZaman) AyniZamanliSinavProgramiOlustur();
        
        else SinavProgramiOlustur();
    }

    private void SinavProgramiOlustur()
    {
        var derslikList = derslikDAO.GetDerslikListesi(bolum);
        
        sinavList = [];
        
        var isGunuSayisi = HesaplaIsGunu(baslangicTarihi, bitisTarihi);
        
        var sinifSinavDagilimi = new Dictionary<int, List<int>>
        {
            {1, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(1, bolum), isGunuSayisi)},
            {2, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(2, bolum), isGunuSayisi)},
            {3, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(3, bolum), isGunuSayisi)},
            {4, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(4, bolum), isGunuSayisi)}
        };
        
        int gunSayi = 0;
        
        for (var i = baslangicTarihi; i <= bitisTarihi; i = i.AddDays(1))
        {
            if(i.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            
            var startTime = new TimeSpan(saatAraligi.baslangic,0,0);
            var endTime = new TimeSpan(saatAraligi.bitis,0,0);
            
            var sinifSinavSayisi = new Dictionary<int, int>
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}
            };

            var availableDerslikList = new List<Derslik>(derslikList);
            
            for (var time = startTime; time < endTime;)
            {
                var sinifSaatSinavYapti = false;
                
                int atlamaDk = 0;
                
                var tempDersSureList = new List<(Ders, int)>(dersVeSureler);
                
                foreach (var ders in dersVeSureler)
                {
                    if (sinifSaatSinavYapti) continue;
                    
                    if (sinifSinavSayisi[ders.Item1.GetSinif()] >= 
                        sinifSinavDagilimi[ders.Item1.GetSinif()][gunSayi]) continue;
                    
                    var dersOgrenciSayisi = ogrenciDersDAO.GetDersOgrenciler(ders.Item1.GetKod(), bolum).Count;
                    var temp = dersOgrenciSayisi;

                    var list = new List<Derslik>();

                    var tempDerslikList = new List<Derslik>(availableDerslikList);
                    int kalanKapasite = 0;

                    foreach (var derslik in tempDerslikList)
                    {
                        kalanKapasite += derslik.GetKapasite();
                    }

                    if (kalanKapasite < dersOgrenciSayisi)
                        break;
                    
                    foreach (var derslik in availableDerslikList)
                    {
                        temp -= derslik.GetKapasite();
                        list.Add(derslik);
                        tempDerslikList.Remove(derslik);
                        if (temp <= 0)
                        {
                            break;
                        }
                    }
                    availableDerslikList = new List<Derslik>(tempDerslikList);
                    
                    var sinav = new Sinav(i, time, ders.Item2, ders.Item1);
                    
                    sinifSinavSayisi[ders.Item1.GetSinif()]++;
                    sinifSaatSinavYapti = true;
                    
                    sinav.AddDerslikler(list);
                    sinavList.Add(sinav);

                    var ders1 = ders;
                    tempDersSureList.RemoveAll(x => x.Item1.GetKod() == ders1.Item1.GetKod());
                    
                    atlamaDk = Math.Max(atlamaDk, ders.Item2 + beklemeSuresi);
                    
                    if (availableDerslikList.Count == 0) break;
                }
                dersVeSureler = new List<(Ders, int)>(tempDersSureList);
                
                if (atlamaDk == 0)
                {
                    time += TimeSpan.FromMinutes(beklemeSuresi);
                    continue;
                }
                
                time += TimeSpan.FromMinutes(atlamaDk);
                availableDerslikList = new List<Derslik>(derslikList);
                if(tempDersSureList.Count == 0) break;

                var breakAt = true;

                for (var a = 1; a < 5; a++)
                {
                    if (!(sinifSinavSayisi[a] >= sinifSinavDagilimi[a][gunSayi]))
                    {
                        breakAt = false;
                    }
                }

                if (breakAt) break;
            }
            gunSayi++;
        }
        SinavListesiCiz();
    }
    
    private int HesaplaIsGunu(DateTime baslangic, DateTime bitis)
    {
        int sayac = 0;

        for (var tarih = baslangic; tarih <= bitis; tarih = tarih.AddDays(1))
        {
            if (tarih.DayOfWeek != DayOfWeek.Saturday && tarih.DayOfWeek != DayOfWeek.Sunday)
                sayac++;
        }

        return sayac;
    }

    private List<int> DersleriGunlereDagit(int dersSayisi, int gunSayisi)
    {
        int tabanDeger = dersSayisi / gunSayisi;
        int kalan = dersSayisi % gunSayisi;

        var dagilim = new List<int>();

        for (int i = 0; i < gunSayisi; i++)
        {
            if (i < kalan)
                dagilim.Add(tabanDeger + 1);
            else
                dagilim.Add(tabanDeger);
        }

        return dagilim;
    }

    private void AyniZamanliSinavProgramiOlustur()
    {
        var derslikList = derslikDAO.GetDerslikListesi(bolum);
        
        sinavList = [];
        
        var isGunuSayisi = HesaplaIsGunu(baslangicTarihi, bitisTarihi);
        
        var sinifSinavDagilimi = new Dictionary<int, List<int>>
        {
            {1, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(1, bolum), isGunuSayisi)},
            {2, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(2, bolum), isGunuSayisi)},
            {3, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(3, bolum), isGunuSayisi)},
            {4, DersleriGunlereDagit(dersDAO.GetSinifDersSayisi(4, bolum), isGunuSayisi)}
        };
        
        int gunSayi = 0;
        
        for (var i = baslangicTarihi; i <= bitisTarihi; i = i.AddDays(1))
        {
            if(i.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            
            var startTime = new TimeSpan(saatAraligi.baslangic,0,0);
            var endTime = new TimeSpan(saatAraligi.bitis,0,0);
            
            var sinifSinavSayisi = new Dictionary<int, int>
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}
            };

            var availableDerslikList = new List<Derslik>(derslikList);
            
            for (var time = startTime; time < endTime;)
            {
                var sinifSaatSinavYapti = new Dictionary<int, bool>
                {
                    {1, false},
                    {2, false},
                    {3, false},
                    {4, false}
                };
                
                int atlamaDk = 0;
                
                var tempDersSureList = new List<(Ders, int)>(dersVeSureler);
                
                foreach (var ders in dersVeSureler)
                {
                    if (sinifSaatSinavYapti[ders.Item1.GetSinif()]) continue;
                    
                    if (sinifSinavSayisi[ders.Item1.GetSinif()] >= 
                        sinifSinavDagilimi[ders.Item1.GetSinif()][gunSayi]) continue;
                    
                    var dersOgrenciSayisi = ogrenciDersDAO.GetDersOgrenciler(ders.Item1.GetKod(), bolum).Count;
                    var temp = dersOgrenciSayisi;

                    var list = new List<Derslik>();

                    var tempDerslikList = new List<Derslik>(availableDerslikList);
                    int kalanKapasite = 0;

                    foreach (var derslik in tempDerslikList)
                    {
                        kalanKapasite += derslik.GetKapasite();
                    }
                    
                    if (kalanKapasite < dersOgrenciSayisi)
                        break;
                    
                    foreach (var derslik in availableDerslikList)
                    {
                        temp -= derslik.GetKapasite();
                        list.Add(derslik);
                        tempDerslikList.Remove(derslik);
                        if (temp <= 0)
                        {
                            break;
                        }
                    }
                    availableDerslikList = new List<Derslik>(tempDerslikList);
                    
                    var sinav = new Sinav(i, time, ders.Item2, ders.Item1);
                    
                    sinifSinavSayisi[ders.Item1.GetSinif()]++;
                    sinifSaatSinavYapti[ders.Item1.GetSinif()] = true;
                    
                    sinav.AddDerslikler(list);
                    sinavList.Add(sinav);

                    var dersKod = ders.Item1.GetKod();
                    tempDersSureList.RemoveAll(x => x.Item1.GetKod() == dersKod);
                    
                    atlamaDk = Math.Max(atlamaDk, ders.Item2 + beklemeSuresi);
                    
                    if (availableDerslikList.Count == 0) break;
                }
                dersVeSureler = new List<(Ders, int)>(tempDersSureList);
                
                if (atlamaDk == 0)
                {
                    time += TimeSpan.FromMinutes(beklemeSuresi);
                    continue;
                }
                
                time += TimeSpan.FromMinutes(atlamaDk);
                availableDerslikList = new List<Derslik>(derslikList);
                if(tempDersSureList.Count == 0) break;

                var breakAt = true;

                for (var a = 1; a < 5; a++)
                {
                    if (!(sinifSinavSayisi[a] >= sinifSinavDagilimi[a][gunSayi]))
                    {
                        breakAt = false;
                    }
                }

                if (breakAt) break;
            }

            gunSayi++;
        }
        
        SinavListesiCiz();
    }

    private void SinavListesiCiz()
    {
        foreach (var sinav in sinavList)
        {
            var sinavTarihi = new TextBlock()
            {
                Text = $"{sinav.GetTarih():dd.MM.yyyy}\n" +
                       $"{sinav.GetTarih().ToString("dddd", new System.Globalization.CultureInfo("tr-TR"))}"
            };
            SinavGrid.Children.Add(sinavTarihi);
            
            var sinavSaati = new TextBlock()
            {
                Text = $"{sinav.GetSaat():hh\\:mm}"
            };
            SinavGrid.Children.Add(sinavSaati);
            
            var sinavDersAdi = new TextBlock()
            {
                Text = $"({sinav.GetDers().GetSinif()}. sınıf) {sinav.GetDers().GetAd()}"
            };
            SinavGrid.Children.Add(sinavDersAdi);
            
            var sinavDersOgretmen = new TextBlock()
            {
                Text = $"{sinav.GetDers().GetOgretmen()}"
            };
            SinavGrid.Children.Add(sinavDersOgretmen);
            
            var sinavDerslikler = new TextBlock();

            foreach (var derslik in sinav.GetDerslikler())
            {
                sinavDerslikler.Text += derslik.GetAd();
                if(!sinav.GetDerslikler().Last().Equals(derslik)) 
                    sinavDerslikler.Text += " - ";
            }
            SinavGrid.Children.Add(sinavDerslikler);

            var derslikGoruntuleButton = new Button()
            {
                Content = "Oturma Düzenini\nGörüntüle",
                Tag = sinav.GetDers().GetKod()//
            };
            derslikGoruntuleButton.Click += DerslikGoruntulButton_Click;
            SinavGrid.Children.Add(derslikGoruntuleButton);

            foreach (var child in SinavGrid.Children)
            {
                if (child is TextBlock textBlock)
                {
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Background = new SolidColorBrush(Colors.AliceBlue);
                    textBlock.Padding = new Thickness(5);
                    textBlock.Margin = new Thickness(10);
                }
            }
        }

        if (dersVeSureler.Count != 0)
        {
            var uyariString = "";
            foreach (var item in dersVeSureler)
            {
                uyariString += item.Item1.GetAd() + "\n";
            }
            
            MessageBox.Show(
                $"Tarih kısıtlamasından ve derslik azlığından dolayı şu dersler programa eklenemedi:\n{uyariString}",
                "Uyarı",
                MessageBoxButton.OK);
        }
    }

    private Sinav GetSinav(string dersKodu)
    {
        return sinavList.Find(x => x.GetDers().GetKod().Equals(dersKodu)) 
               ?? throw new InvalidOperationException();
    }

    private void DerslikGoruntulButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;

        var sinav = GetSinav(button.Tag.ToString() ?? throw new InvalidOperationException());
        var window = new DerslikSecmeWindow(sinav, 
            ogrenciDersDAO.GetDersOgrenciler(sinav.GetDers().GetKod(), bolum));
        window.Show();
    }
    
    private void ExcelOlusturButton_Click(object sender, RoutedEventArgs e)
    {
        string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            ?.Parent?.Parent?.Parent?.FullName ?? string.Empty;

        string folderPath = Path.Combine(projectRoot, "files");
        Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, "SinavProgrami.xlsx");


        try
        {
            using (var wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add($"{sinavTuru} Programı");

                ws.Range("A1:E1").Merge();
                ws.Cell("A1").Value = $"{BolumHelper.GetIsim(bolum)} Bölümü {sinavTuru} Sınav Programı";
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Font.FontSize = 14;
                ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.Orange;

                ws.Cell("A2").Value = "Tarih";
                ws.Cell("B2").Value = "Sınav Saati";
                ws.Cell("C2").Value = "Ders Adı";
                ws.Cell("D2").Value = "Öğretim Elemanı";
                ws.Cell("E2").Value = "Derslik";

                var headerRange = ws.Range("A2:E2");
                headerRange.Style.Fill.BackgroundColor = XLColor.Orange;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


                int row = 3;
                foreach (var sinav in sinavList)
                {
                    var dersliklerString = "";
                    foreach (var derslik in sinav.GetDerslikler())
                    {
                        dersliklerString += derslik.GetAd();
                        if (!sinav.GetDerslikler().Last().Equals(derslik))
                            dersliklerString += " - ";
                    }

                    ws.Cell(row, 1).Value = $"{sinav.GetTarih():dd.MM.yyyy}";
                    ws.Cell(row, 2).Value = $"{sinav.GetSaat():hh\\:mm}";
                    ws.Cell(row, 3).Value = sinav.GetDers().GetAd();
                    ws.Cell(row, 4).Value = sinav.GetDers().GetOgretmen();
                    ws.Cell(row, 5).Value = dersliklerString;

                    var range = ws.Range($"A{row}:E{row}");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    row++;
                }

                ws.Columns().AdjustToContents();
                ws.Column(3).Width = 35;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 20;

                wb.SaveAs(filePath);

                ResultText.Text = $"Excel başarıyla oluşturuldu: {filePath}";

            }
        }
        catch (IOException ex)
        {
            ResultText.Text = "Dosya başka bir program tarafından kullanılıyor!";
        }

    }
}