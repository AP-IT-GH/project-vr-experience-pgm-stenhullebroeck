# VR Project: Robo Gulag

## Inleiding

Robo Gulag is een VR-game waarin één speler het opneemt tegen een vijand in een futuristische 1-tegen-1 shooter. In één van de driedimensionale virtuele arena's gebruikt de speler een geweer om de vijand uit te schakelen. De game combineert een real-time spelervaring met immersieve virtual reality technologie, waarbij snelheid, tactiek en positionering centraal staan.

In deze tutorial zal je leren hoe de VR Game "Duel Arena" tot stand kwam, welke technieken en AI-componenten gebruikt zijn en hoe je zelf een gelijkaardig project kan opzetten. Tegen het einde zal je alles begrijpen van deze objecten in de game en de rol van AI in de interactie of de dynamiek van het spel.

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
#### Observaties
Tijdens het trainen ontvangt de agent per tijdstap een set observaties die hem informatie geven over zijn omgeving en interne status. De observaties zijn verzameld in de methode CollectObservations() en bevatten:

- Huidige health (HealthManager.health): laat de agent inschatten hoe lang hij nog kan overleven en eventueel defensiever kan gaan handelen.
- Aanvalsbereik (attackRad): Voor het inschatten van de afstand tot het doelwit.
- Zichtbaarheid van doelwit (targetVisible): een boolean die aangeeft of er momenteel een vijand in het zichtveld is.
- Richting naar doelwit (dirDiff): de hoek tussen de voorwaartse richting van de agent en het doelwit.

#### Acties
Deze agent gebruikt 2 continue en 1 discrete actie.

- Continue acties (ContinuousActions[0], [1])
    - x: Rotatie (links/rechts draaien)
    - z: Voorwaartse beweging
- Discrete actie (DiscreteActions[0])
    - 1 Betekent een poging tot aanvallen, enkel zinvol als een vijand zichtbaar is en de cooldown is verstreken.

#### Beloningen
Beloningen vormen de kern van gedragssturing. De agent ontvangt positieve of negatieve beloningen afhankelijk van zijn gedrag en context.

- Positieve Beloningen
    - +0.5 wanneer er een vijand zichtbaar is (VisibleTargets.Count > 0)
    - + (1 - 0.01 × hoek): hoe beter het doelwit gecentreerd is in het gezichtsveld, hoe groter de beloning
    - +10 bij een succesvolle aanval (raak en geen obstakel ertussen)
    - +30 bij het uitschakelen van een vijand (gezondheid doelwit ≤ 0)

- Negatieve Beloningen
    - -0.1 per tijdstap zonder zicht op een vijand
    - -0.01 per tijdstap (algemene tijdsdruk, voorkomt stilstand)
    - - (hoekverandering × 0.001) als de agent roteert zonder zicht op een doelwit
    - -1 bij een mislukte aanval (bijv. doelwit buiten bereik of buiten FOV)
    - -1 als hij probeert aan te vallen zonder zicht op een vijand
    - -10 bij sterven

### Beschrijving & Gedrag Objecten
 - VR Player
    - De VR Player is het object dat de speler bestuurt via een VR-headset en -controllers. Het beweegt zich door de arena via teleportatie en kan met de handen een geweer oppakken, vasthouden, richten en schieten. Het object reageert op de fysieke input van de speler. Wanneer het geraakt wordt door een projectiel, verliest het health, wat beheerd wordt door de HealthManager.
 - Gun
    - Het geweer is een fysiek object dat door de speler kan worden opgepakt. Via de VR-controllers wordt bepaald wanneer het wapen wordt vastgehouden of losgelaten. Bij het indrukken van de trigger vuurt de gun een kogel af in de richting waarin het wordt gericht. Tijdens het schieten wordt een geluid afgespeeld en een laserstraal getoond. De gun valt op de grond wanneer deze wordt losgelaten, en kan opnieuw worden opgepakt.
 - CompanionBot (AI-Agent)
    - De CompanionBot is een getrainde ML-Agent die zich autonoom gedraagt. Het navigeert door de arena met obstakelvermijding, detecteert de speler via line-of-sight, en opent het vuur zodra de speler in zicht en binnen bereik is. Het gedrag is ontstaan door reinforcement learning, waarbij het beloningen krijgt voor onder andere zicht op de speler, nauwkeurige positionering, succesvolle aanvallen en kills. Strafpunten worden gegeven voor inefficiënt gedrag zoals rondtollen of missen. Hierdoor leert de bot zich strategisch en doelgericht te gedragen.
 - Obstakels
    - Dit zijn statische objecten verspreid over de arena's. Ze blokkeren schoten en zichtlijnen, en worden gebruikt door zowel de speler als de AI voor dekking. Hierdoor beïnvloeden ze direct de positionering en het gedrag tijdens gevechten.
- GameField
    - Een compacte arena die ontworpen is voor korte, intense confrontaties. Door de vele obstakels wordt close-range tactiek gestimuleerd, wat leidt tot snel en agressief gedrag van zowel de speler als de AI.
- SmallerTerrain
    - Een grotere, meer open arena die ruimte biedt voor positionele strategieën en langere gevechten. Hier leert de AI zich anders te bewegen: het zoekt vaker dekking op afstand en benadert de speler via langere routes.
- Terrain
    - Dit is de algemene omgeving waarin het spel begint. Hier bevindt zich het keuzemenu. Het heeft geen invloed op het spelgedrag, maar vormt de visuele introductie van het spel.
- HealthManager
    - Dit leeg object beheert de health van de speler en bot. Het houdt bij hoeveel de health van beide objecten is.
- UIManager
    - Dit leeg object is verantwoordelijk voor het beheren van de verschillende UI Canvas-elementen die tijdens het spel kunnen getoond worden. Het regelt ook wanneer deze kunnen worden getoond.
- GameManager
    - Dit leegt object is de centrale controller voor het spel die de speltoesstanden beheert: opstarten, pauzeren, stopzetten van het spel. Dit beheert ook de selectie van de arena's.
- StartCanvas
    - het hoofdmenu dat in het begin van het spel verschijnt om een arena te kiezen en het spel te starten.
- GamePlayCanvas
    - Dit is de UI-Canvas die zichtbaar is tijdens het spelen van het spel. Dit is eigenlijk de healthbar waarop de speler zijn health te zien is. Deze wordt aan de hand van de HealthManager aangepast.
- PauseCanvas
    - Dit is de canvas die geactiveerd wordt, wanneer er gepauzeerd wordt. Dit biedt de optie om verder te spelen of het spel stop te zetten.
GameOverCanvas
    - Deze canvas wordt geactiveerd wanneer ofwel de speler of de bot dood gaan en het spel wordt beëindigt. Hiermee kan terug naar het hoofdmenu worden gegaan om het spel dan terug opnieuw te spelen.

### One Pager
#### VRXP Project: Auto Battler
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
![Tensor grafiek Trainingen](TensorImages\GrafiekTrainenTensor.png)
Bij Bovenstaande versie kan men de oranje Training (De oudere Training) en de paarse Training (De geüpdated Training) vergelijken met elkaar. De oranje lijn geeft diepe negatieve beloningen, duidend op slecht gedrag dat waarschijnlijk herhaaldelijk wordt afgestraft. De paarse lijn fluctueerd erg, maar bereikt uiteindelijk regelmatige hoge cumulatieve beloningen, wat een indicatie is van succesvol gedrag, afgewisseld met meer leerinstabiliteit.

![Tensor grafiek Trainingen](TensorImages\TensorEpisodeGrafiek.jpg)
Bij deze bovenstaande grafiek wordt getoond hoe lang de episodes gemiddeld duren. De oranje lijn vertoond wisselvallig gedrag. Episodes fluctueren sterk wat wijst op instabiliteit in het leerproces. De paarse lijn toont een vrij consistente episode lengte richting het einde van de training, rond de maximale waarde, wat duidt op stabiel en gewenst gedrag.

### Opvallende Waarnemingen bij trainingen
In de vroege fases van de trainingen vertoonde de agent voornamelijk roterend gedrag ("jittering") en draaide hij vaak rond zonder richting. Dit leidde ertoe dat het detecteren en aanvallen van het doelwit moeizaam verliep, wat resulteerde in langzamere leerprogressie.

Na het introduceren van een strafmechanisme (negatieve beloningen) bij het overmatig draaien zonder visueel contact zorgde voor efficiënter gedrag. De agent begon minder te roteren en focuste meer op de doelgerichte verplaatsing. Deze aanpassingen leidde wel tot iets minder exploratief gedrag.

Kleinere negatieve beloningen per tijdstap waarin er geen zicht is op een doelwit, gecombineerd met relatief grotere positieve beloningen bij visueel contact, bleken het meest effectief in gedragsvorming. Vooral wanneer die positieve beloning proportioneel toenam bij kleinere hoeken tussen het doelwit en het centrum van het gezichtsveld (field of view), verbeterde de precisie aanzienlijk.

Wanneer het doelwit zichtbaar was, handelde de agent doorgaans accuraat en consistent. Echter, bij verlies van visueel contact raakte hij soms gedesoriënteerd, wat leidde tot inefficiënt zoeken.

In latere trainingsfasen ontwikkelde de agent herkenbare routines, zoals het systematisch scannen van gekende spawnlocaties. Dit wijst op het ontstaan van interne zoekstrategieën.

Bij het introduceren van willekeurige startposities verminderde deze oriëntatie, wat tijdelijk leidde tot een lagere efficiëntie. De vaste scanpatronen bleken minder toepasbaar, waardoor de agent zich opnieuw moest aanpassen.

## Conclusie
In dit project hebben we een AI-gestuurde vijand ontwikkeld en geïntegreerd in een virtual reality game, waarbij we gebruikmaakten van reinforcement learning om een realistische en uitdagende tegenstander te creëren.

De resultaten tonen aan dat de AI-agent zelfstandig leert navigeren, de speler detecteert en effectief aanvalt binnen een VR-omgeving. Dankzij gerichte beloningen en strafmechanismen ontwikkelde het gedrag zich van chaotisch naar strategisch en geloofwaardig. Zelfs met beperkte middelen bleek het mogelijk een dynamische, meeslepende AI-tegenstander te trainen die het spel realistisch en uitdagend maakt.

In de toekomst zouden we het model kunnen verbeteren door meerdere AI-agenten te trainen met onderlinge samenwerking of concurrentie. Daarnaast zou het gebruik van geavanceerdere algoritmes of neurale netwerken de leercurve verder kunnen optimaliseren. Ook kan het toevoegen van meer gevarieerde omgevingen en speldoelen zorgen voor bredere generalisatie van het aangeleerde gedrag.
