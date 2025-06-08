# VR Project: Robo Gulag

## Inleiding

Duel Arena is een VR-game waarin één speler het opneemt tegen een vijand in een futuristische 1-tegen-1 shooter. In één van de driedimensionale virtuele arena gebruikt de speler een geweer om de vijand uit te schakelen. De game combineert een real-time spelervaring met immersieve virtual reality technologie, waarbij snelheid, tactiek en postionering centraal staan.

In deze tutorial zal je leren hoe de VR Game "Duel Arena" tot stand kwam, welke technieken en AI-componenten gebruikt zijn en hoe je zelf een gelijkaardig project kan opzetten. Tegen het einde van deze objecten in de game en de rol van AI in de interactie of dynamiek van het spel.

## Methoden

### Installatie
- Unity versie
    - 6000.0.36f1

- Packages (Via Unity Package Manager)
    - ML-Agents package: v3.0.0
    - AI Navigation: v2.0.8
    - XR Core Interaction Toolkit: v3.1.1
    - XR Plugin Management: v4.5.0
    - OpenXR Plugin: v1.14.1

- Anaconda & Tensorboard
    - Python v3.12.7

- Gebruikte resource packs (voor het bouwen van de omgevingen, Dit zijn gratis assets)
    - Voxel Nature Pack Rocky Mountain by maxparata
        - https://maxparata.itch.io/rocky-mountains
    - Voxel Environment Country Side by maxparata
        - https://maxparata.itch.io/counrty-side
    - Voxel Robot assets for free by maxparata
        - https://maxparata.itch.io/voxel-mechas
    - Tiny Voxel Dungeons by maxparata
        - https://maxparata.itch.io/tinyvoxeldungeon
    - Voxel Space Colony by maxparata 
        - https://maxparata.itch.io/voxelspacecolony
    - Ancient Environment Pack by maxparata
        - https://maxparata.itch.io/voxel-ancient-environment
    - Voxel Desert Town by maxparata
        - https://maxparata.itch.io/voxel-desert-town
    - Voxel Guns N Rifles by maxparata
        - https://maxparata.itch.io/voxel-guns-n-riffles-monogon
    - Voxel Zombie Apocalypse by maxparata
        - https://maxparata.itch.io/voxelzombieapocalypse
    - Voxel Graveyard Assets by maxparata
        - https://maxparata.itch.io/voxelgraveyard
    - Voxel Cyberpunk City by maxparata
        - https://maxparata.itch.io/cyberpunkcity-monogon

### Simulatie VR Game

De simulatie start met een keuzemoment waarin de speler één van twee arena’s selecteert. Na selectie wordt de speler in de gekozen arena geplaatst. Daar beweegt hij zich vrij rond, zoekt de vijand en probeert deze uit te schakelen met een VR-geweer. Er zijn obstakels op het speelveld die gebruikt kunnen worden voor dekking. Het spel eindigt zodra de speler of de vijand wordt uitgeschakeld. De speler kan het spel ook pauzeren of stoppen als die dat wilt.

### Observaties, Acties & Beloningen

### Beschrijving Objecten
 - VR Player
    - De speler bestuurt dit object via een VR-headset en -controllers. Het spel wordt vanuit die object gespeeld. Het object vormt de connectie tussen de speler en de virtuele wereld.
 - Gun
    - Het geweer dat de VR Player gebruikt. Via de controllers van de VR headset kan er afgevuurd worden, het wapen loslaten en terug oppakken. Het geweer beweegt mee met de speler.
 - CompanionBot (AI-Agent)
    - Een getrainde ML-Agent die autonoom beweegt, rond obstakels loopt en naar de speler zoekt.
 - Obstakels
    - Vaste objecten in de arena's die zowel de speler als de als de bot gebruiken om zich te verschuilen. De objecten blokkeren zichtlijnen en schoten en hebben een directe impact op strategie en overleving.
- GameField
    - Een compacte arena waarin het spel zich plaatsvindt. Door de meerdere obstakels is het ontworpen voor korte, intense duels.
- SmallerTerrain
    - Een grotere open arena met meer ruimte om te bewegen. Het is geschikt voor langere gevechten en postionele tactieken. 
- Terrain
    - De omgeving waarin de speler zich initieel bevindt en het keuzemenu zich bevindt.
- HealthManager
    - Dit leeg object beheert de health van de speler en bot. Het houdt bij hoeveel de health van beide objecten is.
- UIManager
    - Dit leeg object is verantwoordelijk voor het beheren van de verschillende UI Canvas-elementen die tijdens het spel kunnen getoond worden. Het regelt ook wanneer deze kunnen worden getoond.
- GameManager
    - Dit leegt object is de centrale controller voor het spel die de speltoesstanden beheert: opstarten, pauzeren, stopzetten van het spel.
- StartCanvas
    - het hoofdmenu dat in het begin van het spel verschijnt om een arena te kiezen en het spel te starten.
- GamePlayCanvas
    - Dit is de UI-Canvas die zichtbaar is tijdens het spelen van het spel. Dit is eigenlijk de healthbar waarop de speler zijn health te zien is.
- PauseCanvas
    - Dit is de canvas die geactiveerd wordt, wanneer er gepauzeerd wordt. Dit biedt de optie om verder te spelen of het spel stop te zetten.
GameOverCanvas
    - Deze canvas wordt geactiveerd wanneer ofwel de speler of de bot dood gaan en het spel wordt beëindigt. Hiermee kan terug naar het hoofdmenu worden gegaan om het spel dan terug opnieuw te spelen.

### Gedragingen Objecten
- VR Player
    - Beweegt vrij rond in de arena via teleportatie. Het kan een geweer oppakken, vasthouden, richten en ermee schieten. Deze VR Player wordt bestuurd via headset en handcontrollers. Bij contact met projectielen verliest deze health (geregistreerd door HealthManager). Dit object reageert dus op de input van de VR-hardware.
- Gun
    - Wordt opgepakt door de speler. De gun detecteert wanneer er op de trigger wordt gedrukt en vuurt dan kogels af in de richting waarin het geweer wordt gehouden. Als de gun wordt losgelaten dan valt deze op de grond, het kan terug opgepakt worden. Het geweer zal ook een geluid en laser laten zien wanneer het wordt geschoten.
- CompanionBot (AI-Agent)
    - Dit object zoekt visueel naar de speler (line-of-sight). De agent beweegt vrij rond. Vuurt kogels af wanneer deze de speler gedetecteerd heeft en in bereik is. Beweegt zich rond obstakels. De ML-agent is getraind via reinforcement learning met beloningen.
- Obstakels
    - Zijn statische objecten die niet bewegen, maar wel invloed hebben op het gedrag van de speler en AI-Agent. 
- GameField / SmallerTerrain
    -  Ze beïnvloeden de tactiek van het gevecht: GameField, meer close-range confrontatie, meer obstakels. SmallerTerrain, meer open ruimtes, andere bewegingspatronen van AI.
- Terrain
    - Zorgt gewoon voor visuele achtergrond voor het begin van het spel
- HealthManager
    - Houdt de health van de speler en CompanionBot bij. 
- UIManager
    - Houdt bij welke UI-Canvassen er worden getoond.
- GameManager
    - Houdt spelstatus bij (Start, Palying, Pauze, GameOver). Initieert de arena selectie.
- StartCanvas
    - Wordt getoond aan het begin. Wacht op gebruikersinput om een arena te kiezen. Na selectie start de game.
- GamePlayCanvas
    - Toont tijdens het spel de health van de speler. Wordt geüpdatet door HealthManager.
- PauseCanvas
    - Wordt getoond wanneer het spel gepauzeerd is. Kan spel hervatten of terug naar menu gaan.
- GameOverCanvas
    - Wordt getoond wanneer het spel beëindigt. Laat toe om terug naar het hoofdmenu te gaan.
    
### One Pager
#### VRXP Project: Robo Gulag
##### Groepsleden
    - Eveline Decubber
    - Sten Hullebroeck
    - Sander Claes

##### Beschrijving van het spel.
    Auto Battler is een VR-game dat geïnspireerd is op Mechabellum en TABS. De speler plaatst enkele eenheden op een futuristisch slagveld. De troepen bestaan uit: DPS, healers, snipers en tanks. Elk van deze eenheden zijn ML-Agents met unieke eigenschappen. Wanneer de speler klaar is, vechten deze troepen tegen getrainde AI agents en kan de speler kijken wie er wint. 

##### VR Simulatie in draaiboek
    1. De speler komt terecht in een futuristische arena.
    2. Voor het gevecht kiest de speler zijn specifieke troepen met 3. een bepaald budget uit een reeks eenheden.
    3. De speler plaatst zijn troepen dan strategisch op het slagveld.
    4. De speler bevestigt zijn opstelling en drukt op play.
    5. Het gevecht tussen de troepen begint, de speler observeert dit in VR en krijgt direct feedback over zijn opstelling.
    6. Uiteindelijk wint of verliest de speler en eindigt het gevecht. 
    7. De speler heeft de mogelijkheid om opnieuw te spelen.

##### Meerwaarde van AI.
    De AI-component zorgt ervoor dat er complexe samenwerking en gevechtstactieken mogelijk zijn, zonder dat dit vooraf helemaal gescript moet zijn. Zonder AI zouden de gevechtspatronen voorspelbaar en rigide zijn, maar met AI ontstaan er unieke en evoluerende gevechtspatronen. Dit maakt het spel veel interessanter. 

##### Meerwaarde van Virtual Reality
    VR zorgt ervoor dat de speler een unieke spelervaring krijgt. Hij staat rondom de troepen en kan deze bekijken van verschillende hoeken. Hierdoor kan men beter tactische beslissingen nemen en geeft het spel een interactief element dat zeer leuk is. VR verhoogt ook de immersie en betrokkenheid van de speler.

##### Interactie van de speler
    De speler kiest bepaalde troepen en plaatst deze op het slagveld aan de hand van VR-hand interactie. Tijdens de strijd zelf is er geen interactie van de speler en observeert deze enkel het gevecht tussen de troepen. Hierdoor krijgt hij meer de rol van een strategische commander die kijkt over zijn troepen.

### Afwijkingen One Pager
We zijn afgestapt van het auto battler-concept omdat het trainen van meerdere AI-karakters en hun samenwerking te complex bleek binnen de beschikbare tijd. We hebben daarom gekozen voor een eenvoudiger maar uitdagend alternatief: een 1-tegen-1 VR-shooter waarin slechts één AI-agent moest worden getraind.

Het nieuwe draaiboek is meer op actie gericht: Eerst zal de speler een arena kiezen om het spel te spelen. Dan wordt de speler in die arena geplaatst en begint het spel. De speler loopt vrij rond en zoekt de vijand. De speler ziet de vijand en probeert deze uit te schakelen door erop te schieten. Ondertussen vermijdt de speler dat hij zelf geraakt wordt door de schoten van de vijand. Dit blijft doorgaan totdat ofwel de speler of de vijand sterft en dan eindigt het spel.

De AI zorgt ervoor dat de vijand zicht dynamisch beweegt en niet te voorspelbaar is. 

De VR-ervaring is nu veel interactiever tegenover de autobattler versie. De speler staat nu midden in het gevecht in plaats van enkel het gevecht te observeren. De speler gebruikt nu zelf handbewegingen om rond te bewegen, richten en schieten. Dit maakt de beleving veel directer en intenser dan bij het originele autobattler concept.
## Resultaten
### Tensorboard

### Opvallende Waarnemingen bij trainingen

## Conclusie