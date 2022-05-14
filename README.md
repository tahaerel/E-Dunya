<h1>👋 PushUP Ekibi - E-Dünya Projesi</h1>

Proje Unity oyun motoru 2020.3.34f1 ile oluşturulmuştur.

Amaç:
Türkiye Cumhuriyeti Devleti üzerinde bulunan kamu sektöründeki ve özel sektördeki şirketleri bir evren üzerinde toplamak, yapılabilecek işlemleri blockchain üzerinden 
bu sanal evrende yapmak.

Assets -> Project -> Scenes -> Login sahnesi açılarak Metamask mobil uygulamasıyla birlikte Metamask cüzdanına bağlanılır.
Kullanıcıya bir id verilir ve tüm blockchain işlemleri bu sanal kimliği üzerinden gerçekleştirir.
(NFT alım satım, akıllı kontratlar)

Projede blockchan entegresi için Moralis Web3 Development Platform, evreni multiplayer hale getirmek için Photon Pun 2 Game Server kullanılmıştır.
Moralis üzerinden Etherium mainnet üzerinden bir server başlatılarak bu serverin Moralis Server URI ve Moralis Application Id ' si Unity'de login sahnesinde bulunan Moralis Connection prefabının Moralis Setup gameobjectine yazılır.
WalletConnect Metamask olarak seçilir.

Multiplayer entegresi için Photon pun 2 sitesinden https://dashboard.photonengine.com Pun app'i oluşturularak server id si alınır. Bu id PhotonServerSettings dosyasında bulunan App Id Pun kısmına yazılır ve bağlantı sağlanır.


