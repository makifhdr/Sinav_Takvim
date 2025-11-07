using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class KullaniciEkleWindow
{
    KullaniciDAO KullaniciDAO = new KullaniciDAO();
    public KullaniciEkleWindow()
    {
        InitializeComponent();
    }
    
    private void Onayla_Button_Click(object sender, EventArgs e)
    {
        if (BolumComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            if (!KullaniciDAO.KullaniciEkle(epostaBox.Text, sifreBox.Text,
                    Enum.Parse<Bolum>(selectedItem.Tag.ToString() ?? throw new InvalidOperationException())))
            {
                MessageBox.Show(
                    "Hatalı veri girişi",
                    "Uyarı",
                    MessageBoxButton.OK);
                return;
            }
            MessageBox.Show(
                "Kullanıcı kaydı başarılı!",
                "Uyarı",
                MessageBoxButton.OK);
            Close();
            
        }
        
    }
}