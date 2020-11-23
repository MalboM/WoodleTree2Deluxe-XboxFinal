
# SNOW SHADER


## FEATURES

Lo snow shader da noi proposto presenta le seguenti features e risolve i problemi del precedente shader :

- **PERFORMANCE** <br>
  . abilitazione al gpu instancing <br>
  . metodo più efficiente per il calcolo del triplanar normal mapping <br>

-  **NORMAL MAPPING** <br>
  . Dettagli di superficie visibili senza attivazione del Rim <br>
  . Due normal map (base e detail normal) dove l'aggiunta del detail è opzionale come nel precedente shader<br>
  . Disabilitazione delle detail options <br>
  . Tiling e bumping indipendente delle due normal <br>
  . Blending tra le due normali funzionante (la detail normal aggiunge correttamente dettagli di superficie) <br>
  . Miglioramento del sistema di bumping delle normali <br>

- **ILLUMINAZIONE**<br>
  . Aggiunta di un effetto di illuminazione esterna dell'oggetto configurabile nella direzione e nell'intensità <br>
  . Migliore gestione delle ambient lights di Unity <br>
  . Miglioramento del Rim effects con una gestione più intuitiva del suo colore
  . Gestione più efficientre del tint color <br>

- **SPARKLE SYSTEM**<br>
 . 2 nuovi sistemi di sparkling (procedural e texture based) oltre all'integrazione del vecchio sistema <br>
 . Aggiunta del colore per lo Sparkle <br>
 . Nuovi sparkles animati nel tempo <br>
 . Sparkle configurabili <br>


## PARAMETRI

Qui in seguito vi è l'elenco completo dei parametri impostabili all'interno dello shader

### Color

**Tint Color** : fornisce una tinta base alla mesh <br>
**Mix factor** : nel caso fosse impostata una main texture questo parametro
permette di mixare la texture con la tinta proveniente dal _Tint color_ <br>

### Normal mapping

**Overall normal strenght** : risalta la risultante del normal mapping, sia nel caso di sola base normal, sia nel caso venga attivata la detail normali

**Activate detail normal** : toggle per l'attivazione e applicazione della seconda normal texture. <br>
Il toggle nasconde le relative opzioni tramite uno script _hideifdisable.cs_ che forniamo nel pacchetto <br>
**Base Normal strength** : esegue il bump delle normali contenute nella texture base normal <br>
**Detail Normal strength** : esegue il bump delle normali contenute nella texture detail normal <br>


### Rim light settings

**Rim sharpness** : aumenta/diminuisce la nitidezza della rim lights, ovvero quanto vi è dispersione di tale luce sulla superficie <br>
**Rim intensity** : aumenta l'intensità della luce diffusa dal rim <br>
**Rim color** : permette di editare il colore associato alla rim light <br>


### Sparkle settings

**Sparkle mode** : permette di utilizzare una delle tre modalità di generazione dell'effetto di sparkle<br>
Sono presenti 3 modalità :
 - _Old Sparkle_ : il metodo originale basato sull'uso della texture MainTex<br>
 - _Procedural_ : genera uno sparkle senza necissità di utilizzare texture <br>
 - _Texture noise Sparkle_ : genera lo sparkle tramite la combinazione di due textures mask<br>

**Sparkle settings** :  tali opzioni sono quelle comuni a tutti gli sparkle generator <br>
**Sparkle intensity** : parametrizza l'intensità di emissione degli sparkle <br>
**Sparkle falloff** : parametrizza la rapidità di decadimento in base alla distanza dalla camera <br>
**Sparkle color** : determina il colore associato all'emissione degli sparkle <br>

#### Procedural settings

Sono parametri specifici per il procedural generator, si attivano soltanto quando SPARKLE MODE è settata su PROCEDURAL<br>

**Sparkle sharpness** : aumenta/riduce la nitidezza degli sparkle <br>
**Sparkle animation speed :** aumenta/riduce la rapidità di cambiamento nel tempo degli sparkle <br>
**Noise scale** : aumenta/riduce la granulosità degli sparkle <br>

#### Texture noise settings

Sono parametri specifici per il texture noise sparkle generator, si attivano soltanto quando SPARKLE MODE è settata su NEW_TEXTURE <br>

**Speckle sharpness** : aumenta/riduce la nitidezza dello sparkle <br>
**Speckle size Amp** : amplifica/riduce le dimensione dei singoli sparkle nella scena. <br>
_Nota_: gli sparkle non vengono scalati "in place", ovvero questo parametro non
agisce come uno zoom della texture ma semplicemente fa si che si generino sparkle con dimensioni differenti. Inoltre l'amplificazione avviene su scala logaritmica per rendere più intuitive le modifiche alla dimensione <br>
**Texture noise animation speed** : parametro che indica la velocità con cui gli sparkle sono soggetti ad una variazione in base al tempo <br>
**Fake Light Position** : direzione da cui proverebbe una fake light ad illuminare l'oggetto <br>
**Fake Light Contribution** : parametro per aumentare/ridurre la quantità di illuminazione che proviene dalla fake light<br>
**Color Brightness** : aumenta/riduce la luminosità del diffuseColor<br>

