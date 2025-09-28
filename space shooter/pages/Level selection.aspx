<%@ Page Title="Level selection" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Level selection.aspx.cs" Inherits="space_shooter.pages.Player_View.Level_Select" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .level.button {
            display: inline;
            height: 60px;
            width: 60px;
            padding: 0;
        }

        .pic_text{
            width:50%;
            height:50%;
        }

        .pic_button{
            width:80px;
            height:80px;
        }

        #score-list{
            list-style-type: none;
        }

        .score_style{
            margin-left:5%;

        }
        /*
        instade of 2 - user.last_level()
        last_level() -> return user.Max_level+1;
        can't write 2 + in this form
        .level.button:nth-child(n+user.last_level())
        */
        .level.button:nth-child(n + 2) { /* Selects every button starting from the 2 */
            background-image: url("/Assets/UI/lock.png");
            background-repeat: no-repeat;
            background-position: center;
            background-size: contain; /* Adjust image size as desired */
            padding: 0;
            display: inline;
            height: 60px;
            width: 60px;
        }

        .top_button_pic{
            height:7.5vh;
            width:4vw;
        }

        .top.button{
            padding:0.2%;
            padding-top:0.7%;
            height:9vh;
            width:5vw;
        }

        .popup.title.with_button{
            margin-left:15%;
        }

        #achievement {
            position: fixed;  /* Ensures the popup stays in place when scrolling */
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);  /* Positions the popup in the center */
            padding: 20px;
            border: 1px solid #ccc;
            display: none;  /* Hide the popup initially */
            height:50vh;
        }

        #settings {
            position: fixed;  /* Ensures the popup stays in place when scrolling */
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);  /* Positions the popup in the center */
            padding: 20px;
            border: 1px solid #ccc;
            display: none;  /* Hide the popup initially */
            height:50vh;
        }

        #information {
            position: fixed;  /* Ensures the popup stays in place when scrolling */
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);  /* Positions the popup in the center */
            padding: 10px;
            border: 1px solid #ccc;
            display: none;  /* Hide the popup initially */
            height:90vh;
            width:50vw;
        }

        #secound_line{
            border: 10px solid cyan;
            border-color:#88DBD8;
            height:20vh;
            width:90vw;
            overflow-x: scroll;
            white-space: nowrap;
            justify-content: space-between;
            padding:1%;
            margin:5%;
        }

        .title {
            display: inline-block;
        }

    </style>
    <script>
        //let is_sound_on = user.is_sound_on,is_music_on = user.is_music_on;
        let is_sound_on = true, is_music_on = true;
        window.onload = function () {
            //let userMaxLevel = user.Max_level
            let userMaxLevel = 1;
            const buttons = document.querySelectorAll(".level.button");
            buttons.forEach(button => {
                if (Number(button.textContent) > userMaxLevel) {
                    button.textContent = "";
                    button.disabled = true;
                }
            });
        }

        function open_level(button) {
            //user.level = button.textContent;
            window.location.href = 'Game.aspx';
        }

        function music() {
            is_music_on = !is_music_on;
            if (is_music_on) {
                document.getElementById('music_button_pic').src = "/Assets/Main_Menu/BTNs_Active/Music_BTN.png";
                document.getElementById('music_button_pic').alt = "active music button";
            }
            else {
                document.getElementById('music_button_pic').src = "/Assets/Main_Menu/Setting/Music_BTN.png";
                document.getElementById('music_button_pic').alt = "unactive music button";
            }
            //user.is_music_on = is_music_on
        }

        function sound() {
            is_sound_on = !is_sound_on;
            if (is_sound_on) {
                document.getElementById('sound_button_pic').src = "/Assets/Main_Menu/BTNs_Active/Sound_BTN.png";
                document.getElementById('music_button_pic').alt = "active sound button";
            }
            else {
                document.getElementById('sound_button_pic').src = "/Assets/Main_Menu/Setting/Sound_BTN.png";
                document.getElementById('music_button_pic').alt = "unactive sound button";
            }
            //user.is_sound_on = is_sound_on
        }

        function close_settings() {
            hidePopUp('settings');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container">
            <div class="row">
                <!-- 
                    col = coloum
                    xl/l/md/s/xs - device size
                    12/coloum amount = 12/6 = 2
                    full row = 12
                -->
                <div class="col-md-6">
                    <center>
                        <h1 class="title">Level selection</h1>
                    </center>
                </div>
                <div class="col-md-2">
                    <center>
                        <button type="button" class="top button" onclick="achievement.style.display = 'block'">
                            <img
                                aria-hidden="true" alt="achievement" crossorigin="anonymous" 
                                src="\Assets\UI\achievement.png" class="top_button_pic"
                            />
                        </button>
                    </center>
                </div>
                <div class="col-md-2">
                    <center>
                        <button type="button"class="top button" onclick="settings.style.display = 'block';">
                            <img 
                                aria-hidden="true" alt="settings" crossorigin="anonymous" 
                                src="\Assets\UI\cogwheel.png" class="top_button_pic"
                            />
                        </button>
                    </center>
                </div>
                <div class="col-md-2">
                    <center>
                        <button type="button" class="top button" onclick="information.style.display = 'block';">
                            <img
                                aria-hidden="true" alt="information" crossorigin="anonymous" 
                                src="\Assets\UI\info.png" class="top_button_pic"
                            />
                        </button>
                    </center>
                </div>

            </div>
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-8" id="secound_line">
                    <button type="button" class="level button" onclick="open_level(this)">1</button>
                    <button type="button" class="level button" onclick="open_level(this)">2</button>
                    <button type="button" class="level button" onclick="open_level(this)">3</button>
                    <button type="button" class="level button" onclick="open_level(this)">4</button>
                    <button type="button" class="level button" onclick="open_level(this)">5</button>
                    <button type="button" class="level button" onclick="open_level(this)">6</button>
                    <button type="button" class="level button" onclick="open_level(this)">7</button>
                    <button type="button" class="level button" onclick="open_level(this)">8</button>
                    <button type="button" class="level button" onclick="open_level(this)">9</button>
                    <button type="button" class="level button" onclick="open_level(this)">10</button>
                    <button type="button" class="level button" onclick="open_level(this)">11</button>
                    <button type="button" class="level button" onclick="open_level(this)">12</button>
                    <button type="button" class="level button" onclick="open_level(this)">13</button>
                    <button type="button" class="level button" onclick="open_level(this)">14</button>
                    <button type="button" class="level button" onclick="open_level(this)">15</button>
                    <button type="button" class="level button" onclick="open_level(this)">16</button>
                    <button type="button" class="level button" onclick="open_level(this)">17</button>
                    <button type="button" class="level button" onclick="open_level(this)">18</button>
                    <button type="button" class="level button" onclick="open_level(this)">19</button>
                    <button type="button" class="level button" onclick="open_level(this)">20</button>
                    <button type="button" class="level button" onclick="open_level(this)">21</button>
                    <button type="button" class="level button" onclick="open_level(this)">22</button>
                    <button type="button" class="level button" onclick="open_level(this)">23</button>
                    <button type="button" class="level button" onclick="open_level(this)">24</button>
                    <button type="button" class="level button" onclick="open_level(this)">25</button>
                    <button type="button" class="level button" onclick="open_level(this)">26</button>
                    <button type="button" class="level button" onclick="open_level(this)">27</button>
                    <button type="button" class="level button" onclick="open_level(this)">28</button>
                    <button type="button" class="level button" onclick="open_level(this)">29</button>
                    <button type="button" class="level button" onclick="open_level(this)">30</button>
                </div>
                <div class="col-md-2"></div>
            </div>
        </div>
    </section>

    <div class="popup" id="achievement">
        <div class="m_container">
            <div class="m_card">
                <button type="button" class="top button" onclick="hidePopUp('achievement')">
                    <img
                        aria-hidden="true" alt="back" crossorigin="anonymous" 
                        src="\Assets\Main_Menu\Setting\Backward_BTN.png" class="top_button_pic back"
                    />
                </button>
                <h1 class="popup title with_button">Scoreboard:</h1>
            </div>
            <div class="m_card">
                <ul id="score-list"></ul>
                <script>
                    //should be made in c#
                    function addScore(score) {
                        let scoreList = document.getElementById('score-list');
                        if (scoreList) {
                            const li = document.createElement('li');
                            li.classList.add('score_style');
                            li.classList.add('text');
                            const scoreText = document.createTextNode("Score: " + score);
                            li.appendChild(scoreText);
                            scoreList.appendChild(li);
                        }
                    }
                    //if (user.gameover) {
                        //addScore(user.score);
                        addScore(1000);
                    //}
                </script>
            </div>
            <div class="m_card">
                <button type="button" onclick="hidePopUp('achievement')" class="button" 
                    style="margin-left:45%;">Close</button>
            </div>
        </div>
    </div>

    <div class="popup" id="settings">
        <div class="m_container">
            <div class="m_card">
                <button type="button" class="button top" onclick="hidePopUp('settings')">
                    <img
                        aria-hidden="true" alt="back" crossorigin="anonymous" 
                        src="\Assets\Main_Menu\Setting\Backward_BTN.png" class="top_button_pic back"
                    />
                </button>
                <img alt="Settings" class="pic_text" src="../Assets/Main_Menu/Setting/Header.png"/>
            </div>
            <div class="m_card">
                <button type="button" class="button" id="music_button" onclick="music()">
                    <img 
                        aria-hidden="true" alt="unactive music button" crossorigin="anonymous" 
                        class="pic_button" src="../Assets/Main_Menu/BTNs_Active/Music_BTN.png" id="music_button_pic"
                    />
                </button>
                <img
                    alt="Music" 
                    class="pic_text" src="../Assets/Main_Menu/Setting/Music.png"
                />
            </div>
            <div class="m_card">
                <button type="button" class="button" id="sound_button" onclick="sound()">
                    <img 
                        aria-hidden="true" alt="unactive sound button" crossorigin="anonymous"
                        class="pic_button" src="../Assets/Main_Menu/BTNs_Active/Sound_BTN.png" id="sound_button_pic"
                    />
                </button>
                <img alt="Sound" class="pic_text" src="../Assets/Main_Menu/Setting/Sound.png"/><br />
            </div>
            <div class="m_card">
                <button type="button" onclick="close_settings();" class="button back"
                    style="margin-left:30%;">Close</button>
            </div>
        </div>
    </div>

    <div class="popup" id="information">
        <div class="m_container">
            <div class="m_card">
                <button type="button" class="top button" onclick="hidePopUp('information')">
                    <img
                        aria-hidden="true" alt="back" crossorigin="anonymous" 
                        src="\Assets\Main_Menu\Setting\Backward_BTN.png" class="top_button_pic back"
                    />
                </button>
                <h1 class="popup title with_button">Controls:</h1>
            </div>
            <div class="m_card">
                <p class="popup text">
                  Controls: <br />
                  Use the Arrow Keys or WASD Keys to move:<br />
                    Up Arrow \ W: Move Up<br />
                    Down Arrow \ S: Move Down<br />
                    Left Arrow \ A: Move Left<br />
                    Right Arrow \ D: Move Right<br />
                   Press the Space Bar to shoot projectiles.<br />
                   P: Pause/Resume the game<br />
                </p>
                <p class="popup text" style="color:red">If the <b>buttons get stuck</b>, release and click again.</p>
                  <p class="popup title"> Ending screen:</p>
                <p class="popup text">
                  Enter: Proceed to the next level<br />
                  Backspace: Return to the Level selection screen
                </p>
            </div>
            <div class="m_card">
                <button type="button" onclick="hidePopUp('information')" class="back button">Close</button>
            </div>
        </div>
    </div>
    
</asp:Content>
