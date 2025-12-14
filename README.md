** Current real info: 14.12.2025 **
Project Structure:
  Client Manager - API - The library that holds the class (ApiService) which will work with the server in the app.
  Model - describe the C# side of the tables and entities in DB:
    Data Transfer Objects - DTOs - All entities, with null as defaults to force manual update.
    Diagrams - The DB diagram, built in DrawIo.
    Entities - The C# holding objects for records based on BaseEntity with the field Id and basic functions.
    Tables - The C# holding objects for tables based on List<Entity>s.
  Server Manager - API - The project that runs the server:
    Controllers - Holds all the controllers, split by function, to all entities.
    ExceptionHandler.cs - Holds a middleware of the server that runs the exception handling, creating detailed errors from         the server's errors, ViewModel's errors, and sends them to the client and server.
    program.cs - runs the server's life cycle, opens swager, runs the middleware.
  
    
  

**this readme is under development, it's just a structure, no real info **

Space Shooter Project 🚀
Overview

[Briefly describe the game in 1-2 sentences. What is the core objective? E.g.,
A classic arcade-style 2D space shooter where players must survive endless waves of enemy ships and bosses.]
This project was developed using [Specify the primary language/framework, e.g., Python and Pygame, C# and Unity, etc.].

Getting Started
PrerequisitesTo run this game locally, you'll need the following installed on your system:
[Language/Runtime]: [Specify version, e.g., Python 3.8+]
[Required Library 1]: [e.g., Pygame, specific Unity version]
[Required Library 2]: [Optional, e.g., Tiled map editor]

Installation:
Follow these steps to get a copy of the project up and running on your local machine.Clone the repository:
git clone https://github.com/OmerTheProgrammer/space-shooter-project.git
Navigate to the project directory: cd space-shooter-project
Install dependencies (if applicable, e.g., for Python projects):pip install -r requirements.txt

Run the game:
[Specify the command to launch the game, e.g., python main.py]

How to Play:
Action, Key, Input,Description
Move Up, "[Key, e.g., W or Up Arrow]", Moves the player ship up.
Move Down, "[Key, e.g., S or Down Arrow]", Moves the player ship down.
Move Left, "[Key, e.g., A or Left Arrow]", Moves the player ship left.
Move Right, "[Key, e.g., D or Right Arrow]", Moves the player ship right.
Fire Weapon, "[Key, e.g., Spacebar]", Fires the primary weapon.
Pause Game, "[Key, e.g., P or Esc]", Pauses and resumes the game.

Game Mechanics Objective:
[State the winning condition or goal, e.g., Achieve the highest score before dying.]
Power-ups: [List any special items, e.g., Triple Shot, Shield, Speed Boost.]
Enemies: [Describe the enemy types, e.g., Basic drones, Fast interceptors, End-level Bosses.]

Branching and Development
The main, stable version of the project is housed on the master branch.master: The current stable, tested, and playable version of the game.
[Other Branch Name, e.g., develop]: [Describe its purpose, e.g., For in-progress features or experimental work.]

Contributing
We welcome contributions!
Please follow these steps to contribute:
Fork the project.Create your feature branch (git checkout -b feature/AmazingFeature).
Commit your changes (git commit -m 'Add some AmazingFeature').
Push to the branch (git push origin feature/AmazingFeature).
Open a Pull Request.ContactProject Link: https://github.com/OmerTheProgrammer/space-shooter-project.gitContact/Email: [Your Email Address]

LicenseDistributed under the [License Name, e.g., MIT, GPL] License.
See LICENSE.txt for more information.Acknowledgments
[Any tutorials, assets, or open-source libraries you used]
[Name of the original game/concept inspiration, if applicable]
