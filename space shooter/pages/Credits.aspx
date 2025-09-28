<%@ Page title="Credits" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Credits.aspx.cs" Inherits="space_shooter.pages.Credits" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .credit.title{
            margin-left:8%;
            width:80%;
        }
        .credit.text{
            margin-left:15%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container">
            <div class="row">
                <center>
                    <h1 class="credit title">Thanks </h1>
                </center>
            </div>
            <div class="row">
                <center>
                    <h3 class="credit title">For Game Ideas: </h3>

                    <p class="credit text text-md-start">
                        Project: Space Shooter (Redux, plus fonts and sounds) <br />
                        by <a href="https://www.kenney.nl/" target="_blank" class="link">Kenney Vleugels</a>, 
                        <a href="http://creativecommons.org/publicdomain/zero/1.0/" target="_blank" class="link">License (CC0)</a>.<br/>
                        You may use these graphics in personal and commercial projects.
                    </p>
                    <p class="credit text text-md-start">
                            Ron Michalashvili (Full Stack Developer) For The Draw Screen Idea. <br />
                    </p>
                </center>
            </div>
            <div class="row">
                    <!-- 
                        col = coloum
                        xl/l/md/s/xs - device size
                        12/coloum amount = 12/6 = 2
                        full row = 12
                    -->
                <div class="col-md-6">
                    <center>
                        <h3 class="credit title text-md-center">For Music: </h3>

                        <p class="credit text text-md-start">
                            Piece: Rumores de la Caleta. <br/> 
                            Artist: Isaac Albéniz (1860–1909). <br/> 
                            Instruments: Piano. <br/> 
                            Written: 1887. <br/>
                            License: Public Domain. <br/>
                            Edited by: <a href="https://theouterlinux.gitlab.io" target="_blank" class="link">TheOuterLinux</a>. <br/>
                        </p>
                    </center>
                </div>
                <div class="col-md-6">
                    <center>
                        <h3 class="credit title text-md-center">For Graphic Design: </h3>

                        <p class="credit text text-md-start">
                            <a href="https://www.youtube.com/watch?v=8_eMgS6UszY&list=PLIY8eNdw5tW_ZQawyxK0Dd1cZXwcNFWn8&index=1" class="link">Simple Snippets</a>. <br />
                            Alex Chulkov (Graphic Designer).<br />
                        </p>
                    </center>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <center>
                        <h3 class="credit title text-md-center">Textures: </h3>

                        <p class="credit text text-md-start">
                            Project: Space Shooter, <br />
                            with Redux, plus fonts and sound, <br />
                            by <a href="https://www.kenney.nl/" target="_blank" class="link">Kenney Vleugels</a>, 
                            <a href="http://creativecommons.org/publicdomain/zero/1.0/" target="_blank" class="link">License (CC0)</a>.<br/>
                            You may use these graphics in personal and commercial projects.
                        </p>
                        <p class="credit text text-md-start">
                            <a href="https://www.youtube.com/watch?v=8_eMgS6UszY&list=PLIY8eNdw5tW_ZQawyxK0Dd1cZXwcNFWn8&index=1" class="link">Simple Snippets</a>. <br />
                            Alex Chulkov (Graphic Designer), <br />
                            For Buttons Design. <br />
                            <a href="https://stock.adobe.com/images/vector-transparent-pattern-background-alpha-background-png/491941791" class="link">Stock Adobe</a>.
                        </p>
                    </center>
                </div>
                <!-- 
                        col = coloum
                        xl/l/md/s/xs - device size
                        12/coloum amount = 12/6 = 2
                        full row = 12
                -->
                <div class="col-md-6">
                    <center>
                        <h3 class="credit title text-md-center">Code Assistance: </h3>
                        <p class="credit text text-md-start">
                            <a href="https://www.youtube.com/watch?v=8_eMgS6UszY&list=PLIY8eNdw5tW_ZQawyxK0Dd1cZXwcNFWn8&index=1" class="link">Simple Snippets</a>. <br />
                            gemini. <br />
                            chat gpt 4 - website genrator.
                        </p>
                    </center>
                </div>
            </div>
        </div>
    </section>
    <div style="margin-top:10vh;"></div>
    
</asp:Content>
