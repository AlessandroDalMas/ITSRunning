ANNOTAZIONI:

Sono in fase di riscrittura e riorganizzazione del codice.
Nella prima stesura molte cose erano mal organizzate e disordinate.
Il codice è funzionanente e testato, però ho eliminato, ovviamente, tutte le connection string.
Le coordinate GPS al momento sono calcolate in random a partire dalla current location dell'utente per tutta la durata della fase di test.
All'interno è presente una parte sviluppata con SignalR ma completamente commentata, causa inutilizzo momentaneo.


Al momento è composto da:

- Libreria per gli accessi al DB
- Libreria per i modelli dei dati
- Azure function per processare le telemetrie
- Web App per visualizzazione client e api
- Worker per l'esecuzione dei comandi

Ho utilizzato il pattern CQRS per dividere gli accessi al DB tra le diverse operazioni necessarie dell'applicativo.


Criticità incontrate:
Ho speso tanto tempo per capire le varie risorse ed i pro/con delle varie opzioni disponibili per la creazione della stessa struttura.
