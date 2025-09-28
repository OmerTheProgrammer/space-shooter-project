<%@ Page Title="Home" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="space_shooter.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pic_button{
            height: 14vh;
            width:20vw;
            margin: 18px;
        }

        #home_title{
            margin-left:45%;
        }

        #exit_button{
            background-color: transparent;
            border:hidden;

            width:26vw;
        }

        #exit_pic{
            margin-left:-20%;
        }

    </style>
    <script>
        function confirmExit() {
            if (confirm("Are you sure you want to leave, already?")) {
                window.close();
                // if didn't close, Redirect to google
                window.location.href = 'https://www.google.com';
            }
        }

        document.addEventListener('DOMContentLoaded', (event) => {
            document.getElementById('exit_button').onclick = confirmExit;
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- middle of this page -->
    <section>
        <div class="container">
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-3">
                    <h1 class="title" id="home_title">Home</h1>
                </div>
                <div class="col-md-5"></div>
            </div>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <a href="Log In.aspx">
                        <img src="\Assets\Main_Menu\Start_BTN.png" alt="START" class="pic_button"/></a>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <button id="exit_button" class="pic_button" onclick="confirmExit()">
                        <img src="\Assets\Main_Menu\Exit_BTN.png" alt="EXIT" class="pic_button" 
                            id="exit_pic"/></button>
                </div>
                <div class="col-md-4"></div>
            </div>

        </div>
    </section>
</asp:Content>
