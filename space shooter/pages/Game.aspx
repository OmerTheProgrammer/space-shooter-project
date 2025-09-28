<%@ Page Title="Game" Language="C#" MasterPageFile="~/game_master_page.Master" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="space_shooter.pages.Game1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- library links: -->
    <!-- phaser: -->
    <script src="../libraries/phaser/phaser.min.js"></script>

    <style>
        body {
            /*m_container*/
            justify-content:center;
            align-content:center;
            flex-direction: column;
            height:15vh;
            width:80vw;
        }

        canvas {
            display: block;
            /*m_card*/
            display:inline-block;
            padding : 0 1%;
        }

        #game_section{
            padding:1%;
            padding-left:12%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                    <script>
                        
                        const size = [2000, 1000];
                        //scenes = The_game_scene, The_end_scene, A_boss_scene [1800, 730]
                        let currentScene, scenes = [], The_counter, counter_ended = false;
                        let killed = 0, maxEnemies = 0, counter_running = false, shield_life_decrese_count = 0;
                        let music_isnt_active = true, sound_isnt_active = true, level = 1/* user.level */, level_drop_rate = 60;
                        let win = false, tico = false, is_endless = false, shield_next_level_was_hidden = false;
                        let gameover = false, invulnerable = false, og_x = size[0] / 2, og_y = size[1] - 100, health = 5, score = 0;
                        let power_up_types, shield_life = 0, player_lasers_count = 1, shield_max_life = 0;
                        let score_coldown = 0, ending_coldown = 0, whoosh_coldown = 0, hide_setting_coldown = 0;
                        let cursors, player, Lasers, enemies, enemyLasers, power_ups, shield, explosionSprite;
                        let gameover_text, tico_text, win_text, bg_music;

                        function print() {
                            const displayList = currentScene.children.list;

                            for (let i = 0; i < displayList.length; i++) {
                                console.log(displayList[i]);
                                if (displayList[i].displayFrame) {
                                    console.log(displayList[i].displayFrame.texture.key);
                                }
                                else if (displayList[i].texture.key) {
                                    console.log(displayList[i].texture.key);
                                }
                                console.log(displayList[i].type);
                                console.log(displayList[i].visible);
                                console.log(displayList[i].active);
                            }
                        }

                        class Counter {
                            CreateCountDownCounter(count_from, end_func) {
                                if (count_from <= 0) {
                                    console.error("Invalid countFrom value. Must be greater than 0.");
                                    return;
                                }

                                counter_running = true;
                                counter_ended = false;
                                this.end_func = end_func;
                                this.remainingTime = count_from;
                                this.timeText = currentScene.add.text(size[0] / 2, size[1] / 2, this.remainingTime, {
                                    fontSize: '480px',
                                    color: 'aqua',
                                    align: 'center',
                                });
                                this.timeText.setOrigin(0.5, 0.5); // Center the text

                                this.countdownEvent = currentScene.time.addEvent({
                                    delay: 1000,
                                    callback: this.UpdateCounter,
                                    callbackScope: this,
                                    loop: false,
                                    repeat: this.remainingTime
                                });
                            }

                            UpdateCounter() {
                                this.timeText.setText(this.remainingTime);
                                this.remainingTime -= 1;
                
                                if (this.remainingTime < 0) {
                                    this.remainingTime = 0;
                                    this.countdownEvent.remove(); // Stop the Counter event
                                    this.timeText.setVisible(false);
                                    if (counter_running) {
                                        counter_running = false;
                                        paused = false;
                                        counter_ended = true;
                                        this.end_func();
                                    }
                                }
                            }
                        }
                        The_counter = new Counter();

                        class Game_scene extends Phaser.Scene {
                            constructor() {
                                super({ key: 'Game_scene' });
                            }

                            preload() {
                                currentScene = this;
                                scenes[0] = this;
                                //backgrounds
                                this.load.image('background', "/Assets/Backgrounds/blue.png");
                                //music
                                this.load.audio('battle music', '/Assets/battle_music.ogg');
                                this.load.audio('player laser sound', '/Assets/Sounds/sfx_laser1.ogg');
                                this.load.audio('enemy laser sound', '/Assets/Sounds/sfx_laser2.ogg');
                                this.load.audio('taking damage', '/Assets/Sounds/sfx_twoTone.ogg');
                                this.load.audio('whoosh', '/Assets/Sounds/Whoosh.mp3');
                                //main sprites
                                this.load.image('player', '/Assets/player.png');
                                this.load.image('player laser', '/Assets/laserRed.png');
                                this.load.image('enemy', '/Assets/enemyShip.png');
                                this.load.image('enemy laser', '/Assets/laserGreen.png');
                                //power_ups
                                power_up_types = ["gold bolt", "gold star", "red pill", "silver shield"];
                                this.load.image('powerUp ' + power_up_types[0], '/Assets/Power-ups/bolt_gold.png');
                                this.load.image('powerUp ' + power_up_types[1], '/Assets/Power-ups/star_gold.png');
                                this.load.image('powerUp ' + power_up_types[2], '/Assets/Power-ups/pill_red.png');
                                this.load.image('powerUp ' + power_up_types[3], '/Assets/Power-ups/shield_silver.png');
                                //shield
                                this.load.image('full shield', '/Assets/Effects/shield3.png');
                                this.load.image('mid shield', '/Assets/Effects/shield2.png');
                                this.load.image('sliver shield', '/Assets/Effects/shield1.png');
                                this.load.audio('shield up sound', '/Assets/Sounds/sfx_shieldDown.ogg');
                                this.load.audio('shield down sound', '/Assets/Sounds/sfx_shieldUp.ogg');

                                //explosion loding
                                for (let i = 1; i <= 64; i++) {
                                    this.load.image('explosion ' + i, '/Assets/explosion/explosion' + i + '.png');
                                }
                                //cabom_animation if this.anims didn't load
                                if (!this.anims) {
                                    this.anims.create({
                                        key: 'explode',
                                        frames: Array.from({ length: 64 }, (v, k) => ({ key: 'explosion ' + (k + 1) })),
                                        frameRate: 30,
                                        hideOnComplete: true
                                    });
                                }
                            }

                            create() {
                                // Background
                                this.background = this.add.tileSprite(0, 0, size[0], size[1], 'background').setOrigin(0, 0);
                                bg_music = this.sound.add('battle music', { volume: 0.1, loop: true });
                                bg_music.play();
                                bg_music.coldown = 0;
                                music_isnt_active = false;

                                // Player sprite
                                og_x = size[0] / 2;
                                og_y = size[1] - 100;
                                player = this.physics.add.sprite(og_x, og_y, 'player');  // Position player at the bottom center
                                player.setVisible(false);
                                player.setActive(false);
                                player.setBounce(0);
                                player.setCollideWorldBounds(true);
                                player.setScale(1.2);
                                player.lastFired = 0;

                                //Laser sprite
                                Lasers = this.physics.add.group({
                                    defaultKey: 'player laser',
                                    maxSize: 80
                                });

                                // Enemy group
                                enemies = this.physics.add.group({
                                    defaultKey: 'enemy',
                                    maxSize: 35,
                                    was_hidden: false
                                });

                                // Enemy laser group
                                enemyLasers = this.physics.add.group({
                                    defaultKey: 'enemy laser',
                                    maxSize: 80
                                });

                                //power_ups
                                power_ups = this.physics.add.group({
                                    defaultKey: 'powerUp',
                                    maxSize: 10
                                });

                                shield = this.physics.add.sprite(player.x, player.y, 'full shield');
                                shield.setVisible(false);
                                shield.setActive(false);
                                if (!shield_next_level_was_hidden) {
                                    shield.was_deactivated = false;
                                    shield_next_level_was_hidden = false;
                                    shield_max_life = 0;
                                }
                                shield.setBounce(0);
                                shield.setCollideWorldBounds(true);

                                // Collisions
                                this.physics.add.overlap(Lasers, enemies, hitEnemy, null, this);
                                this.physics.add.overlap(enemyLasers, player, hitPlayer, null, this);
                                this.physics.add.overlap(enemies, player, kamikaza, null, this);
                                this.physics.add.overlap(player, power_ups, collectPowerUp, null, this);
                                this.physics.add.overlap(enemies, enemies, swoop_by, null, this);
                                //shield collision
                                this.physics.add.overlap(enemyLasers, shield, hitPlayer, null, this);
                                this.physics.add.overlap(enemies, shield, kamikaza, null, this);
                                this.physics.add.overlap(shield, power_ups, collectPowerUp, null, this);

                                //cabom_animation again
                                this.anims.create({
                                    key: 'explode',
                                    frames: Array.from({ length: 64 }, (v, k) => ({ key: 'explosion ' + (k + 1) })),
                                    frameRate: 30,
                                    hideOnComplete: true
                                });

                                cursors = this.input.keyboard.createCursorKeys();//arrow keys, space, shift
                                const keysToAdd = ['A', 'S', 'D', 'W', 'P', 'BACKSPACE', 'ENTER'];
                                for (const wasdKey of keysToAdd) {
                                    cursors[`${wasdKey}_key`] = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes[wasdKey]);
                                }
                                cursors.P_key.wasClicked = false;
                                /*
                                cursors = A_key, BACKSPACE_key, D_key, ENTER_key, P_key, S_key, W_key, down arrow,
                                     left arrow, right, shift, space, up arrow, 
                                */

                                const rates = [30, 40, 50, 60, 80, 90, 95];//jumps of 5, last = endless
                                switch (level) {
                                    case 1:
                                        level_drop_rate = rates[0];
                                        maxEnemies = 3;
                                        break;

                                    case 2:
                                        level_drop_rate = rates[0];
                                        maxEnemies = 5;
                                        break;

                                    case 3:
                                        level_drop_rate = rates[0];
                                        maxEnemies = 8;
                                        break;

                                    case 4:
                                        level_drop_rate = rates[0];
                                        maxEnemies = 10;
                                        break;

                                    case 5:
                                        level_drop_rate = rates[0];
                                        maxEnemies = 3;
                                        break;

                                    case 6:
                                        level_drop_rate = rates[1];
                                        maxEnemies = 5;
                                        break;

                                    case 7:
                                        level_drop_rate = rates[1];
                                        maxEnemies = 8;
                                        break;

                                    case 8:
                                        level_drop_rate = rates[1];
                                        maxEnemies = 10;
                                        break;

                                    case 9:
                                        level_drop_rate = rates[1];
                                        maxEnemies = 3;
                                        break;

                                    case 10:
                                        level_drop_rate = rates[1];
                                        maxEnemies = 5;
                                        break;

                                    case 11:
                                        level_drop_rate = rates[2];
                                        maxEnemies = 8;
                                        break;

                                    case 12:
                                        level_drop_rate = rates[2];
                                        maxEnemies = 10;
                                        break;

                                    case 13:
                                        level_drop_rate = rates[2];
                                        maxEnemies = 3;
                                        break;

                                    case 14:
                                        level_drop_rate = rates[2];
                                        maxEnemies = 5;
                                        break;

                                    case 15:
                                        level_drop_rate = rates[2];
                                        maxEnemies = 8;
                                        break;

                                    case 16:
                                        level_drop_rate = rates[3];
                                        maxEnemies = 10;
                                        break;

                                    case 17:
                                        level_drop_rate = rates[3];
                                        maxEnemies = 3;
                                        break;

                                    case 18:
                                        level_drop_rate = rates[3];
                                        maxEnemies = 5;
                                        break;

                                    case 19:
                                        level_drop_rate = rates[3];
                                        maxEnemies = 8;
                                        break;

                                    case 20:
                                        level_drop_rate = rates[3];
                                        maxEnemies = 10;
                                        break;

                                    case 21:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 8;
                                        break;

                                    case 22:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 10;
                                        break;

                                    case 23:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 3;
                                        break;

                                    case 24:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 5;
                                        break;

                                    case 25:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 8;
                                        break;

                                    case 26:
                                        level_drop_rate = rates[5];
                                        maxEnemies = 10;
                                        break;

                                    case 28:
                                        level_drop_rate = rates[4];
                                        maxEnemies = 3;
                                        break;

                                    case 27:
                                        level_drop_rate = rates[5];
                                        maxEnemies = 5;
                                        break;

                                    case 29:
                                        level_drop_rate = rates[5];
                                        maxEnemies = 8;
                                        break;

                                    case 30:
                                        level_drop_rate = rates[5];
                                        maxEnemies = 10;
                                        break;

                                    default:// endless mode
                                        level_drop_rate = rates[6];
                                        is_endless = true;
                                        this.time.addEvent({
                                            delay: 1000,
                                            callback: spawn_enemy,
                                            callbackScope: this,
                                            loop: true
                                        });
                                        break;
                                }

                                update_level(level);
                                The_counter.CreateCountDownCounter(3, () => {
                                    player.setVisible(true);
                                    player.setActive(true);
                                    player.setPosition(og_x, og_y);
                                    player.setVelocity(0);
                                    if (shield_next_level_was_hidden) {
                                        shield.setVisible(true);
                                        shield.setActive(true);
                                    }
                                });
                            }

                            update(time, delta) {
                                // Background scrolling
                                this.background.tilePositionY -= 10; //60 is max
                                update_music();
                                manu_actions();
                                if (!paused) {
                                    player_actions(time, delta);
                                    if (shield.active) {
                                        shield_update();
                                    }
                                    enemies_update(time, delta);

                                    if (killed === maxEnemies && !is_endless) {
                                        win = true;
                                    }
                                    if (health === 0) {
                                        gameover = true;
                                    }
                                    if (win && gameover) {
                                        tico = true;
                                        win = false;
                                        gameover = false;
                                    }
                                    if (gameover || win || tico) {
                                        this.time.delayedCall(3000, end);
                                    }
                                }

                                update_progress_bar(killed, maxEnemies, is_endless);
                                update_help_bar(health);
                                update_score(score);
                            }
                        }

                        class End_scene extends Phaser.Scene {
                            constructor() {
                                super({ key: 'End_scene' });
                            }

                            start_end() {
                                if (!win) {
                                    this.level_select_button.x = size[0] / 2;
                                }
                                this.startEnd = false;
                                if (currentScene.time.now > ending_coldown) {
                                    if (gameover) {
                                        end_text(gameover_text, 'losing');
                                    }
                                    else if (win) {
                                        score += 10000;
                                        end_text(win_text, 'wining');
                                    }
                                    else {//tico
                                        score += 5000;
                                        end_text(tico_text, 'losing');
                                    }
                                    update_level(level);
                                    ending_coldown = currentScene.time.now + 5000;
                                }
                            }

                            preload() {
                                currentScene = this;
                                scenes[1] = this;
                                //music
                                this.load.audio('losing music', '/Assets/Sounds/sfx_lose.ogg');
                                this.load.audio('wining music', '/Assets/Sounds/sfx_zap.ogg');
                                //buttons
                                this.load.image('Level selection button', "/Assets/Main_Menu/level_select_button.png");
                                this.load.image('next level button', "/Assets/Main_Menu/next_level_button.png");
                            }

                            create() {
                                this.background = this.add.tileSprite(0, 0, size[0], size[1], 'background').setOrigin(0, 0);
                                bg_music = this.sound.add('battle music', { volume: 0.1, loop: true });
                                bg_music.play();
                                bg_music.coldown = 0;
                                music_isnt_active = false;

                                //game_over_text
                                gameover_text = this.add.text(size[0] / 2, size[1] / 2, "Gameover!", {
                                    font: "360px Arial", // Adjust font and style
                                    fill: "#ff0000",  // Text color (red)
                                    align: "center"    // Center alignment
                                });
                                gameover_text.setOrigin(0.5, 0.5);
                                gameover_text.setWordWrapWidth(size[0]);
                                gameover_text.setVisible(false);
                                gameover_text.ended = false;

                                win_text = this.add.text(size[0] / 2, size[1] / 2, "Win!", {
                                    font: "360px Arial", // Adjust font and style
                                    fill: "#62FFA6",  // Text color (teal)
                                    align: "center"    // Center alignment
                                });
                                win_text.setOrigin(0.5, 0.5);
                                win_text.setWordWrapWidth(size[0]);
                                win_text.setVisible(false);
                                win_text.ended = false;

                                tico_text = this.add.text(size[0] / 2, size[1] / 2, "draw.", {
                                    font: "360px Arial", // Adjust font and style
                                    fill: "#FF9D51",  // Text color (orange)
                                    align: "center"    // Center alignment
                                });
                                tico_text.setOrigin(0.5, 0.5);
                                tico_text.setWordWrapWidth(size[0]);
                                tico_text.setVisible(false);
                                tico_text.ended = false;

                                this.level_select_button = this.add.sprite((size[0] / 2) - 350, size[1] / 2, 'Level selection button');
                                this.level_select_button.setOrigin(0.5, 0.5);
                                this.level_select_button.setScale(0.15, 0.15);
                                this.level_select_button.setInteractive();
                                this.level_select_button.setVisible(false);
                                this.level_select_button.on('pointerdown', () => {
                                    window.location.href = 'Level selection.aspx';
                                });

                                this.next_level_button = this.add.sprite((size[0] / 2) + 350, size[1] / 2 + 45, 'next level button');//next_level_button
                                this.next_level_button.setOrigin(0.5, 0.5);
                                this.next_level_button.setScale(0.15, 0.15);
                                this.next_level_button.setInteractive();
                                this.next_level_button.setVisible(false);
                                this.next_level_button.on('pointerdown', () => {
                                    level += 1;
                                    restart_level();
                                });

                                cursors = this.input.keyboard.createCursorKeys();//arrow keys, space, shift
                                const keysToAdd = ['P', 'BACKSPACE', 'ENTER'];
                                for (const wasdKey of keysToAdd) {
                                    cursors[`${wasdKey}_key`] = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes[wasdKey]);
                                }
                                /*
                                cursors = BACKSPACE_key,  ENTER_key, P_key, down arrow, left arrow, right arrow, shift,
                                    space, up arrow, 
                                */
                                this.startEnd = true;
                            }

                            update() {
                                this.background.tilePositionY -= 10; //60 is max
                                update_music();
                                manu_actions();
                                if (this.startEnd) {
                                    this.start_end();
                                }
                            }
                        }

                        const config = {
                            type: Phaser.AUTO,
                            scale: {
                                parent: 'game_section',
                                mode: Phaser.Scale.FIT,
                                min: {
                                    width: 225,
                                    height: 325
                                },
                                max: {
                                    width: 2000,
                                    height: 1000
                                },
                            },
                            width: size[0],
                            height: size[1],
                            scene: [Game_scene, End_scene],
                            physics: {
                                default: 'arcade',
                                arcade: {
                                    debug: false
                                }
                            }
                        };

                        const game = new Phaser.Game(config);

                        function end() {
                            paused = hide_all(true);
                            currentScene.scene.start('End_scene');
                        }

                        function end_text(text, music) {
                            if (!text.ended) {
                                text.setVisible(true);
                                text.ended = true;
                                scenes[1].sound.stopAll();
                                music_isnt_active = true;
                                music_ended = false;
                                if (is_music_on && music_isnt_active) {
                                    scenes[1].sound.add(music + ' music', { volume: 0.6, loop: false }).play();
                                    if (tico) {
                                        scenes[1].sound.add('wining music', { volume: 0.6, loop: false }).play();
                                    }
                                    is_music_on = false; //to not reactivate the bg_music
                                    music_ended = true;
                                }
                                text.coldown = scenes[1].time.now + 1600;
                            }
                            if (scenes[1]) {
                                scenes[1].time.addEvent({
                                    delay: 1400,
                                    callback: () => {
                                        if (music_ended) {
                                            is_music_on = true; //to return to normal
                                        }
                                        text.setVisible(false);
                                        scenes[1].level_select_button.setVisible(true);
                                        if (!is_endless && win) {
                                            scenes[1].next_level_button.setVisible(true);
                                        }
                                    },
                                    callbackScope: scenes[1]
                                });
                            }
                            //user.score = score;
                            //user.level = level;
                        }

                        function hide_all(is_next_level) {
                            counter_running = false;
                            if (!counter_running) {//will always be true
                                if (The_counter.countdownEvent) {
                                    The_counter.countdownEvent.remove();
                                }
                                if (The_counter.timeText) {
                                    The_counter.timeText.setVisible(false);
                                }
                                The_counter.remainingTime = 0;
                                The_counter.is_hiding = true;
                            }


                            if (currentScene === scenes[0]) {
                                if (enemies) {
                                    enemies.getChildren().forEach(function (enemy, index) {
                                        if (enemy.active) {
                                            enemy.setActive(false);
                                            enemy.setVisible(false);
                                            enemy.setVelocityX(0);
                                            enemy.setVelocityY(0);
                                            enemy.was_hidden = true;
                                        }
                                    });
                                }

                                if (enemyLasers.getChildren().length > 0) {
                                    enemyLasers.getChildren().forEach(function (enemy_laser) {
                                        enemy_laser.setActive(false);
                                        enemy_laser.setVisible(false);
                                    });
                                }

                                if (Lasers.getChildren().length > 0) {
                                    Lasers.getChildren().forEach(function (laser) {
                                        laser.setActive(false);
                                        laser.setVisible(false);
                                    });
                                }

                                if (power_ups.getChildren().length > 0) {
                                    power_ups.getChildren().forEach(function (powerUp) {
                                        powerUp.setActive(false);
                                        powerUp.setVisible(false);
                                        powerUp.setVelocity(0);
                                    });
                                }

                                if (explosionSprite) {
                                    explosionSprite.setActive(false);
                                    explosionSprite.setVisible(false);
                                    explosionSprite.destroy();
                                }

                                if (shield.active) {
                                    shield.setActive(false);
                                    shield.setVisible(false);
                                    shield.was_deactivated = true;
                                    if (is_next_level) {
                                        shield_next_level_was_hidden = true;
                                    }
                                }
                                player.setActive(false);
                                player.setVisible(false);
                                player.setVelocityX(0);
                                player.setVelocityY(0);
                            }
                            else if (currentScene === scenes[1]) {
                                scenes[1].level_select_button.setActive(false);
                                scenes[1].level_select_button.setVisible(false);
                                scenes[1].next_level_button.setActive(false);
                                scenes[1].next_level_button.setVisible(false);
                            }
                            return true;//paused = t
                        }

                        function show_all() {
                            The_counter.CreateCountDownCounter(3, () => {
                                if (currentScene === scenes[0]) {
                                    enemies.getChildren().forEach(function (enemy, index) {
                                        if (enemy.was_hidden) {
                                            enemy.setActive(true);
                                            enemy.setVisible(true);
                                            enemy.was_hidden = false;
                                        }
                                    });

                                    enemyLasers.getChildren().forEach(function (enemy_laser) {
                                        enemy_laser.setActive(true);
                                        enemy_laser.setVisible(true);
                                    });

                                    Lasers.getChildren().forEach(function (laser) {
                                        laser.setActive(true);
                                        laser.setVisible(true);
                                    });

                                    power_ups.getChildren().forEach(function (powerUp) {
                                        activatePowerup(powerUp);
                                    });

                                    if (explosionSprite) {
                                        explosionSprite.setActive(true);
                                        explosionSprite.setVisible(true);
                                    }

                                    if (shield.was_deactivated) {
                                        shield.setActive(true);
                                        shield.setVisible(true);
                                        update_shield_pos();
                                        shield.was_deactivated = false;
                                    }
                                    player.setActive(true);
                                    player.setVisible(true);
                                }
                                else if (currentScene === scenes[1]) {
                                    scenes[1].level_select_button.setActive(true);
                                    scenes[1].level_select_button.setVisible(true);
                                    scenes[1].next_level_button.setActive(true);
                                    scenes[1].next_level_button.setVisible(true);
                                }
                                paused = false;
                            });
                        }

                        function activatePowerup(powerUp) {
                            if (powerUp) {
                                powerUp.setActive(true);
                                powerUp.setVisible(true);
                                powerUp.setVelocityY(100);
                            }
                        }

                        function collectPowerUp(sprite, powerUp) {
                            if (player.active && powerUp.active && sprite.active) {//if sprite is shield, he has to active too
                                powerUp.setActive(false);
                                powerUp.setVisible(false);

                                if (!powerUp.coldown) {
                                    powerUp.coldown = 0;
                                }
                                if (scenes[0].time.now > powerUp.coldown) {
                                    if (powerUp.texture.key === 'powerUp ' + power_up_types[0]) {//gold bolt
                                        score += 120;
                                        if (player_lasers_count < 9) {
                                            player_lasers_count += 1;
                                        }
                                        else {
                                            score += 140
                                        }
                                    } else if (powerUp.texture.key === 'powerUp ' + power_up_types[1]) {//gold star
                                        score += 500;
                                    }
                                    if (powerUp.texture.key === 'powerUp ' + power_up_types[2]) {//red pill
                                        score += 5;
                                        health++;
                                    } else if (powerUp.texture.key === 'powerUp ' + power_up_types[3]) {//silver shield
                                        if (!shield.active) {
                                            activateShield(3);
                                            score += 45
                                        } else {
                                            shield_life += 3;
                                            shield_max_life += 3;
                                            score += 30;
                                        }
                                    }
                                    powerUp.coldown = scenes[0].time.now + 1500;
                                }
                            }
                        }

                        function dropPowerUp(x, y) {
                            let powerUpType_i = Phaser.Math.Between(0, power_up_types.length - 1);
                            let powerUp = power_ups.get(x, y, 'powerUp ' + power_up_types[powerUpType_i]);
                            activatePowerup(powerUp);
                        }

                        function create_enemies() {
                            if (!is_endless && !counter_running) {
                                let activeEnemies = enemies.countActive(true);//on screen and weren't killed
                                let toSpawn = maxEnemies - activeEnemies - killed;//calcs how many he sould re add to the screen
                                for (let i = 0; i < toSpawn; i++) {//spawn the needed amount on screen
                                    spawn_enemy();
                                }
                            }
                        }

                        function enemies_update(time, delta) {
                            create_enemies();
                            // Update enemies and their shooting logic
                            enemies.getChildren().forEach(function (enemy) {
                                if (enemy.active) {
                                    enemy.y += 0.5; // Move enemies downwards
                                    //left = -50, right = 1850
                                    if (enemy.y > size[1]+50 || enemy.x > size[0]+50 || enemy.x < -50) {
                                        enemy.setActive(false);
                                        enemy.setVisible(false);
                                    }

                                    // Enemy shooting logic
                                    if (!enemy.lastFired) {//אם אין לenemy lastFired תיצור ותאפס
                                        enemy.lastFired = 0;
                                    }
                                    if (time > enemy.lastFired) { //אם הזמן יותר גדול מזמן הירייה האחרון, תירה
                                        EnemyfireLaser(enemy);
                                        enemy.lastFired = time + Phaser.Math.Between(500, 2000); // Random fire rate for enemies
                                    }
                                }
                            });

                            // Update enemy Lasers
                            enemyLasers.getChildren().forEach(function (enemyLaser) {
                                if (enemyLaser.active) {
                                    enemyLaser.update(time, delta);
                                    if (enemyLaser.y > size[1] || enemyLaser.x > size[0]+50 || enemyLaser.x < -50) {
                                        enemyLaser.setActive(false);
                                        enemyLaser.setVisible(false);
                                    }
                                }
                            });
                        }

                        function EnemyfireLaser(enemy) {
                            let enemyLaser = enemyLasers.get(enemy.x, enemy.y + 20);
                            if (enemyLaser) {
                                enemyLaser.setActive(true);
                                enemyLaser.setVisible(true);
                                enemyLaser.setVelocityY(300);
                                enemy.setVelocityX(Phaser.Math.Between(-300, 300));
                                make_sound('enemy laser sound', 0.5);
                            }
                        }

                        function hitEnemy(laser, enemy) {
                            if (laser.active && enemy.active) {
                                laser.setActive(false);
                                laser.setVisible(false);
                                kill_enemy(enemy);
                            }
                        }

                        function kill_enemy(enemy) {
                            //if enemy.active is in hitEnemy or kamikaza
                            score += 15;
                            killed += 1;
                            explosion_activater(enemy);
                            if (Phaser.Math.Between(0, 100) <= level_drop_rate) {//level_drop_rate% droping
                                dropPowerUp(enemy.x, enemy.y);
                            }
                            else {
                                score += 100;
                            }
                            enemy.setActive(false);
                            enemy.setVisible(false);
                        }

                        function hitPlayer(sprite, enemyLaser) {
                            //sprite = player/shield
                            if (player.active && enemyLaser.active && sprite.active) {//if sprite is shield, he has to active too
                                if (enemyLaser) {
                                    enemyLaser.setActive(false);
                                    enemyLaser.setVisible(false);
                                }
                                if (health > 0 && !invulnerable) {
                                    handlePlayerHit(player);
                                }
                            }
                        }

                        function kamikaza(sprite, enemy) {
                            if (player.active && enemy.active && sprite.active) {//if sprite is shield, he has to active too
                                if (health > 0 && !invulnerable) {
                                    handlePlayerHit(player);
                                    kill_enemy(enemy);
                                }
                            }
                        }

                        function explosion_activater(sprite) {
                            explosionSprite = scenes[0].add.sprite(sprite.x, sprite.y - 50, 'explosion 1');
                            explosionSprite.play('explode').stopAfterRepeat(0);
                            explosionSprite.setScale(3);
                        }

                        function handlePlayerHit(player) {
                            make_sound('taking damage', 0.4);
                            if (shield.active) {
                                shield_life -= 1;
                                shield_life_decrese_count++;
                                if (shield_life <= 0) {
                                    deactivateShield();
                                }
                            } else {
                                health -= 1;
                            }
                            invulnerable = true;
                            if (!paused) {
                                explosion_activater(player);
                                player.setActive(false);
                                player.setVisible(false);
                                if (shield.active) {
                                    shield.setActive(false);
                                    shield.setVisible(false);
                                    shield.was_deactivated = true;
                                }
                            }

                            scenes[0].time.addEvent({
                                delay: 2000,
                                callback: () => {
                                    if (!paused) {
                                        player.setPosition(og_x, og_y);
                                        player.setActive(true);
                                        player.setVisible(true);
                                        if (shield.was_deactivated) {
                                            shield.setActive(true);
                                            shield.setVisible(true);
                                            shield.was_deactivated = false;
                                        }

                                        scenes[0].time.addEvent({
                                            delay: 200,
                                            callback: () => {
                                                invulnerable = false;
                                            },
                                            callbackScope: scenes[0]
                                        });
                                    }
                                },
                                callbackScope: scenes[0]
                            });
                        }

                        function spawn_enemy() {
                            if (!paused) {
                                let x = Phaser.Math.Between(0, size[0]);
                                let enemy = enemies.get(x, -50);
                                if (enemy) {
                                    enemy.setActive(true);
                                    enemy.setVisible(true);
                                    enemy.setVelocityY(50);
                                    enemy.was_hidden = false;
                                }
                            }
                        }

                        function update_music() {
                            //is_music_on/is_sound_on updates in master page
                            // Update music state only after the timeout (here 1s)
                            if (!bg_music.coldown) {
                                bg_music.coldown = 0;
                            }
                            if (is_music_on && music_isnt_active && bg_music.coldown < scenes[0].time.now) {
                                if (!bg_music) {
                                    bg_music = scenes[0].sound.add('battle music', { volume: 0.1, loop: true });
                                } 
                                bg_music.play();
                                music_isnt_active = false;
                                bg_music.coldown = scenes[0].time.now + 1000;
                            } else if (!is_music_on && !music_isnt_active) {
                                //bg_music = scenes[0].sound.get('battle music');
                                if (bg_music) {
                                    bg_music.stop();
                                    music_isnt_active = true;
                                }
                            }
                        }

                        function resize() {
                            game.scale.resize(size[0], size[1]);
                            size[0] = game.canvas.width;
                            size[1] = game.canvas.height;
                            if (currentScene === scenes[0]) {
                                og_x = size[0] / 2;
                                og_y = size[1] - 100;
                                rescale(1.2);
                            }
                        }

                        function rescale(scale) {
                            if (currentScene === scenes[0]) {
                                enemies.getChildren().forEach(function (enemy, index) {
                                    enemy.setScale(scale);
                                });
                                enemyLasers.getChildren().forEach(function (enemy_laser) {
                                    enemy_laser.setScale(scale);
                                });
                                Lasers.getChildren().forEach(function (laser) {
                                    laser.setScale(scale);
                                });
                                power_ups.getChildren().forEach(function (powerUp) {
                                    powerUp.setScale(scale);
                                });
                                if (shield.active) {
                                    shield.setScale(scale);
                                }
                                player.setScale(scale);
                            }

                        }
                        function manu_actions() {
                            resize();
                            //hides counter
                            if (counter_ended) {
                                The_counter.timeText.setVisible(false);
                            }

                            //pause button
                            if (cursors.P_key.isDown && !cursors.P_key.wasClicked) {
                                cursors.P_key.wasClicked = true;
                                if (settings.style.display === 'block' && currentScene.time.now > hide_setting_coldown) {
                                    close_settings();
                                    hide_setting_coldown = currentScene.time.now + 500;
                                }
                                else if (currentScene.time.now > hide_setting_coldown) {
                                    open_settings();
                                    hide_setting_coldown = currentScene.time.now + 500;
                                }
                            }
                            else {
                                cursors.P_key.wasClicked = false;
                            }

                            //next level button
                            if (cursors.ENTER_key.isDown && scenes[1].next_level_button.visible) {
                                scenes[1].next_level_button.emit('pointerdown');
                            }
                            //Level selection button
                            if (cursors.BACKSPACE_key.isDown && scenes[1].level_select_button.visible) {
                                scenes[1].level_select_button.emit('pointerdown');
                            }
                        }

                        function player_actions(time, delta) {
                            if (!player.active) {
                                player.setVelocityX(0);
                                player.setVelocityY(0);
                                return;
                            }

                            // Player movement
                            if ((cursors.left.isDown || cursors.A_key.isDown)) {
                                player.setVelocityX(-300);

                            } else if ((cursors.right.isDown || cursors.D_key.isDown)) {
                                player.setVelocityX(300);
                            } else {
                                player.setVelocityX(0);
                            }

                            //vertical movement
                            if ((cursors.down.isDown || cursors.S_key.isDown)) {
                                player.setVelocityY(300);
                            } else if ((cursors.up.isDown || cursors.W_key.isDown)) {
                                player.setVelocityY(-300);
                            } else {
                                player.setVelocityY(0);
                            }

                            laser_update(time, delta);
                        }

                        function laser_update(time, delta) {
                            if (cursors.space.isDown && time > player.lastFired) {
                                shot_laser();
                                player.lastFired = time + 150;
                            }
                            Lasers.getChildren().forEach(function (laser) {
                                if (laser.active) {
                                    laser.update(time, delta);
                                    if (laser.y < 0) {
                                        laser.setActive(false);
                                        laser.setVisible(false);
                                    }
                                }
                            });
                        }

                        function shot_laser() {
                            const laser_offset = 18; // Fixed distance between lasers
                            let start_offset;
                            if (player_lasers_count % 2 === 0) {// אם כמות הלייזרים שהספינה יורה הם זוגיים
                                /*
                                כנקודת ההתחלה של הלייזר = (-קבוע הרווח * מציאת נקודת האמצע למספר לייזרים) + חצי מקבוע הרווח
                                 כלומר מחשב את נקודת התחלה הרחוקה מהאמצע לשמאל (-) ומאזן את הזוגי עם הדמות
                                */
                                start_offset = -laser_offset * (player_lasers_count / 2) + laser_offset / 2;
                            } else {// אם כמות הלייזרים שהספינה יורה הם אי-זוגיים
                                /*
                                כנקודת ההתחלה של הלייזר = (-קבוע הרווח * מציאת נקודת האמצע למספר לייזרים)
                                 כלומר מחשב את נקודת התחלה הרחוקה מהאמצע לשמאל (-)
                                */
                                start_offset = -laser_offset * (player_lasers_count / 2);
                            }

                            for (let i = 0; i < player_lasers_count; i++) {
                                const x = player.x + start_offset + i * laser_offset;// יוצר רווח לכל לייזר חדש בציר ה x
                                //יוצר רווח לכל לייזר חדש בציר ה y
                                const y = player.y - 50 + Math.abs(i - (player_lasers_count - 1) / 2) * 16;
                                let laser = Lasers.get(x, y);
                                if (laser && laser.y < size[1]) {
                                    laser.setActive(true);
                                    laser.setVisible(true);
                                    laser.setVelocityY(-500);
                                    make_sound('player laser sound', 0.4);
                                }
                            }
                        }

                        function make_sound(sound_name, volume) {
                            if (is_sound_on) {
                                if (sound_isnt_active) {
                                    currentScene.sound.add(sound_name, { volume: volume, loop: false }).play();
                                    sound_isnt_active = false;
                                }
                                else {
                                    sound_isnt_active = true;
                                }
                            }
                            else {
                                const sound = currentScene.sound.get(sound_name);
                                if (sound) {
                                    sound.pause();
                                    sound.currentTime = 0;
                                    sound_isnt_active = true;
                                }
                            }
                        }

                        function activateShield(life) {
                            make_sound('shield up sound', 0.4);
                            shield.setActive(true);
                            shield.setVisible(true);
                            update_shield_pos();
                            shield_life += life;
                            shield_max_life += life;
                        }

                        function deactivateShield() {
                            make_sound('shield down sound', 0.4);
                            shield.setActive(false);
                            shield.setVisible(false);
                            shield_life = 0;
                            shield_max_life = 0;
                            shield_life_decrese_count = 0;
                            shield_next_level_was_hidden = false;
                        }

                        function shield_update() {
                            update_shield_pos();
                            var precent = shield_life / shield_max_life;
                            //3 becouse it's the minimum life addition
                            if (shield_life_decrese_count % 3 == 0 && shield_life_decrese_count != 0) {
                                shield_max_life -= 3;
                                shield_life_decrese_count = 0;
                            }

                            if (precent >= 0.8) {
                                shield.setTexture('full shield');
                            } else if (precent >= 0.4) {
                                shield.setTexture('mid shield');
                            }
                            else {
                                shield.setTexture('sliver shield');
                            }
                        }

                        function update_shield_pos() {
                            const left_limit = size[0] - 80;
                            const low_limit = size[1] - 90;
                            if (player.x >= left_limit) {
                                player.x = left_limit;
                            }
                            else if (player.x <= 80) {//right limit
                                player.x = 80;
                            }
                            if (player.y >= low_limit) {
                                player.y = low_limit;
                            }
                            else if (player.y <= 58) {//high limit
                                player.y = 58;
                            }
                            shield.x = player.x;
                            shield.y = player.y;
                        }

                        function restart_level() {
                            killed = 0;
                            gameover = false;
                            win = false;
                            tico = false;
                            invulnerable = false;
                            score_coldown = 0;

                            scenes[1].level_select_button.setVisible(false);
                            scenes[1].next_level_button.setVisible(false);

                            currentScene.sound.stopAll();
                            update_music();

                            currentScene.scene.start('Game_scene');
                        }

                        function swoop_by(enemy1, enemy2) {
                            if (is_sound_on && currentScene.time.now > whoosh_coldown &&
                                enemy1.visible && enemy2.visible){
                                make_sound('whoosh', 0.3);
                                whoosh_coldown = currentScene.time.now + 400;
                            }
                        }
                    </script>
    <section id="game_section">

    </section>
</asp:Content>
