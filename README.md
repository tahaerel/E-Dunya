<h1> PushUP Ekibi - E-Dünya Projesi</h1>

<p align=center>
 
![e dunya wallpaper](https://user-images.githubusercontent.com/63150746/168457349-86619477-f3f5-4a7b-9f06-2d27ac33fa6e.png)
  </p>



Proje Unity oyun motoru 2020.3.34f1 sürümü ile oluşturulmuştur.

Amaç:
Türkiye Cumhuriyeti Devleti üzerinde bulunan kamu sektöründeki ve özel sektördeki şirketleri bir evren üzerinde toplamak, yapılabilecek işlemleri blockchain üzerinden 
sosyal bir sanal evren üzerinden yapmak.

Assets -> Project -> Scenes -> Login sahnesi açılarak Metamask mobil uygulamasıyla birlikte Metamask cüzdanına bağlanılır.
Kullanıcıya bir id verilir ve tüm blockchain işlemleri bu sanal kimliği üzerinden gerçekleştirir.
(NFT alım satım, akıllı kontratlar)

Projede blockchan entegresi için ücretsiz olan Moralis Web3 Development Platform, evreni multiplayer hale getirmek için ücretsiz olan Photon Pun 2 Game Server kullanılmıştır.
Moralis üzerinden Etherium mainnet üzerinden bir server başlatılarak bu serverin Moralis Server URI ve Moralis Application Id ' si Unity'de login sahnesinde bulunan Moralis Connection prefabının Moralis Setup gameobjectine yazılır.
WalletConnect Metamask olarak seçilir.

Multiplayer entegresi için Photon pun 2 sitesinden https://dashboard.photonengine.com Pun app'i oluşturularak server id si alınır. Bu id PhotonServerSettings dosyasında bulunan App Id Pun kısmına yazılır ve bağlantı sağlanır.

<p align=center>
 
<img src="https://user-images.githubusercontent.com/63150746/168443294-402c5b7e-37f8-408f-b4b4-10be40e9e49d.png" width="250" height="150">
  </p>
