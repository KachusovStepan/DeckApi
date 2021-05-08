# RESTful service (RESTful WEB API) for interacting with decks of cards

Author: Kachusov Stepan

## API (Interaction interface):

| Action        | Method           | Endpoint  |
| ------------- |:-------------:| :--------:|
| Create a named deck of cards | POST | api/decks/{name} | 
| Remove a named deck of cards | DELETE | api/decks/{name} |
| Get a list of deck names | GET | api/decks |
| Shuffle the deck | GET | api/decks/{name}/shuffle |

<br>
The shuffling algorithm is set through the application configuration.

The configuration is located in the DeckApi project folder in the appsettings.json file:

There are two possible shuffle algorithms:
1) "Simple" - simple algorithm
2) "Manual" - manual shuffle emulation

They can be selected by assigning a value to the ShuffleType key in the DeckRepository section:

```
"DeckRepository" : {
    "ShuffleType": "Manual"
  }
```

## Decks are stored in memory.

The IDeckRepository interface will allow you to change the storage location to the database.

(Implement the interface and change the dependency injection setting of the di container)

## When launched, the api will run on https:
1) https://localhost:5001
2) http://localhost:5000

Can be changed in configuration DeckApi/Properties/lounchSettings.json

## Logging
At startup, a **logs** folder will be created in the root of the **DeckApi** project, where logs will be saved