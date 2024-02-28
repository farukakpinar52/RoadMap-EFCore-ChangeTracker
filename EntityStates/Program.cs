// See https://aka.ms/new-console-template for more information
using EntityStates.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

Console.WriteLine("Hello, World!");

ChangeTrackerDbContext context = new ChangeTrackerDbContext();
#region Entity States
//Entity nesnelerinin durumlarını ifade eder.

#region 1.Detached
//Urun urun= new Urun();

//Console.WriteLine(context.Entry(urun).State);
#endregion

#region 2.Added
//Urun urun = new Urun() { Fiyat = 32, UrunAdi = "Babab" };
//Console.WriteLine(context.Entry(urun).State); //veri henüz context nesnesi üzerinden işleme tabi olmadığı için state durumu Detached halinde
//await context.Uruns.AddAsync(urun);
//Console.WriteLine(context.Entry(urun).State); //burada veri eklendiği için state durumu Added oldu
//await context.SaveChangesAsync();
#endregion

#region 3.Unchanged
//Veritabanından sorgulama neticesinde EF ile elde edilen veriler bu state durumu ile gelir.
//var urunler =await context.Uruns.ToListAsync();
//var datas = context.ChangeTracker.Entries();

#endregion

#region 4.Modified
//Veritabanından sorgulama neticesinde EF ile elde edilen veriler üzerinde bir update yapıldığında savechanges bunun bir update olduğunu algılar ve state durumunu Modified 'a çevirir
//var urun = await context.Uruns.FirstOrDefaultAsync(u=>u.Id==3);
//Console.WriteLine(context.Entry(urun).State);

//urun.Fiyat = 425;
//urun.UrunAdi = "modified";

//Console.WriteLine(context.Entry(urun).State);
//await context.SaveChangesAsync(false); //ChangeTracker bağlantısını koparmadık
//Console.WriteLine(context.Entry(urun).State);
//context.ChangeTracker.AcceptAllChanges(); //ChangeTracker bağlantısı kaldırıldı
//Console.WriteLine(context.Entry(urun).State);
#endregion

#region 5.Deleted
//Nesnenin silindiğini ifade eder. SaveChanges fonksiyonu başlatıldığında delete sorgusu oluşturulacağı anlamına gelir.
//var urun = await context.Uruns.FirstOrDefaultAsync(u => u.Id == 3);
//context.Uruns.Remove(urun);
//Console.WriteLine(context.Entry(urun).State); //deleted

//await context.SaveChangesAsync();


#endregion

#endregion

