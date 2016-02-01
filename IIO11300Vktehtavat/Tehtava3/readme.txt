PNG DPI Editor
--------------

Tämän pienen ohjelman avulla voi tarkistaa ja muokata PNG-kuvien DPI-asetuksia. Tein sen avuksi 
Teeleidi-ohjelman kehittämiseen, koska tarvitsemme siinä ikoneita (DPI 96) ja etikettejä (DPI 360).
Väärät DPI-arvot aiheuttavat ongelmia ja niiden muokkaaminen tiedosto kerrallaan on hidasta.

Ohjelmaan avataan kerrallaan kansiollinen kuvia valikon Tiedosto > Avaa kansio -toiminnolla.
Tämän jälkeen kansion PNG-kuvatiedojen nimet ja DPI-tiedot (tai niiden puuttuminen) listataan
DataGridiin. DPI-asetuksia pääsee muokkaamaan tuplaklikkaamalla haluttua riviä.

Tiedostot luetaan BinaryReaderilla ja kirjoitetaan BinaryWriterillä. PNG-spesifikaation vaatimat
CRC-tarkisteet luodaan Damien Guardin kirjoittaman Crc32-luokan avulla.