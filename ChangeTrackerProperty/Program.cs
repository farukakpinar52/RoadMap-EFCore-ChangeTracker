// See https://aka.ms/new-console-template for more information
using EntityStates.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");

EntityStates.DAL.Entities.ChangeTrackerDbContext context = new();

#region ChangeTracker NEDİR ?
//context nesnesi üzerinden verilen tüm veriler/nesneler otomatik olarak bir takip mekanizması tarafından izlenirler.
//Bu takip mekanizmasına CHANGE TRACKER denir. Change Tracker ile nesneler üzerindeki değişiklikler/işlemler takip edilerek bu işlemlerin fıtratına uygun sql sorguları generate edilecek. İşte bu işleme Change Tracking denir
#endregion

#region ChangeTracker Property ve Metodları
//takip edilen nesneye erişebilmemizi ve gerektiği takdirde işlemler gerçekleştirmemizi sağlar
//context sınıfının base classı olan DbContext sınıfının bir memberıdır.

//List<Urun> urunler = await context.Set<Urun>().ToListAsync();
//urunler[6].Fiyat = 4442; //update
//context.Uruns.Remove(urunler[7]); //delete
//urunler[8].UrunAdi = "Ürün Adı Değişti"; //update

//var datas = context.ChangeTracker.Entries(); //resultview üzerinde nesnelerin durumu Unchanged olarak görünür.
//işlem yapılan satırların ise deleted ve updated olarak görülür.
//Console.WriteLine();

#region 1.DetectChanges Metodu
//EfCore context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri ChangeTracker sayesinde takip edebilmekte ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntülerini oluşturabilir.
//Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir.SaveChanges fonksiyonu çağırılığı anda nesneler EfCore tarafından otomatik olarak kontrol edilirler.

//Ancak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasını opsiyonel olarak gerçekleştirmek isteyebiliriz. işte bunun için DetectChanges fonksiyonu kullanılabilir.
//SaveChanges fonksiyonu normalde otomatik DetectChanges fonksiyonunu tetiklese bile biz kendimiz manuel olarak bunu yapmak istediğimizde DetectChanges fonksiyonunu çalıştırarak son verilerin güncel olduğunun tespit edilmesini sağlayabiliriz.
//Urun urun = await context.Uruns.FirstOrDefaultAsync(u=>u.Id==3);
//urun.Fiyat = 832;

//context.ChangeTracker.DetectChanges();
//await context.SaveChangesAsync();
//Console.WriteLine();
#endregion

#region 2.AutoDetectChangesEnabled Property'si
//ilgili metotlar (SaveChanges, Entries) tarafından detectchanges metodunun otomatik olarak tetiklenmesinin konfigürasyonunu yapmamızı sağlayan property'dir.
//Savechanges tetiklenince içinde default olarak detectchanges otomatik olarak çağırılır. bu durumda detectchanges fonksiyonunun kullanımını irademizle yönetmek istediğimizde maliyet/performans açısından bunun otomatikliğini kapatabilmekteyiz. 
// context.ChangeTracker.AutoDetectChangesEnabled = false;   <--- bu durumda otomatiklik kapanır
#endregion

#region 3.Entries Metodu
//EntityEntry türünden veri elde etmemizi sağlar. Context'teki Entry metodunun koleksiyonel halidir.
// changeTracker mekanizması tarafından izlenen her entity nesnesinin bilgisini EntityEntry türünden elde etmemizi sağlar ve bazı işlemleri yapmamıza olanak tanır.
// bu metot üzerinden gelen verilerin state leri üzerinden işlemler yapabiliriz.
//Entries metodu, DetectChanges metodunu tetikler.

//var urunler = await context.Uruns.ToListAsync();
//urunler.FirstOrDefault(u => u.Id == 5).Fiyat = 213;
//context.Uruns.Remove(urunler.FirstOrDefault(u => u.Id == 6));
//urunler.FirstOrDefault(u => u.Id == 7).UrunAdi = "Yeni değişen ürün";

//context.ChangeTracker.Entries().ToList().ForEach(entry =>
//{
//    if (entry.State == EntityState.Detached)  Console.WriteLine("takipsiz veri");
//    if (entry.State == EntityState.Deleted) Console.WriteLine("silinmiş veri");
//    if (entry.State == EntityState.Added) Console.WriteLine("eklenmiş veri");
//    if (entry.State == EntityState.Modified) Console.WriteLine("değişmiş veri");
//    if(entry.State == EntityState.Unchanged) Console.WriteLine("değişim yok");
//});


#endregion

#region 4.AcceptAllChanges Metodu
//SaveChanges() çalıştığında tüm takip mekanizmasını ortadan kaldırır. SaveChanges başarılı olmazsa tüm takip kaldırılacağı için değişiklikler gerçekleşmez bu sebeple düzeltme yapmak için tüm işlemleri baştan yapmak gerekir.

//SaveChanges(false) EFCore'a gerekli veritabanı komutlarını yürütmesini söyler ancak takibi bırakmaz, takip devam eder. Takip AcceptAllChanges fonksiyonu çalıştırılırsa kesilir.
//var urunler = await context.Uruns.ToListAsync();
//urunler.FirstOrDefault(u => u.Id == 5).Fiyat = 24;
////context.Uruns.Remove(urunler.FirstOrDefault(u => u.Id == 11));
//urunler.FirstOrDefault(u => u.Id == 7).UrunAdi = "ürün";

////buradaki değişimler SaveChanges(false) ile veritabanına yansır fakat bağlantı kopmaz.
//await context.SaveChangesAsync(false);
////bağlantıyı AcceptAllChanges ile koparırız

//context.ChangeTracker.AcceptAllChanges();
//Console.WriteLine();

#endregion

#region 5.HasChanges metodu
//takip edilen neesneler arasında değişiklik olup olmadığını kontrol eder bunu ypaabilmek için DetectChanges fonksiyonunu bir kez tetikler.
//bool takipDevamMı = context.ChangeTracker.HasChanges();
#endregion

#endregion

#region Context nesnesi üzerinden ChangeTracker kullanımı

// context.ChangeTracker... şekinde devam ederseniz gelen tüm nesneler üzerinden işlemler yapılır
//context.Entry... şeklinde devam edersen o an ki nesne üzerinden işlemler yaparsın

//var urun = await context.Uruns.FirstOrDefaultAsync(u=>u.Id==10);
//urun.Fiyat = 4242; //bu veri Update | Modified işlemi uygulandı. Peki biz bu veririn değişmeden önceki haline nasıl ulaşırız ?
//urun.UrunAdi = "Patataes";

#region a.CurrentValues ile değişen verilerin heap üzerindeki hallerine ulaşmak
//Console.WriteLine( "Verinin en güncel hali :");
//Console.WriteLine(context.Uruns.Entry(urun).CurrentValues.GetValue<float>(nameof(urun.Fiyat)));
//Console.WriteLine(context.Uruns.Entry(urun).CurrentValues.GetValue<string>(nameof(urun.UrunAdi))+"\n");
//burada SetValue ile değer atanabilir.
#endregion

#region b.OriginalValues ile değişen verilerin önceki değerlerine ulaşmak
//Console.WriteLine("Verinin önceki halleri : ");
//Console.WriteLine(context.Uruns.Entry(urun).OriginalValues.GetValue<float>(nameof(urun.Fiyat)));
//Console.WriteLine(context.Uruns.Entry(urun).OriginalValues.GetValue<string>(nameof(urun.UrunAdi)));
#endregion

#region c.GetDatabaseValues
//Console.WriteLine("Verinin veritabanındaki hali : ");
//Console.WriteLine(context.Uruns.Entry(urun).GetDatabaseValues().GetValue<float>(nameof(urun.Fiyat)));
//Console.WriteLine(context.Uruns.Entry(urun).GetDatabaseValues().GetValue<string>(nameof(urun.UrunAdi)));


#endregion
#endregion

#region ChangeTracker 'ın interceptor(yol kesici/araya girici) olarak kullanımı
//aşağıdaki context sınıfımız içinde override edilerek düzenlenmiş olan SaveChanges fonksiyonunu oluştururken ChangeTracker ile araya girerek gerekli olan şartlı işlemleri gerçekleştirebiliriz.
//public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = Default)
//{
//    var entries = ChangeTracker.Entries(); //tüm takipteki veriler yakalanır
//    foreach (var entry in entries)
//    {
//        if (entry.State == EntityState.Added)
//        {
//            //... işlemler
//        }
//        else if (entry.State == EntityState.Modified)
//        {
//            //işlemler
//        }
//    }
// return base.SaveChangesAsync(cancellationToken)
//}
#endregion