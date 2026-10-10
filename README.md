# Micro:bit GSR – Játékos hazugságvizsgáló

### Bőrellenállás-mérés Bluetooth-kapcsolattal

A projekt egy egyszerű, BBC micro:bit V2 alapú mérőeszközt mutat be, amely a bőr elektromos ellenállásának változását követi, és a mérési adatokat Bluetooth Low Energy (BLE) kapcsolaton továbbítja egy megjelenítő alkalmazásnak.

A rendszer elsősorban oktatási célra készült, az 5–8. évfolyamos tanulók számára. Jól használható a digitális kultúra robotika és programozás témakörében, valamint a fizikaórákon az Ohm-törvény, a feszültségosztó és az elektromos ellenállás élményalapú bemutatására.

## Mit mérünk?

A GSR (Galvanic Skin Response) a bőr elektromos vezetőképességének változásával kapcsolatos jelenség. Az izgalom, a stressz vagy a fokozott figyelem megváltoztathatja a verejtékmirigyek működését, ezáltal a bőr elektromos ellenállását is.

**Fontos:** az eszköz nem valódi hazugságvizsgáló, és nem alkalmas annak eldöntésére, hogy valaki igazat mond-e. A „hazugságvizsgáló” elnevezést játékos, figyelemfelkeltő céllal használjuk.

## Szükséges hardver

- BBC micro:bit V2
- 220 kΩ-os ellenállás (kiindulási érték)
- Két bőrelektróda
- Csatlakozóvezetékek
- Elemes tápellátás
- Bluetooth LE-képes Android-telefon vagy megfelelő Windows-számítógép

A bőr és a fix ellenállás feszültségosztót alkot. A micro:bit P0 analóg bemenete a feszültségosztó középpontjának feszültségét méri.

## Biztonság

A micro:bit a mérés során kizárólag elemes táplálással működik. A BLE-adatátvitelnek köszönhetően a vizsgált személy galvanikusan el van választva a számítógéptől.

A mérőáramkört ne csatlakoztassuk USB-n keresztül számítógéphez, amíg az elektródák a vizsgált személy bőrével érintkeznek.

## A micro:bit programja

A program a Microsoft MakeCode környezetben készült.

A Bluetooth-bővítmény telepítése után a projektbeállításoknál engedélyezni kell a **No Pairing Required** üzemmódot.

A program működése:

1. Elindítja a Bluetooth UART szolgáltatást.
2. Várja a fogadóalkalmazás parancsát.
3. A `G` karakter érkezésekor beolvassa a P0 analóg bemenet értékét.
4. A 0–1023 közötti mérési eredményt Bluetooth-kapcsolaton visszaküldi.
5. A LED-kijelző ikonokkal jelzi a kapcsolat állapotát.

A mintavétel gyakoriságát a fogadóalkalmazás határozza meg.

## Android-alkalmazás

Az Android-alkalmazás MIT App Inventor környezetben készült.

Feladata a BLE-kapcsolat létrehozása, a mérési adatok lekérése és megjelenítése.

Az `.aia` projektfájl az MIT App Inventorba importálható, így a program szabadon tanulmányozható és továbbfejleszthető.

## Windows-alkalmazás

A C# nyelven készített Windows-alkalmazás a mérési adatok grafikus megjelenítésére szolgál.

A projekt forráskódja Visual Studióban nyitható meg. A program nagy méretű számkijelzővel és valós idejű grafikonnal segíti a bőrellenállás változásának megfigyelését.

## Oktatási alkalmazások

A mérőeszközzel több egyszerű kísérlet is végezhető:

- Nyugalmi állapot mérése
- Fejszámolás közbeni változások megfigyelése
- Váratlan ingerekre adott reakciók vizsgálata
- A mérési eredmények grafikus feldolgozása
- A szenzoradatok értelmezésének és korlátainak megbeszélése

A projekt különösen alkalmas a fizika, biológia és digitális kultúra tantárgyak közötti kapcsolatok bemutatására.

## Forráskód és felhasználás

A repository tartalmazza a micro:bit programját, az MIT App Inventor projektfájlt, a Windows-alkalmazás C# forráskódját és a kapcsolódó dokumentációt.

A projekt oktatási célú felhasználásra és továbbfejlesztésre készült.

**Szerző:** Piláth Károly

**Kapcsolódó blog:** https://pilath.wordpress.com/2026/10/10/ujra-elokerult-a-micro-epitsunk-hazugsagvizsgalot/
