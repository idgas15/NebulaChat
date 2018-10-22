# NebulaChat
This is a simple chat application.

## Installation 
After cloning the repository, open a terminal window or command prompt, and navigate to `NebulaChat.WebApi` folder and restore packages.
```
dotnet restore
```

Update the database (Local SQLServer Database) Switch to `NebulaChat.Data` folder and run:
```
dotnet ef database update
```


Now lets install all packages NebulaChatClient for the Angular application. Switch to the `NebulaChatClient` 
```
npm install
```

## Running the application 

### Web Api
From the `NebulaChat.WebApi`folder run (running on `https://localhost:44359` 
```
dotnet run
```

### Angular Application
From the `NebulaChatClient`folder run (running on `http://localhost:4200` 
```
npm start
```

### Use
The database will be empty . Resgister a user, the login to begin