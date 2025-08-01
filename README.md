#ChunkeeApp

ChunkeeApp, dosya parçalama(chunk) ve birleştirme işlemleri yapan, SOLID ve DRY prensiplerine uygun yazıolmış, bir .net core konsol uygulamasıdır.

##Kullanılan Teknolojiler
C#
.NET Core
SQLite
xUnit

##Kurulum ve Kullanım 
1. Bu repoyu klonlayın:
git clone https://github.com/mnakgol/ChunkeeApp.git
2. Proje dizinine gidin: cd ChunkeeApp
3. Gerekli bağımlılıkları yükleyin.
4. Uygulamayı çalıştırın: dotnet run
5. Uygulamayı çalıştırdığınızda aşağıdaki seçenekler sunulur:
1 yazarak chunk işlemini başlatabilirsiniz.
Sonrasında istenilen path bilgilerini girerek enter ile ilerlediğiniz zaman Chunk işlemi yapılır, Orjinal dosya hash'i de veritabanında tutulur.
2 yazarak merge işlemini başlatabilirsiniz.
Sonrasında istenilen path bilgilerini girerek enter ile ilerlediğiniz zaman Chunk işlemi yapılır, yeni oluşturulan dosyanın hashi Orjinal dosya hash'i ile karşılaştırılır ve gerekli conuç dönülür.

##Geliştirici
Mehmet N. AKGÖL




